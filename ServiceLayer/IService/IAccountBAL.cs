using DataObject.RequestModel;
using DataObject.ResponseModel;

namespace ServiceLayer.IService
{
    public interface IAccountBAL
    {
        public Task<ResponseModel<bool>> AddUserBAL(AccountUserModel accountUserModel);
        public Task<ResponseModel<bool>> LoginUserBAL(AccountUserModel accountUserModel);
        public Task<ResponseModel<AccountUserResponseModel>> GetAllUserBAl ();
    }
}
