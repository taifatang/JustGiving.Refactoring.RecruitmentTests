using System;
using System.Threading.Tasks;
using JustGiving.Finance.Core.Calculators;
using JustGiving.Finance.Core.DonationTypes;
using Moq;
using NUnit.Framework;

namespace JustGiving.Finance.Core.UnitTests.Calculators
{
    [TestFixture]
    public class GiftAidCalculatorShould
    {
        private GiftAidCalculator _calculator;
        private Mock<ITaxClient> _taxClientMock;
        [SetUp]
        public void SetUp()
        {
            _taxClientMock = new Mock<ITaxClient>();

            _taxClientMock.Setup(x => x.GetRateAsync()).ReturnsAsync(20.0m);

            _calculator = new GiftAidCalculator(_taxClientMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _taxClientMock = null;
            _calculator = null;
        }

        [Test]
        public async Task Calculate_Donation_Amount()
        {
            var donation = new Donation(1100.08m);
            var result = await _calculator.GiftAidAmountAsync(donation);

            Assert.That(result > 0);
        }

        [Test]
        public async Task Retreive_Tax_Rate_From_Remotely()
        {
            var donation = new Donation(1.01m);
            await _calculator.GiftAidAmountAsync(donation);

            _taxClientMock.Verify(x => x.GetRateAsync(), Times.Once);
        }

        [Test]
        public void Throw_If_Tax_Rate_Is_Zero()
        {
            var donation = new Donation(1.01m);
            _taxClientMock.Setup(x => x.GetRateAsync()).ReturnsAsync(0);

            Assert.ThrowsAsync<InvalidOperationException>(() => _calculator.GiftAidAmountAsync(donation));
        }

        [Test]
        public void Not_Throw_If_Tax_Rate_Is_Greater_Than_Zero()
        {
            var donation = new Donation(1.01m);
            _taxClientMock.Setup(x => x.GetRateAsync()).ReturnsAsync(0.01m);

            Assert.DoesNotThrowAsync(() => _calculator.GiftAidAmountAsync(donation));
        }

        [TestCase(0, 0)]
        [TestCase(-0, 0)]
        [TestCase(-100, 0)]
        [TestCase(10, 2.50)]
        [TestCase(400, 100)]
        [TestCase(1000000000, 250000000)]
        public async Task Calculate_Expected_Result(decimal donationAmount, decimal expectedGiftAid)
        {
            var donation = new Donation(donationAmount);
            _taxClientMock.Setup(x => x.GetRateAsync()).ReturnsAsync(20.0m);

            var result = await _calculator.GiftAidAmountAsync(donation);

            Assert.That(result == expectedGiftAid);
        }

        [Test]
        public async Task Calculate_Expected_Result_For_Maximum_Decimal_Value(  )
        {
            var donation = new Donation(decimal.MaxValue);
            _taxClientMock.Setup(x => x.GetRateAsync()).ReturnsAsync(20.0m);

            var result = await _calculator.GiftAidAmountAsync(donation);

            Assert.That(result == 19807040628566084398385987584M);
        }

        [Test]
        public async Task Calculate_Expected_Result_For_Minimum_Decimal_Value()
        {
            var donation = new Donation(decimal.MinValue);
            _taxClientMock.Setup(x => x.GetRateAsync()).ReturnsAsync(20.0m);

            var result = await _calculator.GiftAidAmountAsync(donation);

            Assert.That(result == 0);
        }

        [TestCase(2.66, 0.67)]
        [TestCase(2524658.16519, 631164.54)]
        [TestCase(0.1, 0.03)]
        [TestCase(0.01, 0)]
        [TestCase(0.02, 0.01)]
        [TestCase(0.03, 0.01)]
        [TestCase(0.04, 0.01)]
        public async Task Round_Result_To_Nearest_Two_Decimal(decimal donationAmount, decimal expectedGiftAid)
        {
            var donation = new Donation(donationAmount);
            _taxClientMock.Setup(x => x.GetRateAsync()).ReturnsAsync(20.0m);

            var result = await _calculator.GiftAidAmountAsync(donation);

            Assert.That(result == expectedGiftAid);
        }

        [Test]
        public async Task Include_Supplement_Amount_For_Running()
        {
            var donation = new Donation(10, new Running());

            _taxClientMock.Setup(x => x.GetRateAsync()).ReturnsAsync(20.0m);

            var result = await _calculator.GiftAidAmountAsync(donation);

            Assert.That(result == 2.63m);
        }
        [Test]
        public async Task Infer_Supplement_Amount_For_Swimming()
        {
            var donation = new Donation(10, new Swiming());

            _taxClientMock.Setup(x => x.GetRateAsync()).ReturnsAsync(20.0m);

            var result = await _calculator.GiftAidAmountAsync(donation);

            Assert.That(result == 2.58m);
        }
        [Test]
        public async Task Infer_Supplement_Amount_For_When_Not_Specified()
        {
            var donation = new Donation(10);

            _taxClientMock.Setup(x => x.GetRateAsync()).ReturnsAsync(20.0m);

            var result = await _calculator.GiftAidAmountAsync(donation);

            Assert.That(result == 2.50m);
        }
    }
}
