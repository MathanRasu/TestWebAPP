using DataAccessLayer.IDataAccessLayer;
using DataObject.RequestModel;
using DataObject.ResponseModel;
using ServiceLayer.IService;

namespace ServiceLayer.Service
{
    public class AccountBAL : IAccountBAL
    {
        private IAccountDal accountDal;
        public AccountBAL(IAccountDal _accountDal)
        {
            this.accountDal = _accountDal;

        }
        public async Task<ResponseModel<bool>> AddUserBAL(AccountUserModel accountUserModel)
        {
            return await accountDal.AddUserDAL(accountUserModel);
        }

        public async Task<ResponseModel<AccountUserResponseModel>> GetAllUserBAl()
        {
            return await accountDal.GetAllUserDAL();
        }

        public async Task<ResponseModel<bool>> LoginUserBAL(AccountUserModel accountUserModel)
        {
            return await accountDal.LoginUserDAL(accountUserModel);
        }
    }
}
