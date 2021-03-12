using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JwtAuth
{
    public class MyFirstHostedService : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Log();
                await Task.Delay(1000, token);
            }
        }

        private async Task Log()
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString());
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
