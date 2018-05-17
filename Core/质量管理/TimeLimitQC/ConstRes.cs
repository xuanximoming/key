using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 资源字符串常量
    /// </summary>
    public class ConstRes
    {
        public static readonly string[] transmode = new string[]{
         "zh_en/Chinese-simp to English",
         "zt_en/Chinese-trad to English",
         "en_zh/English to Chinese-simp",
         "en_zt/English to Chinese-trad",
         "en_nl/English to Dutch",
         "en_fr/English to French",
         "en_de/English to German",
         "en_el/English to Greek",
         "en_it/English to Italian",
         "en_ja/English to Japanese",
         "en_ko/English to Korean",
         "en_pt/English to Portuguese",
         "en_ru/English to Russian",
         "en_es/English to Spanish",
         "nl_en/Dutch to English",
         "nl_fr/Dutch to French",
         "fr_en/French to English",
         "fr_de/French to German",
         "fr_el/French to Greek",
         "fr_it/French to Italian",
         "fr_pt/French to Portuguese",
         "fr_nl/French to Dutch",
         "fr_es/French to Spanish",
         "de_en/German to English",
         "de_fr/German to French",
         "el_en/Greek to English",
         "el_fr/Greek to French",
         "it_en/Italian to English",
         "it_fr/Italian to French",
         "ja_en/Japanese to English",
         "ko_en/Korean to English",
         "pt_en/Portuguese to English",
         "pt_fr/Portuguese to French",
         "ru_en/Russian to English",
         "es_en/Spanish to English",
         "es_fr/Spanish to French",
        };

        public const string cstOrderType = "DrectSoft.Common.Eop.Order,DrectSoft.Core";
        public const string cstEmrModelType = "DrectSoft.Core.Model.EmrModel,DrectSoft.Core.ModelEntity";
        public const string cstPatientType = "DrectSoft.Common.Eop.Inpatient,DrectSoft.Core";

        #region showlist bl_zlkzgzk

        public const string cstTableNameQcRule = "时限规则";
        public const string cstSqlSelectQcRule = "select RuleCode, Description from QCRule";
        public const string cstShowlistColsQcRule = "RuleCode//规则代码//80///Description//规则描述//200";
        #endregion

        public const string cstEditStatusBrowse = "编辑状态：浏览规则";
        public const string cstEditStatusNew = "编辑状态：新增规则";
        public const string cstEditStatusModify = "编辑状态：修改规则";

        public const string cstFieldCaptionId = "代码";
        public const string cstFieldCaptionDescript = "描述";

        public const string cstSaveCheckNeedQcRuleId = "必须输入规则代码";
        public const string cstSaveCheckNeedQcRuleName = "必须输入规则名称";
        public const string cstSaveCheckExistSameRuleId = "已经存在相同代码的规则";
        public const string cstSaveCheckExistSameCondId = "已经存在相同代码的规则条件";
        public const string cstSaveCheckExistSameResultId = "已经存在相同代码的规则结果";
        public const string cstSaveCheckExistSameId = "已经存在相同代码";
        public const string cstSaveSuccess = "保存成功";
        public const string cstLoadQcRulesFail = "读取规则数据失败";
        public const string cstConfirmDelete = "您确定要删除吗？";

        #region DataViewTimeLimit

        public const string cstFieldDoctorId = "DocotorID";
        public const string cstFieldCaptionDoctorId = "住院医生";
        public const string cstFieldFirstPage = "NoOfInpat";
        public const string cstFieldCaptionFirstPage = "首页序号";
        public const string cstFieldPatNameHospNo = "NoOfRecord";
        public const string cstFieldCaptionPatNameHospNo = "患者姓名";
        public const string cstFieldInfo = "tipwarninfo";
        public const string cstFieldCaptionInfo = "提示或警告信息";
        public const string cstFieldTipStatus = "tipstatus";
        public const string cstFieldCaptionTipStatus = "提示状态";
        public const string cstFieldTimeLimit = "TimeLimit";
        public const string cstFieldCaptionTimeLimit = "时限";

        public const string cstFieldCondTime = "ConditionTime";
        public const string cstFieldResultTime = "ResultTime";
        public const string cstFieldPatName = "Name";
        public const string cstFieldPatHospNo = "NoOfRecord";
        public const string cstFieldTipStatus2 = "FoulState";
        public const string cstFieldDoctorId2 = "DocotorID";
        public const string cstFieldDoctorName = "UserName";
        public const string cstFieldTipInfo = "Reminder";
        public const string cstFieldWarnInfo = "FoulMessage";

        public const string cstOverTime = "超出";
        public const string cstOnTime = "剩余";

        public const string cstDayChn = "天";
        public const string cstHourChn = "时";
        public const string cstMinuteChn = "分";

        #endregion

        public const string cstAndOper = " [and] ";

        public const int cstCondTypeNo = 3900;
        public const int cstResultTypeNo = 4000;
        public const int cstDoctorLevelNo = 1999;
    }
}
