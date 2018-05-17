using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.DSSqlHelper;

namespace DrectSoft.Emr.QCTimeLimit.QCEntity
{
    /// <summary>
    /// 质量控制规则分类库
    /// RuleCategory表对应的实体类
    /// </summary>
    public class RuleCategory
    {
        #region Property
        /// <summary>
        /// 规则分类代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 规则分类描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 拼音
        /// </summary>
        public string PY { get; set; }

        /// <summary>
        /// 五笔
        /// </summary>
        public string WB { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
        #endregion

        public const string cstFieldCaptionId = "代码";
        public const string cstFieldCaptionDescript = "描述";

        /// <summary>
        /// 捞取所有的分类
        /// </summary>
        const string c_SqlQCCategory = "select * from RuleCategory order by code desc";

        #region 获得所有的病历质量分类库
        /// <summary>
        /// 获得所有的病历质量分类库 DataTable -> Dictionary<string, RuleCategory>
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, RuleCategory> GetAllRuleCategory()
        {
            try
            {
                DataTable dataTableCategory = GetAllRuleCategorys();
                return GetAllRuleCategory(dataTableCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得病历质量分类库 DataTable -> Dictionary<string, RuleCategory>
        /// </summary>
        /// <param name="dataTableCategory"></param>
        /// <returns></returns>
        public static Dictionary<string, RuleCategory> GetAllRuleCategory(DataTable dataTableCategory)
        {
            try
            {
                Dictionary<string, RuleCategory> dictCategory = new Dictionary<string, RuleCategory>();
                foreach (DataRow dr in dataTableCategory.Rows)
                {
                    RuleCategory category = new RuleCategory();

                    #region RuleCategory实例赋值
                    category.Code = dr["CODE"].ToString();
                    category.Description = dr["DESCRIPTION"].ToString();
                    category.PY = dr["PY"].ToString();
                    category.WB = dr["WB"].ToString();
                    category.Memo = dr["MEMO"].ToString();
                    #endregion

                    dictCategory.Add(category.Code, category);
                }
                return dictCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有时限规则分类
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllRuleCategorys()
        {
            try
            {
                DataTable dtRuleCategory = DS_SqlHelper.ExecuteDataTable(c_SqlQCCategory, CommandType.Text);
                return dtRuleCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
