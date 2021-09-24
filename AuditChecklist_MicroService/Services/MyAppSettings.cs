using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditChecklist_MicroService.Services
{
    public class MyAppSettings
    {
        public const string SectionName = "MySettings";

        public ExternalUrl ExternalUrl { get; set; }
    }

    public class ExternalUrl {
        public string Authorization { get; set; }


    }


}


