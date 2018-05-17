using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace DrectSoft.Emr.TemplateFactory
{
    public partial class Formtestdrag : UserControl
    {
        public Formtestdrag()
        {
            InitializeComponent();
        }

        private void Formtestdrag_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop("aaa", DragDropEffects.All);
        }

        //protected override void OnDragEnter(DragEventArgs drgevent)
        //{
        //    drgevent.Effect= DragDropEffects.All;
        //    base.OnDragEnter(drgevent);
        //}
    }
}
