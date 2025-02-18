using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObject.RequestModel;
using DataObject.ResponseModel;

namespace DataAccessLayer.IDataAccessLayer
{
    public interface IAccountDal
    {
        public Task<ResponseModel<bool>> AddUserDAL(AccountUserModel accountUserModel);
        public Task<ResponseModel<bool>> LoginUserDAL(AccountUserModel accountUserModel);
        public Task<ResponseModel<AccountUserResponseModel>> GetAllUserDAL();

    }
}
