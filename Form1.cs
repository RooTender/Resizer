using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Sizer
{
    public partial class Form1 : Form
    {
        OpenFileDialog ofd = new OpenFileDialog();
        SaveFileDialog svd = new SaveFileDialog();

        enum sizes
        {
            _1920,
            _1080,
            _1024,
            _800
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Hide app
            this.ShowInTaskbar = false;
            Hide();
            
            //Notify about working
            this.notifyIcon1.BalloonTipText = "Sizer is working in background!";
            this.notifyIcon1.ShowBalloonTip(1000);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            openFile(sizes._1920);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            openFile(sizes._1080);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            openFile(sizes._1024);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            openFile(sizes._800);
        }

        private void openFile(sizes size)
        {
            ofd.Title = "Choose image to resize";
            ofd.Filter = "JPG Files|*.jpg";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string pathToFile = ofd.FileName;

                Bitmap BITMAP = new Bitmap(pathToFile);

                //if (File.Exists(pathToFile)) File.Delete(pathToFile);

                if (size == sizes._1920)
                {
                    BITMAP = ResizeImage(BITMAP, 1920, 1080);
                }
                if (size == sizes._1080)
                {
                    BITMAP = ResizeImage(BITMAP, 1280, 720);
                }
                if (size == sizes._1024)
                {
                    BITMAP = ResizeImage(BITMAP, 1024, 600);
                }
                if (size == sizes._800)
                {
                    BITMAP = ResizeImage(BITMAP, 800, 480);
                }

                notifyIcon1.BalloonTipTitle = "Resised!";
                notifyIcon1.BalloonTipText = "New image size is " + BITMAP.Width + " x " + BITMAP.Height;
                notifyIcon1.ShowBalloonTip(1000);

                try
                {
                    var bytes = ImageToByte(BITMAP);
                    File.WriteAllBytes("C:/Users/TEMP.VLO30-11.014/Desktop/Resized.jpg", bytes);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }

            ofd.Dispose();
        }

        private Bitmap ResizeImage(Image image, int width, int height) 
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
