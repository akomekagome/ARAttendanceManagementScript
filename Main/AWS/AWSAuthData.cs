using UnityEngine;

namespace ARAM.Main.AWS
{
    [CreateAssetMenu(fileName = "AWSAuthData", menuName = "ARAM/Data/AWSAuthData")]
    public class AWSAuthData : ScriptableObject
    {
        public string keyId;
        public string secretKey;
    }
}