using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Odliczacz
{
    public partial class Form1 : Form
    {
        //definicja zmiennych dla min, sek i przycisku stop/start
        System.Timers.Timer t;
        double m, s;
        bool st = true;
        

        public Form1()
        {
            InitializeComponent();
            button3.Enabled = false; //wyszarzenie stop
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            t = new System.Timers.Timer();
            t.Interval = 1000; //1s
            t.Elapsed += OnTimeEvent;
        }
        private void playSimpleSound() //dzwiek po skonczeniu odliczania
        {
            SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\Windows Foreground.wav");
            simpleSound.Play();
        }

        private void OnTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                s -= 1;
                if (s == -1) //przekrecenie minuty
                {
                    s = 59;
                    m -= 1;
                }
                else if (m == 0 & s == 0) //eventy przy wyzerowaniu
                {
                    t.Stop();
                    playSimpleSound();
                    button1.Text = "Start";
                    button3.Enabled = false;
                    
                }
                //update zegara
                label3.Text = string.Format("{0}:{1}", m.ToString().PadLeft(2, '0'), s.ToString().PadLeft(2, '0'));
            }));
        }

        private void button1_Click(object sender, EventArgs e) //wypelnienie zegara danymi
        {
            m = double.Parse(textBox2.Text);
            s = double.Parse(textBox3.Text);
            if (m == 0 & s == 0)
            {
                MessageBox.Show("Nieprawidłowa wartość!");
                return;
            }

            if (s >= 60) //zamiana nadmiarowych sek na min
            {
                m = Math.Round(s / 60);
                s -= m * 60;
            }

            label3.Text = string.Format("{0}:{1}", m.ToString().PadLeft(2, '0'), s.ToString().PadLeft(2, '0'));
            t.Start();
            this.button1.Text = "Reset"; //zmiana start na reset
            st = true;
            button3.Text = "Stop"; //wlaczenie stop
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (st == true) //zmiana stanu przycisku stop/start
            {
                t.Stop();
                button3.Text = "Start";
                st = false;
            }
            else
            {
                t.Start();
                button3.Text = "Stop";
                st = true;
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Stop timer
            t.Stop();
            Application.DoEvents();
        }
    }
}
