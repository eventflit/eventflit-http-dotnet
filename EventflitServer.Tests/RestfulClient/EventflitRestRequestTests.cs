﻿using System;
using NUnit.Framework;
using EventflitServer.RestfulClient;
using EventflitServer.Tests.RestfulClient.Fakes;

namespace EventflitServer.Tests.RestfulClient
{
    [TestFixture]
    public class When_using_a_eventflit_rest_request
    {
        private TestObjectFactory _factory;

        [TestFixtureSetUp]
        public void Setup()
        {
            _factory = new TestObjectFactory();
        }

        [Test]
        public void then_the_request_should_return_the_resource()
        {
            // Act
            var request = new EventflitRestRequest("testUrl");

            // Assert
            Assert.IsNotNull(request.ResourceUri);
            StringAssert.AreEqualIgnoringCase("testUrl", request.ResourceUri);
        }

        [Test]
        public void then_the_request_should_return_the_body_as_a_string_when_present()
        {
            // Arrange
            var request = new EventflitRestRequest("testUrl");

            // Act
            request.Body = _factory.Create("Test Property 1", 2, true);
            var jsonString = request.GetContentAsJsonString();

            // Assert
            Assert.IsNotNull(request);
            StringAssert.Contains("{\"Property1\":\"Test Property 1\",\"Property2\":2,\"Property3\":true}", jsonString);
        }

        [Test]
        public void then_the_request_should_return_the_body_as_null_when_not_present()
        {
            // Arrange
            var request = new EventflitRestRequest("testUrl");

            // Act
            var jsonString = request.GetContentAsJsonString();

            // Assert
            Assert.IsNotNull(request);
            Assert.IsNull(jsonString);
        }

        [Test]
        public void then_the_request_should_throw_an_exception_when_given_an_unpopulated_resource_uri()
        {
            // Arrange
            ArgumentNullException caughtException = null;

            // Act
            try
            {
                var request = new EventflitRestRequest(null);
            }
            catch (ArgumentNullException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.IsNotNull(caughtException);
            StringAssert.AreEqualIgnoringCase($"The resource URI must be a populated string{Environment.NewLine}Parameter name: resourceUri", caughtException.Message);
        }
    }
}