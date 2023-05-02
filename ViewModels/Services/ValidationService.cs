using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp2.ViewModels.Services
{
    internal class ValidationService
    {

        public static bool isPasswordValid(string input)
        {
            // Validate strong password
            //Atleast 8 character with atleast 1 capital, 1 small alphabet, 1 number and 1 special character.

            Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            return validateGuidRegex.IsMatch(input);  
        }
    }
}
