namespace PlanetaOpic {
    internal class Program {
        static void Main(string[] args) {
            var parameters = ReadParameters();
            var interpreter = new Interpreter(parameters.programPath, parameters.cells);
            interpreter.Run();
            Console.ReadKey();
        }

        /// <summary>
        /// Reads program path and number of cells from the console.
        /// </summary>
        /// <returns></returns>
        static (string programPath, uint cells) ReadParameters() {
            // Program path input
            Console.Write("Zadej cestu k programu pro interpretaci: ");
            string program = Console.ReadLine();
            
            // Number of cells input
            while (true) {
                try {
                    Console.Write("Zadej pocet bunek: ");
                    uint cells = 30000; // default value
                    string input = Console.ReadLine();
                    if (input == null|| input == "") return (program, cells); // return default value
                    cells = uint.Parse(input);
                    if (cells <= 0) {
                        Console.WriteLine("Pocet bunek nesmi byt 0!");
                        throw new FormatException("Value lower than 0!");
                    }
                    return (program, cells);
                } catch (FormatException) {
                    Console.WriteLine("Zadany pocet bunek neni cislo!");
                }
            }
        }
    }
}
