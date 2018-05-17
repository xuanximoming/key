using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MedicalRecordManage.UCControl;
namespace MedicalRecordManage.Object
{
    public class RecordWriteUpModel
    {
        private int indexField;
        private int typeField;
        private int countField;
        private MedicalRecordWriteUp  controlField;
        private DataTable tableField;
        public RecordWriteUpModel(int indexs, int types, int counts, MedicalRecordWriteUp controls, DataTable tables)
        {
            indexField = indexs;
            typeField = types;
            countField = counts;
            controlField = controls;
            tableField = tables;
        }
        public DataTable MyDataTable
        {
            get
            {
                return this.tableField;
            }
            set
            {
                this.tableField = value;
            }
        }

        public int Index
        {
            get
            {
                return this.indexField;
            }
            set
            {
                this.indexField = value;
            }
        }
        public int Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
        public int Count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
        public MedicalRecordWriteUp MyControl
        {
            get
            {
                return this.controlField;
            }
            set
            {
                this.controlField = value;
            }
        }
    }
}
