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