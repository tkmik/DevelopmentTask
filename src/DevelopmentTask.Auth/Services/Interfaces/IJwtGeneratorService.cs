namespace DevelopmentTask.Auth.Services.Interfaces
{
    public interface IJwtGeneratorService
    {
        string Generate(string code);
    }
}
