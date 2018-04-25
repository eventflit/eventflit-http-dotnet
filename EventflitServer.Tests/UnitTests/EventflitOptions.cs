using NSubstitute;
using NUnit.Framework;
using System;

namespace EventflitServer.Tests.UnitTests
{
    [TestFixture]
    public class When_creating_a_new_EventflitOptions_instance
    {
        [Test]
        public void a_default_RestClient_should_be_used_if_one_is_not_set_on_EventflitOptions_parameter()
        {
            var options = new EventflitOptions();
            Assert.IsNotNull(options.RestClient);
        }

        [Test]
        public void Port_defaults_to_80()
        {
            var options = new EventflitOptions();
            Assert.AreEqual(80, options.Port);
        }

        [Test]
        public void when_Encrypted_option_is_set_Port_is_changed_to_443()
        {
            var options = new EventflitOptions() { Encrypted = true };
            Assert.AreEqual(443, options.Port);
        }

        [Test]
        public void when_Encrypted_option_is_set_Port_is_changed_to_443_unless_Port_has_already_been_modified()
        {
            var options = new EventflitOptions() { Port = 90 };
            options.Encrypted = true;
            Assert.AreEqual(90, options.Port);
        }

        [Test]
        public void the_default_options_should_be_used_to_create_the_base_url_when_no_settings_are_changed()
        {
            var options = new EventflitOptions();

            StringAssert.IsMatch("http://service.eventflit.com", options.GetBaseUrl().AbsoluteUri);
        }

        [Test]
        public void the_default_cluster_is_null()
        {
          var options = new EventflitOptions();

          Assert.AreEqual(null, options.Cluster);
        }

        [Test]
        public void the_default_encrypted_options_should_be_used_to_create_the_base_url_when_encrypted_is_true()
        {
            var options = new EventflitOptions();
            options.Encrypted = true;

            StringAssert.IsMatch("https://service.eventflit.com", options.GetBaseUrl().AbsoluteUri);
        }

        [Test]
        public void the_new_cluster_should_be_used_to_create_the_base_url()
        {
          var options= new EventflitOptions();
          options.Cluster = "eu";

          StringAssert.IsMatch("http://api-eu.eventflit.com", options.GetBaseUrl().AbsoluteUri);
        }

        [Test]
        public void the_new_port_should_be_used_to_create_the_base_url()
        {
            var options = new EventflitOptions();
            options.Port = 100;

            StringAssert.IsMatch("http://service.eventflit.com:100", options.GetBaseUrl().AbsoluteUri);
        }

        [Test]
        public void the_new_port_should_be_used_to_create_the_base_url_when_its_encrypted()
        {
            var options = new EventflitOptions();
            options.Encrypted = true;
            options.Port = 100;

            StringAssert.IsMatch("https://service.eventflit.com:100", options.GetBaseUrl().AbsoluteUri);
        }

        [Test]
        public void the_new_cluster_should_be_used_to_create_the_base_url_when_its_encrypted_and_has_a_custom_port()
        {
          var options = new EventflitOptions();
          options.Encrypted = true;
          options.Cluster = "eu";
          options.Port = 100;

          StringAssert.IsMatch("https://api-eu.eventflit.com:100", options.GetBaseUrl().AbsoluteUri);
        }

        [Test]
        public void the_cluster_should_be_ignored_when_host_name_is_set_first()
        {
          var options = new EventflitOptions();
          options.HostName = "api.my.domain.com";
          options.Cluster = "eu";

          StringAssert.IsMatch("http://api.my.domain.com", options.GetBaseUrl().AbsoluteUri);
          Assert.AreEqual(null, options.Cluster);
        }

        [Test]
        public void the_cluster_should_be_ignored_when_host_name_is_set_after()
        {
          var options = new EventflitOptions();

          options.Cluster = "eu";
          StringAssert.IsMatch("http://api-eu.eventflit.com", options.GetBaseUrl().AbsoluteUri);

          options.HostName = "api.my.domain.com";
          StringAssert.IsMatch("http://api.my.domain.com", options.GetBaseUrl().AbsoluteUri);
          Assert.AreEqual(null, options.Cluster);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void https_scheme_is_not_allowed_when_setting_host()
        {
            var httpsOptions = new EventflitOptions();
            httpsOptions.HostName = "https://service.eventflit.com";
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void http_scheme_is_not_allowed_when_setting_host()
        {
            var httpsOptions = new EventflitOptions();
            httpsOptions.HostName = "http://service.eventflit.com";
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void ftp_scheme_is_not_allowed_when_setting_host()
        {
            var httpsOptions = new EventflitOptions();
            httpsOptions.HostName = "ftp://service.eventflit.com";
        }

        [Test]
        public void the_json_deserialiser_should_be_the_default_one_when_none_is_set()
        {
            var options = new EventflitOptions();

            Assert.IsInstanceOf<DefaultDeserializer>(options.JsonDeserializer);
        }

        [Test]
        public void the_json_deserialiser_should_be_the_supplied_one_when_set()
        {
            var options = new EventflitOptions();
            options.JsonDeserializer = new FakeDeserialiser();

            Assert.IsInstanceOf<FakeDeserialiser>(options.JsonDeserializer);
        }

        [Test]
        public void the_json_deserialiser_should_be_the_supplied_one_when_set_with_a_custom_and_the_set_to_null()
        {
            var options = new EventflitOptions();
            options.JsonDeserializer = new FakeDeserialiser();
            options.JsonDeserializer = null;

            Assert.IsInstanceOf<DefaultDeserializer>(options.JsonDeserializer);
        }

        [Test]
        public void the_json_serialiser_should_be_the_default_one_when_none_is_set()
        {
            var options = new EventflitOptions();

            Assert.IsInstanceOf<DefaultSerializer>(options.JsonSerializer);
        }

        [Test]
        public void the_json_serialiser_should_be_the_supplied_one_when_set()
        {
            var options = new EventflitOptions();
            options.JsonSerializer = new FakeSerialiser();

            Assert.IsInstanceOf<FakeSerialiser>(options.JsonSerializer);
        }

        [Test]
        public void the_json_serialiser_should_be_the_default_one_when_set_with_a_custom_and_the_set_to_null()
        {
            var options = new EventflitOptions();
            options.JsonSerializer = new FakeSerialiser();
            options.JsonSerializer = null;

            Assert.IsInstanceOf<DefaultSerializer>(options.JsonSerializer);
        }

        private class FakeDeserialiser : IDeserializeJsonStrings
        {
            public T Deserialize<T>(string stringToDeserialize)
            {
                throw new NotImplementedException();
            }
        }

        private class FakeSerialiser : ISerializeObjectsToJson
        {
            public string Serialize(object objectToSerialize)
            {
                throw new NotImplementedException();
            }
        }
    }
}
