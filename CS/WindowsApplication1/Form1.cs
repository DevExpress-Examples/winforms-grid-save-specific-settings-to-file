using System;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private DataTable CreateTable(int RowCount)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Name", typeof(string));
            tbl.Columns.Add("ID", typeof(int));
            tbl.Columns.Add("Number", typeof(int));
            tbl.Columns.Add("Date", typeof(DateTime));
            for (int i = 0; i < RowCount; i++)
                tbl.Rows.Add(new object[] { String.Format("Name{0}", i), i, 3 - i, DateTime.Now.AddDays(i) });
            return tbl;
        }
        

        public Form1()
        {
            InitializeComponent();
            gridControl1.DataSource = CreateTable(20);
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("test.xml", FileMode.Create);
            GridLayoutSerializer.SaveLayout(gridView1, fs);
            fs.Close(); 
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            gridView1.RestoreLayoutFromXml("test.xml");
        }
    }
}
