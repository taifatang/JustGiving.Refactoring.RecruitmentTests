using System;
using System.Threading.Tasks;
using JustGiving.Finance.Core.Calculators;

namespace JustGiving.Finance.GiftAidCalculator.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please Enter donation amount:");
                var userInput = Console.ReadLine();
                if (InputIsNullEmptyOrEqualsToQ(userInput))
                {
                    break;
                }
                var donationAmount = decimal.Parse(userInput);

                CalculateAsync(donationAmount).Wait();
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        static async Task CalculateAsync(decimal donationAmount)
        {
            var calculator = new Core.Calculators.GiftAidCalculator(new UkTaxClient());
            var giftAidAmount = await calculator.GiftAidAmountAsync(new Donation(donationAmount));

            Console.WriteLine(giftAidAmount);
        }

        private static bool InputIsNullEmptyOrEqualsToQ(string userInput)
        {
            return string.IsNullOrEmpty(userInput) || string.Equals(userInput, "q", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
