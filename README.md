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
