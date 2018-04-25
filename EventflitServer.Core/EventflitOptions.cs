﻿using System;
using System.Text.RegularExpressions;
using EventflitServer.RestfulClient;

namespace EventflitServer
{
    /// <summary>
    /// Options to be set on the <see cref="Eventflit">Eventflit</see> instance.
    /// </summary>
    public class EventflitOptions : IEventflitOptions
    {
        /// <summary>
        /// The default Rest API Host for contacting the Eventflit server, it does not contain a cluster name
        /// </summary>
        public const string DEFAULT_REST_API_HOST = "service.eventflit.com";

        private static int DEFAULT_HTTPS_PORT = 443;
        private static int DEFAULT_HTTP_PORT = 80;

        IEventflitRestClient _eventflitClient;
        bool _encrypted;
        bool _portModified;
        bool _hostSet;
        int _port = DEFAULT_HTTP_PORT;
        string _hostName;
        string _cluster;
        ISerializeObjectsToJson _jsonSerializer;
        IDeserializeJsonStrings _jsonDeserializer;

        /// <inheritedDoc/>
        public bool Encrypted
        {
            get
            {
                return _encrypted;
            }
            set
            {
                _encrypted = value;
                if (_encrypted && _portModified == false)
                {
                    _port = DEFAULT_HTTPS_PORT;
                }
            }
        }

        /// <inheritDoc/>
        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
                _portModified = true;
            }
        }

        /// <inheritDoc/>
        public string Cluster
        {
          get
          {
            return _cluster;
          }
          set
          {
            if (_hostSet == false) {
              _cluster = value;
              _hostName = "api-"+_cluster+".eventflit.com";
            }
          }
        }

        /// <inheritDoc/>
        public IEventflitRestClient RestClient
        {
            get
            {
                if (_eventflitClient == null)
                {
                    _eventflitClient = new EventflitRestClient(GetBaseUrl(), Eventflit.LIBRARY_NAME, Eventflit.VERSION);
                }

                return _eventflitClient;
            }
            set { _eventflitClient = value; }
        }

        /// <inheritDoc/>
        public string HostName
        {
            get
            {
                return _hostName ?? DEFAULT_REST_API_HOST;
            }
            set
            {
                if (Regex.IsMatch(value, "^.*://"))
                {
                    string msg = string.Format("The scheme should not be present in the host value: {0}", value);
                    throw new FormatException(msg);
                }

                _hostSet = true;
                _cluster = null;
                _hostName = value;
            }
        }

        /// <inheritDoc/>
        public ISerializeObjectsToJson JsonSerializer
        {
            get
            {
                if (_jsonSerializer == null)
                {
                    _jsonSerializer = new DefaultSerializer();
                }

                return _jsonSerializer;

            }
            set { _jsonSerializer = value; }
        }

        /// <inheritDoc/>
        public IDeserializeJsonStrings JsonDeserializer
        {
            get
            {
                if (_jsonDeserializer == null)
                {
                    _jsonDeserializer = new DefaultDeserializer();
                }

                return _jsonDeserializer;
            }
            set
            {
                _jsonDeserializer = value;
            }
        }

        /// <inheritDoc/>
        public Uri GetBaseUrl()
        {
            string baseUrl = (Encrypted ? "https" : "http") + "://" + HostName + GetPort();

            return new Uri(baseUrl);
        }

        private string GetPort()
        {
            var port = string.Empty;

            if (Port != DEFAULT_HTTP_PORT)
            {
                port += (":" + Port);
            }

            return port;
        }
    }
}
