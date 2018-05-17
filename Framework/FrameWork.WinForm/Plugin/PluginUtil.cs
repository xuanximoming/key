using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraTreeList;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraTreeList.Columns;
using System.Windows.Forms;
using DrectSoft.Core;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using DevExpress.XtraGrid.Views.Layout;
using Microsoft.Practices.EnterpriseLibrary.Data;
using DevExpress.XtraGrid.Views.Card;
using System.Runtime.InteropServices;

namespace DrectSoft.FrameWork.WinForm.Plugin
{
    /// <summary>
    /// 供插件调用的公共方法【由于Oracle中读取的字段都是大写，所以要把绑定字段和数据源中的列名称都改为大写】
    /// </summary>
    public class PluginUtil
    {





        Database m_IDatabase;

        /// <summary>
        /// .ctor()
        /// </summary>
        /// <param name="iDatabase"></param>
        public PluginUtil(Database iDatabase)
        {
            m_IDatabase = iDatabase;
        }

        /// <summary>
        /// XtraGridCard中绑定字段及数据源转大写
        /// </summary>
        /// <param name="view"></param>
        public void ConvertGridCardDataSourceUpper(LayoutView view)
        {
            if (GetServerType() == ServerType.Oracle)
            {
                ConvertGridCardDataSourceUpperInner(view);
            }
        }

        private void ConvertGridCardDataSourceUpperInner(LayoutView view)
        {
            foreach (LayoutViewColumn column in view.Columns)
            {
                column.FieldName = column.FieldName.ToUpper();
            }

            if (view.DataSource != null)
            {
                ConvertDataSourceUpper(view.DataSource as DataTable);
                ConvertDataSourceUpper(view.DataSource as DataView);
            }
        }

        /// <summary>
        /// XtraGridCard中绑定字段及数据源转大写
        /// </summary>
        /// <param name="view"></param>
        public void ConvertGridCardDataSourceUpper(CardView view)
        {
            if (GetServerType() == ServerType.Oracle)
            {
                ConvertGridCardDataSourceUpperInner(view);
            }
        }

        private void ConvertGridCardDataSourceUpperInner(CardView view)
        {
            foreach (GridColumn column in view.Columns)
            {
                column.FieldName = column.FieldName.ToUpper();
            }

            if (view.DataSource != null)
            {
                ConvertDataSourceUpper(view.DataSource as DataTable);
                ConvertDataSourceUpper(view.DataSource as DataView);
            }
        }

        /// <summary>
        /// XtraGrid中绑定字段及数据源转大写
        /// </summary>
        /// <param name="grid"></param>
        public void ConvertGridDataSourceUpper(GridView grid)
        {
            if (GetServerType() == ServerType.Oracle)
            {
                ConvertGridDataSourceUpperInner(grid);
            }
        }

        private void ConvertGridDataSourceUpperInner(GridView grid)
        {
            foreach (GridColumn column in grid.Columns)
            {
                column.FieldName = column.FieldName.ToUpper();
            }

            if (grid.DataSource != null)
            {
                ConvertDataSourceUpper(grid.DataSource as DataTable);
                ConvertDataSourceUpper(grid.DataSource as DataView);
            }
        }

        /// <summary>
        /// XtraTreeList中绑定字段及数据源转大写
        /// </summary>
        /// <param name="treeList"></param>
        public void ConvertTreeListDataSourceUpper(TreeList treeList)
        {
            if (GetServerType() == ServerType.Oracle)
            {
                ConvertTreeListDataSourceUpperInner(treeList);
            }
        }

        private void ConvertTreeListDataSourceUpperInner(TreeList treeList)
        {
            foreach (TreeListColumn column in treeList.Columns)
            {
                column.FieldName = column.FieldName.ToUpper();
            }

            if (treeList.DataSource != null)
            {
                ConvertDataSourceUpper(treeList.DataSource as DataTable);
                ConvertDataSourceUpper(treeList.DataSource as DataView);
            }
        }

        /// <summary>
        /// DataTable中ColumnName转大写
        /// </summary>
        /// <param name="dt"></param>
        public void ConvertDataSourceUpper(DataTable dt)
        {
            if (GetServerType() == ServerType.Oracle)
            {
                if (dt != null)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        column.ColumnName = column.ColumnName.ToUpper();
                    }
                }
            }
        }

        /// <summary>
        /// DataTable中ColumnName转大写
        /// </summary>
        /// <param name="dv"></param>
        public void ConvertDataSourceUpper(DataView dv)
        {
            if (GetServerType() == ServerType.Oracle)
            {
                if (dv != null)
                {
                    foreach (DataColumn column in dv.Table.Columns)
                    {
                        column.ColumnName = column.ColumnName.ToUpper();
                    }
                }
            }
        }

        /// <summary>
        /// 返回应有的属性值，由于oracle中的列名均为大写，，导致在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ConvertProperty(string value)
        {
            if (GetServerType() == ServerType.Oracle)
            {
                return value.ToUpper();
            }
            return value;
        }

        private ServerType GetServerType()
        {
            if (m_IDatabase is OracleDatabase)
            {
                return ServerType.Oracle;
            }
            else if (m_IDatabase is SqlDatabase)
            {
                return ServerType.SqlSerer;
            }
            else
            {
                return ServerType.Null;
            }
        }
    }

    enum ServerType
    {
        Null,
        SqlSerer,
        Oracle
    }
}
