using System.Runtime.Serialization;

namespace StudentPractice.Web.Api
{
    [DataContract]
    public class ServiceResponse<T> : ServiceResponse
    {
        [DataMember]
        public T Result { get; set; }

        public ServiceResponse(bool isValid, T result, string message = "", int errorCode = 0)
            : base(isValid, message, errorCode)
        {
            Result = result;
        }
    }

    [DataContract]
    public class ServiceResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int ErrorCode { get; set; }

        public ServiceResponse(bool isValid, string message = "", int errorCode = 0)
        {
            Success = isValid;
            Message = message;
            ErrorCode = errorCode;
        }
    }
}
