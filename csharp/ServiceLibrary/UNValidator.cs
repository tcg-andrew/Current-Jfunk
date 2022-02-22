using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;

namespace ServiceLibrary
{
    public class UNValidator : UserNamePasswordValidator
    {
        public UNValidator()
            : base()
        {
        }

        public override void Validate(string userName, string password)
        {
            if (userName == "rails" && password == "fX@7*n]Ra3")
                return;
            throw new System.IdentityModel.Tokens.SecurityTokenException("Invalid username or password");
        }
    }
}
