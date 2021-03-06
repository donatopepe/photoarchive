using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PhotoArchiveCoilsWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sharp7;


namespace PhotoArchiveCoilsWeb
{
    public class MyServiceA : BackgroundService
    {
        //private readonly FileContext _context;
        private readonly IOptions<ApplicationConfiguration> _optionsApplicationConfiguration;



        //public MyServiceA(FileContext context, IOptions<ApplicationConfiguration> o)
        public MyServiceA(IOptions<ApplicationConfiguration> o, IConfiguration config)
        {
            //_context = context;
            _optionsApplicationConfiguration = o;
            ImageSave.Instance().connection = config.GetConnectionString("DefaultConnection");
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("MyServiceA is starting.");




            // Create and connect the client
            var client = new S7Client();
            int result = client.ConnectTo("127.0.0.1", 0, 1);
            if (result == 0)
            {
                Console.WriteLine("Connected to 127.0.0.1");
                Console.WriteLine("\n---- Read DB 1");

                byte[] db1Buffer = new byte[18];
                result = client.DBRead(1, 0, 18, db1Buffer);
                if (result != 0)
                {
                    Console.WriteLine("Error: " + client.ErrorText(result));
                }
                int db1dbw2 = S7.GetIntAt(db1Buffer, 2);
                Console.WriteLine("DB1.DBW2: " + db1dbw2);

                double db1ddd4 = S7.GetRealAt(db1Buffer, 4);
                Console.WriteLine("DB1.DBD4: " + db1ddd4);

                double db1dbd8 = S7.GetDIntAt(db1Buffer, 8);
                Console.WriteLine("DB1.DBD8: " + db1dbd8);

                double db1dbd12 = S7.GetDWordAt(db1Buffer, 12);
                Console.WriteLine("DB1.DBD12: " + db1dbd12);

                double db1dbw16 = S7.GetWordAt(db1Buffer, 16);
                Console.WriteLine("DB1.DBD16: " + db1dbw16);
            }
            else
            {
                Console.WriteLine(client.ErrorText(result));
            }




            


            stoppingToken.Register(() => Console.WriteLine("MyServiceA is stopping."));


            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("MyServiceA is doing background work.");

                
                Console.WriteLine("Numero di telecamere configurate: " + _optionsApplicationConfiguration.Value.Cams.Length);

                for (int i = 0; i < _optionsApplicationConfiguration.Value.Cams.Length; i++)
                {
                    if (ImageSave.Instance().connection!=null)
                    await ImageSave.Instance().ImageSaveAsync(_optionsApplicationConfiguration.Value.Cams[i]);


                }
                

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }


            // Disconnect the client
            client.Disconnect();


            Console.WriteLine("MyServiceA background task is stopping.");

        }
    }
}
