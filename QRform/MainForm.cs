using System;
using System.Drawing;
using System.Windows.Forms;
using QRLibrary;

namespace QRform
{
    public partial class MainForm : Form
    {
        QRcode QR;
        bool CodeExist;
        string Request;
        Bitmap bmp;
        public MainForm()
        {
            CodeExist = false;
            InitializeComponent();
        }
        private void CorrectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            QRBox.Focus();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            Request = RequestTextBox.Text;
            var level = CorrectionComboBox.SelectedIndex;
            if (Request == null)
                MessageBox.Show("Input text.");
            else if (level < 0)
                MessageBox.Show("Choose correction level.");
            else
            {
                QR = new QRcode(Request, level);
                if (QR.Mask != -1)
                { 
                    bmp = new Bitmap(QR.Side * 5, QR.Side * 5);
                for (int i = 0; i < QR.Side; i++)
                {
                    for (int j = 0; j < QR.Side; j++)
                        if (QR.Matrix[i, j] == 1)
                            DrawPixelSquare(i * 5, j * 5, 5, bmp, Color.Black);
                        else
                            DrawPixelSquare(i * 5, j * 5, 5, bmp, Color.White);
                }
                CodeExist = true;
                QRBox.Image = bmp;
                }
                else
                    if(level!=3)
                        MessageBox.Show("Your request exceeded the maximum possible amount of data for current correction level.");
                    else
                        MessageBox.Show("Your request exceeded the maximum possible amount of data for QR-code.");
            }
        }

        private void DrawPixelSquare(int x, int y, int lenght,Bitmap bmp,Color color)
        {
            for (int i = 0; i < lenght; i++)
            {
                for (int j = 0; j < lenght; j++)
                    bmp.SetPixel(x+i, y+j, color);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(CodeExist)
            {
                if(Request== RequestTextBox.Text)
                {
                    SaveFileDialog save = new SaveFileDialog(); // save будет запрашивать у пользователя, место, в которое он захочет сохранить файл. 
                    save.Filter = "PNG|*.png|JPEG|*.jpg|GIF|*.gif|BMP|*.bmp"; //создаём фильтр, который определяет, в каких форматах мы сможем сохранить нашу информацию. В данном случае выбираем форматы изображений. Записывается так: "название_формата_в обозревателе|*.расширение_формата")
                    if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK) //если пользователь нажимает в обозревателе кнопку "Сохранить".
                    {
                        QRBox.Image.Save(save.FileName); //изображение из pictureBox'a сохраняется под именем, которое введёт пользователь
                    }
                }
                else
                    MessageBox.Show("You changed text. Create new code.");
            }
            else
                MessageBox.Show("Code doesn't exist.");
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (CodeExist)
            {
                if (Request == RequestTextBox.Text)
                {
                    Clipboard.SetImage(QRBox.Image);
                    MessageBox.Show("Code copied.");
                }
                else
                    MessageBox.Show("You changed text. Create new code.");
            }
            else
                MessageBox.Show("Code doesn't exist.");
        }
    }
}
