using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JwtAuth
{
    public class LogEvent : INotification
    {
        public string message;
        public LogEvent(string message)
        {
            this.message = message;
        }
    }

    public class FileNotificationHandler : INotificationHandler<LogEvent>
    {
        public Task Handle(LogEvent notification, CancellationToken cancellationToken)
        {
            string message = notification.message;

            Log(message);

            return Task.FromResult(0);
        }

        private void Log(string message)
        {
            //Write code here to log message(s) to a text file
            Console.WriteLine("Write code here to log message(s) to a text file"+ message);
        }
    }

    public class DBNotificationHandler : INotificationHandler<LogEvent>
    {
        public Task Handle(LogEvent notification, CancellationToken cancellationToken)
        {
            string message = notification.message;

            Log(message);

            return Task.FromResult(0);
        }
        private void Log(string message)
        {
            //Write code here to log message(s) to the database
            Console.WriteLine("Write code here to log message(s) to the database" + message);
        }
    }
}
