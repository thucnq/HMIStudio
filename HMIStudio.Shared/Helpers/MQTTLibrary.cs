using System;
using MQTTnet;
using MQTTnet.Server;

namespace HMIStudio.Shared.Helpers
{
    public static class MQTTLibrary
    {
        public static void StartServer()
        {
            // Configure MQTT server.
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(1884);

            var mqttServer = new MqttFactory().CreateMqttServer();
            mqttServer.StartAsync(optionsBuilder.Build());

        }
    }
}
