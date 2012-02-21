using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Polaris.Bal.Extensions {
    public static class ValidationExtensions {

        public static ValidationResults Validate(this IUser user) {
            return Validation.Validate<IUser>(user);
        }

    }
}
