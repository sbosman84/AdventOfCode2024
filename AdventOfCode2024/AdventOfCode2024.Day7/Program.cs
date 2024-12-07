using System.Diagnostics;
using System.Reflection;

Console.WriteLine("Advent Of Code 2024");
Console.WriteLine("--- Day 7: Bridge Repair ---");

var lines = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));
var stopwatch = new Stopwatch();
stopwatch.Start();

long totalCalibrationResultPart1 = 0;
long totalCalibrationResultPart2 = 0;

foreach (var line in lines)
{
    var parts = line.Split(':');

    long testValue = long.Parse(parts[0].Trim());
    var numberStrings = parts[1].Trim().Split(' ');
    var numbers = Array.ConvertAll(numberStrings, long.Parse);        

    // Part 1: Using only '+' and '*'
    bool equationCanBeTruePart1 = EvaluateEquations(numbers, testValue, operators: 2);
    if (equationCanBeTruePart1)
    {
        totalCalibrationResultPart1 += testValue;
    }

    // Part 2: Including concatenation '||' operator
    bool equationCanBeTruePart2 = EvaluateEquations(numbers, testValue, operators: 3);
    if (equationCanBeTruePart2)
    {
        totalCalibrationResultPart2 += testValue;
    }
}

Console.WriteLine($"Part 1 - Total calibration result (using '+' and '*'): {totalCalibrationResultPart1}");
Console.WriteLine($"Part 2 - Total calibration result (including '||'): {totalCalibrationResultPart2}");

stopwatch.Stop();
Console.WriteLine($"Calculation performed in: {stopwatch.ElapsedMilliseconds} ms");
Console.ReadKey();

static bool EvaluateEquations(long[] numbers, long testValue, int operators)
{
    // Generate all possible combinations of '+', '*', '||' operators
    int operatorCount = numbers.Length - 1;
    int totalCombinations = (int)Math.Pow(operators, operatorCount);

    for (int combination = 0; combination < totalCombinations; combination++)
    {
        long result = numbers[0];
        int tempCombination = combination;

        for (int i = 0; i < operatorCount; i++)
        {
            long currentNumber = numbers[i + 1];
            int operatorIndex = tempCombination % operators;
            tempCombination /= operators;

            switch (operatorIndex)
            {
                case 0:
                    // '+' operator
                    result += currentNumber;
                    break;
                case 1:
                    // '*' operator
                    result *= currentNumber;
                    break;
                case 2:
                    // '||' operator (concatenation)
                    result = ConcatenateNumbers(result, currentNumber);
                    break;
            }
        }

        if (result == testValue)
        {
            return true;
        }
    }

    return false;
}

static long ConcatenateNumbers(long left, long right)
{
    long multiplier = 1;
    while (multiplier <= right)
    {
        multiplier *= 10;
    }
    return left * multiplier + right;
}