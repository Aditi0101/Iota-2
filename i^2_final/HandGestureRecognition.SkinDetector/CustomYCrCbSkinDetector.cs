using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace HandGestureRecognition.SkinDetector
{
    class CustomYCrCbSkinDetector : IColorSkinDetector
    {
        public override Image<Gray, byte> DetectSkin(Image<Bgr, byte> Img, IColor min, IColor max)
        {
            //Code adapted from here
            // http://blog.csdn.net/scyscyao/archive/2010/04/09/5468577.aspx
            // Look at this paper for reference (Chinese!!!!!)
            // http://www.chinamca.com/UploadFile/200642991948257.pdf

            Image<Ycc, Byte> currentYCrCbFrame = Img.Convert<Ycc, Byte>();
            Image<Gray, Byte> skin = new Image<Gray, Byte>(Img.Width, Img.Height);

            int y, cr, cb, l, x1, y1, value;

            int rows = Img.Rows;
            int cols = Img.Cols;
            Byte[, ,] YCrCbData = currentYCrCbFrame.Data;
            Byte[, ,] skinData = skin.Data;

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    y = YCrCbData[i, j, 0];
                    cr = YCrCbData[i, j, 1];
                    cb = YCrCbData[i, j, 2];

                    cb -= 109;
                    cr -= 152;
                    x1 = (819 * cr - 614 * cb) / 32 + 51;
                    y1 = (819 * cr + 614 * cb) / 32 + 77;
                    x1 = x1 * 41 / 1024;
                    y1 = y1 * 73 / 1024;
                    value = x1 * x1 + y1 * y1;
                    if (y < 100)
                        skinData[i, j, 0] = (value < 700) ? (byte)255 : (byte)0;
                    else
                        skinData[i, j, 0] = (value < 850) ? (byte)255 : (byte)0;

                }
            CvInvoke.Erode(skin, skin, null, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
            // CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(12, 12), new Point(6, 6));
            CvInvoke.Dilate(skin, skin, null, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
            return skin;
        }
    }
}
