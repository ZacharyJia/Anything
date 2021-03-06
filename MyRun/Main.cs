﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.IO;
using System.Threading;

namespace MyRun
{
    public partial class Main : Form
    {
        //注册热键的api
        [DllImport("user32")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint control, Keys vk);
        //解除注册热键的api
        [DllImport("user32")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        //设置活动窗体
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        Point mouseOff;
        bool leftFlag;


        //the instance of runForm
        RunForm run;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;

            //Build a context menu of the winform
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menu_setting = new MenuItem("Setting");
            MenuItem menu_quit = new MenuItem("Quit");
            contextMenu.MenuItems.Add(menu_setting);
            contextMenu.MenuItems.Add(menu_quit);
            this.ContextMenu = contextMenu;
            notifyIcon1.ContextMenu = contextMenu;
            //bind the menu and its event handler
            menu_quit.Click += quit;
            menu_setting.Click += setting;

            //注册热键 (窗体句柄,热键ID,辅助键,实键) 
            //辅助键说明: None = 0,   Alt = 1,  crtl= 2,  Shift = 4,   Windows = 8
            //如果有多个辅助键则,例如 alt+crtl是3 直接相加就可以了
            RegisterHotKey(this.Handle, 123, 1, Keys.R);
            run = new RunForm();

            int x = Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
            this.SetDesktopLocation(x, y);

            //synth.Speak("鹏神哈哈哈哈!");

            ThreadStart threadStart = new ThreadStart(loop);
            Thread thread = new Thread(threadStart);
            thread.Start();

        }

        //新线程循环获取内容并朗读
        private void loop()
        {
            while (true)
            {
                SpeechSynthesizer synth = new SpeechSynthesizer();
                //synth.Speak("知行论坛有新消息！");
                string news = getZhixingNews();
                news = news.Replace("\n", "");
                if (news != "")
                {
                    synth.Speak("知行论坛有新帖：");
                    synth.Speak(news);
                }
                Thread.Sleep(1000 * 10);
            }
        }

        //从树莓派获取知行论坛数据
        private string getZhixingNews()
        {
            var response = Http.CreateGetHttpResponse("http://192.168.42.1/zhixing.php", null, null, null);
            if (response != null)
            {
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            else
            {
                return "";
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnregisterHotKey(this.Handle, 123);
        }

        //处理快捷键消息
        protected override void WndProc(ref Message m)
        {
            switch(m.Msg)
            {
                case 0x0312:
                    if(m.WParam.ToString() == "123")
                    {
                        if(run.IsDisposed)
                        {
                            run = new RunForm();
                            run.Show();
                        }
                        else
                        {
                            run.Visible = !run.Visible;
                        }
                        if(run.Visible)
                        {
                            SetForegroundWindow(run.Handle);
                            run.clearInput();
                        }
                    }
                    break;
            }
            base.WndProc(ref m);
        }


        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);
                leftFlag = true;
            }
        }

        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if(leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);
                Location = mouseSet;
            }
        }

        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            if(leftFlag)
            {
                leftFlag = false;
            }
        }

        private void quit(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void setting(object sender, EventArgs e)
        {
            new programList().Show();
        }

        private void notifyIcon1_DoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = !this.Visible;
        }


    }
}
