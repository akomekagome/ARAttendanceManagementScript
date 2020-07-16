using UnityEngine;

namespace ARAM.Main.Freees
{
    [CreateAssetMenu(fileName = "FreeeAuthData", menuName = "ARAM/Data/FreeeAuthData")]
    public class FreeeAuthData : ScriptableObject
    {
        public string clientId;
        public string clientSecret;
        public string redirectUrl;
        public string companyName;
        public string authorizationCode;
    }
}