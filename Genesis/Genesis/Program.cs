using Genesis.Genesis.Helpers;
using Genesis.Genesis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Genesis.Genesis
{
    public static class Program
    {
        public static async void Initialize()
        {
            await DBEngine.Initialize();

            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        public static async Task<(bool, ResponseObject?)> Login(string username, string password)
        {
            var request = new RequestObject
            {
                Request = new Request { RequestType = RequestType.Authentication },
                Authentication = new Authentication 
                { 
                    User = new User 
                    {
                        UserID = username,
                        Password = password
                    }
                }
            };

            var response = await DBEngine.HandleRequestAsync(request, RequestType.Authentication);

            return response.Item1 ? (true, response.Item2) : (false, null);
        }

        public static async Task<(bool, ResponseObject?)> Register(string username, string displayName, string password)
        {
            var request = new RequestObject
            {
                Request = new Request { RequestType = RequestType.Registration},
                Registration = new Registration 
                {
                    User = new User
                    {
                        UserID = username,
                        Username = displayName,
                        Password = password
                    }
                }
            };
            var response = await DBEngine.HandleRequestAsync(request, RequestType.Registration);

            return response.Item1? (true, response.Item2) : (false, null);
        }
    }
}
