//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
	/// <summary>
	/// The <see cref="GenericDatabase"/> is used when no specific behavior is required or known for a database.
	/// </summary>
	/// <remarks>
	/// This database exposes the <see cref="DbProviderFactory"/> used to allow for a provider 
	/// agnostic programming model.
	/// </remarks>
	[DatabaseAssembler(typeof(GenericDatabaseAssembler))]
	[ContainerPolicyCreator(typeof(GenericDatabasePolicyCreator))]
	public class GenericDatabase : Database
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericDatabase"/> class with a connection string and 
		/// a provider factory.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="dbProviderFactory">The provider factory.</param>
		public GenericDatabase(string connectionString, DbProviderFactory dbProviderFactory
		)
			: base(connectionString, dbProviderFactory)
		{
		}

		/// <summary>
		/// This operation is not supported in this class.
		/// </summary>
		/// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
		/// <remarks>There is no generic way to do it, the operation is not implemented for <see cref="GenericDatabase"/>.</remarks>
		/// <exception cref="NotSupportedException">Thrown whenever this method is called.</exception>
		protected override void DeriveParameters(DbCommand discoveryCommand)
		{
			throw new NotSupportedException(Resources.ExceptionParameterDiscoveryNotSupportedOnGenericDatabase);
		}

        #region add new 
        /// <summary>
        /// <para>Gets the parameter token used to delimit parameters for the SQL Server database.</para>
        /// </summary>
        /// <value>
        /// <para>The '' symbol.</para>
        /// </value>
        protected override char ParameterToken
        {
            get { return '\x0'; }
        }

        /// <summary>
        /// 获取Schema与指定表在数据库中定义一致的空DataTable
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>只包含结构的空DataTable</returns>
        public override DataTable GetOriginalEmptyTable(string tableName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 获取指定SQL表所有列的详细定义
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override DataTable GetTableColumnDefinitions(string tableName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 存储过程输出参数的标记
        /// </summary>
        protected override string ProcedureOutputFlag
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// 将CommandText转换成FormatString
        /// </summary>
        /// <param name="currentCmd">包含CommandText和必要参数的DbCommand</param>
        /// <returns>FormatString</returns>
        public override string GetCommandTextFormatString(DbCommand currentCmd)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 生成存储过程的输入参数串
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        protected override string GetProcedureParametersString(IDataParameterCollection parameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 将存储过程型的Command命令转换成合适的、可执行的SQL语法命令
        /// </summary>
        /// <param name="command">需要转换的Command</param>
        /// <returns></returns>
        public override string ConvertStoredProcedure2Sql(DbCommand command)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 获取与Database一致的Command对象
        /// </summary>
        /// <returns></returns>
        public override DbCommand NewCommand()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}
