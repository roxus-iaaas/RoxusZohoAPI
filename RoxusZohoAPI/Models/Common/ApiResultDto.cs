using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Models.Common
{
    public class ApiResultDto<T>
    {
        public ResultCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public enum ResultCode
    {
        OK = 0,
        NoContent = 1,
        BadRequest = 2,
        NotFound = 3,
        Unauthorize = 4,
        Created = 5,
    }
}
