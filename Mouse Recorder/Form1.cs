using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(UInt16 virtualKeyCode);

        protected ListViewItem ActionList;
        int PlayFromList, RecToList;
        string lmc = "L_CLICK";
        string rmc = "R_CLICK";
        bool lastM1down = false;
        bool lastM2down = false;
        bool playHotkey = false;
        bool recHotkey = false;
        bool stopHotkey = false;
        public static int defaultspeed = 25;
        public static int speed = defaultspeed;
        public static Boolean repearttemp = false;
        public static Boolean repeart = repearttemp;
        public static int playRepeatValuedefault = 1;
        public static int playRepeatValue = playRepeatValuedefault;
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            PlayFromList = 0;
            RecToList = 0;
            timer_hotkeys.Start();
            string[] args = Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++)
            {
                if (i == 1 )
                {
                    Open(args[i].ToString());
                }
              
            }
       
           
        }


        private void timer1_Tick(object sender, EventArgs e)
        {

            ActionList = new ListViewItem(Cursor.Position.X.ToString());

            ActionList.SubItems.Add(Cursor.Position.Y.ToString());

            if (Control.MouseButtons == MouseButtons.Left)
            {
                ActionList.SubItems.Add(lmc.ToString());
            }
            else if (Control.MouseButtons == MouseButtons.Right)
            {
                ActionList.SubItems.Add(rmc.ToString());
            }
            else
            {
                ActionList.SubItems.Add("move".ToString());
            }

            listView1.Items.Add(ActionList);
            RecToList++;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(playRepeatValue);
            timer2.Interval = Decimal.ToInt32(speed);
            int X = int.Parse(listView1.Items[PlayFromList].SubItems[0].Text);
            int Y = int.Parse(listView1.Items[PlayFromList].SubItems[1].Text);
            uint dX = uint.Parse(listView1.Items[PlayFromList].SubItems[0].Text);
            uint dY = uint.Parse(listView1.Items[PlayFromList].SubItems[1].Text);

            if (PlayFromList != RecToList)
            {

                progressBar1.Increment(+1);

                Cursor.Position = new Point(X, Y);

                if (lastM1down == false)
                {
                    if (listView1.Items[PlayFromList].SubItems[2].Text == lmc)
                    {
                        mouse_event(MOUSEEVENTF_LEFTDOWN, dX, dY, 0, 0);
                        lastM1down = true;
                    }
                }
                else if (lastM1down == true)
                {
                    if (listView1.Items[PlayFromList].SubItems[2].Text == lmc)
                    {
                        lastM1down = true;
                    }
                    if (listView1.Items[PlayFromList].SubItems[2].Text == "move")
                    {
                        mouse_event(MOUSEEVENTF_LEFTUP, dX, dY, 0, 0);
                        lastM1down = false;
                    }
                }

                if (lastM2down == false)
                {
                    if (listView1.Items[PlayFromList].SubItems[2].Text == rmc)
                    {
                        mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, dX, dY, 0, 0);
                        lastM1down = true;
                    }
                }
                else if (lastM2down == true)
                {
                    if (listView1.Items[PlayFromList].SubItems[2].Text == rmc)
                    {
                        lastM2down = true;
                    }
                    if (listView1.Items[PlayFromList].SubItems[2].Text == "move")
                    {
                        mouse_event(MOUSEEVENTF_RIGHTUP, dX, dY, 0, 0);
                        lastM2down = false;
                    }
                }

                PlayFromList++;
            }

            if (PlayFromList == RecToList)
            {

                Console.WriteLine(repearttemp);
                if (repearttemp == true)
                {
                    PlayFromList = 0;
                    progressBar1.Value = 0;
                }
                else
                {
                    playRepeatValue--;

                    progressBar1.Value = 0;
                    if (playRepeatValue == 0)
                    {

                        mouse_event(MOUSEEVENTF_LEFTUP, dX, dY, 0, 0);
                        Thread.Sleep(50);
                        playRepeatValue = 1;
                        timer2.Stop();

                    }

                    PlayFromList = 0;
                }

            }

        }

        private void button_rec_Click(object sender, EventArgs e)
        {
            PlayFromList = 0;
            RecToList = 0;
            progressBar1.Value = 0;
            timer1.Stop();
            listView1.Items.Clear();

            timer1.Start();
            timer2.Stop();
        }


        private void button_stop_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            //playRepeatValue = playRepeatValuedefault;

            Console.WriteLine("button_stop_Click");
            Console.WriteLine(playRepeatValue);

            timer1.Stop();
            timer2.Stop();
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            if (Decimal.ToInt32(RecToList) > 0)
            {
                progressBar1.Value = 0;
                Console.WriteLine("button_play_Click");
                PlayFromList = 0;
                progressBar1.Maximum = RecToList;
                timer2.Start();
            }


        }


        private void button_clear_Click(object sender, EventArgs e)
        {
            PlayFromList = 0;
            RecToList = 0;
            timer1.Stop();
            timer2.Stop();
            listView1.Items.Clear();
            progressBar1.Value = 0;

        }



        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Console.WriteLine(path);
            System.Diagnostics.Process.Start(path);



        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new options();

            f.ShowDialog();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer_hotkeys_Tick(object sender, EventArgs e)
        {

            //if (ModifierKeys == Keys.Alt)
            //{
            //    if (playHotkey == false)
            //    {
            //        playHotkey = true;
            //    }
            //    else
            //    {
            //        playHotkey = false;
            //    }

            //    Thread.Sleep(50);

            //}

            //if (ModifierKeys == Keys.Control)
            //{
            //    if (recHotkey == false)
            //    {
            //        recHotkey = true;
            //    }
            //    else
            //    {
            //        recHotkey = false;
            //    }

            //    Thread.Sleep(50);

            //}

            if (ModifierKeys == Keys.Control)
            {
                Console.WriteLine("durdur");
                if (stopHotkey == false)
                {
                    repeart = false;
                    stopHotkey = true;
                }
                else
                {
                    stopHotkey = false;
                }

                Thread.Sleep(50);

            }

            if (playHotkey == true)
            {
                //playRepeatValue = decimal.ToInt32(replayRepeat);
                PlayFromList = 0;
                timer1.Stop();
                timer2.Start();

                playHotkey = false;
                recHotkey = false;
                stopHotkey = false;

                Thread.Sleep(50);
            }

            if (recHotkey == true)
            {
                timer1.Start();
                timer2.Stop();

                playHotkey = false;
                recHotkey = false;
                stopHotkey = false;

                Thread.Sleep(50);
            }

            if (stopHotkey == true)
            {
                timer1.Stop();
                timer2.Stop();

                playHotkey = false;
                recHotkey = false;
                stopHotkey = false;

                Thread.Sleep(50);
            }

        }


        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        public void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Mouse Dosyası|*.gsm";
            file.FilterIndex = 2;
            file.RestoreDirectory = true;
            file.CheckFileExists = false;
            file.Title = "kayıt Dosyası Seçiniz..";

            if (file.ShowDialog() == DialogResult.OK)
            {
                listView1.Items.Clear();
                Open(file.FileName);
            }
        }

        private void Open (string file){
            Console.WriteLine(file);
            if (File.Exists(file))
            {
                foreach (var item in File.ReadLines(file))
                {
                    Console.WriteLine(item);
                    string[] tokens = item.ToString().Split(' ');

                    ActionList = new ListViewItem(tokens[0]);

                    ActionList.SubItems.Add(tokens[1]);

                    if (tokens[2].ToString() == "1" && tokens[3].ToString() == "0")
                    {
                        ActionList.SubItems.Add(lmc.ToString());
                    }
                    else if (tokens[2].ToString() == "0" && tokens[3].ToString() == "1")
                    {
                        ActionList.SubItems.Add(rmc.ToString());
                    }
                    else
                    {
                        ActionList.SubItems.Add("move".ToString());
                    }
                    RecToList++;
                    listView1.Items.Add(ActionList);
                }
            }
            else
            {
                MessageBox.Show("Böyle Bir Dosya Yok");

            }


        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Mouse Dosyası|*.gsm";
            sfd.Filter = "Mouse File (.gsm) | *.gsm";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName.ToString();
                if (filename != "")
                {
                    using (StreamWriter sw = new StreamWriter(filename))
                    {
                        foreach (ListViewItem item in listView1.Items)
                        {
                            var clk = "0 0";

                            if (item.SubItems[2].Text == lmc)
                            {
                                clk = "1 0";
                            }
                            if (item.SubItems[2].Text == rmc)
                            {
                                clk = "0 1";
                            }
                            sw.WriteLine("{0}{1}{2}{3}{4}", item.SubItems[0].Text, " ", item.SubItems[1].Text, " ", clk);
                        }
                    }
                }
            }
        }


    }






}
