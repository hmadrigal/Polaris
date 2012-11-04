using System;
using System.ComponentModel;
using Polaris.Windows.Services;
using System.Windows;

namespace Polaris.Windows.Controls
{
    public class LogicalKeyEventArgs : EventArgs
    {
        public DependencyLogicalKey Key { get; private set; }

        public LogicalKeyEventArgs(DependencyLogicalKey key)
        {
            Key = key;
        }
    }

    public delegate void LogicalKeyPressedEventHandler(object sender, LogicalKeyEventArgs e);

    public class LogicalKeyBase :  DependencyLogicalKey
    {



    }
}