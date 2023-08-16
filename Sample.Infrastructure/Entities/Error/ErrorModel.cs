using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Infrastructure.Entities.Error
{
    public class ErrorModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
