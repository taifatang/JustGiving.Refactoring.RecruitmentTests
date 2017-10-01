using System.Threading.Tasks;
using JustGiving.Finance.Core.Calculators;

namespace JustGiving.Finance.GiftAidCalculator.TestConsole
{
   public class UkTaxClient: ITaxClient
    {
        public Task<decimal> GetRateAsync()
        {
            return Task.FromResult(20.0m);
        }
    }
}
