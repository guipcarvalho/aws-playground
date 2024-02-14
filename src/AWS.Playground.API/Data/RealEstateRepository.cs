using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using AWS.Playground.API.Data;

namespace AWS.Playground.API
{
    public class RealEstateRepository(IAmazonDynamoDB dynamoDB) : IRealEstateRepository
    {
        private readonly IAmazonDynamoDB _dynamoDB = dynamoDB;
        private const string RealtyTableName = "Realty"; // Global variable for table name

        public async Task InsertRealtyAsync(Realty realty, CancellationToken cancellationToken)
        {
            var table = Table.LoadTable(_dynamoDB, RealtyTableName);

            var document = new Document
            {
                ["Id"] = realty.Id,
                ["Address"] = realty.Address,
                ["Price"] = realty.Price
            };

            await table.PutItemAsync(document, cancellationToken);
        }

        // Implement other CRUD methods here
        public async Task<Realty?> GetRealtyAsync(Guid id, CancellationToken cancellationToken)
        {
            var table = Table.LoadTable(_dynamoDB, RealtyTableName);

            var document = await table.GetItemAsync(id, cancellationToken);

            if (document != null)
            {
                return document.FromDocument<Realty>();
            }

            return null;
        }

        public async Task UpdateRealtyAsync(Realty realty, CancellationToken cancellationToken)
        {
            var table = Table.LoadTable(_dynamoDB, RealtyTableName);

            await table.UpdateItemAsync(realty.ToDocument(), cancellationToken);
        }

        public async Task DeleteRealtyAsync(Guid id, CancellationToken cancellationToken)
        {
            var table = Table.LoadTable(_dynamoDB, RealtyTableName);

            await table.DeleteItemAsync(id, cancellationToken);
        }

        public async Task<List<Realty>> GetAllRealtyAsync(CancellationToken cancellationToken)
        {
            var table = Table.LoadTable(_dynamoDB, RealtyTableName);

            var search = table.Scan(new ScanFilter());

            var realties = new List<Realty>();

            do
            {
                var documents = await search.GetNextSetAsync(cancellationToken);

                foreach (var document in documents)
                {
                    realties.Add(document.FromDocument<Realty>());
                }

            } while (!search.IsDone);

            return realties;
        }
    }
}
