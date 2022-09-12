using System;
using PX.Data;
using PX.Objects.IN;

namespace AUGSE2022
{
    [Serializable]
    [PXCacheName("AUGLine")]
    public class AUGLine : IBqlTable
    {
        #region OrderID
        [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(AUGOrder.orderID))]
        [PXParent(
                typeof(Select<AUGOrder,
                        Where<AUGOrder.orderID,
                        Equal<Current<AUGLine.orderID>>>>)
                )]
        [PXUIField(DisplayName = "Order ID")]
        public virtual int? OrderID { get; set; }
        public abstract class orderID : PX.Data.BQL.BqlInt.Field<orderID> { }
        #endregion

        #region LineNbr
        [PXDBInt(IsKey = true)]
        [PXLineNbr(typeof(AUGOrder.lastLineNbr))]
        [PXUIField(DisplayName = "Line Nbr")]
        public virtual int? LineNbr { get; set; }
        public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }
        #endregion

        #region InventoryID
        [PX.Objects.IN.AnyInventory(Filterable = true)]
        [PXUIField(DisplayName = "Inventory ID")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region Quantity
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Quantity")]
        [PXDefault(TypeCode.Decimal, "1.0", PersistingCheck = PXPersistingCheck.NullOrBlank)] 
        public virtual Decimal? Quantity { get; set; }
        public abstract class quantity : PX.Data.BQL.BqlDecimal.Field<quantity> { }
        #endregion

        #region BaseUnit
        [INUnit(DisplayName = "Base Unit", Visibility = PXUIVisibility.SelectorVisible)]
        [PXDefault(typeof(Search<InventoryItem.salesUnit, Where<InventoryItem.inventoryID, Equal<Current<AUGLine.inventoryID>>>>), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string BaseUnit { get; set; }
        public abstract class baseUnit : PX.Data.BQL.BqlString.Field<baseUnit> { }
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
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion

        #region NoteID
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    }
}