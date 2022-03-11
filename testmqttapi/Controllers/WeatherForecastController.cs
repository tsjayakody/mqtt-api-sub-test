using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace testmqttapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        [HttpGet]
        public async Task GetMessages()
        {
            var mqttFactory = new MqttFactory();
            IMqttClient client = mqttFactory.CreateMqttClient();


            var options = new MqttClientOptionsBuilder()
                .WithClientId("mqttx_236547263")
                .WithTcpServer("test.mosquitto.org", 1883)
                .WithCleanSession()
                .Build();

            client.UseConnectedHandler(async e => {
                System.Diagnostics.Debug.Print("connected to the broker");
                Console.WriteLine("connected to the broker");

                var topicFilter = new MqttTopicFilterBuilder()
                                        .WithTopic("test221")
                                        .Build();

                await client.SubscribeAsync(topicFilter);
            });

            client.UseDisconnectedHandler(e => {
                Console.WriteLine("Disconnected from the broker.");
            });

            client.UseApplicationMessageReceivedHandler(e => {
                Console.WriteLine($"Recieved Message - { Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            });

            await client.ConnectAsync(options);

        }
    }
}
