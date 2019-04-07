using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextExtractor
{
    public partial class Form1 : Form
    {
        private List<List<string>> data = new List<List<string>>();
        private List<string> header = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var regex = new Regex(textBox2.Text);
                var results = Regex.Matches(textBox1.Text, textBox2.Text);

                listView1.Columns.Clear();
                listView1.Items.Clear();
                data.Clear();
                foreach (string name in regex.GetGroupNames())
                    listView1.Columns.Add(name);
                listView1.Columns.RemoveAt(0);

                header = new List<string>(regex.GetGroupNames());
                header.RemoveAt(0);

                toolStripStatusLabel1.Text = string.Format("{0} records found\r\n", results.Count);

                for (int i = 0; i < results.Count; i++)
                {
                    Match match = results[i];
                    var item_val = new List<string>();
                    foreach (Group group in match.Groups)
                        item_val.Add(group.Value);
                    item_val.RemoveAt(0);

                    ListViewItem item = new ListViewItem(item_val.ToArray());
                    listView1.Items.Add(item);

                    data.Add(item_val);

                }
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            } catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var csv_data = string.Join(", ", header) + "\r\n";

            foreach(List<string> values in data)
                csv_data += string.Join(", ", values) + "\r\n";

            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, csv_data, Encoding.UTF8);
            }
        }
    }
}
