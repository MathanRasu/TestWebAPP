using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.IDataAccessLayer;
using DataObject.Context.Interface;
using DataObject.Entity;
using DataObject.RequestModel;
using DataObject.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DataAccessLayer.DataAccessLayer
{
    public class AccountDal : IAccountDal
    {
        private IApplicationDbContext applicationDbContext;

        private IConfiguration configuration;
        public AccountDal(IApplicationDbContext _applicationDbContext, IConfiguration _configuration)
        {
            this.applicationDbContext = _applicationDbContext;
            configuration = _configuration;
        }

        //public async Task<ResponseModel<bool>> AddUserDAL(AccountUserModel accountUserModel)
        //{
        //    try
        //    {
        //        AccountUser accountUser = new AccountUser()
        //        {
        //            IsActive = true,
        //            UserName = accountUserModel.UserName,
        //            Password = accountUserModel.Password,
        //            CreatedBy = 1,
        //            CreatedDate = DateTime.Now,
        //            UpdatedBy = 1,
        //            UpdateDate = DateTime.Now
        //        };

        //        await applicationDbContext.AccountUser.AddAsync(accountUser);
        //        await applicationDbContext.SaveChangesAsync(default);
        //        return new ResponseModel<bool>
        //        {
        //            IsSuccess = true,
        //            StatusMessage = "Add user successfully",
        //            ResponseMessage = "Success",
        //            StatusCode = 200
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseModel<bool>()
        //        {
        //            StatusCode = 500,
        //            IsSuccess = false,
        //            ResponseMessage = ex.Message,
        //            StatusMessage = ex.StackTrace
        //        };
        //    }
        //}
        public async Task<ResponseModel<bool>> AddUserDAL(AccountUserModel accountUserModel)
        {
            try
            {
                string connectionstring = configuration.GetConnectionString("ARConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand("sp_Adduser_TestWeb", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("IsActive",true);
                    cmd.Parameters.AddWithValue("UserName", accountUserModel.UserName);
                    cmd.Parameters.AddWithValue("Password", accountUserModel.Password);
                    cmd.Parameters.AddWithValue("CreatedBy", 1);
                    cmd.Parameters.AddWithValue("CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("UpdatedBy", 1);
                    cmd.Parameters.AddWithValue("UpdateDate", DateTime.Now);

                    connection.Open();
                    var sdr = cmd.ExecuteNonQuery();
                }
                return new ResponseModel<bool>
                {
                    IsSuccess = true,
                    StatusMessage = "Add user successfully",
                    ResponseMessage = "Success",
                    StatusCode = 200
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    ResponseMessage = ex.Message,
                    StatusMessage = ex.StackTrace
                };
            }
        }
        public async Task<ResponseModel<AccountUserResponseModel>> GetAllUserDAL()
        {
            try
            {
                var result = await applicationDbContext.AccountUser.Select(x => new AccountUserResponseModel()
                {
                    IsActive = x.IsActive,
                    Password = x.Password,
                    UserId = x.UserId,
                    UserName = x.UserName,
                }).ToListAsync();
                return new ResponseModel<AccountUserResponseModel>()
                {
                    IsSuccess = true,
                    ListResult = result,
                    ResponseMessage = "User Retervide successfully"
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel<AccountUserResponseModel>()
                {
                    ResponseMessage = ex.Message,
                    IsSuccess = false,
                    StatusCode = 500,
                    StatusMessage = ex.StackTrace
                };
            }
        }

        public async Task<ResponseModel<bool>> LoginUserDAL(AccountUserModel accountUserModel)
        {
            try
            {
                var ValidUser = await applicationDbContext.AccountUser.
                                        Where(x => x.UserName == accountUserModel.UserName && x.Password == accountUserModel.Password).FirstOrDefaultAsync();

                if (ValidUser != null && ValidUser.IsActive)
                {
                    var token = GenerateJSONWebToken(accountUserModel);
                    return new ResponseModel<bool>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Token = token,
                        ResponseMessage = "logged in successfully"
                    };
                }
                return new ResponseModel<bool>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    ResponseMessage = "You credential is wrong"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>()
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    ResponseMessage = ex.Message,
                    StatusMessage = ex.StackTrace
                };
            }
        }

        private string GenerateJSONWebToken(AccountUserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
              configuration["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
