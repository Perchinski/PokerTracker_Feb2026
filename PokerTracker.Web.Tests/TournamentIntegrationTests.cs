using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using PokerTracker.Data;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace PokerTracker.Web.Tests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, "TestUserId")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's db context registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add an in-memory db
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Configure test authentication
                services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme", options => { });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated(); // will seed if there is any data seeding in OnModelCreating
            });
        }
    }

    [TestFixture]
    public class TournamentIntegrationTests
    {
        private CustomWebApplicationFactory factory;
        private HttpClient client;

        [OneTimeSetUp]
        public void Setup()
        {
            factory = new CustomWebApplicationFactory();
            client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            client?.Dispose();
            factory?.Dispose();
        }

        [Test]
        public async Task Index_ReturnsSuccessResult()
        {
            // Act
            var response = await client.GetAsync("/"); // Start with Home index which allows anonymous anyway
            
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.Content.Headers.ContentType?.ToString(), Does.StartWith("text/html"));
        }

        [Test]
        public async Task TournamentIndex_ReturnsSuccessResult()
        {
            // Act
            var response = await client.GetAsync("/Tournament/Index");
            
            // Assert
            // Because we pass authorization headers and added a test scheme, this should bypass [Authorize] and hit the endpoint.
            response.EnsureSuccessStatusCode();
            Assert.That(response.Content.Headers.ContentType?.ToString(), Does.StartWith("text/html"));
        }

        [Test]
        public async Task Add_ReturnsSuccessResult()
        {
            // Act
            var response = await client.GetAsync("/Tournament/Add");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.Content.Headers.ContentType?.ToString(), Does.StartWith("text/html"));
        }

        [Test]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            // Act
            var response = await client.GetAsync("/Tournament/Details/999999");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Edit_InvalidId_ReturnsForbiddenOrNotFound()
        {
            // Act
            var response = await client.GetAsync("/Tournament/Edit/999999");

            // Assert
            // If the tournament doesn't exist, the service returns null and the controller returns Forbid()
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Forbidden));
        }
    }
}
