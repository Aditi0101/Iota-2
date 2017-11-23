using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.Cvb;

namespace i_2_final
{
    class global
    {
        public static int mouse_x_cordinate;
        public static int mouse_y_cordinate;
        public static int gesture_number;
        public static int security_fag = 0;
        public static Capture capture = null;
        public static bool gesture = false;
    }
}
