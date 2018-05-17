using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace DrectSoft.FrameWork
{
    public interface IPagedList
    {
        int TotalPage //总页数
        {
            get;
        }

        int TotalCount
        {
            get;
            set;
        }

        int PageIndex
        {
            get;
            set;
        }

        int PageSize
        {
            get;
            set;
        }

        bool IsPreviousPage
        {
            get;
        }

        bool IsNextPage
        {
            get;
        }
    }

    public class PagedList<T> : List<T>, IPagedList
    {
        public PagedList(IQueryable<T> source, int? index, int? pageSize)
        {
            if (index == null) { index = 1; }
            if (pageSize == null) { pageSize = 20; }
            this.TotalCount = source.Count();
            this.PageSize = pageSize.Value;
            this.PageIndex = index.Value;
            this.AddRange(source.Skip((index.Value - 1) * pageSize.Value).Take(pageSize.Value));
        }

        public PagedList(List<T> source, int? index, int? pageSize)
        {
            if (index == null) { index = 1; }
            if (pageSize == null) { pageSize = 20; }
            this.TotalCount = source.Count();
            this.PageSize = pageSize.Value;
            this.PageIndex = index.Value;
            this.AddRange(source.Skip((index.Value - 1) * pageSize.Value).Take(pageSize.Value));
        }

        public PagedList(IEnumerable<T> source, int? index, int? pageSize)
        {
            if (index == null) { index = 1; }
            if (pageSize == null) { pageSize = 20; }
            this.TotalCount = source.Count();
            this.PageSize = pageSize.Value;
            this.PageIndex = index.Value;
            this.AddRange(source.Skip((index.Value - 1) * pageSize.Value).Take(pageSize.Value));
        }

        public int TotalPage
        {
            get { return (int)System.Math.Ceiling((double)TotalCount / PageSize); }
        }

        public int TotalCount
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public bool IsPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool IsNextPage
        {
            get
            {
                return ((PageIndex) * PageSize) < TotalCount;
            }
        }
    }

    public static class Pagination
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int? index, int? pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int? index)
        {
            return new PagedList<T>(source, index, 20);
        }

        public static PagedList<T> ToPagedList<T>(this List<T> source, int? index, int? pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this List<T> source, int? index)
        {
            return new PagedList<T>(source, index, 20);
        }

        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int? index, int? pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int? index)
        {
            return new PagedList<T>(source, index, 20);
        }

        public static DataTable ToPagedList(this DataTable source, int? index, int? pageSize)
        {
            DataRow[] drows = new DataRow[source.Rows.Count];
            source.Rows.CopyTo(drows, 0);
            var resList = new PagedList<DataRow>(drows.ToList(), index, pageSize);
            return resList.Count > 0 ? resList.CopyToDataTable() : new DataTable();
        }

        public static DataTable ToPagedList(this DataTable source, int? index)
        {
            DataRow[] drows = new DataRow[source.Rows.Count];
            source.Rows.CopyTo(drows, 0);
            var resList = new PagedList<DataRow>(drows.ToList(), index, 20);
            return resList.Count > 0 ? resList.CopyToDataTable() : new DataTable();
        }

        public static DataTable ToPagedList(this DataRow[] source, int? index, int? pageSize)
        {
            var resList = new PagedList<DataRow>(source.ToList(), index, pageSize);
            return resList.Count > 0 ? resList.CopyToDataTable() : new DataTable();
        }

        public static DataTable ToPagedList(this DataRow[] source, int? index)
        {
            var resList = new PagedList<DataRow>(source.ToList(), index, 20);
            return resList.Count > 0 ? resList.CopyToDataTable() : new DataTable();
        }

    }
}