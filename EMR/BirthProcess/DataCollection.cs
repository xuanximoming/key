using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace DrectSoft.Core.BirthProcess
{
    public class DataCollection : CollectionBase
    {
        public string FieldName { get; set; }

        public void Add(DataPoint dataPoint)
        {
            List.Add(dataPoint);
        }

        public void Remove(DataPoint dataPoint)
        {
            List.Remove(dataPoint);
        }

        public void Clear()
        {
            List.Clear();
        }

        public DataPoint this[int animalIndex]
        {
            get
            {
                return (DataPoint)List[animalIndex];
            }
            set
            {
                List[animalIndex] = value;
            }

        }
    }
}
