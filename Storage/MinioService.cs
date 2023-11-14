using Microsoft.AspNetCore.Components.Forms;
using Minio;
using Minio.DataModel.Args;


namespace Agape.Storage
{
    public class MinioService : IStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName = "agape";
        private readonly string _bucketRegion = "us-east-1";
        private readonly string _publicPath = "public/";
        private readonly int _expiry = 24 * 60 * 60; // Expiry time in seconds
        private readonly string _contentType = "application/octet-stream";
        private readonly string _policy; // Assuming policyPublic.json is a JObject

        public MinioService(string connectionString)
        {
            var storageUrl = new Uri(connectionString);

            var endpoint = storageUrl.Host;
            var accessKey = storageUrl.UserInfo.Split(':')[0];
            var secretKey = storageUrl.UserInfo.Split(':')[1];

            //En un entorno de desarrollo en ocasiones se usan puertos personalizados
            if (!string.IsNullOrEmpty(storageUrl.Port.ToString()))
            {
                endpoint += $":{storageUrl.Port}";
            }

            _minioClient = new MinioClient()
                                .WithEndpoint(endpoint)
                                .WithCredentials(accessKey, secretKey)
                                .WithSSL(storageUrl.Scheme == Uri.UriSchemeHttps)
                                .WithRegion(_bucketRegion)
                                .Build();

            _policy = File.ReadAllText("Storage/policyPublic.json"); // Load your policy here
        }

        public async Task<string> SaveAsync(Stream stream, string filename, string? mimeType = null)
        {
            await EnsureCreated();

            mimeType ??= _contentType;
            var objectName = _publicPath + filename;

            // Asegúrate de que la longitud del stream esté disponible.
            if (!stream.CanSeek)
            {
                // Maneja el error o carga el stream en un MemoryStream si es necesario.
                throw new InvalidOperationException("Stream must support seeking to determine its length.");
            }

            // Crear una instancia de PutObjectArgs
            var args = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(mimeType);

            // Usar la instancia de PutObjectArgs en el método PutObjectAsync
            await _minioClient.PutObjectAsync(args);

            // Crear una instancia de PresignedGetObjectArgs
            var presignedArgs = new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithExpiry(_expiry);

            // Usar la instancia de PresignedGetObjectArgs en el método PresignedGetObjectAsync
            var urlPrivate = await _minioClient.PresignedGetObjectAsync(presignedArgs);

            var url = new Uri(urlPrivate);
            var builder = new UriBuilder(url) { Query = "" };

            return builder.Uri.ToString();
        }

        public async Task<string> SaveInputFileAsync(IBrowserFile browserFile)
        {
            using (var stream = browserFile.OpenReadStream())
            {
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                // Asegúrate de rebobinar el MemoryStream para su posterior uso
                memoryStream.Position = default;

                var fileName = $"{Guid.NewGuid()}-{browserFile.Name}";
                
                return await SaveAsync(memoryStream, fileName, browserFile.ContentType);
            }
        }
        public async Task EnsureCreated()
        {
            var existsBucket = await _minioClient.BucketExistsAsync(new BucketExistsArgs()
                .WithBucket(_bucketName));

            if (!existsBucket)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs()
                    .WithBucket(_bucketName));

                await _minioClient.SetPolicyAsync(new SetPolicyArgs()
                    .WithBucket(_bucketName)
                    .WithPolicy(_policy));
            }
        }



    }
}