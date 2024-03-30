using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeCatalog.Helpers
{
    public class ResponseData<T> where T : class
    {
        public T? Model { get; set; }
        public string? Message { get; set; } = "Success";
        public RequestStatus Status { get; set; } = RequestStatus.Success;
    }
}
