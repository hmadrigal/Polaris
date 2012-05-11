using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris
{
    public interface IMovementListener
    {
        void AddCursorEntry(Double normalizedX, Double normalizedY);
    }
}