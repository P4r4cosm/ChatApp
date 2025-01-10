using ServerTCP.ServerOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ChatDb;
namespace ServerTCP.OperationFactories
{
    internal class UserOperationFactory
    {
        private static Dictionary<string, Type> OperationsRegistry { get; set; } = new();

        public static UserAbstractOperation CreateOperation(string request, User user)
        {
            Console.WriteLine(request);
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(request);
            var header = data["operation"].ToString();
            if (!OperationsRegistry.TryGetValue(header, out var operationType))
            {
                throw new InvalidOperationException($"Operation {header} not recognized");
            }

            var dataForOperation = JsonSerializer.Deserialize<Dictionary<string, object>>(data["data"].ToString());

            // Создание экземпляра операции
            var operation = (UserAbstractOperation)Activator.CreateInstance(operationType, user);
            operation.Data = dataForOperation;
            return operation;
        }
        public static void RegisterOperation(string operationName, Type operationType)
        {
            if (!typeof(UserAbstractOperation).IsAssignableFrom(operationType))
            {
                throw new ArgumentException($"Type {operationType.Name} must inherit from AbstractOperationServer<T>");
            }
            OperationsRegistry[operationName] = operationType;
        }
    }
}
