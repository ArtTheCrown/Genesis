﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Genesis.Helpers
{
    public class ResponseObject
    {
        public Response? Response { get; set; }
        public System? System { get; set; }
        public Authentication? Authentication { get; set; }
        public Registration? Registration { get; set; }
        public Communication? Communication { get; set; }
        public Files? Files { get; set; }
        public Kaizen? Kaizen { get; set; }
        public Report? Report { get; set; }
    }
}

