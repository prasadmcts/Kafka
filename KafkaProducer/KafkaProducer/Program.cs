using System;
using Confluent.Kafka;

namespace KafkaProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var conf = new ProducerConfig { BootstrapServers = "localhost:9092" };

            Action<DeliveryReport<Null, string>> handler = r =>
                Console.WriteLine(!r.Error.IsError
                    ? $"Delivered message to {r.TopicPartitionOffset}"
                    : $"Delivery Error: {r.Error.Reason}");

            using (var p = new ProducerBuilder<Null, string>(conf).Build())
            {
                for (int i = 0; i < 10000; ++i)
                {
                    p.Produce("my-topic", new Message<Null, string> { Value = i.ToString() }, handler);
                }

                // wait for up to 10 seconds for any inflight messages to be delivered.
                p.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}

/* CD: F:\WF\kafka_2 .13 - 2.8.0-- Git bash

Run below 2 on diff terminals

1. bin/zookeeper-server-start.sh config/zookeeper.properties
2. bin/kafka-server-start.sh config/server.properties

https://kafka.apache.org/quickstart */