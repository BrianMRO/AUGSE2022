using PX.Data;
using PX.Data.BQL.Fluent;

namespace AUGSE2022
{
    public class AUGSetupMaint : PXGraph<AUGSetupMaint>
    {

        public PXSave<AUGSetup> Save;
        public PXCancel<AUGSetup> Cancel;

        public SelectFrom<AUGSetup>.View Preferences;
        public SelectFrom<AUGSetupApproval>.View SetupApproval;

        #region Event Handlers
        protected virtual void _(Events.FieldUpdated<AUGSetup.requestApproval> e)
        {
            PXCache cache = this.Caches[typeof(AUGSetupApproval)];
            foreach (AUGSetupApproval setup in PXSelect<AUGSetupApproval>.Select(this))
            {
                setup.IsActive = (bool?)e.NewValue;
                cache.Update(setup);
            }
        }
        #endregion
    }
}