using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Globalization;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 频次
   /// </summary>
   public class OrderFrequency : EPBaseObject
   {
      #region properties
      /// <summary>
      /// 每个周期最多执行的次数,0表示无限制
      /// </summary>
      public int ExecuteTimesPerPeriod
      {
         get { return _executeTimesPerPeriod; }
         set { _executeTimesPerPeriod = value; }
      }
      private int _executeTimesPerPeriod;

      /// <summary>
      /// 执行周期(单位为周时,执行周期表示1周中允许执行的天数)
      /// </summary>
      public int Period
      {
         get { return _period; }
         set { _period = value; }
      }
      private int _period;

      /// <summary>
      /// 执行周期单位
      /// </summary>
      public OrderExecPeriodUnitKind PeriodUnitFlag
      {
         get
         {
            //hrr:目前这里出现异常情况，_periodUnitFlag会莫名其妙的变成None,所以需要手动做检查
            CheckPeriodUnitFlag(_periodUnitFlag);
            return _periodUnitFlag;
         }
         set { _periodUnitFlag = value; }
      }
      private OrderExecPeriodUnitKind _periodUnitFlag;

      private void CheckPeriodUnitFlag(OrderExecPeriodUnitKind periodUnitFlag)
      {
         if (periodUnitFlag != OrderExecPeriodUnitKind.None)
            return;
         else
         {
            //检查是否与当前的pcdm一致
            if (!string.IsNullOrEmpty(this.Code))
            {
               OrderFrequency temp = new OrderFrequency(this.Code);
               temp.ReInitializeAllProperties();
               _periodUnitFlag = temp._periodUnitFlag;
               _weekDays = temp._weekDays;
               _period = temp._period;
               _executeTime = temp._executeTime;
               _executeTimes = temp._executeTimes;
               _executeTimesPerPeriod = temp._executeTimesPerPeriod;
               _orderManagerFlag = temp._orderManagerFlag;
            }
         }
      }

      /// <summary>
      /// 周标志(指定一周中的哪几天能执行)
      /// </summary>
      public string WeekDays
      {
         get
         {
            if (_weekDays == null) // 生成默认的时间设置
            {
               _weekDays = string.Empty;//如果为空则为赋值为空字符串
               // 如果预设的执行日期不包括当天(或明天,根据设置来),则将预设日期平行移动,自动调整到当天(或明天)
               
               //hrr：这里有bug，因为DayOfWeek不会正好对应着1-7，所以需要手动处理一次
               int aimday = 0;

               ////Sunday = 0,      Monday = 1,      Tuesday = 2,      Wednesday = 3,      Thursday = 4,      Friday = 5,      Saturday = 6,
               ////HIS中周一到周日表示为1-7
               if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                  aimday = 7;
               else
                  aimday = Convert.ToInt32(DateTime.Now.DayOfWeek, CultureInfo.CurrentCulture);
               // 现在默认为从第二天开始
               aimday += 1;
               string days = _weekDays.ToString();

               if (!days.Contains(aimday.ToString(CultureInfo.CurrentCulture)))
               {
                  int tempday = 0; // 保留aimday之前的那天
                  foreach (char c in days)
                  {
                     if ((c < 49) || (c > 55))//49:1 55:7   也就意味49-55是合法的
                        continue;
                     if ((c - 48) > aimday)
                        break;
                     tempday = c - 49;
                  }
                  StringBuilder newDays = new StringBuilder();
                  StringBuilder newDays2 = new StringBuilder();
                  // 平行移动执行日期，距离为aimday 和 beforeday之间的差额
                  int length = aimday - tempday;
                  foreach (char dw in days)
                  {
                     if ((dw < 49) || (dw > 55))
                        continue;
                     tempday = dw - 48 + length;
                     if (tempday > 7)
                     {
                        tempday -= 7;
                        newDays2.Append(tempday);
                     }
                     else
                        newDays.Append(tempday);
                  }
                  _weekDays = newDays2.ToString() + newDays.ToString();
               }
            }
            return _weekDays;
         }
         set { _weekDays = value; }
      }
      private string _weekDays;

      /// <summary>
      /// 执行时间(医嘱每天执行的时间点,24小时制,精确到小时,用'0'补足2位,多个时以','隔开)
      /// </summary>
      public string ExecuteTime
      {
         get { return _executeTime; }
         set
         {
            _executeTime = value;

            ExecuteTimes.Clear();
            try
            {
               if (!String.IsNullOrEmpty(_executeTime))
               {
                  string[] hours = _executeTime.Split(new char[] { ',' });
                  foreach (string hour in hours)
                  {
                     if (!String.IsNullOrEmpty(hour))
                        ExecuteTimes.Add(Convert.ToInt32(hour));
                  }
                  ExecuteTimes.Sort();
               }
            }
            catch (InvalidCastException castEx)
            {
               throw new InvalidCastException(string.Format("规则：执行时间(医嘱每天执行的时间点,24小时制,精确到小时,用'0'补足2位,多个时以','隔开){0}数据不符合规则导致转换至Int32类型错误，原数据：{1}"
                  ,Environment.NewLine 
                  ,_executeTime)
                  , castEx);
            }
            catch (Exception ex)
            {
               throw new InvalidOperationException(string.Format("规则：执行时间(医嘱每天执行的时间点,24小时制,精确到小时,用'0'补足2位,多个时以','隔开){0}数据不符合规则，转换失败，{0}={1}", "ExecuteTime", _executeTime), ex);
            }
         }
      }
      private string _executeTime;

      /// <summary>
      /// 医嘱每天执行时间点的数值
      /// </summary>
      public List<int> ExecuteTimes
      {
         get
         {
            if (_executeTimes == null)
               _executeTimes = new List<int>();
            return _executeTimes;
         }
      }
      private List<int> _executeTimes;

      ///// <summary>
      ///// 频次在指定范围内的执行时间点（根据外部传入参数计算获得）
      ///// </summary>
      //public List<DateTime> ExecutePoints
      //{
      //   get
      //   {
      //      if (_executePoints == null)
      //         _executePoints = new List<DateTime>();
      //      return _executePoints;
      //   }
      //}
      //private List<DateTime> _executePoints;

      /// <summary>
      /// 自备标志
      /// </summary>
      public bool Provide4Oneself
      {
         get { return _provide4Oneself; }
         set { _provide4Oneself = value; }
      }
      private bool _provide4Oneself;

      /// <summary>
      /// 医嘱管理标志
      /// </summary>
      public OrderManagerKind OrderManagerFlag
      {
         get { return _orderManagerFlag; }
         set { _orderManagerFlag = value; }
      }
      private OrderManagerKind _orderManagerFlag;

      /// <summary>
      /// 将频次信息组合成文字
      /// </summary>
      public string Text
      {
         get
         {
            string result;
            // 仅用于临时医嘱的频次不需要显示内容
            if (OrderManagerFlag == OrderManagerKind.ForTemp)
               result = "";
            else
            {
               string period;
               if ((Period == 1) && (PeriodUnitFlag != OrderExecPeriodUnitKind.Week))
                  period = "";
               else
                  period = Period.ToString(CultureInfo.CurrentCulture);

               switch (PeriodUnitFlag)
               {
                  case OrderExecPeriodUnitKind.Day:
                     result = String.Format(CultureInfo.CurrentCulture
                        , "每{1}天{0}次[{2}]", ExecuteTimesPerPeriod, period, ExecuteTime);
                     break;
                  case OrderExecPeriodUnitKind.Hour:
                     result = String.Format(CultureInfo.CurrentCulture
                        , "每{1}小时{0}次", ExecuteTimesPerPeriod, period);
                     break;
                  case OrderExecPeriodUnitKind.Minute:
                     result = String.Format(CultureInfo.CurrentCulture
                        , "每{1}分钟{0}次", ExecuteTimesPerPeriod, period);
                     break;
                  case OrderExecPeriodUnitKind.Week:
                     result = String.Format(CultureInfo.CurrentCulture
                        , "每周{1}天,每天{0}次[{2}]", ExecuteTimesPerPeriod, Period, ExecuteTime);
                     break;
                  default:
                     result = "";
                     break;
               }
            }
            return result;
         }
      }

      /// <summary>
      /// 平均每天执行次数
      /// </summary>
      public decimal ExecuteTimesPerDay
      {
         get
         {
            // 公式：每周期执行次数×换算成按天执行的次数
            return ExecuteTimesPerPeriod
               * OrderFrequency.ConvertPeriod2PerDay(PeriodUnitFlag, Period);
         }
      }

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { return GetQuerySentenceFromXml("SelectOrderFrequencyBook"); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
         get { return FormatFilterString("ID", Code); }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public OrderFrequency()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      public OrderFrequency(string code)
         : base(code)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      /// <param name="name"></param>
      public OrderFrequency(string code, string name)
         : base(code, name)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public OrderFrequency(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion

      #region public methods
      /// <summary>
      /// 将指定类型的周期换算成按天执行时的次数
      /// </summary>
      /// <param name="periodFlag">周期类型</param>
      /// <param name="period">>周期(如果类型是"周",则表示每周执行几天)</param>
      /// <returns>按天执行时的次数</returns>
      public static decimal ConvertPeriod2PerDay(OrderExecPeriodUnitKind periodFlag, int period)
      {
         switch (periodFlag)
         {
            case OrderExecPeriodUnitKind.Week:
               return Decimal.Divide(period, 7);
            case OrderExecPeriodUnitKind.Day:
               return Decimal.Divide(1, period);
            case OrderExecPeriodUnitKind.Hour:
               return Decimal.Divide(24, period);
            case OrderExecPeriodUnitKind.Minute:
               return Decimal.Divide(1440, period);
            default:
               throw new ArgumentException(MessageStringManager.GetString("EnumFlagNotExist"));
         }
      }

      /// <summary>
      /// 确定两个对象是否具有相同的值
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public override bool Equals(object obj)
      {
         if (obj == null)
            return false;
         OrderFrequency aimObj = (OrderFrequency)obj;

         if (aimObj != null)
         {
            return ((aimObj.Code == Code)
               && (aimObj.ExecuteTimesPerPeriod == ExecuteTimesPerPeriod)
               && (aimObj.Period == Period)
               && (aimObj.PeriodUnitFlag == PeriodUnitFlag)
               && (aimObj.WeekDays == WeekDays)
               && (aimObj.ExecuteTime == ExecuteTime));
         }
         return false;
      }

      /// <summary>
      /// 返回该实例的哈希代码
      /// </summary>
      /// <returns></returns>
      public override int GetHashCode()
      {
         return Code.GetHashCode();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public new OrderFrequency Clone()
      {
         return (OrderFrequency)(base.Clone());
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         if (String.IsNullOrEmpty(Code))
            return "";

         if (String.IsNullOrEmpty(Name))
            ReInitializeProperties();

         if (String.IsNullOrEmpty(Name))
            return String.Format(CultureInfo.CurrentCulture, "{0}", Code);
         else
            return String.Format(CultureInfo.CurrentCulture, "{0}", Name.Trim());
      }

      /// <summary>
      /// 转换成美康合理用药要求的格式
      /// </summary>
      /// <returns></returns>
      public string ConvertToMedicomFrequency()
      {
         //原则为：分母为天数，分子为次数（次/天），请看如下列举： 
         // 频次      频次次数  频率间隔 频率间隔单位 传入值(次/天)
         // 6/日      6         1       日          6/1 
         // 1/晚      1         1       日          1/1
         // 1/6小时   1         6       小时        4/1 
         // 1/12小时  1         12      小时        2/1 
         // 1/48小时  1         48      小时        1/2 
         // 即刻      1         1       日          1/1 
         // 1/10分钟  1         10      分钟        144/1 
         // 1/30分钟  1         20      分钟        48/1 
         // 早餐前    1         1       日          1/1 
         // 4/7日     4         7       日          4/7 
         // 1/双日    1         2       日          1/2 
         // 频次间隔单位为“日”转换关系：
         //       传入值 =频次次数 / 频率间隔 
         // 频次间隔单位为“小时”转换关系: 
         //       当小于24小时：传入值 = ((24/频率间隔)*频次次数)/频次次数 
         //       当大于24小时：传入值 = 频次次数/((频率间隔/24)*频次次数) 
         // 频次间隔单位为“分钟”转算关系: 
         //       传入值 = ((24*60/频率间隔)*频次次数)/频次次数
         int temp;
         switch (PeriodUnitFlag)
         {
            case OrderExecPeriodUnitKind.Week:
               return Period.ToString() + "/7";
            case OrderExecPeriodUnitKind.Day:
               return ExecuteTimesPerPeriod.ToString() + "/" + Period.ToString();
            case OrderExecPeriodUnitKind.Hour:
               if (Period <= 24)
               {
                  temp = (24 / Period * ExecuteTimesPerPeriod);
                  return temp.ToString() + "/" + ExecuteTimesPerPeriod.ToString();
               }
               else
               {
                  temp = (Period / 24 * ExecuteTimesPerPeriod);
                  return ExecuteTimesPerPeriod.ToString() + "/" + temp.ToString();
               }
            case OrderExecPeriodUnitKind.Minute:
               temp = 1440 / Period * ExecuteTimesPerPeriod;
               return temp.ToString() + "/" + ExecuteTimesPerPeriod.ToString();
            default:
               return "";
         }
      }

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         ReInitializeProperties();
      }

      /// <summary>
      /// 根据医嘱开始时间计算在指定范围内的执行时间点。如果是以小时或分钟为单位的频次，会根据开始时间进行推算。
      /// </summary>
      /// <param name="orderStartTime">医嘱的开始时间</param>
      /// <param name="startTime">执行的开始日期</param>
      /// <param name="endTime">执行的结束日期(不包括此时间点)</param>
      /// <returns>指定时间范围内的执行时间点</returns>
      public List<DateTime> CalcExecutePoints(DateTime orderStartTime, DateTime startTime, DateTime endTime)
      {
         if (startTime < orderStartTime)
            startTime = orderStartTime;

         if (endTime < startTime)
            throw new ArgumentException("结束时间不能早于开始时间");
         //TimeSpan timeDiff = endTime.Date - startTime.Date;
         //if (timeDiff.TotalDays != 1)
         //   throw new ArgumentException("开始、结束时间必须在相邻的两天里");

         switch (PeriodUnitFlag)
         {
            case OrderExecPeriodUnitKind.Day:
               return GetExecutePointAccordingToDay(orderStartTime, startTime, endTime, Period, ExecuteTimes);
            case OrderExecPeriodUnitKind.Week:
               return GetExecutePointAccordingToWeek(orderStartTime, startTime, endTime, WeekDays, ExecuteTimes);
            case OrderExecPeriodUnitKind.Hour:
               return GetExecutePointAccordingToHour(orderStartTime, startTime, endTime, Period);
            case OrderExecPeriodUnitKind.Minute:
               return GetExecutePointAccordingToMinute(orderStartTime, startTime, endTime, Period);
            //None不应该为非法，只是表示不操作而已
            case OrderExecPeriodUnitKind.None:
               return new List<DateTime>();
            default:
               throw new ArgumentException("枚举值不合法，参数：" + PeriodUnitFlag.ToString(), "PeriodUnitFlag");
            //MessageStringManager.GetString("EnumFlagNotExist"));
         }
      }
      #endregion

      #region static private methods
      private static List<DateTime> GetExecutePointAccordingToDay(DateTime orderStartTime, DateTime startTime, DateTime endTime, int period, List<int> executeTimes)
      {
         List<DateTime> result = new List<DateTime>();

         // 开始、结束日期不同
         //    若 st 要执行（(st.Date - ot.Date).Days % 周期 == 0）,则添加 [st.Time, 24) 之间的执行时间点
         //    若 et 要执行（(et.Date - ot.Date).Days % 周期 == 0）,则添加 [00, et.Time) 之间的执行时间点
         //    st和et之间要执行的日期,添加该日期[00, 24)之间的执行时间点
         // 开始、结束日期相同
         //    若 st 要执行（(st.Date - ot.Date).Days % 周期 == 0）,则添加 [st.Time, et.Time) 之间的执行时间点
         TimeSpan timeDiff;
         DateTime today = startTime;
         TimeSpan timeBegin;
         TimeSpan timeEnd;
         TimeSpan point;
         if (period == 0)
            return result;
         do
         {
            timeDiff = today.Date - orderStartTime.Date;
            if ((timeDiff.Days % period) == 0) // 能整除周期，说明需要执行
            {
               timeBegin = today.TimeOfDay; // 第一天的起始范围和开始时间有关，后面都是从零点开始
               if (today.Date == endTime.Date) // 最后一天的结束范围和结束时间有关，前面的都到24点截止
                  timeEnd = endTime.TimeOfDay;
               else
                  timeEnd = new TimeSpan(23, 59, 59);
               foreach (int hour in executeTimes) // 取当天[开始时间, 结束时间)之间的执行时间点
               {
                  point = new TimeSpan(hour, 0, 0);// 因为startTime可能不是整点，所以没有用直接比较小时数的方法
                  if ((point >= timeBegin) && (point < timeEnd))
                     result.Add(today.Date + point);
               }
            }
            today = today.Date.AddDays(1) + new TimeSpan(0, 0, 0);
         } while (today.Date <= endTime.Date);

         return result;
      }

      private static List<DateTime> GetExecutePointAccordingToWeek(DateTime orderStartTime, DateTime startTime, DateTime endTime, string weekDays, List<int> executeTimes)
      {
         List<DateTime> result = new List<DateTime>();

         // 开始、结束日期不同
         //    st.Date 是否执行：st.WeekDay 在周代码中，[st.Time, 24) 之间的执行时间点
         //    et.Date 是否执行：et.WeekDay 在周代码中，[00, et.Time) 之间的执行时间点
         //    st和et之间要执行的日期,添加该日期[00, 24)之间的执行时间点
         // 开始、结束日期相同
         //    若 st 要执行（st.WeekDay 在周代码中）,则添加 [st.Time, et.Time) 之间的执行时间点
         int wd;
         DateTime today = startTime;
         TimeSpan timeBegin;
         TimeSpan timeEnd;
         TimeSpan point;
         do
         {
            //hrr:此处有bug，星期的代码与周代码中的对应不一致，导致总是算到第二天，需要修改对应保持与HIS的一致
            //Sunday = 0,      Monday = 1,      Tuesday = 2,      Wednesday = 3,      Thursday = 4,      Friday = 5,      Saturday = 6,
            //HIS中周一到周日表示为1-7
            if (today.DayOfWeek == DayOfWeek.Sunday)
               wd = 7;
            else
               wd = (int)today.DayOfWeek;
            //wd = (int)today.DayOfWeek + 1; 
            if (weekDays.IndexOf(wd.ToString()) >= 0) // 在周代码里，说明需要执行
            {
               timeBegin = today.TimeOfDay; // 第一天的起始范围和开始时间有关，后面都是从零点开始
               if (today.Date == endTime.Date) // 最后一天的结束范围和结束时间有关，前面的都到24点截止
                  timeEnd = endTime.TimeOfDay;
               else
                  timeEnd = new TimeSpan(23, 59, 59);
               foreach (int hour in executeTimes) // 取当天[开始时间, 结束时间)之间的执行时间点
               {
                  point = new TimeSpan(hour, 0, 0);// 因为startTime可能不是整点，所以没有用直接比较小时数的方法
                  if ((point >= timeBegin) && (point < timeEnd))
                     result.Add(today.Date + point);
               }
            }
            today = today.Date.AddDays(1) + new TimeSpan(0, 0, 0);
         } while (today.Date <= endTime.Date);

         return result;
      }

      private static List<DateTime> GetExecutePointAccordingToHour(DateTime orderStartTime, DateTime startTime, DateTime endTime, int period)
      {
         // 折算成按分钟执行
         return GetExecutePointAccordingToMinute(orderStartTime, startTime, endTime, period * 60);
      }

      private static List<DateTime> GetExecutePointAccordingToMinute(DateTime orderStartTime, DateTime startTime, DateTime endTime, int period)
      {
         List<DateTime> result = new List<DateTime>();
         
         // 按分钟
         //    先找到不小于开始时间的最小执行时间，然后累加周期，知道大于等于结束时间
         
         TimeSpan timeDiff = startTime - orderStartTime;
         int minPeriodCount = timeDiff.Minutes / period; // 两个日期之间完整的周期数

         DateTime point = orderStartTime.AddMinutes( period * minPeriodCount);
         while (point < startTime)
            point.AddMinutes(period);

         while (point < endTime)
         {
            result.Add(point);
            point.AddMinutes(period);
         }

         return result;
      }
      #endregion
   }
}
