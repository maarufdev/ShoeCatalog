
namespace ShoeCatalog.Helpers
{
    public static class ResponseDataBuilder<T> where T : class
    {
        public static ResponseData<T> SuccessResponse(ResponseData<T> result)
        {
            return new ResponseData<T>
            {
                Status = RequestStatus.Success,
                Model = result.Model,
                Message = "Success"
            };
        }

        public static ResponseData<T> CreateException(ResponseData<T> ex)
        {
            return new ResponseData<T>
            {
                Status = RequestStatus.Failed,
                Message = ex.Message,
                Model = null,
            };
        }
    }
}
