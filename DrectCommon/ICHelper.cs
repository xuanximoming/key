using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DrectSoft.Common
{
    /// <summary>
    /// 功能描述：健康卡读取接口
    /// 创 建 者：bwj
    /// 创建日期：2012-11-01
    /// </summary>
    public class ICHelper
    {

        /// <summary>
        /// 当前设备
        /// bwj
        /// 2012-11-01
        /// </summary>
        public static IntPtr currentDevice;
        /// <summary>
        /// 默认端口
        /// bwj
        /// 2012-11-01
        /// </summary>
        public const int DEFAULTPORT = 0;
        /// <summary>
        /// 当前端口
        /// bwj
        /// 2012-11-01
        /// </summary>
        public static int currentPort = 0;
        /// <summary>
        /// 关闭设备
        /// bwj
        /// 2012-11-01
        /// </summary>
        /// <param name="_curDevice"></param>
        /// <returns></returns>
        private static int CloseDevice(IntPtr _curDevice)
        {
            try
            {
                int result = 0;


                result = ICAPI.CloseDevice(_curDevice);

                return result;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 打开设备
        /// bwj
        /// 2012-11-01
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private static IntPtr OpenDevice(int port)
        {
            try
            {
                IntPtr result = IntPtr.Zero;
                currentPort = DEFAULTPORT;

                if (port != DEFAULTPORT)
                {
                    currentPort = port;
                }

                result = ICAPI.OpenDevice(currentPort);
                currentDevice = result;
                return result;

            }
            catch (Exception ce)
            {
                throw new Exception("打开端口失败,错误:" + ce.Message.ToString());
            }
        }
        private static IntPtr OpenDevice()
        {
            try
            {
                IntPtr result = IntPtr.Zero;
                if (currentPort == DEFAULTPORT)
                {
                    currentPort = DEFAULTPORT;
                }
                result = ICAPI.OpenDevice(currentPort);
                currentDevice = result;
                return result;

            }
            catch (Exception ce)
            {
                throw new Exception("打开端口失败,错误:" + ce.Message.ToString());
            }
        }
        /// <summary>
        /// 上电复位
        /// bwj
        /// 2012-11-01
        /// </summary>
        /// <param name="_curDevice"></param>
        /// <returns></returns>
        private static int PowerOn(IntPtr _curDevice)
        {
            try
            {
                int result = 0;
                Byte[] ErrMsg = new Byte[60];

                result = ICAPI.PowerOn(_curDevice, ICAPI.SLOT_PSAM, ref ErrMsg[0]);
                if (result != 0)
                {
                    throw new Exception("PSAM卡上电失败." + System.Text.Encoding.Default.GetString(ErrMsg));

                }

                ErrMsg = new Byte[60]; ;
                result = ICAPI.PowerOn(_curDevice, ICAPI.SLOT_USER, ref ErrMsg[0]);
                if (result != 0)
                {
                    throw new Exception("用户卡上电失败." + System.Text.Encoding.Default.GetString(ErrMsg));

                }
                return result;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 读持卡用户基本信息
        /// bwj
        /// 2012-11-01
        /// </summary>
        /// <returns></returns>
        public static PersonalBasicInfoType ReadPersonalBasicInfo()
        {
            try
            {
                PersonalBasicInfoType result;
                //初始化端口
                currentPort = 0;
                //打开设备
                currentDevice = OpenDevice();
                //上电复位
                Byte[] ATR = new Byte[60];
                ICAPI.PowerOn(ICHelper.currentDevice, ICAPI.SLOT_PSAM, ref ATR[0]);
                ATR = new Byte[60];
                ICAPI.PowerOn(ICHelper.currentDevice, ICAPI.SLOT_USER, ref ATR[0]);
                //读数据

                Byte[] chsXM = new Byte[30 + 1];              //姓名
                Byte[] chsXB = new Byte[2 + 1];          //性别代码
                Byte[] chsMZ = new Byte[2 + 1];           //民族代码
                Byte[] chsCSRQ = new Byte[8 + 1];             //出生日期，YYYYMMDD格式
                Byte[] chsSFZH = new Byte[18 + 1];             //居民身份证号
                int iNum = ICAPI.iR_DDF1EF06Info(ICHelper.currentDevice,
                        ref chsXM[0],              //姓名
                        ref chsXB[0],              //性别代码
                        ref chsMZ[0],              //民族代码
                        ref chsCSRQ[0],            //出生日期，YYYYMMDD格式
                        ref chsSFZH[0]            //居民身份证号     
                        );
                if (iNum != 0)
                {
                    CloseDevice(currentDevice);
                    throw new Exception("读卡失败,请检查:\r\n1.设备是否连接\r\n2.卡片是否存在");
                }
                result.name = Encoding.Default.GetString(chsXM);
                result.gendercode = Encoding.ASCII.GetString(chsXB);
                result.nationalcode = Encoding.ASCII.GetString(chsMZ);
                result.birthday = Encoding.ASCII.GetString(chsCSRQ);
                result.pid = Encoding.ASCII.GetString(chsSFZH);
                //关闭设备
                CloseDevice(currentDevice);
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }




    }

    /// <summary>
    /// 持卡人基本信息结类型
    /// bwj
    /// 2012-11-01
    /// </summary>
    public struct PersonalBasicInfoType
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string name;
        /// <summary>
        /// 性别代码
        /// </summary>
        public string gendercode;
        /// <summary>
        /// 民族代码
        /// </summary>
        public string nationalcode;
        /// <summary>
        /// 出生日期
        /// </summary>
        public string birthday;
        /// <summary>
        /// 身份证号
        /// </summary>
        public string pid;
    }

    /// <summary>
    /// API映射
    /// bwj
    /// 2012-11-01
    /// </summary>
    class ICAPI
    {
        #region API映射
        public const int ERROR_SUCCESS = 0;
        public const int ERROR_INVALID_HANDLE = 6;
        public const int ERROR_INVALID_PARAMETER = 87;
        public const int ERROR_OPEN_FAILED = 110;
        public const int SLOT_USER = 0x01;
        public const int SLOT_PSAM = 0x11;


        [DllImport("WSB_RWInterface.dll")]
        public static extern IntPtr OpenDevice(int port);
        [DllImport("WSB_RWInterface.dll")]
        public static extern int CloseDevice(IntPtr hCard);
        [DllImport("WSB_RWInterface.dll")]
        public static extern int PowerOn(IntPtr hDev, int slot, ref Byte ATR); //slot 1  User; slot 0x11 SAM1
        [DllImport("WSB_RWInterface.dll")]
        public static extern int SendAPDU(IntPtr hDev, Byte bySlot,
            ref Byte pbyC_Command,
                            long byLen, ref Byte pbyR_Command, ref int pnRes);
        [DllImport("WSB_RWInterface.dll")]
        public static extern int iR_DDF1EF06Info(IntPtr hDev,
                             ref Byte XM,              //姓名
                             ref Byte XB,              //性别代码
                             ref Byte MZ,              //民族代码
                             ref Byte CSRQ,            //出生日期，YYYYMMDD格式
                             ref Byte SFZH            //居民身份证号

                             );
        #endregion
    }
}
