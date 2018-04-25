using System;
using System.Configuration;

namespace EventflitServer.Tests
{
    internal static class Config
    {
        private const string EVENTFLIT_APP_ID = "EVENTFLIT_APP_ID";
        private const string EVENTFLIT_APP_KEY = "EVENTFLIT_APP_KEY";
        private const string EVENTFLIT_APP_SECRET = "EVENTFLIT_APP_SECRET";
        private const string EVENTFLIT_HTTP_HOST = "EVENTFLIT_APP_HOST";
        private const string EVENTFLIT_WEBSOCKET_HOST = "EVENTFLIT_APP_WEB_SOCKET_HOST";

        private static string _appId;
        private static string _appKey;
        private static string _appSecret;
        private static string _httpHost;
        private static string _websocketHost;

        static Config()
        {
            _appId = Environment.GetEnvironmentVariable(EVENTFLIT_APP_ID);
            if (string.IsNullOrEmpty(_appId))
            {
                _appId = ConfigurationManager.AppSettings.Get(EVENTFLIT_APP_ID);
            }

            _appKey = Environment.GetEnvironmentVariable(EVENTFLIT_APP_KEY);
            if (string.IsNullOrEmpty(_appKey))
            {
                _appKey = ConfigurationManager.AppSettings.Get(EVENTFLIT_APP_KEY);
            }

            _appSecret = Environment.GetEnvironmentVariable(EVENTFLIT_APP_SECRET);
            if (string.IsNullOrEmpty(_appSecret))
            {
                _appSecret = ConfigurationManager.AppSettings.Get(EVENTFLIT_APP_SECRET);
            }

            _httpHost = Environment.GetEnvironmentVariable(EVENTFLIT_HTTP_HOST);
            if (string.IsNullOrEmpty(_httpHost))
            {
                _httpHost = ConfigurationManager.AppSettings.Get(EVENTFLIT_HTTP_HOST);
            }

            _websocketHost = Environment.GetEnvironmentVariable(EVENTFLIT_WEBSOCKET_HOST);
            if (string.IsNullOrEmpty(_websocketHost))
            {
                _websocketHost = ConfigurationManager.AppSettings.Get(EVENTFLIT_WEBSOCKET_HOST);
            }
        }

        public static string AppId
        {
            get
            {
                return _appId;
            }
            set
            {
                _appId = value;
            }
        }

        public static string AppKey
        {
            get
            {
                return _appKey;
            }
            set
            {
                _appKey = value;
            }
        }

        public static string AppSecret
        {
            get
            {
                return _appSecret;
            }
            set
            {
                _appSecret = value;
            }
        }

        public static string HttpHost
        {
            get
            {
                return _httpHost;
            }
            set
            {
                _httpHost = value;
            }
        }

        public static string WebSocketHost
        {
            get
            {
                return _websocketHost;
            }
            set
            {
                _websocketHost = value;
            }
        }
    }
}
