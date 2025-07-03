using WalksAndRails.Api.Models.Domain;

namespace WalksAndRails.Api.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateWalkAsync(Walk walk);
        Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null);
        Task<Walk?> GetWalkByIdAsync(Guid id);
        Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);
        Task<Walk?> DeleteWalkAsync(Guid id);
    }
}
