using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Genesis.Helpers
{
    public class RequestObject
    {
        public Request? Request { get; set; }
        public Authentication? Authentication { get; set; }
        public Registration? Registration { get; set; }
        public Communication? Communication { get; set; }
        public Kaizen? Kaizen { get; set; }
        public Report? Report { get; set; }
    }
    public enum RequestType
    {
        System,
        Request,
        Authentication,
        Registration,
        Communication,
        Kaizen,
        Report,
        Debug,
        Files
    }
}
