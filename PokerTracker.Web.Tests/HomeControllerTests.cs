using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PokerTracker.Controllers;
using PokerTracker.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;

namespace PokerTracker.Web.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<ILogger<HomeController>> mockLogger;
        private HomeController controller;

        [SetUp]
        public void Setup()
        {
            mockLogger = new Mock<ILogger<HomeController>>();
            
            // Controller needs a controller context to set HttpContext
            controller = new HomeController(mockLogger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            controller?.Dispose();
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Arrange (done in Setup)

            // Act
            var result = controller.Index();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void Privacy_ReturnsViewResult()
        {
            // Arrange (done in Setup)

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void AccessDenied_Returns403View()
        {
            // Arrange (done in Setup)

            // Act
            var result = controller.AccessDenied() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Errors/Error403"));
        }

        [Test]
        public void Error_With404StatusCode_Returns404View()
        {
            // Arrange
            controller.HttpContext.Request.Path = "/test-path";

            // Act
            var result = controller.Error(StatusCodes.Status404NotFound) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Errors/Error404"));
        }

        [Test]
        public void Error_With500StatusCode_Returns500View()
        {
            // Arrange
            controller.HttpContext.Request.Path = "/test-path";

            // Act
            var result = controller.Error(StatusCodes.Status500InternalServerError) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Errors/Error500"));
        }

        [Test]
        public void Error_With403StatusCode_Returns403View()
        {
            // Arrange
            controller.HttpContext.Request.Path = "/test-path";

            // Act
            var result = controller.Error(StatusCodes.Status403Forbidden) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Errors/Error403"));
        }

        [Test]
        public void Error_With400StatusCode_Returns400View()
        {
            // Arrange
            controller.HttpContext.Request.Path = "/test-path";

            // Act
            var result = controller.Error(StatusCodes.Status400BadRequest) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Errors/Error400"));
        }

        [Test]
        public void Error_WithUnhandledException_LogsErrorAndReturns500_WhenNoStatusCodeProvided()
        {
            // Arrange
            var featureCollection = new FeatureCollection();
            featureCollection.Set<IExceptionHandlerPathFeature>(new ExceptionHandlerFeature
            {
                Error = new System.Exception("Test exception"),
                Path = "/error-path"
            });
            controller.HttpContext.Features.Set<IExceptionHandlerPathFeature>(featureCollection.Get<IExceptionHandlerPathFeature>());
            
            // Act
            var result = controller.Error() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Errors/Error500"));
        }

        [Test]
        public void Error_WithNoStatusCodeAndNoException_ReturnsDefaultErrorView()
        {
            // Arrange
            // Act
            var result = controller.Error() as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Errors/Error"));
            Assert.That(result.Model, Is.InstanceOf<ErrorViewModel>());
        }

        [Test]
        public void Error_WithUnknownStatusCode_ReturnsDefaultErrorView()
        {
            // Arrange
            // Passing a random status code
            int unknownStatusCode = 418;

            // Act
            var result = controller.Error(unknownStatusCode) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Errors/Error"));
            Assert.That(result.Model, Is.InstanceOf<ErrorViewModel>());
        }

        [Test]
        public void Error_WithActiveActivity_UsesActivityIdForRequestId()
        {
            // Arrange
            var activity = new Activity("TestActivity").Start();

            try
            {
                // Act
                var result = controller.Error() as ViewResult;
                var model = result?.Model as ErrorViewModel;

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(model, Is.Not.Null);
                Assert.That(model!.RequestId, Is.EqualTo(activity.Id));
            }
            finally
            {
                activity.Stop();
            }
        }

        private class ExceptionHandlerFeature : IExceptionHandlerPathFeature
        {
            public System.Exception Error { get; set; } = new System.Exception();
            public string Path { get; set; } = string.Empty;
            public Microsoft.AspNetCore.Routing.RouteValueDictionary RouteValues { get; set; } = new();
            public Microsoft.AspNetCore.Http.Endpoint Endpoint { get; set; } = null!;
        }
    }
}