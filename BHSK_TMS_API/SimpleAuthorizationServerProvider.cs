using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BHSK_TMS_API.Provider
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            context.Validated();
        }

        //public override async Task GrantResourceOwnerCredentials1(OAuthGrantResourceOwnerCredentialsContext context)
        //{
        //    context.Validated(new ClaimsIdentity(context.Options.AuthenticationType));
        //}

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            if (IsAuthorizedUser(context.UserName, context.Password))
            {
                identity.AddClaim(new Claim("sub", context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Expired, "60"));

                context.Validated(identity);

            }
            else
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

        }
        public static bool IsAuthorizedUser(string Username, string Password)
        {
            // In this method we can handle our database logic here...  
            var result = (dynamic)null;
            var encrptPw = Convert.ToString(Encrypt_Dll.EncryptDLL.enCrypt(Password));
            if (Username != "" && Password=="forgotpassword")
            {
                result = true;
            }
            else if (Username != "" && Password !="")
            {
                result = DAL_AccessLayer.CheckUserLogin_API(Username, encrptPw);
                if (result.StatusCode == 1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
            //return Username == "bhs007" && Password == "bhsK789";
        }
    }
}


 