﻿using System.Threading.Tasks;
using tusdotnet.Adapters;
using tusdotnet.Extensions;

namespace tusdotnet.Validation.Specifications
{
    internal class FileIsNotCompleted : Specification
    {
        public override async Task Validate(ContextAdapter context)
        {
            var fileId = context.GetFileId();

            var fileUploadLength = context.Configuration.Store.GetUploadLengthAsync(fileId, context.CancellationToken);
            var fileOffset =
                context.Configuration.Store.GetUploadOffsetAsync(context.GetFileId(), context.CancellationToken);

            await Task.WhenAll(fileUploadLength, fileOffset);

            if (fileUploadLength != null && fileOffset.Result == fileUploadLength.Result)
            {
                await BadRequest("Upload is already complete.");
            }
        }
    }
}
