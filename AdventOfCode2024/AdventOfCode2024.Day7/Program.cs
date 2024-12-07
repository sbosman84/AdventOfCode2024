using System.Diagnostics;
using System.Reflection;

Console.WriteLine("Advent Of Code 2024");
Console.WriteLine("--- Day 7: Bridge Repair ---");

var lines = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));
var stopwatch = new Stopwatch();
stopwatch.Start();

long totalCalibrationResult = 0;

foreach (var line in lines)
{
    var parts = line.Split(':');

    long testValue = long.Parse(parts[0].Trim());
    var numberStrings = parts[1].Trim().Split(' ');
    var numbers = Array.ConvertAll(numberStrings, int.Parse);

    // Generate all possible combinations of '+' and '*' operators
    int operatorCount = numbers.Length - 1;
    int totalCombinations = 1 << operatorCount;

    for (int combination = 0; combination < totalCombinations; combination++)
    {
        long result = numbers[0];

        for (int i = 0; i < operatorCount; i++)
        {
            int currentNumber = numbers[i + 1];
            int operatorBit = (combination >> i) & 1;

            if (operatorBit == 0)
            {
                // '+' operator
                result += currentNumber;
            }
            else
            {
                // '*' operator
                result *= currentNumber;
            }
        }

        if (result == testValue)
        {
            totalCalibrationResult += testValue;
            break;
        }
    }
}

Console.WriteLine($"Total calibration result: {totalCalibrationResult}");

stopwatch.Stop();
Console.WriteLine($"Calculation performed in: {stopwatch.ElapsedMilliseconds} ms");
Console.ReadKey();