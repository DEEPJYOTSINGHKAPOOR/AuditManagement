using AuditSeverity_MicroService.Repository.IRepository;
using Global_MicroService.Const;
using Global_MicroService.Dtos;
using Global_MicroService.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AuditSeverity_MicroService.Repository
{
    public class AuditSeverityRepository : IAuditSeverityRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public AuditSeverityRepository(IHttpClientFactory clientFactory )
        {
            _clientFactory = clientFactory;
        }
        public async Task<AuditResponseModel> Manipulate(AuditRequestModel auditRequestModel, string token)
        {
            Console.WriteLine("Url is:"+ Urls.AuditBenchmark + ((int)auditRequestModel.auditDetail.AuditType));
            var request = new HttpRequestMessage(HttpMethod.Get, Urls.AuditBenchmark + ((int)auditRequestModel.auditDetail.AuditType));

            var client = _clientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

            HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

            Console.WriteLine("httpResponseMessage: " + httpResponseMessage.StatusCode);



            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Success");

                var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();

                var auditBenchmarkDto =  JsonConvert.DeserializeObject<AuditBenchmarkDto>(jsonString);


                AuditResponseModel auditResponseModel = solve(auditBenchmarkDto.Benchmark, auditRequestModel); ;
                return auditResponseModel;
            }
            else
            {
                return null;
            }
        }
        private AuditResponseModel solve( int benchMarkLimit, AuditRequestModel auditRequestModel)
        {
            AuditTypeEnum auditTypeEnum = auditRequestModel.auditDetail.AuditType;

            var list = auditRequestModel.auditDetail.responses;

            int count = 0;
            foreach (var item in list)
            {
                if (item.Answer == false)
                {
                    count += 1;
                }
            }

            bool isWithinLimits = (count <= benchMarkLimit);

            AuditResponseModel auditResponseModel = new AuditResponseModel();

            Random random = new Random();

            string x = (random.Next(1,1000)).ToString() + auditRequestModel.ProjectName + auditRequestModel.auditDetail.AuditDate.ToString();
            auditResponseModel.AuditId = x.GetHashCode();

            //auditResponseModel 
            if (auditTypeEnum == AuditTypeEnum.Internal)
            {
                if (isWithinLimits) 
                {
                    auditResponseModel.AuditExecutionStatus = AuditResultEnum.Green;
                    auditResponseModel.RemedialActionDuration = "No action needed";
                }else
                {
                    auditResponseModel.AuditExecutionStatus = AuditResultEnum.Red;
                    auditResponseModel.RemedialActionDuration = "Action to be taken in 2 weeks";
                }
            }
            else 
            {
                if (isWithinLimits)
                {
                    auditResponseModel.AuditExecutionStatus = AuditResultEnum.Green;
                    auditResponseModel.RemedialActionDuration = "No action needed";
                }
                else
                {
                    auditResponseModel.AuditExecutionStatus = AuditResultEnum.Red;
                    auditResponseModel.RemedialActionDuration = "Action to be taken in 1 week";
                }
            
            }
            return auditResponseModel;
        }
    }



   
}
/*
 * Authorization microservice
 *  /login:generating the token ....no authorize bcoz it will generate the token...it's anonymous
 *  /authentication : => will take token as input header and [Authorize]
 * */