using UnityEngine;

namespace ARAM.Main.Managers
{
    public class CameraProvider : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        public Camera MainCamera => camera;
    }
}