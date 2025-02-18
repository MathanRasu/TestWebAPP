using DataObject.RequestModel;
using DataObject.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ServiceLayer.IService;

namespace TestWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IAccountBAL accountBAL;

        public AccountController(IAccountBAL _accountBAL)
        {
            this.accountBAL = _accountBAL;
        }

        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<ResponseModel<bool>> LoginUser(AccountUserModel userModel)
        {
            return await accountBAL.LoginUserBAL(userModel);
        }

        [HttpPost("AddUser")]
        public async Task<ResponseModel<bool>> AddUser(AccountUserModel userModel)
        {
            return await accountBAL.AddUserBAL(userModel);
        }

        [HttpGet("GetAllUser")]
        public async Task<ResponseModel<AccountUserResponseModel>> GetAllUser()
        {
            return await accountBAL.GetAllUserBAl();
        }
    }
}
