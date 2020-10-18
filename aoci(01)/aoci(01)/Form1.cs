using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace aoci_01_
{
    public partial class Form1 : Form
    {
        private Image<Bgr, byte> sourceImage; //глобальная переменная

        private VideoCapture capture;

        int frameCount = 0;

        private double cannyThreshold;
        private double cannyThresholdLinking;

        private universal universal = new universal();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sourceImage = universal.openImg();
            imageBox1.Image = sourceImage;
        }

        private void process_Click(object sender, EventArgs e)
        {
            imageBox2.Image = universal.effect(sourceImage, cannyThreshold, cannyThresholdLinking); //.Resize(640, 480, Inter.Linear)
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // инициализация веб-камеры
            capture = new VideoCapture();
            capture.ImageGrabbed += ProcessFrame;
            capture.Start(); // начало обработки видеопотока
        }

        // захват кадра из видеопотока
        private void ProcessFrame(object sender, EventArgs e)
        {
            var frame = new Mat();
            capture.Retrieve(frame); // получение текущего кадра

            Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();

            //Image<Gray, byte> grayImage = image.Convert<Gray, byte>();
            //var tempImage = grayImage.PyrDown();
            //var destImage = tempImage.PyrUp();
            //double cannyThreshold = 80.0;
            //double cannyThresholdLinking = 40.0;
            //Image<Gray, byte> cannyEdges = destImage.Canny(cannyThreshold, cannyThresholdLinking);
            //imageBox2.Image = cannyEdges; //.Resize(640, 480, Inter.Linear)
            imageBox1.Image = image; //вывод кадра в нужном окне
            sourceImage = image;
            imageBox2.Image = universal.effect(sourceImage, cannyThreshold, cannyThresholdLinking);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\Users\\click\\Documents\\2020-2021\\AOCI\\01\\";
            //openFileDialog.Filter = "mp4 files (*.mp4, *.avi) | *.mpa; *.avi; | All files (*.*)|*.*";
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла

            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;

                // инициализация веб-камеры
                capture = new VideoCapture(fileName);
                capture.ImageGrabbed += ProcessFrame;
                capture.Start(); // начало обработки видеопотока

                timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var frame = capture.QueryFrame();

            imageBox2.Image = frame; //.Resize(640, 480, Inter.Linear)

            frameCount++;

            if (frameCount >= capture.GetCaptureProperty(CapProp.FrameCount))
                timer1.Enabled = false;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            cannyThreshold = vScrollBar1.Value;
            textBox1.Text = vScrollBar1.Value.ToString();
            imageBox2.Image = universal.effect(sourceImage, cannyThreshold, cannyThresholdLinking);
        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            cannyThresholdLinking = vScrollBar2.Value;
            textBox2.Text = vScrollBar2.Value.ToString();
            imageBox2.Image = universal.effect(sourceImage, cannyThreshold, cannyThresholdLinking);
        }

        private void vScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            universal.a = vScrollBar3.Value;
            textBox3.Text = vScrollBar3.Value.ToString();
            imageBox2.Image = universal.effect(sourceImage, cannyThreshold, cannyThresholdLinking);
        }
    }
}
