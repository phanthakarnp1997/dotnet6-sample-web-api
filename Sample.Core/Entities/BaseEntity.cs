using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Entities
{
    public class BaseEntity
    {
        [SwaggerSchema(ReadOnly = true)]
        [JsonIgnore]
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }

        [Column("CREATE_BY")]
        //[JsonIgnore]
        public string? CreateBy { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        [JsonIgnore]
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }

        //[JsonIgnore]
        [Column("UPDATE_BY")]
        public string? UpdateBy { get; set; }

    }
}
