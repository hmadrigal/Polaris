namespace Polaris.Windows.Extensions
{
    using System.Windows.Input;

    public static class CommandExtensions
    {
        public static void ExecuteCommand(this ICommand command, object arg = null)
        {
            if (command != null && command.CanExecute(arg))
            {
                command.Execute(arg);
            }
        }
    }
}