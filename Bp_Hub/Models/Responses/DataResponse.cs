namespace Bp_Hub.Models.Responses
{
    public class DataResponse<T> : ErrorResponse
    {
        public DataResponse()
        {
            IsOk = true;
        }
        public T Data { get; set; }
    }
}
