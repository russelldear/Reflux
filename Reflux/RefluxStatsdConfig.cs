using StatsdClient;

namespace Reflux
{
    public static class RefluxStatsdConfig
    {
        public static void Configure()
        {
            var metricsConfig = new StatsdConfig()
            {                
                StatsdServerName = "127.0.0.1",
                Prefix = "Reflux" 
            };

            DogStatsd.Configure(metricsConfig);
        }
    }
}
