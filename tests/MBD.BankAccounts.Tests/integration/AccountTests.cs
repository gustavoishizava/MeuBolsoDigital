using System.Threading.Tasks;
using MBD.BankAccounts.API;
using MBD.BankAccounts.Tests.integration.Settings;
using Xunit;

namespace MBD.BankAccounts.Tests.integration
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class AccountTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;

        public AccountTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }        
    }
}