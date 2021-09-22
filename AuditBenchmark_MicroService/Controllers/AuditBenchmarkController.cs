using Global_MicroService.Const;
using Global_MicroService.Dtos;
using Global_MicroService.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AuditBenchmark_MicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditBenchmarkController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;


        public AuditBenchmarkController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        private async Task<HttpResponseMessage> CheckTokenValidity(string scheme, string token)
        {
            if (token != null && token.Length != 0)
            {
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Post, Urls.AuthenticatedOrNot);
                HttpResponseMessage response = await client.SendAsync(request);

                return response;
            }
            else
            {
                return null;
            }
        }


        [HttpGet(Name = "GetAuditBenchmark")]
        public async Task<IActionResult> GetAuditBenchmarks([FromHeader] string authorization)
        {

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                var result = await CheckTokenValidity(headerValue.Scheme, headerValue.Parameter);
                if (result != null && result.StatusCode != HttpStatusCode.OK)
                {
                    return Unauthorized("Authorization Failed! Might be due to invalid token!");
                }
                else if (result == null)
                {
                    return Unauthorized("Please provide authorization token!");
                }
            }
            else if (authorization == null)
            {
                return Unauthorized("Please provide authorization token!");
            }



            //* statically writing.
            var auditBenchmarkDto = new List<AuditBenchmarkDto>() {
                new AuditBenchmarkDto() {
                    AuditType = Global_MicroService.Enums.AuditTypeEnum.Internal ,
                    Benchmark = 3,
                },
                new AuditBenchmarkDto() {
                    AuditType = Global_MicroService.Enums.AuditTypeEnum.SOX ,
                    Benchmark = 1,
                },
             };

            return Ok(auditBenchmarkDto);
        }

        [HttpGet("{auditType:int}",Name = "GetTypeBenchmark")]
        public async Task<IActionResult> GetTypeBenchmark([FromHeader] string authorization, AuditTypeEnum auditType)
        {
            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                var result = await CheckTokenValidity(headerValue.Scheme, headerValue.Parameter);
                if (result != null && result.StatusCode != HttpStatusCode.OK)
                {
                    return Unauthorized("Authorization Failed! Might be due to invalid token!");
                }
                else if (result == null)
                {
                    return Unauthorized("Please provide authorization token!");
                }
            }
            else if (authorization == null)
            {
                return Unauthorized("Please provide authorization token!");
            }





            if (auditType == AuditTypeEnum.Internal)
            {
                var auditBenchmark = new AuditBenchmarkDto()
                {
                    AuditType = Global_MicroService.Enums.AuditTypeEnum.Internal,
                    Benchmark = 3,
                };
                return Ok(auditBenchmark);
            }
            else
            {
                var auditBenchmark = new AuditBenchmarkDto()
                {
                    AuditType = Global_MicroService.Enums.AuditTypeEnum.SOX,
                    Benchmark = 1,
                };
                return Ok(auditBenchmark);
            }
        }


    }
}
