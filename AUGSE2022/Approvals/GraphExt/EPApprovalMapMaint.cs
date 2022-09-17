using PX.Data;
using System.Collections.Generic;
using System.Linq;
using EP = PX.Objects.EP;

namespace AUGSE2022
{
    public class EPApprovalMapMaint_Extension : PXGraphExtension<EP.EPApprovalMapMaint>
    {
        public static bool IsActive() => true;

        #region Override GetEntityTypeScreens - Enable Approval Maps for New Screens
        public delegate IEnumerable<string> GetEntityTypeScreensDelegate();
        [PXOverride]
        public IEnumerable<string> GetEntityTypeScreens(GetEntityTypeScreensDelegate baseMethod)
        {
            IEnumerable<string> entities = baseMethod();
            List<string> list = new List<string>();

            foreach (string s in entities)
            {
                list.Add(s);
            }
            list.Add("ZZ301000"); // AUG SE 2022 Order Entry

            return list;

        }
        #endregion

        #region EPRule_RowSelected - Override to Enable Approve/Reject Reasons for New Graphs 
        protected virtual void EPRule_RowSelected(PXCache sender, PXRowSelectedEventArgs e, PXRowSelected baseEvent)
        {
            // The baseEvent will hide the Reason fields if not enabled in standard code
            baseEvent.Invoke(sender, e);

            EP.EPRule row = e.Row as EP.EPRule;
            if (row == null)
                return;

            // Add custom graphs that should enable reason fields for approval comments
            bool isSupportedCustomType = new List<string>()
            {
                typeof(AUGSE2022.AUGOrderEntry).FullName,
            }.Contains(Base.AssigmentMap.Current?.GraphType, new PX.Data.CompareIgnoreCase());

            // Make Reason Fields Visible for Custom Screens
            if(isSupportedCustomType == true)
            {
                PXUIFieldAttribute.SetVisible<EP.EPRule.reasonForApprove>(Base.Nodes.Cache, Base.Nodes.Current, isSupportedCustomType && row.StepID != null);
                PXUIFieldAttribute.SetVisible<EP.EPRule.reasonForReject>(Base.Nodes.Cache, Base.Nodes.Current, isSupportedCustomType && row.StepID != null);
            }
        }
        #endregion

    }
}
