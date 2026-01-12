using System;
using System.Linq;

namespace TurnstileSolution;

class Program
{
    static void Main(string[] args)
    {
        // Read input
        int n = int.Parse(Console.ReadLine()!);
        int[] times = Console.ReadLine()!.Split().Select(int.Parse).ToArray();
        int[] directions = Console.ReadLine()!.Split().Select(int.Parse).ToArray();

        // Validate input
        if (times.Length != n || directions.Length != n)
        {
            Console.WriteLine("Error: Array lengths must match n");
            return;
        }

        // Create Person objects
        var people = new Person[n];
        for (int i = 0; i < n; i++)
        {
            var direction = directions[i] == 0 ? TurnstileDirection.Enter : TurnstileDirection.Exit;
            people[i] = new Person(i, times[i], direction);
        }

        // Run simulation
        var simulator = new TurnstileSimulator();
        int[] results = simulator.Simulate(people);

        // Output results
        Console.WriteLine(string.Join(" ", results));
    }
}
