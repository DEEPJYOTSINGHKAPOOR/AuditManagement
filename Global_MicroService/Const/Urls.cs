using System;
using System.Collections.Generic;
using System.Text;

namespace Global_MicroService.Const
{
    public class Urls
    {
        public const string AuditSeverity = "https://localhost:5003/api/auditseverity/";
        public const string AuditBenchmark = "https://localhost:5001/api/auditbenchmark/";
        public const string AuthenticatedOrNot = "https://localhost:5007/api/Users/CheckWhetherAuthorizedOrNot/";
        public const string RegisterNewUser = "https://localhost:5007/api/Users/register/";
    }
}
