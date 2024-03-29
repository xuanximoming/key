using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;


namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 地址信息
   /// </summary>
   public class Address : EPBaseObject
   {
      #region properties
      /// <summary>
      /// 国籍
      /// </summary>
      public CommonBaseCode Country
      {
         get { return _country; }
         set { _country = value; }
      }
      private CommonBaseCode _country;

      /// <summary>
      /// 省（或直辖市） 
      /// </summary>
      public ZoneProvince Province
      {
         get { return _province; }
         set { _province = value; }
      }
      private ZoneProvince _province;

      /// <summary>
      /// 市（或区县）
      /// </summary>
      public ZoneCountry City
      {
         get { return _city; }
         set { _city = value; }
      }
      private ZoneCountry _city;

      /// <summary>
      /// 地址()
      /// </summary>
      public string address
      {
         get { return _address; }
         set { _address = value; }
      }
      private string _address;

      /// <summary>
      /// 完整的地址信息（包括省市、区县、街道等）
      /// </summary>
      public string FullAddress
      {
         get
         {
            // 按从下到上的顺序插入，以使不同情况下信息看起来更合理
            StringBuilder fullAddress = new StringBuilder();
            if (!String.IsNullOrEmpty(address))
               fullAddress.Append(address);

            string cityName = String.Empty;
            if ((City != null) && !String.IsNullOrEmpty(City.Code))
            {
               if (String.IsNullOrEmpty(City.Name))
                  City.ReInitializeProperties();
               cityName = City.Name;
               fullAddress.Insert(0, cityName);
            }

            if ((Province != null) && !String.IsNullOrEmpty(Province.Code))
            {
               if (String.IsNullOrEmpty(Province.Name))
                  Province.ReInitializeProperties();

               // 若有地址信息，则在有区县数据的情况下，插入省市才合理
               // 有些情况下，区县的名称里包含了省市的名称。遇到这种情况就不用再插入省市名称了
               if ((!String.IsNullOrEmpty(cityName) && !cityName.StartsWith(Province.Name))
                  || (String.IsNullOrEmpty(address) && String.IsNullOrEmpty(cityName)))
               {
                  fullAddress.Insert(0, Province.Name);
               }
            }
            return fullAddress.ToString();
         }
      }

      /// <summary>
      /// 电话号码
      /// </summary>
      public string PhoneNo
      {
         get { return _phoneNo; }
         set { _phoneNo = value; }
      }
      private string _phoneNo;

      /// <summary>
      /// 邮政编码
      /// </summary>
      public string Postalcode
      {
         get { return _postalcode; }
         set { _postalcode = value; }
      }
      private string _postalcode;

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { throw new NotImplementedException(); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
         get { throw new NotImplementedException(); }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public Address()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      public Address(string code)
         : base(code)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      /// <param name="name"></param>
      public Address(string code, string name)
         : base(code, name)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public Address(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion

      #region public methods

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         if (Country != null)
            Country.ReInitializeAllProperties();
         if (Province != null)
            Province.ReInitializeAllProperties();
         if (City != null)
            City.ReInitializeAllProperties();
      }
      #endregion
   }
}
