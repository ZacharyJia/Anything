using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace MyRun
{
    public partial class RunForm : Form
    {
        XMLTool xml;
        Dictionary<String, String> data;

        public RunForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             xml = new XMLTool("data.xml");
             data = xml.getData();
             this.AcceptButton = BtnRun;
             AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
             autoCompleteCollection.AddRange(data.Keys.ToArray());
             KeyWord.AutoCompleteCustomSource = autoCompleteCollection;
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            if(KeyWord.Text != "")
            {
                foreach(var item in data)
                {
                    if(item.Key == KeyWord.Text)
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo(item.Value);
                        Process.Start(startInfo);
                        KeyWord.Text = "";
                        this.Visible = false;
                    }
                }
            }
        }

        private void BtnSetting_Click(object sender, EventArgs e)
        {
            new programList().Show();
            this.Visible = false;
        }

        public void clearInput()
        {
            KeyWord.Text = "";
        }

        private void Run_Activated(object sender, EventArgs e)
        {
            KeyWord.Focus();
            xml = new XMLTool("data.xml");
            data = xml.getData();
            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
            autoCompleteCollection.AddRange(data.Keys.ToArray());
            KeyWord.AutoCompleteCustomSource = autoCompleteCollection;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

    }
}
