using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;

namespace WorkFM.Common.Dto
{
    public class FileDto
    {
        public Stream Stream { get; set; }
        public string ObjectName { get; set; }
    }
}
