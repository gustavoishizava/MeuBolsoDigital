namespace MBD.Identity.Domain.Interfaces.Services
{
    public interface IHashService
    {
         string Create(string input);
         bool IsMatch(string input, string hash);
    }
}