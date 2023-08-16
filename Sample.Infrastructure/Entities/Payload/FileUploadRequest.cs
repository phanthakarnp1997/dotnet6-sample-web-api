using Sample.WebAPI.Core.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Infrastructure.Entities.Payload
{
    public class FileUploadRequest<T>
    {
        public List<IFormFile> Files { get; set; }
        public T Payload { get; set; }
    }

}
