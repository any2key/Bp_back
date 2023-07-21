﻿namespace Bp_back.Models.Responses
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
