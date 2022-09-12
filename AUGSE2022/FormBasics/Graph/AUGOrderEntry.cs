using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.WorkflowAPI;
using PX.Objects.EP;
using PX.Objects.IN;
using System.Collections;
using static PX.Objects.IN.InventoryAllocDetEnq;

namespace AUGSE2022
{
    public class AUGOrderEntry : PXGraph<AUGOrderEntry, AUGOrder>
    {

        #region Views
        public SelectFrom<AUGOrder>.View Document;

        public SelectFrom<AUGLine>
            .InnerJoin<InventoryItem>
                .On<InventoryItem.inventoryID.IsEqual<AUGLine.inventoryID>>
            .Where<AUGLine.orderID.IsEqual<AUGOrder.orderID.FromCurrent>>
            .View Transactions;

        public PXSetup<AUGSetup> Setup;

        public SelectFrom<AUGSetupApproval>.View SetupApproval;

        #endregion

        #region Hook to Standard Approval System
        [PXViewName("Approval")]
        public EPApprovalAutomation<AUGOrder, AUGOrder.approved, AUGOrder.rejected, AUGOrder.hold, AUGSetupApproval> Approval;
        #endregion

        #region EPApproval Cache Attached
        [PXDBDate()]
        [PXDefault(typeof(AUGOrder.orderDate), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_DocDate_CacheAttached(PXCache sender)
        {
        }

        [PXDBInt()]
        [PXDefault(typeof(AUGOrder.ownerID), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_DocumentOwnerID_CacheAttached(PXCache sender)
        {
        }

        [PXDBString(60, IsUnicode = true)]
        [PXDefault(typeof(AUGOrder.descr), PersistingCheck = PXPersistingCheck.Nothing)]
        protected virtual void EPApproval_Descr_CacheAttached(PXCache sender)
        {
        }

        //[PXDBLong()]
        //[CurrencyInfo(typeof(AUGOrder.curyInfoID))]
        //protected virtual void EPApproval_CuryInfoID_CacheAttached(PXCache sender)
        //{
        //}

        //[PXDBDecimal(4)]
        //[PXDefault(typeof(AUGOrder.curyExtCost), PersistingCheck = PXPersistingCheck.Nothing)]
        //protected virtual void EPApproval_CuryTotalAmount_CacheAttached(PXCache sender)
        //{
        //}

        //[PXDBDecimal(4)]
        //[PXDefault(typeof(AUGOrder.extCost), PersistingCheck = PXPersistingCheck.Nothing)]
        //protected virtual void EPApproval_TotalAmount_CacheAttached(PXCache sender)
        //{
        //}
        #endregion

        #region Ctor
        public AUGOrderEntry()
        {
            AUGSetup setup = Setup.Current;
        }
        #endregion

        #region Actions
        #region initializeState
        public PXAutoAction<AUGOrder> initializeState;
        #endregion

        #region RemoveHold Action
        public PXAction<AUGOrder> removeHold;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Remove Hold", MapEnableRights = PXCacheRights.Select)]
        protected virtual IEnumerable RemoveHold(PXAdapter adapter) => adapter.Get<AUGOrder>();
        #endregion

        #region Hold Action
        public PXAction<AUGOrder> putOnHold;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Hold", MapEnableRights = PXCacheRights.Select)]
        protected virtual IEnumerable PutOnHold(PXAdapter adapter) => adapter.Get<AUGOrder>();
        #endregion

        #region Reopen Action
        public PXAction<AUGOrder> reopen;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Reopen", MapEnableRights = PXCacheRights.Select)]
        protected virtual IEnumerable Reopen(PXAdapter adapter) => adapter.Get<AUGOrder>();
        #endregion

        #region Complete Action
        public PXAction<AUGOrder> complete;
        [PXButton(CommitChanges = true), PXUIField(DisplayName = "Complete", MapEnableRights = PXCacheRights.Select)]
        protected virtual IEnumerable Complete(PXAdapter adapter) => adapter.Get<AUGOrder>();
        #endregion
        #endregion

        #region AUGOrder_RequestApproval_FieldDefaulting
        protected void _(Events.FieldDefaulting<AUGOrder.requestApproval> e)
        {
            e.NewValue = Setup.Current.RequestApproval;
        }
        #endregion

    }
}