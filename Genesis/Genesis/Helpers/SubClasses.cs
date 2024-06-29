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
        public string? status { get; set; }
        public RequestType? RequestType { get; set; }
    }
    public class Authentication
    {
        public string? status { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
    }
    public class Registration
    {
        public string? status { get; set; }
        public User? User { get; set; }
    }
    public class Communication
    {
        public string? status { get; set; }
    }
    public class Kaizen
    {
        public string? status { get; set; }
    }
    public class Report
    {
        public string? status { get; set; }
    }

    public class User
    {
        public string pfp { get; set; }
        public string userID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public string about { get; set; }
    }
}
