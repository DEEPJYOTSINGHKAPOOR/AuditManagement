using AuditChecklist_MicroService.Repository.IRepository;
using AutoMapper;
using Global_MicroService.Const;
using Global_MicroService.Dtos;
using Global_MicroService.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace AuditChecklist_MicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditChecklistController : ControllerBase
    {

        private readonly IAuditChecklistRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _clientFactory;

        public AuditChecklistController(IAuditChecklistRepository repo, IMapper mapper ,IHttpClientFactory clientFactory) 
        {
            _repo = repo;
            _mapper = mapper;
            _clientFactory = clientFactory;
        }


        private async Task<HttpResponseMessage> CheckTokenValidity(string scheme,string token) {
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
                return null;//!todo : return 404Unauthorized.
            }
        }
        
        
        [HttpGet( Name = "Questions")]
        public async Task<IActionResult> GetQuestions([FromHeader] string authorization)
        {
            

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

        [HttpGet("{auditType:int}" , Name = "TypeQuestions")]
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