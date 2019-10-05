
namespace DrectSoft.Core
{
    /// <summary>
    /// 取得数据存储的对象
    /// </summary>
    public static class DataAccessFactory
    {
        private static SqlDataAccess _sqlDataAccessInstance;

        /// <summary>
        /// 得到一个数据访问对象
        /// </summary>
        /// <returns></returns>
        public static SqlDataAccess GetSqlDataAccess()
        {
            _sqlDataAccessInstance = new SqlDataAccess();
            return _sqlDataAccessInstance;
        }

        /// <summary>
        /// 通过指定的DbName得到数据访问对象
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns>null: 不能连接上数据库</returns>
        public static IDataAccess GetSqlDataAccess(string dbName)
        {
            _sqlDataAccessInstance = new SqlDataAccess(dbName);
            return _sqlDataAccessInstance;
        }

        /// <summary>
        /// 取得Cache访问组件的设置
        /// </summary>
        /// <returns></returns>
        public static string CacheComConfig
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// 默认的数据访问连接。
        /// 以后在自定义的控件和组件内若需要使用数据访问时用此属性。
        /// </summary>
        public static IDataAccess DefaultDataAccess
        {
            get
            {
                if (_defaultDataAccess == null)
                    _defaultDataAccess = new SqlDataAccess();

                return _defaultDataAccess;
            }
            set
            {
                if (value != null)
                    _defaultDataAccess = value;
            }
        }
        private static IDataAccess _defaultDataAccess;
    }
}

