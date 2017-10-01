using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustGiving.Finance.Core.AcceptanceTests.Framework
{
   internal class DecimalWrapper
    {
        public decimal Value { get; set; }

        public static DecimalWrapper Wrap(decimal value)
        {
            return new DecimalWrapper
            {
                Value = value
            };
        }
    }
}
