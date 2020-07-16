using UnityEngine;

namespace ARAM.Main.Managers
{
    
    [CreateAssetMenu(fileName = "PlatformDependentData", menuName = "ARAM/Data/PlatformDependentData")]
    public class PlatformDependentData : ScriptableObject
    {
        public int cameraIndex;
    }
}
