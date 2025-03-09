using ServerTCP.ServerOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using tcpClient.PublicClasses;
namespace tcpClient.ClientOperations
{
    internal static class OperationFactory
    {
        private static Task _serverListener;
        private static CancellationTokenSource _cts;
        private static SslStream _stream;
        public static event Action<PublicMessage> MessageRecived;

        public static Dictionary<string, object> OperationData;

        public static bool AnswerTaken = false;
        public static void Initialize(SslStream sslStream)
        {
            _stream = sslStream;
            _cts = new CancellationTokenSource();
            _serverListener = ListenServer(_cts.Token);
        }
        public async static Task<T> RunOperation<T>(AbstractOperationClient operation) where T : class
        {
            //устанавливаем флаг получении ответа
            AnswerTaken = false;

            //выполняем запуск операции
            await operation.RunOperationAsync();
            while (!AnswerTaken)
            {
                
            }

            //устанавливаем флаг снова в false
            AnswerTaken = false;

            //возвращаем результат операции
            if (OperationData["responseAnswer"].ToString() == $"{operation.Name} OK")
            {
                Console.WriteLine(OperationData["responseAnswer"].ToString());
                return JsonSerializer.Deserialize<T>(OperationData["data"].ToString());
            }
            else
            {
                Console.WriteLine(OperationData["responseAnswer"].ToString());
            }
            return null;

        }

        public async static Task ListenServer(CancellationToken token)
        {
            // Пример бесконечного цикла, который завершится при отмене
            while (!token.IsCancellationRequested)
            {
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>
                    (await SecureCommunication.ReadServerMessageAsync(_stream, token));
                var responseAnswer = data["responseAnswer"].ToString();
                switch (responseAnswer)
                {
                    case "New message":
                        {
                            var message = JsonSerializer.Deserialize<PublicMessage>(data["data"].ToString());
                            MessageRecived?.Invoke(message);
                            //Console.WriteLine($"Сообщение получено : {message.ToString()}");
                            break;
                        }
                    default:
                        {
                            OperationData = data;
                            Console.WriteLine($"Сообщение получено : ");
                            AnswerTaken = true;
                            break;
                        }
                }

            }
        }
    }
}
