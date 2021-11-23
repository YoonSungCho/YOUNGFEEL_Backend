using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.General.Request
{
    public class FileUploadRequest
    {
        public string Name { get; set; }

        public string Extension { get; set; }

        public string Data { get; set; }
    }
}
