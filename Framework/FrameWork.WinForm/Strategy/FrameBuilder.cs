using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.WinForm
{
    /// <summary>
    /// ¿ò¼ÜÉú³ÉÆ÷
    /// </summary>
    public class FrameBuilder
    {
        Dictionary<string, List<FrameStartupStrategy>> m_strategies = new Dictionary<string, List<FrameStartupStrategy>>();

        /// <summary>
        /// Æô¶¯
        /// </summary>
        public void Start()
        {
            foreach(string s in Enum.GetNames(typeof(BuilderStage)))
            {
                if (!m_strategies.ContainsKey(s))
                    continue;
                foreach (FrameStartupStrategy strategy in m_strategies[s])
                {
                    if (!strategy.BuildUp())
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strategy"></param>
        public void AddStrategy(FrameStartupStrategy strategy)
        {
            string key = strategy.GetStage().ToString();
            if(m_strategies.ContainsKey(key))
                m_strategies[key].Add(strategy);
            else
            {
                List<FrameStartupStrategy> fss = new List<FrameStartupStrategy>();
                fss.Add(strategy);
                m_strategies.Add(key, fss);
            }
        }
    }
}
