using Global_MicroService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditSeverity_MicroService.Repository.IRepository
{
    public interface IAuditSeverityRepository
    {
        public Task<AuditResponseModel> Manipulate(AuditRequestModel auditRequestModel, string token);
    }
}
