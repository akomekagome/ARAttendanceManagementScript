using System.IO;
using System.Runtime.InteropServices;
using OpenCvSharp;
using UnityEngine;

namespace ARAM.Main.OpenCv
{
    
    public class OpenCvGateway
    {
        private bool forceFrontalCamera = true;

        //TODO
        public void SaveJpegFromMat(Mat mat, string imagePath)
        {
            var dicPath = Path.GetDirectoryName(imagePath);
            if (dicPath != null && !Directory.Exists(dicPath))
                Directory.CreateDirectory(dicPath);
        }

        public void ShowMat(Mat mat)
        {
            Cv2.ImShow("fuga", mat);
        }

        public bool DetectFaceInImage(string imagePath, string cascadePath)
        {
            var mat = Cv2.ImRead(imagePath);
            return DetectFaceInImage(mat, cascadePath);
        }
        
        public bool DetectFaceInImage(Mat mat, string cascadePath)
        {
            var cascade = new CascadeClassifier();
            cascade.Load(cascadePath);
            
            var faces = cascade.DetectMultiScale(mat, 1.1, 3, 0, new Size(20, 20));
            
            Debug.Log(faces.Length);
            return faces.Length > 0;
        }
    }
}
