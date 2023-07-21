namespace Bp_Hub.Models.Responses
{
    public class Response
    {
        public Response()
        {

        }
        public static ErrorResponse BadResponse(string error)
        {
            return new ErrorResponse() { IsOk = false, Message = error };
        }

        public static Response OK = new Response() { IsOk = true };
        public static ErrorResponse Forbidden = new ErrorResponse() { IsOk = false, Message = "Недостаточно прав для совершения этой операции" };
        public bool IsOk { get; set; }
    }
}
