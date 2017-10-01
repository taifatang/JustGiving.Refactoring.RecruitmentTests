using System.Threading.Tasks;
using JustGiving.Finance.Core.Calculators;

namespace JustGiving.Finance.Core.AcceptanceTests.Framework
{
    internal class SetableTaxClient: ITaxClient
    {
        private decimal _rate;

        public async Task<decimal> GetRateAsync()
        {
            return await Task.FromResult(_rate);
        }

        public void SetRate(decimal newRate)
        {
            _rate = newRate;
        }
    }
}
