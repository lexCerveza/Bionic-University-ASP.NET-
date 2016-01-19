using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace BIIS
{
    /// <summary>
    ///  Configuration class for Biis server. Implemented as Singleton
    /// </summary>
    public sealed class BiisConfigurationManager
    {
        /// <summary>
        /// Instance of config helper class
        /// </summary>
        private static BiisConfigurationManager _biisConfigurationManager;
        /// <summary>
        /// Port, on which Biis server will be opened
        /// </summary>
        public int Port { get; } = int.Parse(ConfigurationManager.AppSettings["Port"]);
        /// <summary>
        /// Folder, which Biis server will host
        /// </summary>
        public string WebRoot { get; } = $"{Environment.CurrentDirectory}{ConfigurationManager.AppSettings["WebRoot"]}";
        /// <summary>
        /// Instance of Biis server config manager
        /// </summary>
        public static BiisConfigurationManager Instance
        {
            get
            {
                if (ReferenceEquals(_biisConfigurationManager, null))
                {
                    _biisConfigurationManager = new BiisConfigurationManager();
                }

                return _biisConfigurationManager;
            }
        }
        /// <summary>
        /// Just private constructor for Singleton class
        /// </summary>
        private BiisConfigurationManager() { }
    }
}