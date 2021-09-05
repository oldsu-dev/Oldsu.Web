using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Oldsu.Web.Validators
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PasswordAttribute : ValidationAttribute
    {
        //Minimum eight characters, at least one uppercase letter, one lowercase letter and one number
        private static Regex _passwordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$");

        public override bool IsValid(object? value)
        {
            if (value is string password)
                return _passwordRegex.IsMatch(password);

            return false;
        }
    }
}