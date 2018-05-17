using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Basic.Paint.NET
{
    public class ShapeSelector
        : IShapeSelector
    {

        private class AdvanceSelectInfo
        {
            private Dictionary<IShape, int> _list;
            private ShapeSelectorCallback _callback;

            public AdvanceSelectInfo(ShapeSelectorCallback callback)
            {
                _list = new Dictionary<IShape, int>();
                _callback = callback;
            }

            public Dictionary<IShape, int> List
            {
                get { return _list; }
            }

            public ShapeSelectorCallback Callback
            {
                get { return _callback; }
            }

        }

        #region Fields
        private IShapeSource _source;
        #endregion

        #region Ctors

        public ShapeSelector(IShapeSource source)
        {
            _source = source;
        }

        #endregion

        #region IShapeSelector Members

        public IShapeSource Source
        {
            get { return _source; }
        }

        public ActionBuildType Select(Point pt, out int index)
        {
            IList<IShape> list = Source.Shapes;
            ShapeData data;
            Rectangle bounds;
            for (int i = 0; i < list.Count; i++)
            {
                data = list[i].Data;
                if (!data.Enable)
                    continue;
                bounds = data.Bounds;
                if (bounds.Contains(pt))
                {
                    index = i;
                    return ActionBuildType.Select;
                }
            }
            index = -1;
            return ActionBuildType.None;
        }

        public void AdvanceSelect(Point pt, ShapeSelectorCallback callback)
        {
            AdvanceSelectInfo info = new AdvanceSelectInfo(callback);
            IList<IShape> list = Source.Shapes;
            IShape shape;
            ShapeData data;
            for (int i = 0; i < list.Count; i++)
            {
                shape = list[i];
                data = shape.Data;
                if (!data.Enable)
                    continue;
                if (data.Bounds.Contains(pt))
                    info.List.Add(shape, i);
            }
            if (info.List.Count > 0)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                ToolStripMenuItem tsmi;
                foreach (IShape s in info.List.Keys)
                {
                    tsmi = new ToolStripMenuItem(s.ToString());
                    tsmi.Tag = s;
                    cms.Items.Add(tsmi);
                }
                cms.Tag = info;
                cms.ItemClicked += new ToolStripItemClickedEventHandler(cms_ItemClicked);
                cms.Show(Control.MousePosition);
            }
        }

        private void cms_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;
            AdvanceSelectInfo info = cms.Tag as AdvanceSelectInfo;
            IShape s = e.ClickedItem.Tag as IShape;
            if (info.Callback != null)
                info.Callback(ActionBuildType.Select, info.List[s]);
        }

        #endregion

    }
}
