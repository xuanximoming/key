using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Resources;

namespace DrectSoft.Common
{
    /// <summary>
    /// 图标助手
    /// </summary>
    public static class ImageHelper
    {
        #region property
        private static ImageList _imageCenter = new ImageList();
        /// <summary>
        /// 所有用到的图标集
        /// </summary>
        public static ImageList ImageCenter
        {
            get
            {
                if (_imageCenter == null)
                {
                    _imageCenter = new ImageList();
                    InitImageCenter();
                }
                return _imageCenter;
            }
        }

        #endregion

        #region ctor
        /// <summary>
        /// 构造
        /// </summary>
        static ImageHelper()
        {
            _imageCenter = new ImageList();
            InitImageCenter();
        }
        #endregion

        #region InitImage
        /// <summary>
        /// 初始化图标列表集
        /// </summary>
        private static void InitImageCenter()
        {//todo:增加新加的图标进去，并获得对应的图标KEY
            //xjt
            //禁用,用resource里对应的方法修改一下
            //_imageCenter.Images.Add(ImageKeyString.BedMappingDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.CardModel, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.EditRecordDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.Edit, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.FirstPageDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.Num1, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.ParticularInfoDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.Details, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.PatientListDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.ColumnModel, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.RefreshDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.Refresh, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.SearchAdviceDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.OrderEdit, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.SearchRecordDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.RecordQuery, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.SearchReportDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.PrintPreview, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.SecondPageDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.Num2, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.ThirdPageDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.Num3, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.ImageDetailsDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.Details, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.ImageEditDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.Edit, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.ImageOrderEditDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.OrderEdit, IconType.Disable));
            //_imageCenter.Images.Add(ImageKeyString.ImageReportQueryDisabled, Resources.ResourceManager.GetSmallIcon(ResourceNames.PrintPreview, IconType.Disable));
            ////可用
            _imageCenter.Images.Add(ImageKeyString.BedMapping, DrectSoft.Resources.ResourceManager.GetSmallIcon(ResourceNames.CardModel, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ChangeWard, Resources.ResourceManager.GetSmallIcon(ResourceNames.WaitingShift, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.EditRecord, Resources.ResourceManager.GetSmallIcon(ResourceNames.Edit, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.FirstPage, Resources.ResourceManager.GetSmallIcon(ResourceNames.Num1, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.HistorySearch, Resources.ResourceManager.GetSmallIcon(ResourceNames.RecordQuery, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.OutHospital, Resources.ResourceManager.GetSmallIcon(ResourceNames.Discharge, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ParticularInfo, Resources.ResourceManager.GetSmallIcon(ResourceNames.Details, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.PatientList, Resources.ResourceManager.GetSmallIcon(ResourceNames.ColumnModel, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.PatientWard, Resources.ResourceManager.GetSmallIcon(ResourceNames.Home, IconType.Normal));
            _imageCenter.Images.Add(ImageKeyString.Refresh, DrectSoft.Resources.ResourceManager.GetSmallIcon(ResourceNames.Refresh, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.SearchAdvice, Resources.ResourceManager.GetSmallIcon(ResourceNames.OrderEdit, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.SearchRecord, Resources.ResourceManager.GetSmallIcon(ResourceNames.RecordQuery, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.SearchReport, Resources.ResourceManager.GetSmallIcon(ResourceNames.PrintPreview, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.SecondPage, Resources.ResourceManager.GetSmallIcon(ResourceNames.Num2, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ThirdPage, Resources.ResourceManager.GetSmallIcon(ResourceNames.Num3, IconType.Normal));
            _imageCenter.Images.Add(ImageKeyString.ImageBedFemale, DrectSoft.Resources.ResourceManager.GetSmallIcon(ResourceNames.BedFemale, IconType.Normal));
            _imageCenter.Images.Add(ImageKeyString.ImageBedMale, DrectSoft.Resources.ResourceManager.GetSmallIcon(ResourceNames.BedMale, IconType.Normal));
            _imageCenter.Images.Add(ImageKeyString.ImageCritical, DrectSoft.Resources.ResourceManager.GetSmallIcon(ResourceNames.IllnessCritical, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageDetails, Resources.ResourceManager.GetSmallIcon(ResourceNames.Details, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageEdit, Resources.ResourceManager.GetSmallIcon(ResourceNames.Edit, IconType.Normal));
            _imageCenter.Images.Add(ImageKeyString.ImageFemale, DrectSoft.Resources.ResourceManager.GetSmallIcon(ResourceNames.Female, IconType.Normal));
            _imageCenter.Images.Add(ImageKeyString.ImageMale, DrectSoft.Resources.ResourceManager.GetSmallIcon(ResourceNames.Male, IconType.Normal));
            _imageCenter.Images.Add(ImageKeyString.ImageMiddling, DrectSoft.Resources.ResourceManager.GetSmallIcon(ResourceNames.IllnessMiddling, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageOrderEdit, Resources.ResourceManager.GetSmallIcon(ResourceNames.OrderEdit, IconType.Normal));
            _imageCenter.Images.Add(ImageKeyString.ImagePayOneself, DrectSoft.Resources.ResourceManager.GetSmallIcon(ResourceNames.PayOneself, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageReportQuery, Resources.ResourceManager.GetSmallIcon(ResourceNames.PrintPreview, IconType.Normal));
            _imageCenter.Images.Add(ImageKeyString.ImageUrgence, DrectSoft.Resources.ResourceManager.GetSmallIcon(ResourceNames.IllnessUrgence, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageInfomation, Resources.ResourceManager.GetSmallIcon(ResourceNames.Information, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageLeaveHospital, Resources.ResourceManager.GetSmallIcon(ResourceNames.LeaveHospital, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageLeaveHospitalToday, Resources.ResourceManager.GetSmallIcon(ResourceNames.LeaveHospitalToday, IconType.Normal));
            _imageCenter.Images.Add(ImageKeyString.ImageNoRecord, DrectSoft.Resources.ResourceManager.GetSmallIcon(ResourceNames.NoRecord, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImagePerformOperation, Resources.ResourceManager.GetSmallIcon(ResourceNames.PerformOperation, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImagePerformOperationToday, Resources.ResourceManager.GetSmallIcon(ResourceNames.PerformOperationToday, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImagePostOperation, Resources.ResourceManager.GetSmallIcon(ResourceNames.PostOperation, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImagePostOperationToday, Resources.ResourceManager.GetSmallIcon(ResourceNames.PostOperationToday, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageTimeLimitCritical, Resources.ResourceManager.GetSmallIcon(ResourceNames.TimeLimitCritical, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageTimeLimitMiddling, Resources.ResourceManager.GetSmallIcon(ResourceNames.TimeLimitMiddling, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageTransferDept, Resources.ResourceManager.GetSmallIcon(ResourceNames.TransferDept, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageTransferToDeliveryRoom, Resources.ResourceManager.GetSmallIcon(ResourceNames.TransferToDeliveryRoom, IconType.Normal));
            //_imageCenter.Images.Add(ImageKeyString.ImageTransferToICU, Resources.ResourceManager.GetSmallIcon(ResourceNames.TransferToICU, IconType.Normal));
        }
        #endregion
        /// <summary>
        /// 取得对应KEY的图标
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Image GetImageBykey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            if (ImageCenter.Images.ContainsKey(key))
            {
                Bitmap bm = ImageCenter.Images[key] as Bitmap;
                bm.MakeTransparent(Color.Magenta);
                return bm;
            }
            return null;
        }

        #region 病区一览用
        /// <summary>
        /// 取得病区一览中的extra字段用的图标集
        /// </summary>
        /// <returns></returns>
        public static ImageList GetImageListExtra()
        {
            ImageList result = new ImageList();
            result.ColorDepth = ColorDepth.Depth24Bit;
            result.ImageSize = new Size(16, 16);
            result.TransparentColor = Color.Magenta;
            result.Images.Add(ImageKeyString.ImageLeaveHospital, ImageCenter.Images[ImageKeyString.ImageLeaveHospital]);
            result.Images.Add(ImageKeyString.ImageLeaveHospitalToday, ImageCenter.Images[ImageKeyString.ImageLeaveHospitalToday]);
            result.Images.Add(ImageKeyString.ImageNoRecord, ImageCenter.Images[ImageKeyString.ImageNoRecord]);
            result.Images.Add(ImageKeyString.ImagePerformOperation, ImageCenter.Images[ImageKeyString.ImagePerformOperation]);
            result.Images.Add(ImageKeyString.ImagePerformOperationToday, ImageCenter.Images[ImageKeyString.ImagePerformOperationToday]);
            result.Images.Add(ImageKeyString.ImagePostOperation, ImageCenter.Images[ImageKeyString.ImagePostOperation]);
            result.Images.Add(ImageKeyString.ImagePostOperationToday, ImageCenter.Images[ImageKeyString.ImagePostOperationToday]);
            result.Images.Add(ImageKeyString.ImageTimeLimitCritical, ImageCenter.Images[ImageKeyString.ImageTimeLimitCritical]);
            result.Images.Add(ImageKeyString.ImageTimeLimitMiddling, ImageCenter.Images[ImageKeyString.ImageTimeLimitMiddling]);
            result.Images.Add(ImageKeyString.ImageTransferDept, ImageCenter.Images[ImageKeyString.ImageTransferDept]);
            result.Images.Add(ImageKeyString.ImageTransferToDeliveryRoom, ImageCenter.Images[ImageKeyString.ImageTransferToDeliveryRoom]);
            result.Images.Add(ImageKeyString.ImageTransferToICU, ImageCenter.Images[ImageKeyString.ImageTransferToICU]);
            return result;
        }

        /// <summary>
        /// 取得病区一览中的危重级别图标集
        /// </summary>
        /// <returns></returns>
        public static ImageList GetImageListIllness()
        {
            ImageList result = new ImageList();
            result.ColorDepth = ColorDepth.Depth24Bit;
            result.ImageSize = new Size(16, 16);
            result.TransparentColor = Color.Magenta;
            result.Images.Add(ImageKeyString.ImageMiddling, ImageCenter.Images[ImageKeyString.ImageMiddling]);
            result.Images.Add(ImageKeyString.ImageCritical, ImageCenter.Images[ImageKeyString.ImageCritical]);
            result.Images.Add(ImageKeyString.ImageUrgence, ImageCenter.Images[ImageKeyString.ImageUrgence]);
            return result;
        }

        /// <summary>
        /// 取得病区一览中的床位和性别图标集
        /// </summary>
        /// <returns></returns>
        public static ImageList GetImageListBedNum()
        {
            ImageList result = new ImageList();
            result.ColorDepth = ColorDepth.Depth24Bit;
            result.ImageSize = new Size(16, 16);
            result.TransparentColor = Color.Magenta;
            result.Images.Add(ImageKeyString.ImageMale, ImageCenter.Images[ImageKeyString.ImageFemale]);
            result.Images.Add(ImageKeyString.ImageFemale, ImageCenter.Images[ImageKeyString.ImageMale]);
            result.Images.Add(ImageKeyString.ImageBedMale, ImageCenter.Images[ImageKeyString.ImageBedMale]);
            result.Images.Add(ImageKeyString.ImageBedFemale, ImageCenter.Images[ImageKeyString.ImageBedFemale]);
            return result;
        }

        /// <summary>
        /// 取得病区一览中的自费图标集
        /// </summary>
        /// <returns></returns>
        public static ImageList GetImageListPay()
        {
            ImageList result = new ImageList();
            result.ColorDepth = ColorDepth.Depth24Bit;
            result.ImageSize = new Size(16, 16);
            result.TransparentColor = Color.Magenta;
            result.Images.Add(ImageKeyString.ImagePayOneself, ImageCenter.Images[ImageKeyString.ImagePayOneself]);
            return result;
        }

        /// <summary>
        /// 获得病区一览中的按钮图标列表
        /// </summary>
        /// <returns></returns>
        //public static ImageList GetImageBar()
        //{
        //    ImageList result = new ImageList();
        //    result.ColorDepth = ColorDepth.Depth24Bit;
        //    result.ImageSize = new Size(16, 16);
        //    result.TransparentColor = Color.Magenta;
        //    result.Images.Add(ImageKeyString.BedMapping, ImageCenter.Images[ImageKeyString.BedMapping]);
        //    result.Images.Add(ImageKeyString.ChangeWard, ImageCenter.Images[ImageKeyString.ChangeWard]);
        //    result.Images.Add(ImageKeyString.EditRecord, ImageCenter.Images[ImageKeyString.EditRecord]);
        //    result.Images.Add(ImageKeyString.FirstPage, ImageCenter.Images[ImageKeyString.FirstPage]);
        //    result.Images.Add(ImageKeyString.HistorySearch, ImageCenter.Images[ImageKeyString.HistorySearch]);
        //    result.Images.Add(ImageKeyString.OutHospital, ImageCenter.Images[ImageKeyString.OutHospital]);
        //    result.Images.Add(ImageKeyString.ParticularInfo, ImageCenter.Images[ImageKeyString.ParticularInfo]);
        //    result.Images.Add(ImageKeyString.PatientList, ImageCenter.Images[ImageKeyString.PatientList]);
        //    result.Images.Add(ImageKeyString.PatientWard, ImageCenter.Images[ImageKeyString.PatientWard]);
        //    result.Images.Add(ImageKeyString.Refresh, ImageCenter.Images[ImageKeyString.Refresh]);
        //    result.Images.Add(ImageKeyString.SearchAdvice, ImageCenter.Images[ImageKeyString.SearchAdvice]);
        //    result.Images.Add(ImageKeyString.SearchRecord, ImageCenter.Images[ImageKeyString.SearchRecord]);
        //    result.Images.Add(ImageKeyString.SearchReport, ImageCenter.Images[ImageKeyString.SearchReport]);
        //    result.Images.Add(ImageKeyString.SecondPage, ImageCenter.Images[ImageKeyString.SecondPage]);
        //    result.Images.Add(ImageKeyString.ThirdPage, ImageCenter.Images[ImageKeyString.ThirdPage]);
        //    result.Images.Add(ImageKeyString.BedMappingDisabled, ImageCenter.Images[ImageKeyString.BedMappingDisabled]);
        //    result.Images.Add(ImageKeyString.EditRecordDisabled, ImageCenter.Images[ImageKeyString.EditRecordDisabled]);
        //    result.Images.Add(ImageKeyString.FirstPageDisabled, ImageCenter.Images[ImageKeyString.FirstPageDisabled]);
        //    result.Images.Add(ImageKeyString.ParticularInfoDisabled, ImageCenter.Images[ImageKeyString.ParticularInfoDisabled]);
        //    result.Images.Add(ImageKeyString.PatientListDisabled, ImageCenter.Images[ImageKeyString.PatientListDisabled]);
        //    result.Images.Add(ImageKeyString.RefreshDisabled, ImageCenter.Images[ImageKeyString.RefreshDisabled]);
        //    result.Images.Add(ImageKeyString.SearchAdviceDisabled, ImageCenter.Images[ImageKeyString.SearchAdviceDisabled]);
        //    result.Images.Add(ImageKeyString.SearchRecordDisabled, ImageCenter.Images[ImageKeyString.SearchRecordDisabled]);
        //    result.Images.Add(ImageKeyString.SearchReportDisabled, ImageCenter.Images[ImageKeyString.SearchReportDisabled]);
        //    result.Images.Add(ImageKeyString.SecondPageDisabled, ImageCenter.Images[ImageKeyString.SecondPageDisabled]);
        //    result.Images.Add(ImageKeyString.ThirdPageDisabled, ImageCenter.Images[ImageKeyString.ThirdPageDisabled]);
        //    //用于突出显示申康接口的信息
        //    //result.Images.Add(ImageKeyString.ImageMiddling, ImageCenter.Images[ImageKeyString.ImageMiddling]);
        //    //result.Images.Add(ImageKeyString.ImageCritical, ImageCenter.Images[ImageKeyString.ImageCritical]);
        //    //result.Images.Add(ImageKeyString.ImageUrgence, ImageCenter.Images[ImageKeyString.ImageUrgence]);
        //    return result;
        //}
        #endregion

        #region 历史病人查询用

        /// <summary>
        /// 取得历史病人查询中的按钮图标集
        /// </summary>
        /// <returns></returns>
        public static ImageList GetImageBarSearch()
        {
            ImageList result = new ImageList();
            result.ColorDepth = ColorDepth.Depth24Bit;
            result.ImageSize = new Size(16, 16);
            result.TransparentColor = Color.Magenta;
            result.Images.Add(ImageKeyString.ImageOrderEdit, ImageCenter.Images[ImageKeyString.ImageOrderEdit]);
            result.Images.Add(ImageKeyString.ImageOrderEditDisabled, ImageCenter.Images[ImageKeyString.ImageOrderEditDisabled]);
            result.Images.Add(ImageKeyString.ImageReportQuery, ImageCenter.Images[ImageKeyString.ImageReportQuery]);
            result.Images.Add(ImageKeyString.ImageReportQueryDisabled, ImageCenter.Images[ImageKeyString.ImageReportQueryDisabled]);
            result.Images.Add(ImageKeyString.ImageEdit, ImageCenter.Images[ImageKeyString.ImageEdit]);
            result.Images.Add(ImageKeyString.ImageEditDisabled, ImageCenter.Images[ImageKeyString.ImageEditDisabled]);
            result.Images.Add(ImageKeyString.ImageDetails, ImageCenter.Images[ImageKeyString.ImageDetails]);
            result.Images.Add(ImageKeyString.ImageDetailsDisabled, ImageCenter.Images[ImageKeyString.ImageDetailsDisabled]);
            return result;
        }

        /// <summary>
        /// 取得历史病历查询中病人性别的图标集
        /// </summary>
        /// <returns></returns>
        public static ImageList GetImageListBrxb()
        {
            ImageList result = new ImageList();
            result.ColorDepth = ColorDepth.Depth24Bit;
            result.ImageSize = new Size(16, 16);
            result.TransparentColor = Color.Magenta;
            result.Images.Add(ImageKeyString.ImageMale, ImageCenter.Images[ImageKeyString.ImageMale]);
            result.Images.Add(ImageKeyString.ImageFemale, ImageCenter.Images[ImageKeyString.ImageFemale]);
            return result;
        }

        #endregion
    }
}
