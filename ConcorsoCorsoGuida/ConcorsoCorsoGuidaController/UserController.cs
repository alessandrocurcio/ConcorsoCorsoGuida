using ConcorsoCorsoGuidaEntities.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ConcorsoCorsoGuidaEntities;
//using RoomReservationsEntities;
//using RoomReservationsManager.Services;
//using RoomReservationsRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConcorsoCorsoGuidaRepository;
using ConcorsoCorsoGuidaManager.Services;
using Microsoft.AspNetCore.Authorization;

namespace ConcorsoCorsoGuidaController
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<User> _logger;
        private IUserService _userService;

        public UserController(ApplicationDbContext applicationDbContext, ILogger<User> logger, IUserService userService)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _userService = userService;
        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<ApiResponse<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            try
            {
                var response = await _userService.Authenticate(model);
                return new ApiResponse<AuthenticateResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new ApiResponse<AuthenticateResponse>(ex);

            }
        }
        [HttpPost]
        [Route("GetUserInfoByToken")]
        public ApiResponse<User> GetUserInfoByToken(AuthenticateRequest model)
        {
            try
            {
                var response = _userService.GetUserInfoByToken(model);
                return new ApiResponse<User>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new ApiResponse<User>(ex);
            }
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            ConcorsoCorsoGuidaManager.Users users = new ConcorsoCorsoGuidaManager.Users(_applicationDbContext);
            return users.GetAll();
        }

        [HttpPost]
        [Route("Save")]
        public ApiResponse<User> Save([FromBody] User inputUser)
        {
            try
            {
                ConcorsoCorsoGuidaManager.Users users = new ConcorsoCorsoGuidaManager.Users(_applicationDbContext);
                return new ApiResponse<User>(users.Save(inputUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new ApiResponse<User>(ex);
            }
        }
        [HttpPost]
        [Route("Delete")]
        public ApiResponse<bool> Delete([FromBody] User inputUser)
        {
            try
            {

                ConcorsoCorsoGuidaManager.Users users = new ConcorsoCorsoGuidaManager.Users(_applicationDbContext);
                return new ApiResponse<bool>(users.Delete(inputUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new ApiResponse<bool>(ex);
            }
        }
      
    }
}
