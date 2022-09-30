using PX.Data;
using PX.SM;
using System;
using System.Drawing;
using System.IO;

namespace AUGSE2022
{
    public class UploadFileMaintExt : PXGraphExtension<UploadFileMaintenance>
    {
        public static bool IsActive() => true;

        protected virtual void _(Events.RowSelected<UploadFile> e, PXRowSelected baseEvent)
        {
            if (baseEvent != null) baseEvent.Invoke(e.Cache, e.Args);
            Base.Files.AllowSelect = false;
            Base.Files.AllowInsert = false;
        }

        #region Initialize
        public override void Initialize()
        {
            base.Initialize();
        }
        #endregion

        //protected virtual void _(Events.RowPersisting<UploadFile> e, PXRowPersisting baseEvent)
        //{
        //    switch (e.Operation)
        //    {
        //        //case PXDBOperation.Insert:
        //        //    throw new PXException("UploadFileMaint Blocked Insertion");
        //        //    break;
        //        //case PXDBOperation.Update:
        //        //    throw new PXException("UploadFileMaint Blocked Update");
        //        //    break;
        //        //case PXDBOperation.Delete:
        //        //    throw new PXException("UploadFileMaint Blocked Deletion");
        //        //    break;
        //        default:
        //            break;
        //    }
        //}

        #region Persist Override
        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate del)
        {

            // Rename image.jpg on upload to resolve an issue with iPad reusing same filename, but only on ZZ301000 screen
            foreach (object obj in Base.Files.Cache.Cached)
            {
                UploadFile file = (UploadFile)obj;
                PX.SM.FileInfo attachment = new PX.SM.FileInfo(file.Name, null, file.Data);

                string FileName = attachment.FullName.Substring(attachment.FullName.Length - 9, 9);
                string FileExt = file.Extansion.ToUpper();
                if (FileExt == "JPG" && Base.Accessinfo.ScreenID == "ZZ.30.10.00")
                {
                    if (FileName == "image.jpg")
                    {
                        // This is not a safe way for Time/Date in the name in different locales, but works for a demo purposes in our region
                        //string NewName = "image-" + DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "-") + ".jpg";
                        // This should be safe in various locales
                        string NewName = "image-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".jpg";

                        attachment.FullName = attachment.FullName.Substring(0, attachment.FullName.Length - 9) + NewName;
                        file.Name = file.Name.Substring(0, file.Name.Length - 9) + NewName;
                    }

                    // Resize jpg files to a max height/width... 1920 pixels by default
                    MemoryStream stream = new MemoryStream(attachment.BinData);

                    var imageIn = Image.FromStream(stream);
                    var imageOut = ImageResizer.Resize(imageIn, 800);
                    byte[] byteArray = ImageResizer.ImageToByteArray(imageOut);

                    file.Data = byteArray;

                }
            }

            del.Invoke();
        }
        #endregion

    }
}
