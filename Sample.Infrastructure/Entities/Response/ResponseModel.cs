using Sample.Infrastructure.Entities.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Infrastructure.Entities.Response
{
    public class ResponseModel<T> 
    {
        public string Status { get; set; }
        public T Data { get; set; }
        public ErrorModel Error { get; set; }
    }
}
