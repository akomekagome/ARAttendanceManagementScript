using Amazon;

public static class AwsSettings
{
    public static readonly RegionEndpoint Region;
    
    static AwsSettings()
    {
        Region = RegionEndpoint.APNortheast1;
    }
}
