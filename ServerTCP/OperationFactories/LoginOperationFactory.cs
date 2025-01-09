using System;
using System.Collections.Generic;
using System.Text.Json;
using ServerTCP.ServerOperations;
using ChatDb;
namespace ServerTCP.OperationFactories
{
    public static class LoginOperationFactory 
    {
        private static Dictionary<string, Type> OperationsRegistry { get; set; } = new();

        public static LoginAbstractOperation<T> CreateOperation<T>(string request)
        {
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(request);
            var header = data["operation"].ToString();
            if (!OperationsRegistry.TryGetValue(header, out var operationType))
            {
                throw new InvalidOperationException($"Operation {header} not recognized");
            }

            var dataForOperation = JsonSerializer.Deserialize<Dictionary<string, object>>(data["data"].ToString());

            // Создание экземпляра операции
            var operation = (LoginAbstractOperation<T>)Activator.CreateInstance(operationType);
            operation.Data = dataForOperation;

            return operation;
        }
        public static void RegisterOperation<T>(string operationName, Type operationType)
           where T : class
        {
            if (!typeof(LoginAbstractOperation<T>).IsAssignableFrom(operationType))
            {
                throw new ArgumentException($"Type {operationType.Name} must inherit from AbstractOperationServer<T>");
            }
            OperationsRegistry[operationName] = operationType;
        }
    }
}
