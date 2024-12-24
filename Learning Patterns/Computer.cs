using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Patterns
{
    class Computer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private static int id_counter = 0;
        public Computer(string name, User user)
        {
            Name = name;
            Id = id_counter++;
            User = user;
        }
        public User User { get; private set; }
        public List<ComputerProgram> computerPrograms { get; private set; } = new List<ComputerProgram>();
        public void AddComputerProgram(ComputerProgram computerProgram)
        {
            this.computerPrograms.Add(computerProgram);
        }
        public void AddComputerProgram(IEnumerable<ComputerProgram> computerPrograms)
        {
            this.computerPrograms.AddRange(computerPrograms);
        }
        public async Task RunProgramAsync(string name)
        {
            var task = computerPrograms.FirstOrDefault(p => p.Name == name);
            await Task.Run(task.StartProgramAsync);
        }
        public async Task RunProgramsAsync()
        {
            var tasks = computerPrograms.Select(program => program.StartProgramAsync());
            await Task.WhenAll(tasks);
        }
        public string ComputerProgramsToString()
        {
            string answer = "\n\t";
            foreach (var program in computerPrograms)
            {
                answer += program.ToString() + "\n\t";
            }
            if (computerPrograms.Count == 0) answer = "Список программ пуст";
            return answer;
        }
        public override string ToString()
        {
            return $"Computer computer_id: {Id}: {Name} \n \t User: {User.ToString()} \nPrograms: {ComputerProgramsToString()}";
        }
    }
}
