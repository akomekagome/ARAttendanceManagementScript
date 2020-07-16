using System.Threading;
using UnityEngine;
using Amazon;
using Amazon.CognitoIdentity;
using ARAM.Utils;
using Cysharp.Threading.Tasks;

namespace ARAM.Main.AWS
{
    
    public class AmazonConginitoProvider : MonoBehaviour
    {
        private AmazonCognitoGateway amazonCognitoGateway;
        private RegionEndpoint region = AwsSettings.Region;
        private static CognitoAWSCredentials credentials;
        public CognitoAWSCredentials Credentials => credentials;

        private void Awake()
        {
            amazonCognitoGateway = new AmazonCognitoGateway(region);
        }

        public bool LogIn()
        {
            var idToken = JsonUtilityUtils.LoadJsonData<string>(SaveDataFilePaths.IdTokenPath);
            credentials = amazonCognitoGateway.LogIn(AmazonCognitoSettings.ProviderName, idToken);
            return credentials != null;
        }

        public async UniTaskVoid ConfirmationAsync(string email, string confirmationCode, CancellationToken token = default)
        {
            await amazonCognitoGateway.ConfirmationAsync(email, confirmationCode);
        }

        public async UniTaskVoid SignInAsync(string email, string password, CancellationToken token = default)
        {
            var idToken = await amazonCognitoGateway.SignInAsync(email, password, token);
            JsonUtilityUtils.SaveJsonData(email, SaveDataFilePaths.UserNamePath);
            JsonUtilityUtils.SaveJsonData(idToken, SaveDataFilePaths.IdTokenPath);
        }

        public async UniTaskVoid SignUpAsync(string email, string password, CancellationToken token = default)
        {
            await amazonCognitoGateway.SignUpAsync(email, password, token);
            JsonUtilityUtils.SaveJsonData(email, SaveDataFilePaths.UserNamePath);
        }
    }
}
