namespace AWS.Playground.API;

public interface IRealEstateRepository
{
    Task InsertRealtyAsync(Realty realty, CancellationToken cancellationToken);
    Task<Realty?> GetRealtyAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateRealtyAsync(Realty realty, CancellationToken cancellationToken);
    Task DeleteRealtyAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Realty>> GetAllRealtyAsync(CancellationToken cancellationToken);
}
