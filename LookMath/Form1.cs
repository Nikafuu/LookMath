using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;

namespace LookMath
{
    public partial class LookMath : Form
    {
        //Public variables
        float MainPicturePositionX = 0, MainPicturePositionY = 0, MainPictureCellValue = 1;
        float MouseX = 0, MouseY = 0;
        bool MouseClicked = false;
        bool MenuPictureClicked = false;
        List<TextBox> MyFunctions = new List<TextBox>();
        int IndexLastFunctions = -1;
        byte[] raw_bmp_data;
        BitmapData bmp_BitmapData;
        string[] RealString = new string[15];
        float[,,] Fl = new float[15, 2000, 2];
        int IndexFl = 0;
        int[,] MyColors = new int[15, 3];
        int CountValue2 = 0;
        int CountValue5 = 0;
        //



        //Initialization application
        public LookMath()
        {
            InitializeComponent();
            this.MouseWheel += PictureMouseWheel;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateTextBox();
            MainPicture_resize();
            GroupFunctions_resize();
            Coordinate_relocate();
            MyColors[0, 2] = 255;
            MyColors[1, 1] = 255;
            MyColors[2, 0] = 255;
            MyColors[3, 0] = 255;
            MyColors[3, 1] = 255;
            MyColors[4, 0] = 255;
            MyColors[4, 2] = 255;
            MyColors[5, 1] = 255;
            MyColors[5, 2] = 255;
            MyColors[6, 0] = 127;
            MyColors[6, 1] = 127;
            MyColors[6, 2] = 127;
            MyColors[7, 0] = 48;
            MyColors[7, 1] = 64;
            MyColors[7, 2] = 128;
            for (int i = 8; i < 15; ++i)
            {
                MyColors[i, 0] = MyColors[i % 8, 0];
                MyColors[i, 1] = MyColors[i % 8, 1];
                MyColors[i, 2] = MyColors[i % 8, 2];
            }
        }
        private void LookMath_Resize(object sender, EventArgs e)
        {
            MainPicture_resize();
            GroupFunctions_resize();
            Coordinate_relocate();
        }
        //



        //Rendering "MainPicture"
        private void MainPicture_Paint(object sender, PaintEventArgs e)
        {
            Pen PenForUsualLines = new Pen(Color.Gray, 1);
            CheckingLastFunctions();
            float CellSize = 30;
            float X, Y;
            Bitmap bmp = new Bitmap(MainPicture.Width, MainPicture.Height);
            int stride_bytes;
            {
                Rectangle FullWindowRactangle = new Rectangle(0, 0, bmp.Width, bmp.Height);
                bmp_BitmapData = bmp.LockBits(FullWindowRactangle, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                stride_bytes = bmp_BitmapData.Stride;
                int numBytes = bmp_BitmapData.Height * stride_bytes;
                raw_bmp_data = new byte[numBytes];
            }

            

            int lHeight = MainPicture.Height;
            int lWidth = MainPicture.Width;
            IndexFl = 0;
            for (int j = 0; j < lHeight; ++j)
            {
                for (int i = 0; i < lWidth; ++i)
                {
                    raw_bmp_data[j * stride_bytes + i * 3 + 0] = 255;
                    raw_bmp_data[j * stride_bytes + i * 3 + 1] = 255;
                    raw_bmp_data[j * stride_bytes + i * 3 + 2] = 255;
                    X = (i - lWidth / 2 - MainPicturePositionX) / CellSize * MainPictureCellValue;
                    Y = (-j + lHeight / 2 - MainPicturePositionY) / CellSize * MainPictureCellValue;
                    if ((i - (int)(lWidth / 2) - (int)(MainPicturePositionX) % 30) % 30 == 0 || (-j + (int)(lHeight / 2) - (int)(MainPicturePositionY) % 30) % 30 == 0)
                    {
                        raw_bmp_data[j * stride_bytes + i * 3 + 0] = 128;
                        raw_bmp_data[j * stride_bytes + i * 3 + 1] = 128;
                        raw_bmp_data[j * stride_bytes + i * 3 + 2] = 128;
                    }
                    if (X == 0 || Y == 0)
                    {
                        raw_bmp_data[j * stride_bytes + i * 3 + 0] = 0;
                        raw_bmp_data[j * stride_bytes + i * 3 + 1] = 0;
                        raw_bmp_data[j * stride_bytes + i * 3 + 2] = 0;
                    }
                    for (int index = 0; index <= IndexLastFunctions; ++index) {
                        if (PaintThisFunction(RealString[index], X, Y, index) && i != 0 && j != 0 && i != lWidth - 1 && j != lHeight - 1)
                        {
                            raw_bmp_data[j * stride_bytes + i * 3 + 0] >>= 2;
                            raw_bmp_data[j * stride_bytes + i * 3 + 1] >>= 2;
                            raw_bmp_data[j * stride_bytes + i * 3 + 2] >>= 2;
                            raw_bmp_data[j * stride_bytes + i * 3 + 0] += Convert.ToByte(MyColors[index, 0] / 4 * 3);
                            raw_bmp_data[j * stride_bytes + i * 3 + 1] += Convert.ToByte(MyColors[index, 1] / 4 * 3);
                            raw_bmp_data[j * stride_bytes + i * 3 + 2] += Convert.ToByte(MyColors[index, 2] / 4 * 3);
                        }
                    }
                    ++IndexFl;
                    IndexFl %= (lWidth + 1);
                }
            }
            Marshal.Copy(raw_bmp_data, 0, bmp_BitmapData.Scan0, bmp_BitmapData.Stride * bmp_BitmapData.Height);
            bmp.UnlockBits(bmp_BitmapData);
            MainPicture.Image = bmp;
        }
        public bool PaintThisFunction(string F, float x, float y, int index)
        {
            int lWidth = MainPicture.Width + 1;
            string l = "", r = "";
            bool b = false;
            bool f = false;
            int w = 0;
            for (int i = 0; i < F.Length; ++i) 
            {
                if (F[i] == '=' || F[i] == '>' || F[i] == '<') 
                {
                    if (F[i] == '>')
                    {
                        w = 1;
                    }
                    if (F[i] == '<')
                    {
                        w = 2;
                    }
                    if (f) 
                    {
                        return false;
                    }
                    b = true;
                    continue;
                }
                if (b)
                {
                    if (F[i] != ' ') 
                    {
                        f = true;
                    }
                    r += F[i];
                }
                else
                {
                    l += F[i];
                }
            }
            if (b == false) 
            {
                return false;
            }
            l += '#';
            r += '#';
            int[] i_1 = { 0 };
            int[] pol1 = { 1 };
            float zn1 = Add(l, i_1, x, y, pol1);
            int[] i_2 = { 0 };
            int[] pol2 = { 1 };
            float zn2 = Add(r, i_2, x, y, pol2);
            if (w == 1 && zn1 > zn2 && pol1[0] == 1 && pol2[0] == 1) 
            {
                return true;
            }
            if (w == 2 && zn1 < zn2 && pol1[0] == 1 && pol2[0] == 1)
            {
                return true;
            }
            Fl[index, (IndexFl + 1) % lWidth, 0] = zn1 - zn2;
            Fl[index, (IndexFl + 1) % lWidth, 1] = pol1[0] + pol2[0];
            if (w == 0 && (zn1 == zn2 && pol1[0] == 1 && pol2[0] == 1 || Fl[index, IndexFl, 0] * Fl[index, (IndexFl + 1) % lWidth, 0] < 0 && Fl[index, (IndexFl + 1) % lWidth, 1] == 2 && Fl[index, IndexFl, 1] == 2 || Fl[index, (IndexFl + 2) % lWidth, 0] * Fl[index, (IndexFl + 1) % lWidth, 0] < 0 && Fl[index, (IndexFl + 1) % lWidth, 1] == 2 && Fl[index, (IndexFl + 2) % lWidth, 1] == 2))
            {
                return true;
            }
            return false;
        }
        public float Factory(string s, int[] i_, float x, float y, int[] pol)
        {
            while (s[i_[0]] == ' ')
            {
                ++i_[0];
            }
            if (s[i_[0]] == 'x')
            {
                ++i_[0];
                while (s[i_[0]] == ' ')
                {
                    ++i_[0];
                }
                return x;
            }
            if (s[i_[0]] == 'y')
            {
                ++i_[0];
                while (s[i_[0]] == ' ')
                {
                    ++i_[0];
                }
                return y;
            }
            if (s[i_[0]] == 's' && s[i_[0] + 1] == 'i' && s[i_[0] + 2] == 'n')
            {
                i_[0] += 3;
                return (float)Math.Sin(Factory(s, i_, x, y, pol));
            }
            if (s[i_[0]] == 'c' && s[i_[0] + 1] == 'o' && s[i_[0] + 2] == 's')
            {
                i_[0] += 3;
                return (float)Math.Cos(Factory(s, i_, x, y, pol));
            }
            if (s[i_[0]] == 't' && s[i_[0] + 1] == 'g')
            {
                i_[0] += 2;
                float temp = Factory(s, i_, x, y, pol);
                if (temp / Math.Acos(0) == (int)(temp / Math.Acos(0)))
                {
                    pol[0] = 0;
                    return 0;
                }
                return (float)Math.Tan(temp);
            }
            if (s[i_[0]] == 'c' && s[i_[0] + 1] == 't' && s[i_[0] + 2] == 'g')
            {
                i_[0] += 3;
                float temp = 1 / (float)Math.Tan(Factory(s, i_, x, y, pol));
                if (temp == float.NaN)
                {
                    pol[0] = 0;
                }
                return temp;
            }
            if (s[i_[0]] >= '0' && s[i_[0]] <= '9')
            {
                float temp = 0;
                int z = 0;
                while (s[i_[0]] >= '0' && s[i_[0]] <= '9' || s[i_[0]] == '.')
                {
                    if (s[i_[0]] != '.')
                    {
                        if (z == 0)
                        {
                            temp *= 10;
                            temp += s[i_[0]] - '0';
                        }
                        else
                        {
                            temp += (float)(Convert.ToDouble(s[i_[0]] - '0') / Math.Pow(10.0, i_[0] - z));
                        }
                    }
                    else
                    {
                        z = i_[0];
                    }
                    ++i_[0];
                }
                while (s[i_[0]] == ' ')
                {
                    ++i_[0];
                }
                return temp;
            }
            if (s[i_[0]] == '(')
            {
                ++i_[0];
                float res = Add(s, i_, x, y, pol);
                if (s[i_[0]] == ')')
                {
                    ++i_[0];
                }
                else
                {
                    pol[0] = 0;
                    i_[0] = s.Length - 1;
                }
                while (s[i_[0]] == ' ')
                {
                    ++i_[0];
                }
                return res;
            }
            if (s[i_[0]] == '-')
            {
                ++i_[0];
                return -Factory(s, i_, x, y, pol);
            }
            if (s[i_[0]] == '+')
            {
                ++i_[0];
                return Factory(s, i_, x, y, pol);
            }
            if (s[i_[0]] == '*' || s[i_[0]] == '/')
            {
                i_[0] = s.Length - 1;
            }
            if (s[i_[0]] == ')')
            {
                return 0;
            }
            pol[0] = 0;
            i_[0] = s.Length - 1;
            return 0;
        }
        public float Mult(string s, int[] i_, float x, float y, int[] pol)
        {
            float res = Factory(s, i_, x, y, pol);
            while (s[i_[0]] == '*' || s[i_[0]] == '/')
            {
                int j = i_[0];
                ++i_[0];
                float temp = Factory(s, i_, x, y, pol);
                if (s[j] == '*')
                {
                    res *= temp;
                }
                else
                {
                    if (Math.Abs(temp) < 0.001)
                    {
                        pol[0] = 0;
                        return 0;
                    }
                    else
                    {
                        res /= temp;
                    }
                }
            }
            return res;
        }
        public float Add(string s, int[] i_, float x, float y, int[] pol)
        {
            float res = Mult(s, i_, x, y, pol);
            while (s[i_[0]] == '-' || s[i_[0]] == '+')
            {
                int j = i_[0];
                ++i_[0];
                float temp = Mult(s, i_, x, y, pol);
                if (s[j] == '+')
                {
                    res += temp;
                }
                else
                {
                    res -= temp;
                }
            }
            return res;
        }
        //



        //Moving the mouse
        private void MainPicture_MouseUp(object sender, MouseEventArgs e)
        {
            MouseClicked = false;
        }
        private void MainPicture_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseClicked)
            {
                MainPicturePositionX -= MouseX - e.X;
                MainPicturePositionY += MouseY - e.Y;
                MouseX = e.X;
                MouseY = e.Y;
            }
            if (CountValue2 <= -13 && CountValue2 <= -13)
            {
                Coordinate.Text = Convert.ToString(Math.Round((e.X - MainPicture.Width / 2 - MainPicturePositionX) / 30.0f * MainPictureCellValue, 14)) + "; " + Convert.ToString(Math.Round((-e.Y + MainPicture.Height / 2 - MainPicturePositionY) / 30.0f * MainPictureCellValue, 14));
                return;
            }
            if (CountValue2 <= -7 && CountValue2 <= -7)
            {
                Coordinate.Text = Convert.ToString(Math.Round((e.X - MainPicture.Width / 2 - MainPicturePositionX) / 30.0f * MainPictureCellValue, 8)) + "; " + Convert.ToString(Math.Round((-e.Y + MainPicture.Height / 2 - MainPicturePositionY) / 30.0f * MainPictureCellValue, 8));
                return;
            }
            if (CountValue2 <= -5 && CountValue2 <= -5)
            {
                Coordinate.Text = Convert.ToString(Math.Round((e.X - MainPicture.Width / 2 - MainPicturePositionX) / 30.0f * MainPictureCellValue, 5)) + "; " + Convert.ToString(Math.Round((-e.Y + MainPicture.Height / 2 - MainPicturePositionY) / 30.0f * MainPictureCellValue, 5));
                return;
            }
            if (CountValue2 <= -3 && CountValue2 <= -3)
            {
                Coordinate.Text = Convert.ToString(Math.Round((e.X - MainPicture.Width / 2 - MainPicturePositionX) / 30.0f * MainPictureCellValue, 4)) + "; " + Convert.ToString(Math.Round((-e.Y + MainPicture.Height / 2 - MainPicturePositionY) / 30.0f * MainPictureCellValue, 4));
                return;
            }
            if (CountValue2 <= -1 && CountValue2 <= -1)
            {
                Coordinate.Text = Convert.ToString(Math.Round((e.X - MainPicture.Width / 2 - MainPicturePositionX) / 30.0f * MainPictureCellValue, 2)) + "; " + Convert.ToString(Math.Round((-e.Y + MainPicture.Height / 2 - MainPicturePositionY) / 30.0f * MainPictureCellValue, 2));
                return;
            }
            Coordinate.Text = Convert.ToString(Math.Round((e.X - MainPicture.Width / 2 - MainPicturePositionX) / 30.0f * MainPictureCellValue, 1)) + "; " + Convert.ToString(Math.Round((-e.Y + MainPicture.Height / 2 - MainPicturePositionY) / 30.0f * MainPictureCellValue, 1));
        }
        private void MainPicture_MouseDown(object sender, MouseEventArgs e)
        {
            MouseClicked = true;
            MouseX = e.X;
            MouseY = e.Y;
        }
        //



        //Click mouse
        private void MenuPicture_Click(object sender, EventArgs e)
        {
            if (MenuPictureClicked)
            {
                MenuPictureClicked = false;
            }
            else
            {
                MenuPictureClicked = true;
            }
            if (MenuPictureClicked)
            {
                MenuPicture.Location = new Point(GroupFunctions.Width + 10, MenuPicture.Location.Y);
                MenuPicture.Text = "<<";
                GroupFunctions.Visible = true;
            }
            else
            {
                MenuPicture.Location = new Point(10, MenuPicture.Location.Y);
                MenuPicture.Text = ">>";
                GroupFunctions.Visible = false;
            }

        }
        //



        //Scroll mouse
        public void PictureMouseWheel(object sender, MouseEventArgs e)
        {
            int kol = -e.Delta / 120;
            if (kol < 0)
            {
                if (CountValue2 == -15 && CountValue5 == -15) 
                {
                    kol = 0;
                }
                while (kol != 0)
                {
                    if (CountValue2 == CountValue5)
                    {
                        --CountValue2;
                    } 
                    else if (CountValue2 < CountValue5)
                    {
                        ++CountValue2;
                        --CountValue5;
                    }
                    else if (CountValue2 > CountValue5)
                    {
                        --CountValue2;
                    }
                    ++kol;
                }
            }
            else
            {
                if (CountValue2 == 15 && CountValue5 == 15)
                {
                    kol = 0;
                }
                while (kol != 0)
                {
                    if (CountValue2 == CountValue5)
                    {
                        ++CountValue2;
                    }
                    else if (CountValue2 > CountValue5)
                    {
                        --CountValue2;
                        ++CountValue5;
                    }
                    else if (CountValue2 < CountValue5)
                    {
                        ++CountValue2;
                    }
                    --kol;
                }
            }
            MainPictureCellValue = (float)(Math.Pow(2, CountValue2) * Math.Pow(5, CountValue5));
        }
        //



        //Working with dynamic functions
        public void CreateTextBox()
        {
            ++IndexLastFunctions;
            MyFunctions.Add(new TextBox());
            MyFunctions[IndexLastFunctions].Size = new Size(GroupFunctions.Width - 5, 30);
            MyFunctions[IndexLastFunctions].Font = new Font("Arial", 13);
            MyFunctions[IndexLastFunctions].Multiline = true;
            MyFunctions[IndexLastFunctions].TextAlign = HorizontalAlignment.Center;
            GroupFunctions.Controls.Add(MyFunctions[IndexLastFunctions]);
        }
        public void DeleteTextBox()
        {
            GroupFunctions.Controls.Remove(MyFunctions[IndexLastFunctions]);
            MyFunctions.Remove(MyFunctions[IndexLastFunctions]);
            --IndexLastFunctions;
        }

        public void CheckingLastFunctions() {
            if (MyFunctions[IndexLastFunctions].Text != "" && IndexLastFunctions != 14) {
                CreateTextBox();
            }
            if (IndexLastFunctions != 0) 
            {
                if (MyFunctions[IndexLastFunctions - 1].Text == "") 
                {
                    DeleteTextBox();
                }
            }
            for (int i = 0; i <= IndexLastFunctions; ++i) 
            {
                RealString[i] = MyFunctions[i].Text;
            }
        }



        //
        


        //Re... Elements
        public void MainPicture_resize() 
        {
            MainPicture.Size = new Size(this.Width - 10, this.Height - 20);
        }
        public void GroupFunctions_resize()
        {
            GroupFunctions.Size = new Size(GroupFunctions.Width, this.Height);
        }
        public void Coordinate_relocate()
        {
            Coordinate.Location = new Point(MainPicture.Width - 30 - Coordinate.Width, MainPicture.Height - 30 - Coordinate.Height);
        }
        //
    }
}
