using Amazon.DynamoDBv2; // Add this using directive

namespace AWS.Playground.API.Config
{
    public static class DependencyRegister
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRealEstateRepository, RealEstateRepository>();
            return services;
        }

        public static IServiceCollection AddDynamoDB(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAmazonDynamoDB>(sp => new AmazonDynamoDBClient(
                new AmazonDynamoDBConfig
                {
                    ServiceURL = configuration["AWS:DynamoDBUrl"]
                }));
            return services;
        }
    }
}