using System;
using System.Web;
using System.ComponentModel;
using System.Configuration;

namespace uk.ac.hull.repository.hydranet.fedora
{
    public class FedoraServer
    {

        private string _serverAddress;
        private int _serverPort;
        private string _adminUsername;
        private string _adminPassword;

        public FedoraServer() 
        {
            _serverAddress = System.Configuration.ConfigurationManager.AppSettings["HydranetServerAddress"];
            if(String.IsNullOrEmpty(_serverAddress))
                throw new FormatException("Invalid HydranetServerAddress check the application or web config file");
            else
                _serverAddress = _serverAddress.Trim();
            if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["HydranetServerPort"], out _serverPort))
                throw new FormatException("Invalid HydranetServerPort check the application or web config file");
            _adminUsername = System.Configuration.ConfigurationManager.AppSettings["HydranetServerUserName"];
            if(String.IsNullOrEmpty(_adminUsername))
                throw new FormatException("Invalid HydranetServerUserName check the application or web config file");
            else
                _adminUsername = _adminUsername.Trim();
            _adminPassword = System.Configuration.ConfigurationManager.AppSettings["HydranetServerPassword"];
            if (String.IsNullOrEmpty(_adminPassword))
                throw new FormatException("Invalid HydranetServerPassword check the application or web config file");
            else
                _adminPassword = _adminPassword.Trim();
        }

        public String ServerAddress
        {
            get
            {
                return _serverAddress;
            }
            
        }

        public int ServerPort
        {
            get
            {
                return _serverPort;
            }
            set
            {
            }
        }

        public String AdminUsername
        {
            get
            {
                return _adminUsername;
            }
            set
            {
            }
        }

        public String AdminPassword
        {
            get
            {
                return _adminPassword;
            }
            set
            {
            }
        }

     
    }
}
