﻿using System.Linq;
using System.Threading.Tasks;
using tusdotnet.Adapters;
using tusdotnet.Constants;
using tusdotnet.Interfaces;
using tusdotnet.Models;

namespace tusdotnet.Validation.Specifications
{
    internal class UploadChecksum : Specification
    {
        public override async Task Validate(ContextAdapter context)
        {
            var providedChecksum = context.Request.Headers.ContainsKey(HeaderConstants.UploadChecksum)
                ? new Checksum(context.Request.Headers[HeaderConstants.UploadChecksum].First())
                : null;

            if (context.Configuration.Store is ITusChecksumStore checksumStore && providedChecksum != null)
            {
                if (!providedChecksum.IsValid)
                {
                    await BadRequest($"Could not parse {HeaderConstants.UploadChecksum} header");
                    return;
                }

                var checksumAlgorithms = (await checksumStore.GetSupportedAlgorithmsAsync(context.CancellationToken)).ToList();
                if (!checksumAlgorithms.Contains(providedChecksum.Algorithm))
                {
                    await BadRequest(
                        $"Unsupported checksum algorithm. Supported algorithms are: {string.Join(",", checksumAlgorithms)}");
                }
            }
        }
    }
}
