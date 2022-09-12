using PX.Data;
using PX.Objects.EP;
using PX.Objects.IN;
using System;

namespace AUGSE2022
{
    [PXPrimaryGraph(typeof(AUGSetupMaint))]
    [PXCacheName("AUGSetupApproval")]
    [Serializable]
    public class AUGSetupApproval : IBqlTable, IAssignedMap
    {
        #region ApprovalID
        [PXDBIdentity(IsKey = true)]
        [PXUIField(DisplayName = "Approval ID")]
        public virtual int? ApprovalID { get; set; }
        public abstract class approvalID : PX.Data.BQL.BqlInt.Field<approvalID> { }
        #endregion

        #region AssignmentMapID
        [PXDBInt()]
        [PXDefault]
        [PXSelector(
            typeof(Search<EPAssignmentMap.assignmentMapID,
                    Where<EPAssignmentMap.entityType,
                    Equal<AssignmentMapType.AssignmentMapTypeOrder>>>),
            typeof(EPAssignmentMap.assignmentMapID),
            typeof(EPAssignmentMap.name),
            SubstituteKey = typeof(EPAssignmentMap.name),
            Filterable = true
            )]
        [PXUIField(DisplayName = "Assignment Map")]
        public virtual int? AssignmentMapID { get; set; }
        public abstract class assignmentMapID : PX.Data.BQL.BqlInt.Field<assignmentMapID> { }
        #endregion

        #region AssignmentNotificationID
        [PXDBInt()]
        [PXSelector(
            typeof(PX.SM.Notification.notificationID),
            SubstituteKey = typeof(PX.SM.Notification.name)
            )]
        [PXUIField(DisplayName = "Notification ID")]
        public virtual int? AssignmentNotificationID { get; set; }
        public abstract class assignmentNotificationID : PX.Data.BQL.BqlInt.Field<assignmentNotificationID> { }
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

        #region IsActive
        [PXDBBool()]
        [PXDefault(typeof(Search<AUGSetup.requestApproval>), PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Active")]
        public virtual bool? IsActive { get; set; }
        public abstract class isActive : PX.Data.BQL.BqlBool.Field<isActive> { }
        #endregion
    }

    #region AssignmentMapConstant
    public static class AssignmentMapType
    {
        public class AssignmentMapTypeOrder : PX.Data.BQL.BqlString.Constant<AssignmentMapTypeOrder>
        {
            public AssignmentMapTypeOrder() : base(typeof(AUGOrder).FullName) { }
        }
    }
    #endregion

}