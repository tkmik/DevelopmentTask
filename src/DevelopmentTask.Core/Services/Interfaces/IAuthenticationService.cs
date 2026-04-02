namespace DevelopmentTask.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateAsync(string code, CancellationToken cancellationToken = default);
    }
}
