using System;
using BIIS.Identities;

namespace BIIS
{
    class Program
    {
        private static readonly BiisConfigurationManager Instance = BiisConfigurationManager.Instance;
        /// <summary>
        /// Enter point of app
        /// </summary>
        static void Main()
        {
            using (var server = new BiisServer(Instance.Port, Instance.WebRoot))
            {
                server.StartServer();
            }
        }

        // BiisButton onClick implmentation
        public static void Button_OnClick(BiisLabel biisControl) => biisControl.Text = DateTime.Now.ToShortDateString();
    }
}
