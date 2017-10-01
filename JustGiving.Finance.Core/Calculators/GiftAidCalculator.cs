using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustGiving.Finance.Core.DonationTypes;

namespace JustGiving.Finance.Core.Calculators
{
    public class GiftAidCalculator
    {
        private readonly ITaxClient _taxClient;

        public GiftAidCalculator(ITaxClient taxClient)
        {
            _taxClient = taxClient;
        }

        public async Task<decimal> GiftAidAmountAsync(Donation donation)
        {
            var taxRate = await _taxClient.GetRateAsync();

            var donationAmount = donation.DonationWithSupplment;

            if (donationAmount < 0)
            {
                return 0;
            }
            if (taxRate == 0 || taxRate == 100)
            {
                throw new InvalidOperationException("Tax rate is incorrectly set");
            }

            var gaRatio = GetGaRatio(taxRate);

            var giftAidAmount = ComputeGiftAid(donationAmount, gaRatio);

            return giftAidAmount.ToNearestDecimal(2);
        }

        private static decimal ComputeGiftAid(decimal donationAmount, decimal giftAidRatio)
        {
            return donationAmount * giftAidRatio;
        }

        private static decimal GetGaRatio(decimal taxRate)
        {
            return taxRate / (100 - taxRate);
        }
    }

    public interface ITaxClient
    {
        Task<decimal> GetRateAsync();
    }

    public static class DecimalExtension
    {
        public static decimal ToNearestDecimal(this decimal value, int decimalPoints)
        {
            return Math.Round(value, decimalPoints, MidpointRounding.AwayFromZero);
        }
    }

    public class Donation
    {
        public decimal Value { get; }
        public IDonationType DonationType { get;}
        public decimal DonationWithSupplment => (Value * DonationType.Supplment) + Value;
        public Donation(decimal value)
        {
            Value = value;
            DonationType = new Default();
        }

        public Donation(decimal value, IDonationType donationType)
        {
            Value = value;
            DonationType = donationType;
        }
    }
}
