using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Runtime.Remoting;

namespace YidanSoft.FrameWork.WinForm
{
    /// <summary>
    /// 默认的创建策略
    /// </summary>
   public class DefaultCreationStrategy:FrameStartupStrategy
   {
       StrategyConfiguration _config;

       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
       public override BuilderStage GetStage()
       {
           return _config.Stage;
       }

       public override bool BuildUp()
       {
           ObjectHandle oh = Activator.CreateInstance(_config.Library, _config.StartClass);
           if (oh == null) return false;

           object obj = oh.Unwrap();
           if (obj == null) return false;
           if (!(obj is IFrameStartup)) return false;

           return ((IFrameStartup)obj).Run() || (!_config.CheckReturn);
       }

       /// <summary>
       /// 根据Xml创建实例
       /// </summary>
       /// <param name="config"></param>
       public DefaultCreationStrategy(StrategyConfiguration config)
       {
           _config = config;
       }
   }

    public class StrategyConfiguration
    {
        BuilderStage _stage;
        string _assembly;
        string _startclass;
        bool _checkreturn = true;

        public BuilderStage Stage
        {
            get { return _stage; }
        }

        public string Library
        {
            get { return _assembly; }
        }

        public string StartClass
        {
            get { return _startclass; }
        }

        public bool CheckReturn
        {
            get { return _checkreturn; }
        }

        public StrategyConfiguration(XmlNode xml)
        {
            if (xml.Attributes["stage"] != null) _stage = (BuilderStage)int.Parse(xml.Attributes["stage"].Value);
            if (xml.Attributes["library"] != null) _assembly = xml.Attributes["library"].Value;
            if (xml.Attributes["class"] != null) _startclass = xml.Attributes["class"].Value;
            if (xml.Attributes["checkreturn"] != null) _checkreturn = bool.Parse(xml.Attributes["checkreturn"].Value);
        }
    }

    public class StrategyConfigurationHelper
    {
        /// <summary>
        /// 实现读取自定义工具条配置信息的委托实现
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public List<StrategyConfiguration> ReadStrategyConfiguration(XmlNode section)
        {
            List<StrategyConfiguration> strategyconfigs = new List<StrategyConfiguration>();
            if (section !=null)
                foreach (XmlNode xmlnode in section.ChildNodes)
                {
                    strategyconfigs.Add(new StrategyConfiguration(xmlnode));
                }
            return strategyconfigs;
        }
    }
}
