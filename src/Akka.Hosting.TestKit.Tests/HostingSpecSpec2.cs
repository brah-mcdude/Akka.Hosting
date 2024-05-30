// -----------------------------------------------------------------------
//  <copyright file="HostingSpecSpec.cs" company="Akka.NET Project">
//      Copyright (C) 2009-2022 Lightbend Inc. <http://www.lightbend.com>
//      Copyright (C) 2013-2022 .NET Foundation <https://github.com/akkadotnet/akka.net>
//  </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using Akka.TestKit.TestActors;
using Xunit;
using Xunit.Abstractions;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Akka.Hosting.TestKit.Tests
{
    public class HostingSpecSpec2 : TestKit
    {
        public HostingSpecSpec2(ITestOutputHelper output)
            : base(nameof(HostingSpecSpec), output, logLevel: LogLevel.Debug)
        {
        }

        protected override void ConfigureAkka(AkkaConfigurationBuilder builder, IServiceProvider provider)
        {
            builder.WithActors((system, registry) =>
            {
                var echo = system.ActorOf(Props.Create(() => new SimpleEchoActor()));
                registry.Register<SimpleEchoActor>(echo);
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void ActorTest(int _)
        {
            var echo = ActorRegistry.Get<SimpleEchoActor>();

            echo.Tell("TestMessage");
            var msg = ExpectMsg("TestMessage");
            Log.Info(msg);
        }

        private class SimpleEchoActor : ReceiveActor
        {
            public SimpleEchoActor()
            {
                var log = Context.GetLogger();

                ReceiveAny(msg =>
                {
                    log.Info($"Received {msg}");
                    Sender.Tell(msg);
                });
            }
        }
    }
}