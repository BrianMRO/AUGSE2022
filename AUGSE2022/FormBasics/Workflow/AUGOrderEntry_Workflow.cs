using PX.Data;
using PX.Data.WorkflowAPI;

namespace AUGSE2022
{
    using State = OrderStatuses;
    using static PX.Data.WorkflowAPI.BoundedTo<AUGOrderEntry, AUGOrder>;
    using static AUGOrder;

    public class AUGOrderEntry_Workflow : PXGraphExtension<AUGOrderEntry>
    {
        public static bool IsActive() => (true);

        private const string
            initialState = "_",
            _actionRemoveHold = "Remove Hold",
            _actionPutOnHold = "Hold",
            _actionComplete = "Complete";

        public override void Configure(PXScreenConfiguration config) => Configure(config.GetScreenConfigurationContext<AUGOrderEntry, AUGOrder>());
        protected virtual void Configure(WorkflowContext<AUGOrderEntry, AUGOrder> context)
        {

            #region Conditions
            Condition Bql<T>() where T : IBqlUnary, new() => context.Conditions.FromBql<T>();

            var conditions = new
            {
                IsOnHold
                    = Bql<hold.IsEqual<True>>(),
                HasLines
                    = Bql<lastLineNbr.IsNotEqual<Zero>>(),
                IsOpen
                    = Bql<status.IsEqual<State.open>>(),
                IsComplete
                    = Bql<status.IsEqual<State.complete>>(),
            }.AutoNameConditions();
            #endregion

            #region Actions
            //var removeHold = context.ActionDefinitions
            //    .CreateNew(_actionRemoveHold, a => a
            //    .WithCategory(PredefinedCategory.Actions)
            //    .PlaceAfter(g => g.Last)
            //    .IsDisabledWhen(!conditions.IsOnHold || !conditions.HasLines)
            //    .IsHiddenWhen(!conditions.IsOnHold)
            //    );
            //var putOnHold = context.ActionDefinitions
            //    .CreateNew(_actionPutOnHold, a => a
            //    .WithCategory(PredefinedCategory.Actions)
            //    .PlaceAfter(removeHold)
            //    .IsDisabledWhen(conditions.IsOnHold)
            //    .IsHiddenWhen(conditions.IsOnHold)
            //    );
            //var complete = context.ActionDefinitions
            //    .CreateNew(_actionComplete, a => a
            //    .WithCategory(PredefinedCategory.Actions)
            //    .PlaceAfter(putOnHold)
            //    );
            #endregion

            context.AddScreenConfigurationFor(screen =>
            {
                screen.StateIdentifierIs<status>()
                    .AddDefaultFlow(flow => flow
                        .WithFlowStates(states =>
                        {
                            states
                                .Add(initialState, flowState => flowState
                                    .IsInitial(g => g.initializeState));
                            states
                                .Add<State.hold>(flowState => flowState
                                    .WithActions(actions =>
                                    {
                                        actions.Add(g => g.removeHold, a => a.IsDuplicatedInToolbar());
                                    })
                                    .WithFieldStates(fields =>
                                    {
                                        fields.AddAllFields<AUGOrder>();
                                    }));
                            states
                                .Add<State.open>(flowState => flowState
                                    .WithActions(actions =>
                                    {
                                        actions.Add(g => g.putOnHold);
                                        actions.Add(g => g.complete, a => a.IsDuplicatedInToolbar());
                                    })
                                    .WithFieldStates(fields =>
                                    {
                                        fields.AddAllFields<AUGOrder>();
                                    }));
                            states
                                .Add<State.complete>(flowState => flowState
                                    .WithActions(actions =>
                                    {
                                        actions.Add(g => g.reopen);
                                    })
                                    .WithFieldStates(fields =>
                                    {
                                        fields.AddAllFields<AUGOrder>();
                                    }));
                        })
                        .WithTransitions(transitions =>
                        {
                            transitions.AddGroupFrom(State.InitialState, ts =>
                            {
                                ts.Add(t => t
                                    .To<State.hold>()
                                    .IsTriggeredOn(g => g.initializeState)
                                    .When(conditions.IsOnHold));
                            });

                            #region Hold
                            transitions.Add(transition => transition
                                .From<State.hold>()
                                .To<State.open>()
                                .IsTriggeredOn(g => g.removeHold)
                                );
                            #endregion

                            #region Open
                            transitions.Add(transition => transition
                                .From<State.open>()
                                .To<State.hold>()
                                .IsTriggeredOn(g => g.putOnHold)
                                );
                            transitions.Add(transition => transition
                                .From<State.open>()
                                .To<State.complete>()
                                .IsTriggeredOn(g => g.complete)
                                );
                            #endregion

                            #region Complete
                            transitions.Add(transition => transition
                                .From<State.complete>()
                                .To<State.hold>()
                                .IsTriggeredOn(g => g.reopen)
                                );
                            #endregion


                        }));

                screen
                    .WithActions(actions =>
                    {
                        actions.Add(g => g.putOnHold, f => f
                            .WithCategory(PredefinedCategory.Actions)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<hold>(true);
                                fas.Add<approved>(false);
                                fas.Add<rejected>(false);
                            }).PlaceAfter(g => g.removeHold)
                            .IsDisabledWhen(conditions.IsOnHold)
                            .IsHiddenWhen(conditions.IsOnHold)
                        );
                        actions.Add(g => g.removeHold, f => f
                            .WithCategory(PredefinedCategory.Actions)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<hold>(false);
                            })
                            .PlaceAfter(g => g.Last)
                            .IsDisabledWhen(!conditions.IsOnHold || !conditions.HasLines)
                            .IsHiddenWhen(!conditions.IsOnHold)
                        );
                        actions.Add(g => g.complete, f => f
                            .WithCategory(PredefinedCategory.Actions)
                            .PlaceAfter(g => g.putOnHold)
                        );
                        actions.Add(g => g.reopen, f => f
                            .WithCategory(PredefinedCategory.Actions)
                            .WithFieldAssignments(fas =>
                            {
                                fas.Add<hold>(true);
                                fas.Add<approved>(false);
                                fas.Add<rejected>(false);
                            })
                            .IsDisabledWhen(conditions.IsOnHold || conditions.IsOpen)
                            .IsHiddenWhen(conditions.IsOnHold || conditions.IsOpen)
                            );
                    });

                return screen;

            });
        }

    }
}