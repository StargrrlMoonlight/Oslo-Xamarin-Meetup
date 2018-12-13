using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace XamAI.Exceptions
{
    public class FaceAPIException : Exception
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode HttpStatus { get; set; }

        public FaceAPIException(string errorCode, string errorMessage, HttpStatusCode statusCode)
            : base(errorMessage + "(" + errorCode + ")")
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            HttpStatus = statusCode;
        }
    }
}
