using System.Threading.Tasks;
using System.Windows.Input;

namespace Papyrus.Desktop.Util.Command
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
