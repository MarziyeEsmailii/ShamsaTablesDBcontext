using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Order
{
    public class OrderReportRequestSto
    {
        public int PageNo { get; set; }

        public int PageSize { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
