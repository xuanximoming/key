using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace DrectSoft.Core.CommonTableConfig
{
    public class CommonTabHelper
    {
        /// <summary>
        /// 获取全部数据元数据类型
        /// </summary>
        /// <returns></returns>
        public static List<BaseDictory> GetAllDataElementType()
        {
            try
            {
                List<BaseDictory> baseDictoryList = new List<BaseDictory>(){
                                new BaseDictory() { Id="S1",Name="S1(普通字符)" },
                                new BaseDictory() { Id="S2",Name="S2(单选)" },
                                new BaseDictory() { Id="S3",Name="S3(单选)" },
                                 new BaseDictory() { Id="S4",Name="S4(大文本字符)" },
                                new BaseDictory() { Id="S9",Name="S9(多选型)" },
                                new BaseDictory() { Id="L",Name="L(布尔型)" },
                                new BaseDictory() { Id="N",Name="N(数值型)" },
                                new BaseDictory() { Id="D",Name="D(日期型)" },
                                new BaseDictory() { Id="DT",Name="DT(日期时间型)" },
                                new BaseDictory() { Id="T",Name="T(时间型)" }
                                //,
                                //new BaseDictory() { Id="BY",Name="BY(二进制型)" }
                               
            };
                return baseDictoryList;

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        /// <summary>
        /// 获取数据元所属的大类
        /// </summary>
        /// <returns></returns>
        public static List<BaseDictory> GetAllDataElemnetClass()
        {
            try
            {
                List<BaseDictory> baseDictoryList = new List<BaseDictory>(){
                                new BaseDictory() { Id="01",Name="标识" },
                                new BaseDictory() { Id="02",Name="人口学健康特点" },
                                new BaseDictory() { Id="03",Name="健康史" },
                                new BaseDictory() { Id="04",Name="健康因素" },
                                new BaseDictory() { Id="05",Name="主诉与健康史" },
                                new BaseDictory() { Id="06",Name="体格检查" },
                                new BaseDictory() { Id="07",Name="临床辅助检查" },
                                new BaseDictory() { Id="08",Name="实验室检查" },
                                new BaseDictory() { Id="09",Name="医学诊断" },
                                new BaseDictory() { Id="10",Name="医学评估" },
                                new BaseDictory() { Id="11",Name="计划与干预" },
                                new BaseDictory() { Id="12",Name="卫生费用" },
                                new BaseDictory() { Id="13",Name="卫生机构" },
                                new BaseDictory() { Id="14",Name="卫生人员" },
                                new BaseDictory() { Id="15",Name="药品、设备、材料" },
                                new BaseDictory() { Id ="16", Name= "卫生管理"},
                                new BaseDictory() { Id="99",Name="其他" }
            };
                return baseDictoryList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private static List<DataElementEntity> dataElementListAll;

        public static List<DataElementEntity> DataElementListAll
        {
            get { return dataElementListAll; }
            set { dataElementListAll = value; }
        }


        /// <summary>
        /// 对象克隆方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Clone<T>(T obj)
        {
            try
            {
                T ret = default(T);
                if (obj != null)
                {
                    XmlSerializer doner = new XmlSerializer(typeof(T));
                    MemoryStream st = new MemoryStream();
                    doner.Serialize(st, obj);
                    st.Seek(0, SeekOrigin.Begin);
                    ret = (T)doner.Deserialize(st);
                }
                return ret;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }


}
