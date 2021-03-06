﻿using System;
using System.Threading.Tasks;
using tusdotnet.Adapters;

namespace tusdotnet.Validation.Specifications
{
    internal class ContentType : Specification
    {
        public override Task Validate(ContextAdapter context)
        {
            if (context.Request.ContentType?.Equals("application/offset+octet-stream", StringComparison.OrdinalIgnoreCase) != true)
            {
                BadRequest($"Content-Type {context.Request.ContentType} is invalid. Must be application/offset+octet-stream");
            }

            return Done;
        }
    }
}
