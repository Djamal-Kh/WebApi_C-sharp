using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAnimal.Common.ResultPattern
{
    public record Error
    {
        public string Message {  get; }
        public string Code { get; }

        public Error(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}
