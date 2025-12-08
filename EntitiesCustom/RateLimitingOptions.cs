using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesCustom
{
    public class RateLimitingOptions
    {
        public int PermitLimit { get; set; }
        public int WindowSeconds { get; set; }
    }
}
