# key
my porject
a new 

修复20180607
/EMREditor/Library.EmrEditor/Src/Document/ZYTextDocument.cs
   public void UpdateCaret()
      myOwnerControl.MoveTextCaretTo(myElement.RealLeft + myElement.Width - 2, myElement.RealTop + myElement.Height, myElement.Height);
	   修改为
       myOwnerControl.MoveTextCaretTo(myElement.RealLeft, myElement.RealTop + myElement.Height, myElement.Height); 
解决第一次左右移动光标时光标移动两个位置，在回车符时，光标移动到回车符后面

修复20180608
\Core\病案首页\IEMMainPage_2012_SX\IemMainPageManger.cs
    private void InsertIemInfo_sx()
	   //插入产妇婴儿情况
	   InsertIemObstetricsBaby(this.IemInfo.IemObstetricsBaby, m_app.SqlHelper);
	   修改为
       if (this.IemInfo.IemObstetricsBaby != null)
            InsertIemObstetricsBaby(this.IemInfo.IemObstetricsBaby, m_app.SqlHelper);
病案首页中有IemObstetricsBaby部分

修改20200321 重新处理行尾是标点符号的换行问题（护士表格病历内容）
    DrectSoft.Common.DS_Common.GetStrEquallong()
		 将：
		 duanStr += allStr[i + 1];
		 i++;
		 修改为
		 duanStr = duanStr.Substring(0, i);
		 i--;
修改20200321 修改
	DrectSoft.Core.NurseDocument.DataLoader.GetPatData()
	将：
		string sql = string.Format(@"select * from inpatient where noofinpat='{0}' ", m_noofinpat);
		DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
	修改为从存储过程取，方便字段拓展：
	SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@NoOfInpat", SqlDbType.Decimal),
                    new SqlParameter("@result", SqlDbType.Structured)
                };
                sqlParam[0].Value = m_noofinpat;
                sqlParam[1].Direction = ParameterDirection.Output;
                DataTable dt = DS_SqlHelper.ExecuteDataTable("EMRPROC.usp_GetPatientInfoForThreeMeas", sqlParam, CommandType.StoredProcedure);

		