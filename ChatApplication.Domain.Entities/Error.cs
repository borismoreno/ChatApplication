using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.Domain.Entities
{
    public class Error
    {
        public int Id { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

    }
}
