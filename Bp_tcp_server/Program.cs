using Autofac;
using Bp_tcp_server;
using Bp_tcp_server.Configuration;
using Bp_tcp_server.Server;
using static System.Net.Mime.MediaTypeNames;


var container = ContainerConfig.Configure();
using (var scope = container.BeginLifetimeScope())
{
    scope.Resolve<IBpConfiguration>().Port = int.Parse(Environment.GetCommandLineArgs()[1]);
    var server = scope.Resolve<IBbServer>();
    await server.Start();
}
