namespace TechXpress.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task UpdateProfilePicture(string userId, string profilePicturePath);
    }

}
