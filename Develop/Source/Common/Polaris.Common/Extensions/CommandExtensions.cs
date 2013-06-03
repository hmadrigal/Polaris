using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Polaris.PhoneLib.SystemEx
{
    public static class CommandExtensions
    {
        /// <summary>
        /// Tries to execute the command. The command won't be executed if it's null or if the CanExecution function returns null
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameter"></param>
        public static void TryExecute(this ICommand command, object parameter = null)
        {
            if (command == null || !command.CanExecute(parameter))
            {
                return;
            }
            command.Execute(parameter);
        }
    }
}
