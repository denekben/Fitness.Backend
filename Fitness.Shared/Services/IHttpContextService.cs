namespace Fitness.Shared.Services
{
    public interface IHttpContextService
    {
        int GetCurrentUserId();
        string GetCurrentUserRoleName();
    }
}
