using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeCatalog.Domain.ViewModels.Common;

namespace ShoeCatalog.Helpers
{
    public static class HelperMethods
    {
        public static List<string> ConvertOrderStatusToString(this OrderStatus _)
        {
            var result = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                         .Select(x => x.ToString()).ToList();

            return result;
        }
    }
}
