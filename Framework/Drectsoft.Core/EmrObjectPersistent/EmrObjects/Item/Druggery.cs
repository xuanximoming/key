using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;
using DrectSoft.Core;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 一般的药品项目
   /// </summary>
   public class Druggery : ItemBase
   {
      #region properties
      /// <summary>
      /// 主键值
      /// </summary>
      public override string KeyValue
      {
         get { return ProductSerialNo.ToString(CultureInfo.CurrentCulture); }
      }

      /// <summary>
      /// 产地序号
      /// </summary>
      public decimal ProductSerialNo
      {
         get { return _productSerialNo; }
         set { _productSerialNo = value; }
      }
      private decimal _productSerialNo;

      /// <summary>
      /// 规格序号
      /// </summary>
      public decimal SpecSerialNo
      {
         get { return _specSerialNo; }
         set { _specSerialNo = value; }
      }
      private decimal _specSerialNo;

      /// <summary>
      /// 临床序号
      /// </summary>
      public decimal ClinicSerialNo
      {
         get { return _clinicSerialNo; }
         set { _clinicSerialNo = value; }
      }
      private decimal _clinicSerialNo;

      /// <summary>
      /// 价格（带单位）
      /// </summary>
      public override string Price
      {
         get
         {
            return String.Format(CultureInfo.CurrentCulture
               , "{0:F}元/{1}", RetailPrice, DepotUnit.Name);
         }
      }

      /// <summary>
      /// 药品形态(剂型)
      /// </summary>
      public DruggeryForm Form
      {
         get { return _form; }
         set { _form = value; }
      }
      private DruggeryForm _form;

      /// <summary>
      /// 药品分类
      /// </summary>
      public DruggeryCatalog Catalog
      {
         get { return _catalog; }
         set { _catalog = value; }
      }
      private DruggeryCatalog _catalog;

      /// <summary>
      /// 规格单位
      /// </summary>
      public ItemUnit SpecUnit
      {
         get { return _specUnit; }
         set { _specUnit = value; }
      }
      private ItemUnit _specUnit;

      /// <summary>
      /// 药库单位
      /// </summary>
      public ItemUnit DepotUnit
      {
         get { return _depotUnit; }
         set { _depotUnit = value; }
      }
      private ItemUnit _depotUnit;

      /// <summary>
      /// 门诊单位
      /// </summary>
      public ItemUnit PoliclinicUnit
      {
         get { return _policlinicUnit; }
         set { _policlinicUnit = value; }
      }
      private ItemUnit _policlinicUnit;

      /// <summary>
      /// 住院单位
      /// </summary>
      public ItemUnit WardUnit
      {
         get { return _wardUnit; }
         set { _wardUnit = value; }
      }
      private ItemUnit _wardUnit;

      /// <summary>
      /// 儿科单位
      /// </summary>
      public ItemUnit PaediatricsUnit
      {
         get { return _paediatricsUnit; }
         set { _paediatricsUnit = value; }
      }
      private ItemUnit _paediatricsUnit;

      /// <summary>
      /// 药品性质
      /// </summary>
      public DruggeryAttributeFlag Attributes
      {
         get { return _attributes; }
         set { _attributes = value; }
      }
      private DruggeryAttributeFlag _attributes;

      /// <summary>
      /// 特殊药品标志
      /// </summary>
      public DruggeryKind SpecialFlag
      {
         get { return _specialFlag; }
         set { _specialFlag = value; }
      }
      private DruggeryKind _specialFlag;

      /// <summary>
      /// 药品来源
      /// </summary>
      public DruggerySource Source
      {
         get { return _source; }
         set { _source = value; }
      }
      private DruggerySource _source;

      /// <summary>
      /// 厂家名称
      /// </summary>
      public string Manufacturer
      {
         get { return _manufacturer; }
         set { _manufacturer = value; }
      }
      private string _manufacturer;

      /// <summary>
      /// 标记主键是否已初始化
      /// </summary>
      public override bool KeyInitialized
      {
         get
         {
            if (_productSerialNo <= 0)
               return false;
            else
               return true;
         }
      }

      /// <summary>
      /// 剂型对应的用法集合
      /// </summary>
      public string DefaultUsageRange
      {
         get { return _defaultUsageRange; }
      }
      private string _defaultUsageRange;

      /// <summary>
      /// 剂型对应的用法集合
      /// </summary>
      public string DefaultUsageRangeCondition
      {
         get
         {
            if (String.IsNullOrEmpty(DefaultUsageRange))
               return "";
            else
               return String.Format(CultureInfo.CurrentCulture
                  , "UseageID in ({0})", DefaultUsageRange);
         }
      }

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { return GetQuerySentenceFromXml("SelectDruggeryBook"); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
          get { return "PlaceID = " + ProductSerialNo.ToString(CultureInfo.CurrentCulture); }
      }
      #endregion

      #region ctor
      /// <summary>
      /// 
      /// </summary>
      public Druggery()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="productSerialNo"></param>
      public Druggery(decimal productSerialNo)
         : base()
      {
         _productSerialNo = productSerialNo;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="productSerialNo"></param>
      /// <param name="code"></param>
      /// <param name="name"></param>
      public Druggery(decimal productSerialNo, string code, string name)
         : base(code, name)
      {
         _productSerialNo = productSerialNo;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public Druggery(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion

      #region public methods

      /// <summary>
      /// 初始化药品默认用法集合(根据剂型)
      /// </summary>
      /// <param name="sqlExecutor"></param>
      public void InitializeDefaultUsageRange(IDataAccess sqlExecutor)
      {
         if (sqlExecutor == null)
            return;

         DataRow dr = sqlExecutor.GetRecord(PersistentObjectFactory.GetQuerySentenceByName("SelectDefUsageRangeByDruggeryForm")
            , String.Format(CultureInfo.CurrentCulture, " DosageID = '{0}'", Form.Code)
            , true);
         if (dr != null)
             _defaultUsageRange = dr["UseageID"].ToString();
         else
            _defaultUsageRange = "";
      }

      /// <summary>
      /// 取药品默认的用法、频次、数量及单位
      /// </summary>
      /// <param name="systemFlag">系统标志(区分门诊还是住院)</param>
      /// <param name="sqlExecutor"></param>
      /// <returns></returns>
      public DruggeryOrderContent GetDefaultUsageFrequency(SystemApplyRange systemFlag, IDataAccess sqlExecutor)
      {
         if (sqlExecutor == null)
            throw new ArgumentNullException(MessageStringManager.GetString("ParameterIsNull", "SQLHelper"));

         DataRow dr = sqlExecutor.GetRecord(PersistentObjectFactory.GetQuerySentenceByName("SelectDefUsageOfDruggery")
            , String.Format(CultureInfo.CurrentCulture, " PlaceID = {0} and Mark in (1400, {1:D})", ProductSerialNo, systemFlag)
            , true);

         DruggeryOrderContent result = new DruggeryOrderContent();

         if (dr != null)
            result.Initialize(dr);

         return result;
      }
      
      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         ReInitializeProperties();

         if (Form != null)
            Form.ReInitializeAllProperties();
         if (Catalog != null)
            Catalog.ReInitializeAllProperties();
         if (Source != null)
            Source.ReInitializeAllProperties();
      }
      #endregion

   }
}
