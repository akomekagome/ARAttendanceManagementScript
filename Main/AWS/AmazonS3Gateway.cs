using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Threading;
using Amazon;
using Amazon.CognitoIdentity;
using Cysharp.Threading.Tasks;

namespace ARAM.Main.AWS
{
    
    public class AmazonS3Gateway
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APNortheast1;
        private static IAmazonS3 s3Client;

        public AmazonS3Gateway(CognitoAWSCredentials credentials)
        {
            s3Client = new AmazonS3Client(credentials);
        }

        public async UniTask UploadFileAsync(string bucketName, string keyName, string filePath, CancellationToken token = default)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);
                await fileTransferUtility.UploadAsync(filePath, bucketName, keyName, token);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}
