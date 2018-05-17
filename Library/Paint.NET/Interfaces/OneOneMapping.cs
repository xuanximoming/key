using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace DrectSoft.Basic.Paint.NET
{
    [Serializable]
    public class OneOneMapping<TA, TB>
    {

        #region Feilds
        private List<TA> _listA;
        private List<TB> _listB;
        private bool _throwOnNotFound;
        #endregion

        #region Ctors

        public OneOneMapping()
        {
            _listA = new List<TA>();
            _listB = new List<TB>();
        }

        #endregion

        #region Members

        public bool ThrowOnNotFound
        {
            get { return _throwOnNotFound; }
            set { _throwOnNotFound = value; }
        }

        public bool Add(TA a, TB b)
        {
            if (_listA.Contains(a))
                return false;
            if (_listB.Contains(b))
                return false;
            _listA.Add(a);
            _listB.Add(b);
            return true;
        }

        public bool Remove(TA a)
        {
            int index = _listA.IndexOf(a);
            if (index != -1)
            {
                RemoveCore(index);
                return true;
            }
            return false;
        }

        public bool Remove(TB b)
        {
            int index = _listB.IndexOf(b);
            if (index != -1)
            {
                RemoveCore(index);
                return true;
            }
            return false;
        }

        private void RemoveCore(int index)
        {
            _listB.RemoveAt(index);
            _listA.RemoveAt(index);
        }

        public void Clear()
        {
            _listB.Clear();
            _listA.Clear();
        }

        public TA[] GetAs()
        {
            return _listA.ToArray();
        }

        public TB[] GetBs()
        {
            return _listB.ToArray();
        }

        public bool TryGetB(TA a, out TB b)
        {
            b = default(TB);
            int index = _listA.IndexOf(a);
            if (index == -1)
                return false;
            b = _listB[index];
            return true;
        }

        public bool TryGetA(TB b, out TA a)
        {
            a = default(TA);
            int index = _listB.IndexOf(b);
            if (index == -1)
                return false;
            a = _listA[index];
            return true;
        }

        public TB this[TA a]
        {
            get
            {
                TB result;
                if (TryGetB(a, out result))
                    return result;
                else if (_throwOnNotFound)
                    throw new ArgumentOutOfRangeException();
                else
                    return default(TB);
            }
        }

        public TA this[TB b]
        {
            get
            {
                TA result;
                if (TryGetA(b, out result))
                    return result;
                else if (_throwOnNotFound)
                    throw new ArgumentOutOfRangeException();
                else
                    return default(TA);
            }
        }

        public Pair<TA, TB>[] ToPairs()
        {
            int count = _listA.Count;
            Pair<TA, TB>[] result = new Pair<TA, TB>[count];
            for (int i = 0; i < count; i++)
                result[i] = new Pair<TA, TB>(_listA[i], _listB[i]);
            return result;
        }

        public void FromPairs(Pair<TA, TB>[] pairs)
        {
            _listA.Clear();
            _listB.Clear();
            foreach (Pair<TA, TB> pair in pairs)
            {
                _listA.Add(pair.A);
                _listB.Add(pair.B);
            }
        }

        #endregion

    }

    public sealed class Pair<TA, TB>
    {

        #region Fields
        private TA _a;
        private TB _b;
        #endregion

        #region Ctors

        public Pair(TA a, TB b)
        {
            _a = a;
            _b = b;
        }

        #endregion

        #region Properties

        public TA A
        {
            get { return _a; }
        }

        public TB B
        {
            get { return _b; }
        }

        #endregion

    }

}
