using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DrectSoft.Emr.Web.Business.Service;
using DrectWeb.Business.Service.Search;
using System.IO;
namespace DrectWeb.Search2.yaodian
{
    public partial class yaodianSearch3 : System.Web.UI.Page
    {
        private static FileInfos _CurrentFileInfos;

        public static  FileInfos CurrentFileInfos
        {
            get {
                if (_CurrentFileInfos == null)
                    _CurrentFileInfos = new FileInfos();
                return _CurrentFileInfos; }
            set { _CurrentFileInfos = value; }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                GetTreeView(TreeView1);
            }
        }
        public void GetTreeView(TreeView treeView1)
        {
            TreeNode tn = new TreeNode();
            tn.Text = "根目录";
            getDirectories(Server.MapPath("~/2010版中国药典"), tn);
            treeView1.Nodes.Add(tn);
        }
        /// <summary>
        /// 循环遍历获得某一目录下的所有文件信息
        /// </summary>
        /// <param name="path">目录名</param>
        /// <param name="tn">树节点</param>
        public static void getDirectories(string path, TreeNode tn)
        {
            string[] fileNames = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);

            //先遍历这个目录下的文件夹
            foreach (string dir in directories)
            {
                TreeNode subtn = new TreeNode();
                subtn.Text = GetShorterFileName(dir);
                //subtn.ImageUrl = "~/Images/open.gif";
                subtn.Expanded = false;
                subtn.NavigateUrl = "#";
                getDirectories(dir, subtn);
                tn.ChildNodes.Add(subtn);
            }

            //再遍历这个目录下的文件
            foreach (string file in fileNames)
            {
                TreeNode subtn = new TreeNode();
                //subtn.ImageUrl = "~/Images/file.gif";
                subtn.Text = GetShorterFileName(file);
                int index = file.IndexOf("DrectWeb");
                subtn.NavigateUrl = "../../"+file.Substring(index+9);
                tn.ChildNodes.Add(subtn);
                CurrentFileInfos.Add(new FileInfo() { Name = subtn.Text, Url = subtn.NavigateUrl });
            }
        }

        /// <summary>
        /// 滤去文件名前面的路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetShorterFileName(string filename)
        {
            int lastindex = filename.LastIndexOf("\\");
            return filename.Substring(lastindex+1);//.Substring(i + 1);
        }
       


    }
    public class DirInfo
    {

        public String Name { get; set; }
        public DirInfo PreDirInfo { get; set; }
        private FileInfos _CurrentFileInfos;

        internal FileInfos CurrentFileInfos
        {
            get { return _CurrentFileInfos; }
            set { _CurrentFileInfos = value; }
        }

 
    }
    public class DirInfos : List<DirInfo>
    {
    
    }
    public class FileInfo
    {

        public String Name { get; set; }
        public String Url { get; set; }
    }
    public class FileInfos : List<FileInfo>
    {
        public void Add(FileInfo f)
        {
            if (GetMatch(f.Name).Count == 0)
            {
                base.Add(f);
            }
        }
        public FileInfos GetMatch(String key)
        {
            FileInfos _FileInfos = new FileInfos();
            foreach (var item in this)
            {
                if (item.Name.IndexOf(key) > 0)
                {
                    _FileInfos.Add(item);
                }
            }
            return _FileInfos;
        }
    }
 
}