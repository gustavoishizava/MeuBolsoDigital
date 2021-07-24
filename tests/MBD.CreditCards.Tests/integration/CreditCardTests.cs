using System.Threading.Tasks;
using MBD.CreditCards.API;
using MBD.CreditCards.Tests.integration.Settings;
using Xunit;

namespace MBD.CreditCards.Tests.integration
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class CreditCardTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public CreditCardTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact]
        public async Task TestName()
        {
            await _testsFixture.AuthenticateAsync();
            var id = await _testsFixture.GetFirstBankAccountIdAsync();            
            var test = await _testsFixture.CreateBankAccountAsync();
        }
    }
}