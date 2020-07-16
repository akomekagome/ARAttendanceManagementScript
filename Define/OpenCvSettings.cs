using System.IO;
using UnityEngine;

public class OpenCvSettings
{
    private const string CascadeLocalPath = "OpenCv/haarcascade_frontalface_default.xml";
    public static readonly string CascadePath;

    static OpenCvSettings()
    {
        CascadePath = Path.Combine(Application.streamingAssetsPath, CascadeLocalPath);
    }
}