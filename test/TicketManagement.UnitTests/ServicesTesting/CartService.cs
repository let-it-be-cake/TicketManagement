using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using TicketManagement.UserInterface.Services;

namespace TicketManagement.UnitTests.ServicesTesting
{
    public class CartServiceTesting
    {
        private IHttpContextAccessor _httpContextAccessor;

        [SetUp]
        public void SetUp()
        {
            var session = new SessionMock();

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(o => o.Session).Returns(session);
            httpContextAccessor.Setup(o => o.HttpContext).Returns(httpContext.Object);

            _httpContextAccessor = httpContextAccessor.Object;
        }

        [Test]
        public void AddToCart_WhenAddedElements_ShouldReturnCollection()
        {
            // Arrange
            var cart = new CartTicketService(_httpContextAccessor);
            var expected = new List<int>
            {
                1,
                2,
                3,
            };

            // Act
            cart.AddToCart(1);
            cart.AddToCart(2);
            cart.AddToCart(3);
            var result = cart.GetAllFromCart();

            if (result == null)
            {
                result.Should().BeEmpty();
            }

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void RemoveFromCart_WhenAddedElements_ShouldReturnCollectionWithoutDeletedElement()
        {
            // Arrange
            var cart = new CartTicketService(_httpContextAccessor);
            var expected = new List<int>
            {
                1,
                3,
            };

            // Act
            cart.AddToCart(1);
            cart.AddToCart(2);
            cart.AddToCart(3);
            cart.RemoveFromCart(2);

            var result = cart.GetAllFromCart();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ClearCart_WhenAddedElements_ShouldReturnEmpty()
        {
            // Arrange
            var cart = new CartTicketService(_httpContextAccessor);

            // Act
            cart.AddToCart(1);
            cart.AddToCart(2);
            cart.AddToCart(3);
            cart.ClearCart();

            var result = cart.GetAllFromCart();

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void ClearCart_WhenAddedDublicateElements_ShouldReturnCollectionWithoutDuplicatedElement()
        {
            // Arrange
            var cart = new CartTicketService(_httpContextAccessor);

            var expected = new List<int>
            {
                1,
                2,
                3,
            };

            // Act
            cart.AddToCart(1);
            cart.AddToCart(2);
            cart.AddToCart(2);
            cart.AddToCart(3);

            var result = cart.GetAllFromCart();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
