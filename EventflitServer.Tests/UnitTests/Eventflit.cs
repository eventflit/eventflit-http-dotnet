using NUnit.Framework;
using System;

namespace EventflitServer.Tests.UnitTests
{
    [TestFixture]
    public class When_creating_a_new_Eventflit_instance
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void appId_cannot_be_null()
        {
            new Eventflit(null, "app-key", "app-secret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void appKey_cannot_be_null()
        {
            new Eventflit("app-id", null, "app-secret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void appSecret_cannot_be_null()
        {
            new Eventflit("app-id", "app-key", null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void appId_cannot_be_empty_string()
        {
            new Eventflit(string.Empty, "app-key", "app-secret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void appKey_cannot_empty_string()
        {
            new Eventflit("app-id", string.Empty, "app-secret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void appSecret_cannot_be_empty_string()
        {
            new Eventflit("app-id", "app-key", string.Empty);
        }
    }
}