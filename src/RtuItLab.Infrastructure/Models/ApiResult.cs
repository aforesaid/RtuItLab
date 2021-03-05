using System.Collections.Generic;

namespace RtuItLab.Infrastructure.Models
{
    public class ApiResult<T>
    { 
        public ApiResult(bool succeeded, int code, T result, List<string> errors)
        {
            Succeeded = succeeded;
            Code = code;
            Result = result;
            Errors = errors;
        }
        public ApiResult() { }
        public bool Succeeded { get; set; }

        public int Code { get; set; }

        public T Result { get; set; }

        public List<string> Errors { get; set; }

        public static ApiResult<T> Success(int code, T result)
            => new ApiResult<T>(true, code, result, new List<string>());

        public static ApiResult<T> Success200(T result)
            => Success(200, result);

        public static ApiResult<T> Failure(int code, List<string> errors)
            => new ApiResult<T>(false, code, default, errors);
    }
}
