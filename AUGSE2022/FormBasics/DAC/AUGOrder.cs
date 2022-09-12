using System;
using PX.Data;
using PX.Data.EP;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.TM;

namespace AUGSE2022
{
#pragma warning disable CS0618 // Type or member is "obsolete" but still required for Approvals
    [PXEMailSource]
#pragma warning restore CS0618 // Type or member is "obsolete" but still required for Approvals
    [PXPrimaryGraph(typeof(AUGOrderEntry))]
    [Serializable]
    [PXCacheName("AUGOrder")]
    public class AUGOrder : IBqlTable, IAssign
    {
        #region OrderID
        [PXDBIdentity]
        public virtual int? OrderID { get; set; }
        public abstract class orderID : PX.Data.BQL.BqlInt.Field<orderID> { }
        #endregion

        #region OrderCD
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [PXDefault]
        [AutoNumber(typeof(AUGSetup.aUGNumberingID), typeof(AccessInfo.businessDate))]
        [PXSelector(
            typeof(AUGOrder.orderCD),
            typeof(AUGOrder.orderCD),
            typeof(AUGOrder.descr),
            typeof(AUGOrder.createdDateTime)
            )]
        [PXUIField(DisplayName = "Order ID")]
        public virtual string OrderCD { get; set; }
        public abstract class orderCD : PX.Data.BQL.BqlString.Field<orderCD> { }
        #endregion

        #region LastLineNbr
        [PXDBInt()]
        [PXDefault(0)]
        public virtual int? LastLineNbr { get; set; }
        public abstract class lastLineNbr : PX.Data.BQL.BqlInt.Field<lastLineNbr> { }
        #endregion

        #region Status
        [PXDBString(1, IsFixed = true, InputMask = "")]
        [OrderStatuses.List]
        [PXDefault(OrderStatuses.Hold)]
        [PXUIField(DisplayName = "Status", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region Descr
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Descr")]
        public virtual string Descr { get; set; }
        public abstract class descr : PX.Data.BQL.BqlString.Field<descr> { }
        #endregion

        #region BranchID
        [PXDBInt()]
        [PXDBDefault(typeof(AccessInfo.branchID))]
        [PXSelector(
            typeof(Search<Branch.branchID, Where<Branch.branchID, Equal<Current<AUGOrder.branchID>>>>),
            typeof(Branch.branchCD),
            SubstituteKey = typeof(Branch.branchCD)
            )]
        [PXUIField(DisplayName = "Branch ID")]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region Hold
        [PXDBBool()]
        [PXDefault(true)]
        [PXUIField(DisplayName = "Hold")]
        public virtual bool? Hold { get; set; }
        public abstract class hold : PX.Data.BQL.BqlBool.Field<hold> { }
        #endregion

        #region OrderDate
        [PXDBDate()]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Order Date")]
        public virtual DateTime? OrderDate { get; set; }
        public abstract class orderDate : PX.Data.BQL.BqlDateTime.Field<orderDate> { }
        #endregion

        #region OwnerID
        [Owner(typeof(workgroupID), DisplayName = "Owner")]
        [PXDefault(typeof(AccessInfo.contactID), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual int? OwnerID { get; set; }
        public abstract class ownerID : PX.Data.BQL.BqlInt.Field<ownerID> { }
        #endregion

        #region WorkgroupID
        [PXDBInt()]
        [PXCompanyTreeSelector]
        [PXUIField(DisplayName = "Workgroup")]
        public virtual int? WorkgroupID { get; set; }
        public abstract class workgroupID : PX.Data.BQL.BqlInt.Field<workgroupID> { }
        #endregion

        #region RequestApproval
        [PXDBBool()]
        [PXUIField(DisplayName = "Request Approval")]
        public virtual bool? RequestApproval { get; set; }
        public abstract class requestApproval : PX.Data.BQL.BqlBool.Field<requestApproval> { }
        #endregion

        #region Approved
        [PXDBBool()]
        [PXUIField(DisplayName = "Approved")]
        public virtual bool? Approved { get; set; }
        public abstract class approved : PX.Data.BQL.BqlBool.Field<approved> { }
        #endregion

        #region Rejected
        [PXBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? Rejected { get; set; }
        public abstract class rejected : PX.Data.BQL.BqlBool.Field<rejected> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion

        #region NoteID
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    }

    #region Statuses
    public static class OrderStatuses
    {
        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute() : base(
                new[]
                {
                    Pair(Hold, PX.Objects.EP.Messages.Hold),
                    Pair(PendingApproval, PX.Objects.EP.Messages.PendingApproval),
                    Pair(Approved, PX.Objects.EP.Messages.Approved),
                    Pair(Rejected, PX.Objects.EP.Messages.Rejected),
                    Pair(Open, PX.Objects.EP.Messages.Open),
                    Pair(Complete, PX.Objects.EP.Messages.Complete),
                })
            { }
        }

        public const string InitialState = "_";
        public const string Hold = "H";
        public const string Open = "O";
        public const string Complete = "C";

        public const string PendingApproval = "P";
        public const string Approved = "A";
        public const string Rejected = "V";   // V = VOIDED


        public class initialState : PX.Data.BQL.BqlString.Constant<initialState>
        {
            public initialState() : base(InitialState) { }
        }

        public class hold : PX.Data.BQL.BqlString.Constant<hold>
        {
            public hold() : base(Hold) { }
        }

        public class pendingApproval : PX.Data.BQL.BqlString.Constant<pendingApproval>
        {
            public pendingApproval() : base(PendingApproval) { }
        }

        public class approved : PX.Data.BQL.BqlString.Constant<approved>
        {
            public approved() : base(Approved) { }
        }

        public class rejected : PX.Data.BQL.BqlString.Constant<rejected>
        {
            public rejected() : base(Rejected) { }
        }

        public class open : PX.Data.BQL.BqlString.Constant<open>
        {
            public open() : base(Open) { }
        }

        public class complete : PX.Data.BQL.BqlString.Constant<complete>
        {
            public complete() : base(Complete) { }
        }
    }
    #endregion

}