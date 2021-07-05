﻿using System;
using System.Net;

namespace Codeworx.Rest
{
    public class UnexpectedHttpStatusCodeException : InvalidOperationException
    {
        public UnexpectedHttpStatusCodeException(HttpStatusCode statusCode)
            : base($"Unexpected http status code ({(int)statusCode}).")
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}
