using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace DrectSoft.Core.NursingDocuments.UserControls
{
    class ThreeMeasureTableConfig
    {
        /// <summary>
        /// 决定小方格曲线以上有哪些行
        /// </summary>
        /// <returns></returns>
        public DataTable InitTableHead()
        {
            DataTable dataTableTableBaseLine = new DataTable();
            dataTableTableBaseLine.Columns.Add("RowName");

            DataRow dr1 = dataTableTableBaseLine.NewRow();
            dr1[0] = "日   期";
            dataTableTableBaseLine.Rows.Add(dr1);

            DataRow dr2 = dataTableTableBaseLine.NewRow();
            dr2[0] = "住院天数";
            dataTableTableBaseLine.Rows.Add(dr2);

            DataRow dr3 = dataTableTableBaseLine.NewRow();
            dr3[0] = "手术后天数";
            dataTableTableBaseLine.Rows.Add(dr3);

            DataRow dr4 = dataTableTableBaseLine.NewRow();
            dr4[0] = "时   间";
            dataTableTableBaseLine.Rows.Add(dr4);

            return dataTableTableBaseLine;
        }

        /// <summary>
        /// 初始化 呼吸，脉搏， 体温 等生命体征的设置数据
        /// </summary>
        /// <returns></returns>
        public ArrayList InitFirstColumnHasSubColumn()
        {
            ArrayList arrayListVitalSigns = new ArrayList();

            VitalSigns vs3 = new VitalSigns();
            vs3.Name = UCThreeMeasureTable.VitalSignsType.TiWen.ToString();//体温
            vs3.CellValue = 0.2f;
            vs3.MaxValue = 43;//改为43度 ywk
            vs3.Unit = "摄氏度";
            arrayListVitalSigns.Add(vs3);

            VitalSigns vs2 = new VitalSigns();
            vs2.Name = UCThreeMeasureTable.VitalSignsType.MaiBo.ToString();//脉搏
            vs2.CellValue = 4f;
            vs2.MaxValue = 200;
            vs2.Unit = "次/分";
            arrayListVitalSigns.Add(vs2);

            VitalSigns vs1 = new VitalSigns();
            vs1.Name = UCThreeMeasureTable.VitalSignsType.HuXi.ToString();//呼吸
            vs1.CellValue = 2f;
            vs1.MaxValue = 90;
            vs1.Unit = "次/分";
            arrayListVitalSigns.Add(vs1);


            return arrayListVitalSigns;
        }

        /// <summary>
        /// 初始化 血压，总入量，总出量，引流量，大便次数，身高，体重，过敏药物的设置数据
        /// </summary>
        /// <param name="dayTimePointXuYa"></param>
        /// <returns></returns>
        public ArrayList InitVitalSignsOtherAtTableBottom(int dayTimePointXuYa)
        {
            ArrayList arrayListVitalSignsOther = new ArrayList();

            VitalSignsOther vso = new VitalSignsOther();
            vso.Name = UCThreeMeasureTable.VitalSignsType.XueYa.ToString();//血压
            vso.TimePointOfDay = dayTimePointXuYa;//设置每天血压的时间点数
            vso.Unit = "mmHg";
            arrayListVitalSignsOther.Add(vso);

            vso = new VitalSignsOther();
            vso.Name = UCThreeMeasureTable.VitalSignsType.PainInfo.ToString();//其他1 现在改为疼痛
            vso.TimePointOfDay = 6;//疼痛的时间点是6个 edit by ywk 
            vso.Unit = "";
            arrayListVitalSignsOther.Add(vso);

            vso = new VitalSignsOther();
            vso.Name = UCThreeMeasureTable.VitalSignsType.ZongRuLiang.ToString();//总入量
            vso.TimePointOfDay = 1;
            vso.Unit = "ml";
            arrayListVitalSignsOther.Add(vso);

            vso = new VitalSignsOther();
            vso.Name = UCThreeMeasureTable.VitalSignsType.ZongChuLiang.ToString();//总出量
            vso.TimePointOfDay = 1;
            vso.Unit = "ml";
            arrayListVitalSignsOther.Add(vso);

            vso = new VitalSignsOther();
            vso.Name = UCThreeMeasureTable.VitalSignsType.YinLiuLiang.ToString();//引流量
            vso.TimePointOfDay = 1;
            vso.Unit = "ml";
            arrayListVitalSignsOther.Add(vso);

            vso = new VitalSignsOther();
            vso.Name = UCThreeMeasureTable.VitalSignsType.DaBianCiShu.ToString();//大便次数
            vso.TimePointOfDay = 1;
            vso.Unit = "";
            arrayListVitalSignsOther.Add(vso);

            vso = new VitalSignsOther();
            vso.Name = UCThreeMeasureTable.VitalSignsType.ShenGao.ToString();//身高
            vso.TimePointOfDay = 1;
            vso.Unit = "cm";
            arrayListVitalSignsOther.Add(vso);

            vso = new VitalSignsOther();
            vso.Name = UCThreeMeasureTable.VitalSignsType.TiZhong.ToString();//体重
            vso.TimePointOfDay = 1;
            vso.Unit = "kg";
            arrayListVitalSignsOther.Add(vso);

            //add by wyt
            vso = new VitalSignsOther();
            vso.Name = UCThreeMeasureTable.VitalSignsType.param1.ToString();//自定义参数1
            vso.TimePointOfDay = 1;
            vso.Unit = "";
            arrayListVitalSignsOther.Add(vso);

            vso = new VitalSignsOther();
            vso.Name = UCThreeMeasureTable.VitalSignsType.param2.ToString();//自定义参数2
            vso.TimePointOfDay = 1;
            vso.Unit = "";
            arrayListVitalSignsOther.Add(vso);
            //---

            vso = new VitalSignsOther();
            vso.Name = UCThreeMeasureTable.VitalSignsType.GuoMingYaoWu.ToString();//过敏药物
            vso.TimePointOfDay = 1;
            vso.Unit = "";
            arrayListVitalSignsOther.Add(vso);


            //vso = new VitalSignsOther();
            //vso.Name = UCThreeMeasureTable.VitalSignsType.TeShuZhiLiao.ToString();//特殊治疗
            //vso.TimePointOfDay = 1;
            //vso.Unit = "";
            //arrayListVitalSignsOther.Add(vso);

            //vso = new VitalSignsOther();
            //vso.Name = UCThreeMeasureTable.VitalSignsType.Other2.ToString();//其他2 留者占空
            //vso.TimePointOfDay = 1;
            //vso.Unit = "";
            //arrayListVitalSignsOther.Add(vso);
            return arrayListVitalSignsOther;
        }


        /// <summary>
        /// 默认情况下时间点是 0,1,2,3,4,5
        /// (wyt)        
        /// </summary>
        /// <returns></returns>
        //public DataTable GetIndxDefault()
        //{
        //    DataTable dataTableIndx = new DataTable();
        //    dataTableIndx.Columns.Add("Indx");
        //    dataTableIndx.Columns.Add("Color");

        //    DataRow dataRowIndx = dataTableIndx.NewRow();
        //    //dataRowTimePoint1[0] = 2;
        //    dataRowIndx[0] = 0;
        //    dataRowIndx[1] = "Black";
        //    dataTableIndx.Rows.Add(dataRowIndx);

        //    dataRowIndx = dataTableIndx.NewRow();
        //    //dataRowTimePoint1[0] = 6;
        //    dataRowIndx[0] = 1;
        //    dataRowIndx[1] = "Black";
        //    dataTableIndx.Rows.Add(dataRowIndx);

        //    dataRowIndx = dataTableIndx.NewRow();
        //    //dataRowTimePoint1[0] = 10;
        //    dataRowIndx[0] = 2;
        //    dataRowIndx[1] = "Black";
        //    dataTableIndx.Rows.Add(dataRowIndx);

        //    dataRowIndx = dataTableIndx.NewRow();
        //    //dataRowTimePoint1[0] = 14;
        //    dataRowIndx[0] = 3;
        //    dataRowIndx[1] = "Black";
        //    dataTableIndx.Rows.Add(dataRowIndx);

        //    dataRowIndx = dataTableIndx.NewRow();
        //    //dataRowTimePoint1[0] = 18;
        //    dataRowIndx[0] = 4;
        //    dataRowIndx[1] = "Black";
        //    dataTableIndx.Rows.Add(dataRowIndx);

        //    dataRowIndx = dataTableIndx.NewRow();
        //    //dataRowTimePoint1[0] = 22;
        //    dataRowIndx[0] = 5;
        //    dataRowIndx[1] = "Black";
        //    dataTableIndx.Rows.Add(dataRowIndx);

        //    return dataTableIndx;
        //}

        /// <summary>
        /// 默认情况下时间点是 2，6，10，14，18，22
        /// 现在改为           3, 7，11, 15 ,19，23（ywk）
        /// </summary>
        /// <returns></returns>
        public DataTable GetTimePointDefault()
        {
            DataTable dataTableTimePoint = new DataTable();
            dataTableTimePoint.Columns.Add("TimePoint");
            dataTableTimePoint.Columns.Add("Color");

            DataRow dataRowTimePoint1 = dataTableTimePoint.NewRow();
            //dataRowTimePoint1[0] = 2;
            dataRowTimePoint1[0] = 3;
            dataRowTimePoint1[1] = "Black";
            dataTableTimePoint.Rows.Add(dataRowTimePoint1);

            dataRowTimePoint1 = dataTableTimePoint.NewRow();
            //dataRowTimePoint1[0] = 6;
            dataRowTimePoint1[0] = 7;
            dataRowTimePoint1[1] = "Black";
            dataTableTimePoint.Rows.Add(dataRowTimePoint1);

            dataRowTimePoint1 = dataTableTimePoint.NewRow();
            //dataRowTimePoint1[0] = 10;
            dataRowTimePoint1[0] = 11;
            dataRowTimePoint1[1] = "Black";
            dataTableTimePoint.Rows.Add(dataRowTimePoint1);

            dataRowTimePoint1 = dataTableTimePoint.NewRow();
            //dataRowTimePoint1[0] = 14;
            dataRowTimePoint1[0] = 15;
            dataRowTimePoint1[1] = "Black";
            dataTableTimePoint.Rows.Add(dataRowTimePoint1);

            dataRowTimePoint1 = dataTableTimePoint.NewRow();
            //dataRowTimePoint1[0] = 18;
            dataRowTimePoint1[0] = 19;
            dataRowTimePoint1[1] = "Black";
            dataTableTimePoint.Rows.Add(dataRowTimePoint1);

            dataRowTimePoint1 = dataTableTimePoint.NewRow();
            //dataRowTimePoint1[0] = 22;
            dataRowTimePoint1[0] = 23;
            dataRowTimePoint1[1] = "Black";
            dataTableTimePoint.Rows.Add(dataRowTimePoint1);

            return dataTableTimePoint;
        }
    }
}
