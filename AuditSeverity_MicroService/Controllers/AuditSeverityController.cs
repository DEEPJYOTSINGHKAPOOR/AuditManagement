using AuditSeverity_MicroService.Logger;
using AuditSeverity_MicroService.Repository.IRepository;
using AuditSeverity_MicroService.Services;
using AutoMapper;
using Global_MicroService.Const;
using Global_MicroService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AuditSeverity_MicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditSeverityController : ControllerBase
    {
        private readonly IAuditSeverityRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOptions<MyAppSettings> _options;

        public AuditSeverityController(IAuditSeverityRepository repo, IMapper mapper, ILoggerManager logger,IHttpClientFactory clientFactory, IOptions<MyAppSettings> options)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
            _clientFactory = clientFactory;
            _options = options;
        }

        private async Task<HttpResponseMessage> CheckTokenValidity(string scheme, string token)
        {
            string authUrl = _options.Value.ExternalUrl.Authorization;

            if (token != null && token.Length != 0)
            {
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Post, authUrl);
                HttpResponseMessage response = await client.SendAsync(request);

                return response;
            }
            else
            {
                HttpResponseMessage response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.Unauthorized;
                return response;
            }
        }




        /// <summary>
        /// Post The AuditRequestDto and Get AuditResponseDto object in response!
        /// </summary>
        /// <param name="auditRequestDto">AuditRequestDto Object</param>
        /// <returns></returns>
        [HttpPost("AuditSeverity")]
        public async Task<IActionResult> AuditSeverity([FromHeader] string authorization ,[FromBody] AuditRequestDto auditRequestDto)
        {

            string authUrl = _options.Value.ExternalUrl.Authorization;

            if (auditRequestDto == null)
            {
                return BadRequest(ModelState);
            }

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
            else if (authorization == null) {
                return Unauthorized("Please provide authorization token!");
            }
            //_logger.LogInformation(auditRequestDto.auditDetail.AuditType.ToString());
            var auditRequestModle = _mapper.Map<AuditRequestModel>(auditRequestDto);

            var auditResponseModel = await _repo.Manipulate(auditRequestModle, headerValue.Parameter);

            if (auditResponseModel == null) {
                return StatusCode(500,ModelState);
            }
            else
            {
                AuditResponseDto auditResponseDto = _mapper.Map<AuditResponseDto>(auditResponseModel);
                
                return CreatedAtRoute("GetAuditSeverity", new {auditId = auditResponseModel.AuditId }, auditResponseModel);
            }
        }


        /// <summary>
        /// To be implemented later
        /// </summary>
        /// <param name="auditId"></param>
        /// <returns></returns>

        [HttpGet(Name ="GetAuditSeverity")]
        public IActionResult GetAuditSeverity(int auditId) {

            return Ok();
        }
    }
}

//

/*
 * 
 * from Swadhin Mukherjee to everyone:    7:30 PM
1) There should be 2 classes
from Swadhin Mukherjee to everyone:    7:31 PM
2) One for Generating Token
from Swadhin Mukherjee to everyone:    7:31 PM
3) Second one for validating token where we have to use [Authorize] filter

*/