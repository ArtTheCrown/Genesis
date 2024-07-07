using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Genesis.Helpers
{
    internal class SubClasses
    {
    }
    public class Request
    {
        public RequestType? RequestType { get; set; }
    }
    public class Response
    {
        public ResponseStatus Status { get; set; }
        public RequestType RequestType { get; set; }
    }
    public class Authentication
    {
        public ResponseStatus Status { get; set; }
        public User? User { get; set; }
    }
    public class Registration
    {
        public ResponseStatus Status { get; set; }
        public User? User { get; set; }
    }
    public class Communication
    {
        public ResponseStatus Status { get; set; }
    }
    public class Kaizen
    {
        public ResponseStatus Status { get; set; }
    }
    public class Report
    {
        public ResponseStatus Status { get; set; }
    }

    public class User
    {
        public string Pfp { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string About { get; set; }
    }

    public enum ResponseStatus
    {
        Success,
        Failure,
        UnauthorizedError,
        UnExpectedFailure,
        AuthenticationFailed,
        IncorrectCredentials
    }
}
