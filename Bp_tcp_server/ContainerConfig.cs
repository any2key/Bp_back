using Autofac;
using Bp_tcp_server.Configuration;
using Bp_tcp_server.Server;
using Bp_tcp_server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bp_tcp_server
{
    public static class ContainerConfig
    {
        public static Autofac.IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<BpServer>().As<IBbServer>();
            builder.RegisterType<FileLogger>().As<IBpLogger>();
            builder.RegisterType<BpConfiguration>().As<IBpConfiguration>().SingleInstance();
            return builder.Build();
        }
    }
}
