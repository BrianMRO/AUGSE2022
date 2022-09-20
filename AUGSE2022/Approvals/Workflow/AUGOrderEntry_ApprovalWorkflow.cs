using PX.Data;
using PX.Data.WorkflowAPI;

namespace AUGSE2022
{
    using static BoundedTo<AUGOrderEntry, AUGOrder>;
    using static AUGOrder;
    using State = OrderStatuses;

    public class AUGOrderEntry_ApprovalWorkflow : PXGraphExtension<AUGOrderEntry_Workflow, AUGOrderEntry>
    {
        public static bool IsActive() => true;

        #region Approval Settings
        private class AUGApprovalSettings : IPrefetchable
        {
            private static AUGApprovalSettings Current =>
                PXDatabase.GetSlot<AUGApprovalSettings>(nameof(AUGApprovalSettings), typeof(AUGSetup), typeof(AUGSetupApproval));

            public static bool IsActive => Current.requestApproval;
            private bool requestApproval;

            void IPrefetchable.Prefetch()
            {
                using (PXDataRecord setup = PXDatabase.SelectSingle<AUGSetup>(new PXDataField<AUGSetup.requestApproval>()))
                {
                    if (setup != null)
                        requestApproval = ((bool?)setup.GetBoolean(0) == true);
                }
            }

            private static bool RequestApproval()
            {
                using (PXDataRecord approval = PXDatabase.SelectSingle<AUGSetupApproval>(
                    new PXDataField<AUGSetupApproval.approvalID>()))
                {
                    if (approval != null)
                        return (int?)approval.GetInt32(0) != null;
                }
                return false;
            }
        }
        #endregion

        public const string ApproveActionName = "Approve";
        public const string RejectActionName = "Reject";

        private static bool ApprovalIsActive => PXAccess.FeatureInstalled<PX.Objects.CS.FeaturesSet.approvalWorkflow>() && AUGApprovalSettings.IsActive;

        #region Override Configure - Show or Hide Approval Workflow
        [PXWorkflowDependsOnType(typeof(AUGSetup), typeof(AUGSetupApproval))]
        public override void Configure(PXScreenConfiguration config)
        {
            if (ApprovalIsActive)
                Configure(config.GetScreenConfigurationContext<AUGOrderEntry, AUGOrder>());
            else
                HideApproveAndRejectActions(config.GetScreenConfigurationContext<AUGOrderEntry, AUGOrder>());
        }
        #endregion

        #region HideApproveAndRejectActions
        protected virtual void HideApproveAndRejectActions(WorkflowContext<AUGOrderEntry, AUGOrder> context)
        {
            var approve = context.ActionDefinitions
                .CreateNew(ApproveActionName, a => a
                .WithCategory(PredefinedCategory.Actions)
                .PlaceAfter(g => g.putOnHold)
                .IsHiddenAlways());
            var reject = context.ActionDefinitions
                .CreateNew(RejectActionName, a => a
                .WithCategory(PredefinedCategory.Actions)
                .PlaceAfter(approve)
                .IsHiddenAlways());

            context.UpdateScreenConfigurationFor(screen =>
            {
                return screen
                    .WithActions(actions =>
                    {
                        actions.Add(approve);
                        actions.Add(reject);
                    });
            });
        }
        #endregion

        protected virtual void Configure(WorkflowContext<AUGOrderEntry, AUGOrder> context)
        {
            #region Conditions
            Condition Bql<T>() where T : IBqlUnary, new() => context.Conditions.FromBql<T>();
            Condition Existing(string name) => (Condition)context.Conditions.Get(name);
            var conditions = new
            {
                IsPendingApproval
                    = Bql<status.IsEqual<State.pendingApproval>>(),
                IsApproved
                    = Bql<approved.IsEqual<True>>(),
                IsRejected
                    = Bql<rejected.IsEqual<True>>(),

                IsOnHold
                    = Existing("IsOnHold"),
                ApprovalRequired
                    = Bql<requestApproval.IsEqual<True>>(),
            }.AutoNameConditions();
            #endregion

            #region Actions
            var approve = context.ActionDefinitions
                .CreateNew(ApproveActionName, a => a
                .WithCategory(PredefinedCategory.Actions)
                .PlaceAfter(g => g.putOnHold)
                .IsHiddenWhen(!conditions.IsPendingApproval)
                .WithFieldAssignments(fas =>
                {
                    fas.Add<approved>(true);
                }));
            var reject = context.ActionDefinitions
                .CreateNew(RejectActionName, a => a
                .WithCategory(PredefinedCategory.Actions)
                .PlaceAfter(approve)
                .IsHiddenWhen(!conditions.IsPendingApproval)
                .WithFieldAssignments(fas =>
                {
                    fas.Add<rejected>(true);
                }));
            #endregion

            context.UpdateScreenConfigurationFor(screen =>
            {
                screen
                    .UpdateDefaultFlow(flow => flow
                        .WithFlowStates(states =>
                        {
                            states
                                .Add<State.pendingApproval>(flowState => flowState
                                    .WithActions(actions =>
                                    {
                                        actions.Add(g => g.putOnHold);
                                        actions.Add(approve, c => c.IsDuplicatedInToolbar());
                                        actions.Add(reject);
                                    })
                                    .WithFieldStates(fields =>
                                    {
                                        fields.AddField<hold>(c => c.IsHidden());
                                    }));
                            states
                                .Add<State.rejected>(flowState => flowState
                                    .WithActions(actions =>
                                    {
                                        actions.Add(g => g.reopen);
                                    })
                                    .WithFieldStates(fields =>
                                    {
                                        fields.AddAllFields<AUGOrder>(c => c.IsDisabled());
                                        fields.AddField<hold>(c => c.IsHidden());
                                    }));
                            states
                                .Add<State.approved>(flowState => flowState
                                    .WithActions(actions =>
                                    {
                                        actions.Add(g => g.putOnHold);
                                        actions.Add(g => g.complete, c => c.IsDuplicatedInToolbar());
                                    })
                                    .WithFieldStates(fields =>
                                    {
                                        fields.AddAllFields<AUGOrder>(c => c.IsDisabled());
                                        fields.AddField<hold>(c => c.IsHidden());
                                    }));
                        })
                        //.WithFlowStates(states =>
                        //{
                        //    states
                        //        .Update<State.approved>(flowState => flowState
                        //            .WithActions(actions =>
                        //            {
                        //                actions.Update(g => g.pushInventoryID, a => a.IsAutoAction());
                        //            }));
                        //})
                        .WithTransitions(transitions =>
                        {
                            transitions
                                .UpdateGroupFrom(State.InitialState, ts =>
                                {
                                    ts.Add(t => t
                                        .To<State.rejected>()
                                        .IsTriggeredOn(g => g.initializeState)
                                        .When(conditions.IsRejected)
                                        .PlaceAfter(tr => tr.To<State.hold>()));
                                    ts.Add(t => t
                                        .To<State.approved>()
                                        .IsTriggeredOn(g => g.initializeState)
                                        .When(conditions.IsApproved)
                                        .PlaceAfter(tr => tr.To<State.rejected>()));
                                    ts.Add(t => t
                                        .To<State.pendingApproval>()
                                        .IsTriggeredOn(g => g.initializeState)
                                        .When(!conditions.IsApproved)
                                        .PlaceAfter(tr => tr.To<State.approved>()));
                                });

                            transitions.UpdateGroupFrom<State.hold>(ts =>
                            {
                                ts.Add(t => t
                                    .To<State.approved>()
                                    .IsTriggeredOn(g => g.removeHold)
                                    .When(!conditions.IsOnHold && conditions.IsApproved)
                                    .PlaceBefore(tr => tr.To<State.open>()));
                                ts.Add(t => t
                                    .To<State.pendingApproval>()
                                    .IsTriggeredOn(g => g.removeHold)
                                    .When(conditions.ApprovalRequired)
                                    .PlaceAfter(tr => tr.To<State.approved>()));
                            });

                            transitions
                                .AddGroupFrom<State.pendingApproval>(ts =>
                                {
                                    ts.Add(t => t
                                        .To<State.approved>()
                                        .IsTriggeredOn(approve)
                                        .When(conditions.IsApproved));

                                    ts.Add(t => t
                                        .To<State.rejected>()
                                        .IsTriggeredOn(reject)
                                        .When(conditions.IsRejected));

                                    ts.Add(t => t
                                        .To<State.hold>()
                                        .IsTriggeredOn(g => g.putOnHold)
                                        .When(conditions.IsOnHold));
                                });

                            transitions
                                .AddGroupFrom<State.rejected>(ts =>
                                {
                                    ts.Add(t => t
                                        .To<State.hold>()
                                        .IsTriggeredOn(g => g.reopen)
                                        .When(conditions.IsOnHold));
                                });

                            transitions
                                .AddGroupFrom<State.approved>(ts =>
                                {
                                    ts.Add(t => t
                                        .To<State.hold>()
                                        .IsTriggeredOn(g => g.putOnHold)
                                        .When(conditions.IsOnHold));
                                    ts.Add(t => t
                                        .To<State.complete>()
                                        .IsTriggeredOn(g => g.complete));
                                });
                        }));

                screen
                    .WithActions(actions =>
                    {
                        actions.Add(approve);
                        actions.Add(reject);
                        actions.Update(g => g.complete, c => c
                            .IsDisabledWhen(!conditions.IsApproved)
                            .IsHiddenWhen(!conditions.IsApproved)
                            );
                    });
                return screen;
            });
        }
    }
}
