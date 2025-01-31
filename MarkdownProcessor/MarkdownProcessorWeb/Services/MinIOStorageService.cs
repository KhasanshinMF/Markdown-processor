using Minio;
using Minio.DataModel.Args;

namespace MarkdownProcessorWeb.Services;

public class MinIOStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;
    
    public MinIOStorageService(IConfiguration configuration)
    {
        var endpoint = configuration["MinIO:Endpoint"];
        var accessKey = configuration["MinIO:AccessKey"];
        var secretKey = configuration["MinIO:SecretKey"];
        var useSSL = configuration.GetValue<bool>("MinIO:UseSSL");
        
        if (string.IsNullOrEmpty(endpoint))
        {
            throw new ArgumentException("MinIO endpoint is not configured.");
        }

        _minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(useSSL)
            .Build();

        _bucketName = configuration["MinIO:BucketName"];
        
        if (string.IsNullOrEmpty(_bucketName))
        {
            throw new ArgumentException("MinIO bucket name is not configured.");
        }
    }
    
    public async Task UploadFileAsync(string key, Stream fileStream)
    {
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(key)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length);

        await _minioClient.PutObjectAsync(putObjectArgs);
    }
    
    public async Task<Stream> DownloadFileAsync(string key)
    {
        var memoryStream = new MemoryStream();

        var getObjectArgs = new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(key)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream));

        await _minioClient.GetObjectAsync(getObjectArgs);

        memoryStream.Position = 0;
        return memoryStream;
    }
    
    public async Task DeleteFileAsync(string key)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(key);

        await _minioClient.RemoveObjectAsync(removeObjectArgs);
    }
}