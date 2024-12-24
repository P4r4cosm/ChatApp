using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Patterns
{
    class ComputerProgram
    {
        private static int id_counter = 0;
        public int Id { get; private set; }
        public string Name { get; private set; }
        public ComputerProgram(string name)
        {
            Name = name;
            Id = id_counter++;
        }

        public void Start()
        {
            Console.WriteLine($"Выполняется программа : {this.ToString()}");
            Console.WriteLine("Выполняются вычисления");
            Thread.Sleep(100000);
            Console.WriteLine("Вычисления окончены");
        }
        public async Task StartProgramAsync()
        {
             await Task.Run(Start);
        }
        public override string ToString()
        {
            return $"id: {Id} - {Name}";
        }
    }
}
