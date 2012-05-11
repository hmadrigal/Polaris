using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Speech.Recognition;

namespace Polaris.Services
{
    internal class RegisteredGrammar
    {
        public Guid CreatorId { get; set; }

        public Guid RegistrationId { get; set; }

        public Grammar Grammar { get; set; }

        public string Command { get; set; }
    }
}