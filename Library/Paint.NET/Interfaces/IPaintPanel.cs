using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace DrectSoft.Basic.Paint.NET
{
    public interface IPaintPanel
    {
        Picture BackGrond { get; }
        Image CurrentImage { get; }
        bool ReadOnly { get; set; }
        Control GetControl();
        void LoadImage(Stream stream);
        void LoadImage2(Image stream);
        void LoadOriginalImage(byte[] image);
        void SaveImage(Stream stream);
        event EventHandler ExitWithoutSave;
        event EventHandler ExitWithSave;
    }
}
