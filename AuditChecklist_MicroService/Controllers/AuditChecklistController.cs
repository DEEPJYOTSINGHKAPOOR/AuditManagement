using AuditChecklist_MicroService.Repository.IRepository;
using AuditChecklist_MicroService.Services;
using AutoMapper;
using AutoMapper.Configuration;
using Global_MicroService.Const;
using Global_MicroService.Dtos;
using Global_MicroService.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace AuditChecklist_MicroService.Controllers
{



    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuditChecklistController : ControllerBase
    {

        private readonly IAuditChecklistRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _clientFactory;

        private string  authorizationUrl;
        private IOptions<MyAppSettings> _options;

        public AuditChecklistController(IAuditChecklistRepository repo, IMapper mapper ,IHttpClientFactory clientFactory, IOptions<MyAppSettings> options) 
        {
            _repo = repo;
            _mapper = mapper;
            _clientFactory = clientFactory;
            _options = options;
        }


        private async Task<HttpResponseMessage> CheckTokenValidity(string scheme,string token) {
            string AuthorizationUrl = _options.Value.ExternalUrl.Authorization;
            if (token != null && token.Length != 0)
            {
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Post, AuthorizationUrl);
                HttpResponseMessage response = await client.SendAsync(request);

                return response;
            }
            else
            {
                HttpResponseMessage response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.Unauthorized;
                return response;//!todo : return 404Unauthorized.
            }
        }
        
        
        [HttpGet( Name = "GetQuestions")]
        public async Task<IActionResult> GetQuestions([FromHeader] string authorization)
        {

            string AuthorizationUrl = _options.Value.ExternalUrl.Authorization;
            //Console.WriteLine(AuthorizationUrl);

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                var result = await CheckTokenValidity(headerValue.Scheme, headerValue.Parameter);
                if (result != null && result.StatusCode != HttpStatusCode.OK)
                {
                    return Unauthorized("Authorization Failed! Might be due to invalid token!");
                }
                else if (result == null) {
                    return Unauthorized("Authorization Failed! Might be due to invalid token!");
                }
            }

            var objList = _repo.GetQuestions();
            if (objList == null)
            {
                return NotFound();
            }

            var objDto = new List<AuditQuestionDto>();
            foreach (var obj in objList)
            {
                var x = _mapper.Map<AuditQuestionDto>(obj);
                objDto.Add(x);
            }
            return Ok(objDto);
        }


        [HttpGet("{auditType:int}" , Name = "GetAuditTypeQuestions")]
        public async Task<IActionResult> GetAuditTypeQuestions(AuditTypeEnum auditType, [FromHeader] string authorization)
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

            var objList = _repo.GetSpecificTypeQuestions(auditType);
            if (objList == null)
            {
                return NotFound();
            }

            var objDto = new List<AuditQuestionDto>();

            foreach (var obj in objList)

            {
                var x = _mapper.Map<AuditQuestionDto>(obj);

                objDto.Add(x);
            }
            return Ok(objDto);

        }
    }
}