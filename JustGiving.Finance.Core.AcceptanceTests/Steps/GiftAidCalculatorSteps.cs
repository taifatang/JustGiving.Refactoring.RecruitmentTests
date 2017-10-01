using System;
using JustGiving.Finance.Core.AcceptanceTests.Framework;
using JustGiving.Finance.Core.Calculators;
using JustGiving.Finance.Core.DonationTypes;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace JustGiving.Finance.Core.AcceptanceTests.Steps
{
    [Binding]
    public class GiftAidCalculatorSteps
    {
        private GiftAidCalculator _calculator;
        private SetableTaxClient _taxClient;

        [BeforeScenario]
        public void BeforeScenario()
        {
            _taxClient = new SetableTaxClient();
            _calculator = new GiftAidCalculator(_taxClient);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _taxClient = null;
            _calculator = null;
        }

        [Given(@"I want to donate (.*)")]
        public void GivenIWantToDonate(decimal value)
        {
            StateManager.Set(GiftAid.DonationValue.ToString(), DecimalWrapper.Wrap(value));
        }

        [Given(@"Tax rate is (.*)")]
        public void GivenTaxRateIs(int rate)
        {
            _taxClient.SetRate(rate);
        }

        [Given(@"Donation type is (.*)")]
        public void GivenDonationTypeIs(string typeName)
        {
            IDonationType donationType = DonationTypeMapper.Map(typeName);
            StateManager.Set(GiftAid.DonationType.ToString(), donationType);
        }

        [When(@"I calculate gift aid amount")]
        public void WhenICalculateGiftAidAmount()
        {
            var donationValue = StateManager.Get<DecimalWrapper>(GiftAid.DonationValue.ToString()).Value;
            var evenType = TryGetDonationType();

            var result = _calculator.GiftAidAmountAsync(new Donation(donationValue, evenType)).Result;

            StateManager.Set(GiftAid.DonationValue.ToString(), DecimalWrapper.Wrap(result));
        }

        [Then(@"(.*) is returned")]
        public void ThenIsReturned(decimal expectedGiftAid)
        {
            var result = StateManager.Get<DecimalWrapper>(GiftAid.DonationValue.ToString()).Value;
              Assert.That(result == expectedGiftAid);
        }

        private static IDonationType TryGetDonationType()
        {
            IDonationType donationType = new Default();
            try
            {
                donationType = StateManager.Get<IDonationType>(GiftAid.DonationType.ToString());
            }
            catch
            {
                // ignored
            }
            return donationType;
        }
    }
}
