using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace breakoutgame
{
    public partial class Form1 : Form
    {
        bool goright;
        bool goleft;
        bool isgameover;
        int score;
        int ballx;
        int bally;
        int playerspeed;
        Random rnd=new Random();
        PictureBox[] blockarray;
        SoundPlayer musicplayer = new SoundPlayer();
        


        public Form1()
        {

            InitializeComponent();
            placeblocks();
            
        }
        private void setupgame()
        {
            isgameover = false;
            score = 0;
            ballx = 5; 
            bally=5;
            playerspeed = 14;
            gametimer.Start();
            txtscore.Text = "score:" +" " +score;
            ball.Left = 376;
            ball.Top = 256;
            block.Left = 347;
            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && (string)x.Tag == "blocks")
                {
                    x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void Gameover(string message)
        {
            isgameover = true;
            gametimer.Stop();
            txtscore.Text = message;
        }
        private void placeblocks()
        {

            blockarray= new PictureBox[16];
            int a = 0;
            int top = 50;
            int left = 70;
            for(int i=0;i<blockarray.Length;i++)
            {
                blockarray[i] = new PictureBox();
                blockarray[i].Height = 25;
                blockarray[i].Width = 90;
                blockarray[i].Tag = "blocks";
                blockarray[i].BackColor = Color.White;
                if (a == 3)
                {
                    top = top + 50;
                    left = 70;
                    a = 0;
                    
                }
                if (a < 3)
                {
                    a++;
                    blockarray[i].Left = left;
                    blockarray[i].Top = top;
                    this.Controls.Add(blockarray[i]);
                    left = left + 130;
                }
            }
            setupgame();

        }
        private void removeblocks()
        {
            foreach(PictureBox x in blockarray)
            {
                this.Controls.Remove(x);
            }
        }
        private void maingametimerevent(object sender, EventArgs e)
        {
            txtscore.Text = "score:" + " " + score;
            if (goleft==true && block.Left > 0)
            {
                block.Left -=playerspeed;
            }
            if(goright==true && block.Left < 485)
            {
                block.Left +=playerspeed;
            }
            ball.Left += ballx;
            ball.Top += bally;
            if(ball.Left<0 || ball.Left > 550)
            {
                ballx=-ballx;
            }
            if (ball.Top < 0)
            {
                bally = -bally;
            }
            if (ball.Bounds.IntersectsWith(block.Bounds))
            {
                bally = rnd.Next(5, 12) * -1;
                if (ballx < 0)
                {
                    ballx = rnd.Next(5, 12) * -1;
                }
                else
                {
                    ballx = rnd.Next(5, 12);
                }
                
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "blocks")
                {
                    if (ball.Bounds.IntersectsWith(x.Bounds))
                    {
                        score += 1;
                        bally = -bally;
                        this.Controls.Remove(x);
                    }
                }
            }
            if (score == 16)
            {
                
                Gameover("you win the game"+" \n"+"enter to play again");
                musicplayer.SoundLocation = "clap.wav";

                musicplayer.Play();


            }
            if (ball.Top > 358)
            {
                Gameover("you lose the game"+" "+"\tyour score is"+" "+score+" \n"+"enter to play again");
                musicplayer.SoundLocation = "lose.wav";

                musicplayer.Play();
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Left)
                goleft= true;
            if(e.KeyCode==Keys.Right)
                goright= true;

        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                goleft = false;
            if (e.KeyCode == Keys.Right)
                goright = false;
            if(e.KeyCode == Keys.Enter && isgameover == true)
            {
                musicplayer.Stop();
                removeblocks();
                placeblocks();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void enter_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Name = Name;
            Properties.Settings.Default.Save();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}






