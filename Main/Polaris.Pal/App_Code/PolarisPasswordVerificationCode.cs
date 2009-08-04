using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polaris.Pal
{
    /// <summary>
    /// Enumeration of the possible results when validating a password for Polaris Membership provider
    /// </summary>
    public enum PolarisPasswordVerificationCode 
    {
        /// <summary>
        /// The password is not valid without a clear reason.
        /// </summary>
        InvalidPasswordUnknownReason = 0,
        /// <summary>
        /// The Password is not valid. The password is an empty string or it has a null value.
        /// </summary>
        InvalidPasswordPasswordIsEmpty = 1,
        /// <summary>
        /// The password does not have the minimum length.
        /// </summary>
        InvalidPasswordIncorrectLength= 2,
        /// <summary>
        /// The password does not have the minimum required non-alphanumeric characters
        /// </summary>
        InvalidPasswordIncorrectMinRequiredNonAlphanumericCharacters  =4,
        /// <summary>
        /// The password does not match the password-strength regular expression
        /// </summary>
        InvalidPasswordDoesNotMatchPasswordStrengthRegularExpression = 8,
        /// <summary>
        /// The password is valid.
        /// </summary>
        ValidPassword = 16
        
    }
}
