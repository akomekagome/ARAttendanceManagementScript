using System;
using System.Collections.Generic;
using System.IO;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System.Linq;
using System.Threading;
using Amazon.CognitoIdentity;
using ARAM.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ARAM.Main.AWS
{
    
    public class AmazonRekognitionGateway
    {
        AmazonRekognitionClient rekoClient;

        public AmazonRekognitionGateway(CognitoAWSCredentials credentials)
        {
            rekoClient = new AmazonRekognitionClient(credentials);
        }
        
        public AmazonRekognitionGateway(string keyId , string secretKey)
        {
            rekoClient = new AmazonRekognitionClient(keyId, secretKey, AwsSettings.Region);
        }

        public AmazonRekognitionGateway(AWSAuthData awsAuthData)
        {
            rekoClient = new AmazonRekognitionClient(awsAuthData.keyId, awsAuthData.secretKey);
        }
        
        public async UniTask<List<string>> AuthenticateFaceAsync(string bucket, string photo, string collectionId, CancellationToken token = default)
        {
            var image = new Image()
            {
                S3Object = new S3Object()
                {
                    Bucket = bucket,
                    Name = photo
                }
            };
            return await AuthenticateFaceAsync(image, collectionId, token);
        }
        
        public async UniTask<List<string>> AuthenticateFaceAsync(MemoryStream stream, string collectionId, CancellationToken token = default)
        {
            var image = new Image()
            {
                Bytes = stream
            };
            return await AuthenticateFaceAsync(image, collectionId, token);
        }

        private async UniTask<List<string>> AuthenticateFaceAsync(Image image, string collectionId, CancellationToken token = default)
        {
            var searchFacesByImageRequest = new SearchFacesByImageRequest()
            {
                CollectionId = collectionId,
                Image = image,
                FaceMatchThreshold = 90f,
                MaxFaces = 1
            };

            try
            {
                var searchFacesByImageResponse = await rekoClient.SearchFacesByImageAsync(searchFacesByImageRequest, token);
                
                var faceIds = searchFacesByImageResponse.FaceMatches
                    .Select(x => x.Face.FaceId)
                    .ToList();
            
                DebugExtensions.DebugShowList(faceIds);
            
                return faceIds;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return default;
            }
        }
    }
}
