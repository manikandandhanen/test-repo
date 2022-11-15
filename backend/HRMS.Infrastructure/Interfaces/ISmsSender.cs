using System.Threading.Tasks;

namespace HRMS.Interfaces
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
