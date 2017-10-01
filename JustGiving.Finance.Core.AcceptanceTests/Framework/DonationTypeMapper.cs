using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustGiving.Finance.Core.DonationTypes;

namespace JustGiving.Finance.Core.AcceptanceTests.Framework
{
    internal class DonationTypeMapper
    {
        public static IDonationType Map(string typeName)
        {
            IDonationType donationType = new Default();
            switch (typeName.ToLower())
            {
                case "swimming":
                    donationType = new Swiming();
                    break;
                case "running":
                    donationType = new Running();
                    break;
            }
            return donationType;
        }
    }
}
