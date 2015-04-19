using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyRun
{
    public partial class programList : Form
    {
        XMLTool xml;
        Dictionary<String, String> data;

        public programList()
        {
            InitializeComponent();

            xml = new XMLTool("data.xml");
            data = xml.getData();

        }

        private void programList_Load(object sender, EventArgs e)
        {
            listView1.DataSource = data.ToList();
            button2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(keyword.Text != "" && program.Text != "")
            {
                if(data.ContainsKey(keyword.Text))
                {
                    MessageBox.Show("The keyword has already exists!");
                }
                else
                {
                    data.Add(keyword.Text, program.Text);
                    xml.Add(keyword.Text, program.Text);
                    refresh();
                }
            }
            else
            {
                MessageBox.Show("KeyWord or program cannot be empty!");
            }
        }

        private void refresh()
        {
            listView1.DataSource = data.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItem == null)
            {
                MessageBox.Show("You have to select a item!");
            }
            else
            {
                MessageBox.Show("delete!");
            }
        }

        private void program_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "可执行文件|*.exe";
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            openFileDialog1.FileOk += fileSelected;
        }

        private void fileSelected(object sender, EventArgs e)
        {
            program.Text = openFileDialog1.FileName;
        }
    }
}
