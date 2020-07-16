using System.IO;
using UnityEngine;

public class SaveImageFilePaths
{
    private const string SaveFaceImagePath = "SaveFaceImages";
    private const string FaceImageName = "SavedScreen.jpeg";
    public static readonly string FaceImagePath;

    static SaveImageFilePaths()
    {
        FaceImagePath = @Path.Combine(Application.persistentDataPath, SaveFaceImagePath, FaceImageName);
    }
}