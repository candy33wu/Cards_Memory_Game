using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hw5
{
    public partial class Form1 : Form
    {
        Pen pen1 = new Pen(Color.Black, 2);
        Rectangle[] rectangle = new Rectangle[16];
        Random RD;
        int counter = 0;
        int difference = -1;
        int back = 16;
        int now = 16;
        int time = 0;//點擊次數
        int[] NullPick= { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//0:表沒選(填入)
        int[] imgNum = new int[16];
        Bitmap img0 = new Bitmap(Properties.Resources._8_0);//封面
        Bitmap[] img = new Bitmap[16] { Properties.Resources._8_1, Properties.Resources._8_2,Properties.Resources._8_3, Properties.Resources._8_4,Properties.Resources._8_5,Properties.Resources._8_6, Properties.Resources._8_7, Properties.Resources._8_8, Properties.Resources._8_1,  Properties.Resources._8_2,  Properties.Resources._8_3, Properties.Resources._8_4, Properties.Resources._8_5,   Properties.Resources._8_6, Properties.Resources._8_7,  Properties.Resources._8_8 };
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            int x = 10;
            int y = 30;
            for (int i = 0; i < 16; i++)
            {
                if (i % 4 != 0)
                {
                    rectangle[i] = new Rectangle(x + (i % 4) * 70, y, 70, 70);
                }
                else
                {
                    if (i == 0)
                        y = 30;
                    else
                        y += 70;
                    rectangle[i] = new Rectangle(x, y, 70, 70);
                }
            }
            RD = new Random();

            for (int i = 0; i < 16; i++)
            {
                imgNum[i] = RD.Next(16);//隨機取16以下的整數
                while (NullPick[imgNum[i]] == 1)//圖片已被放入
                {
                    imgNum[i] = RD.Next(16);
                }
                NullPick[imgNum[i]] = 1;
            }

        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
            for (int i = 0; i < 16; i++)//畫覆蓋牌
            {
                e.Graphics.DrawRectangle(pen1, rectangle[i]);
                Rectangle rect = new Rectangle(rectangle[i].X, rectangle[i].Y, 70, 70);
                e.Graphics.DrawImage(img0, rect);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Stop();
            Size = new Size(450, 450);
          
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (difference == -1)
            {
                for (int i = 0; i < 16; ++i)
                {
                    if (rectangle[i].Contains(e.Location) && NullPick[i] == 1)
                    {
                        now = i;
                        if (time == 0)
                        {
                            timer1.Start();
                        }
                        time++;
                        if (time % 2 == 1)//翻第一張牌
                        {
                            Rectangle rectDest1 = new Rectangle(rectangle[i].X, rectangle[i].Y, 67, 67);
                            this.CreateGraphics().DrawImage(img[imgNum[i]], rectDest1);
                            difference = -1;//不被覆蓋
                            NullPick[i] = 2;//已點選過或配對成功
                            back = i;
                        }
                        if (time % 2 == 0)//比對前一張牌
                        {
                            Rectangle rect = new Rectangle(rectangle[back].X, rectangle[back].Y, 67, 67);
                            this.CreateGraphics().DrawImage(img[imgNum[back]], rect);
                            rect = new Rectangle(rectangle[i].X, rectangle[i].Y, 67, 67);
                            this.CreateGraphics().DrawImage(img[imgNum[i]], rect);

                            if ((imgNum[back] <= 7 && imgNum[back] + 8 != imgNum[i]) || (imgNum[back] > 7 && imgNum[back] - 8 != imgNum[i]))
                            {
                                difference = counter;
                                NullPick[i] = 1;//可再被選
                                NullPick[back] = 1;
                            }
                            else
                            {
                                NullPick[i] = 2;
                                NullPick[back] = 2;
                                difference = -1;
                                back = 16;//歸零
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; ++i) {
                if (NullPick[i] != 2)
                {
                    break;
                }
                if (i == 15)
                    timer1.Stop();
            }
            counter++;
            if (difference != -1 && counter == difference + 1)
            {
                Rectangle rect = new Rectangle(rectangle[now].X, rectangle[now].Y, 67, 67);
                this.CreateGraphics().DrawImage(img0, rect);
                rect = new Rectangle(rectangle[back].X, rectangle[back].Y, 67, 67);
                this.CreateGraphics().DrawImage(img0, rect);
                difference = -1;
                back = 16;
            }
            toolStripLabel1.Text = " " + counter + " Seconds";
        }

        private void RestartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            counter = 0;
            difference = -1;
            back = 16;
            now = 16;
            time = 0;//點擊次數
            for (int i = 0; i < 16; ++i) {
                NullPick[i] = 0;
            }
            RD = new Random();
            for (int i = 0; i < 16; i++)
            {
                imgNum[i] = RD.Next(16);//隨機取16以下的整數
                while (NullPick[imgNum[i]] == 1)//圖片已被放入
                {
                    imgNum[i] = RD.Next(16);
                }
                NullPick[imgNum[i]] = 1;
            }
            for (int i = 0; i < 16; i++)
            {
                this.CreateGraphics().DrawRectangle(pen1, rectangle[i]);
                Rectangle rect = new Rectangle(rectangle[i].X, rectangle[i].Y, 70, 70);
                this.CreateGraphics().DrawImage(img0, rect);
            }
            timer1.Stop();
            toolStripLabel1.Text = " 0 Seconds";
        }
    }
}
