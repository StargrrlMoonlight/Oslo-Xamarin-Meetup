﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace XamAI.Utils
{
    [DataContract]
    public class ClientExceptionMessage
    {
        [DataMember(Name = "code")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }

    [DataContract]
    public class ClientError
    {
        [DataMember(Name = "error")]
        public ClientExceptionMessage Error { get; set; }
    }

    [DataContract]
    public class ServiceError
    {
        [DataMember(Name = "statusCode")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
