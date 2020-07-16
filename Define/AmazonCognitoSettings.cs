using Amazon;

public static class AmazonCognitoSettings
{
    public const string UserPoolId = "ap-northeast-1_j1hlHGhBi";
    public const string AppClientId = "7vs1er3kag7jiu850trt1pa8oa";
    public const string IdPoolId = "ap-northeast-1:12fe942f-b056-4ef6-a23a-38c8adc261fd";
    private static readonly string CognitoPoolRegion = RegionEndpoint.APNortheast1.SystemName;
    public static readonly string ProviderName;

    static AmazonCognitoSettings()
    {
        ProviderName = $"cognito-idp.{CognitoPoolRegion}.amazonaws.com/{UserPoolId}";
    }
}