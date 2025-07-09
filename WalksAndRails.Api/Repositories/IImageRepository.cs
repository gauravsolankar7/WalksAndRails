using WalksAndRails.Api.Models.Domain;

namespace WalksAndRails.Api.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
