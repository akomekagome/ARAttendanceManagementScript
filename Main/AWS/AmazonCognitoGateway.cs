using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model; 
using Amazon.Extensions.CognitoAuthentication;
using Amazon;
using Amazon.CognitoIdentity;
using Cysharp.Threading.Tasks;

namespace ARAM.Main.AWS
{
    
    public class AmazonCognitoGateway
    {
        private string appClientId = AmazonCognitoSettings.AppClientId;
        private string userPoolId = AmazonCognitoSettings.UserPoolId;
        private AmazonCognitoIdentityProviderClient client;
        
        public AmazonCognitoGateway(RegionEndpoint region)
        {
            client = new AmazonCognitoIdentityProviderClient(null, region);
        }

        public CognitoAWSCredentials LogIn(string providerName, string idToken)
        {
            var credentials = new CognitoAWSCredentials(
                AmazonCognitoSettings.IdPoolId,
                AwsSettings.Region
            );
            
            try
            {
                credentials.AddLogin(providerName, idToken);
                return credentials;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public async UniTask ConfirmationAsync(string email, string confirmationCode, CancellationToken token = default)
        {
            var confirmSignUpRequest = new ConfirmSignUpRequest();

            confirmSignUpRequest.Username = email;
            confirmSignUpRequest.ConfirmationCode = confirmationCode;
            confirmSignUpRequest.ClientId = appClientId;

            try
            {
                var confirmSignUpResult = await client.ConfirmSignUpAsync(confirmSignUpRequest, token);
                Debug.Log(confirmSignUpResult.ToString());
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public async UniTask<string> SignInAsync(string email, string password, CancellationToken token = default)
        {
            var userPool = new CognitoUserPool(
                userPoolId,
                appClientId,
                client
            );
            var user = new CognitoUser(
                email,
                appClientId,
                userPool,
                client
            );
            var request = new InitiateSrpAuthRequest()
            {
                Password = password
            };
            var context = await user.StartWithSrpAuthAsync(request).ConfigureAwait(true);

            Debug.Log(user.SessionTokens.IdToken);
            return user.SessionTokens.IdToken;
        }
        
        public async UniTask SignUpAsync(string email, string password, CancellationToken token = default)
        {
            var sr = new SignUpRequest
            {
                ClientId = appClientId,
                Username = email,
                Password = password,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType
                    {
                        Name = "email",
                        Value = email
                    }
                }
            };
            
            try
            {
                var result = await client.SignUpAsync(sr, token);
                Debug.Log(result);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}
