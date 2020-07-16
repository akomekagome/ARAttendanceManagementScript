using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenCvSharp;
using OpenCvSharp.Demo;

namespace ARAM.Main.OpenCv
{
    
    public class WebCamClient
    {
        private WebCamDevice webCamDevice;
        private WebCamTexture webCamTexture;
        private OpenCvSharp.Unity.TextureConversionParams parameters;
        
        private bool forceFrontalCamera = true;

        public WebCamClient(int cameraIndex)
        {
            webCamDevice = WebCamTexture.devices[cameraIndex];
            webCamTexture = new WebCamTexture(webCamDevice.name);
            webCamTexture.Play();
            ReadTextureConversionParameters();
        }

        private void ReadTextureConversionParameters()
        {
            parameters = new OpenCvSharp.Unity.TextureConversionParams
            {
                FlipHorizontally = forceFrontalCamera || webCamDevice.isFrontFacing
            };
            
            if (0 != webCamTexture.videoRotationAngle)
                parameters.RotationAngle = webCamTexture.videoRotationAngle; // cw -> ccw
        }

        public Mat GetMatData()
        {
            return OpenCvSharp.Unity.TextureToMat(webCamTexture, parameters);
        }

        public Texture2D GetTexture2D()
        {
            var tex = new Texture2D(webCamTexture.width, webCamTexture.height);
            tex.SetPixels(webCamTexture.GetPixels());
            tex.Apply();
            return tex;
        }

        public void SaveBytes(byte[] bytes, string imagePath)
        {
            File.WriteAllBytes(imagePath, bytes);
        }
    }
}
