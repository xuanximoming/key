using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class InLookBar : UserControl
    {
        public InLookBar()
        {
            InitializeComponent();
        }

        public List<Band> Bands = new List<Band>();
        public int selectedIndex = -1;
        public void AddBand(string bandName)
        {
            Band bt = new Band();
            bt.Width = this.Width;
            bt.Text = bandName;
            bt.Location = new Point(0, bt.Height * Bands.Count);
            bt.Click += new EventHandler(bt_Click);
            Bands.Add(bt);
            this.Controls.Add(bt);
        }

        void bt_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            OnMouseClick();

        }

        protected  void OnMouseClick()
        {

            Point p = PointToClient(Control.MousePosition);

            for (int i =0;i<Bands.Count;i++)
            {
                if (Bands[i].Bounds.Contains(p))
                {
                    this.selectedIndex = i;
                    break;
                }
            }

            ReSet();
        }

        void ReSet()
        {
            int Y = 0;
            for (int i = 0; i < Bands.Count; i++)
            {

                if (i < this.selectedIndex)
                    Bands[i].Location = new Point(0, Y);
                //画列表
                if (i == this.selectedIndex)
                {
                    Bands[i].Location = new Point(0, Y);

                    Bands[i].listBox.Height = this.Height - this.Bands.Count * Bands[i].Height;
                    Y += Bands[i].Height;
                    Bands[i].listBox.Location = new Point(0, Y);
                    Y += Bands[i].listBox.Height;
                    continue;
                }
                if (i > this.selectedIndex)
                {
                    Bands[i].Location = new Point(0, Y);
                }
                Y += i * Bands[i].Height;
            }
        }
    }

    public class Band : Button
    {
        public ListBox listBox = new ListBox();

        public void AddItem(string itemName)
        {
            listBox.Items.Add(itemName);
        }
    }

    
}
