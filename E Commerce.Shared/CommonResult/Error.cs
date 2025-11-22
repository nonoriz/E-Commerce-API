using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.CommonResult
{
    public class Error
    {
       
        public string Code { get; }
        public string Description { get;}

        public ErrorType Type { get;}

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }


        #region static factory methods to create errors
        //static factory methods to create errors
        public static Error Failure(string code = "General.Failure", string description = "A General Failure Has Occurred")
        {
            return new Error(code, description, ErrorType.Failure);
        }
        public static Error Validation(string code = "General.Validation", string description = "A Validation Error Has Occurred")
        {
            return new Error(code, description, ErrorType.Validation);
        }
        public static Error NotFound(string code = "General.NotFound", string description = "The Requested Resource Was Not Found")
        {
            return new Error(code, description, ErrorType.NotFound);
        }
        public static Error Unauthorized(string code = "General.Unauthorized", string description = "You Are Not Authorized To Perform This Action")
        {
            return new Error(code, description, ErrorType.Unauthorized);
        }
        public static Error Forbbiden(string code = "General.Forbidden", string description = "You Do Not Have Permission To Access This Resource")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }
        public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "The Provided Credentials Are Invalid")
        {
            return new Error(code, description, ErrorType.InvalidCredentials);
        }

        #endregion




    }
}
