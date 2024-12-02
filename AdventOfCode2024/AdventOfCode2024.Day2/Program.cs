using System.Diagnostics;
using System.Reflection;

Console.WriteLine("Advent Of Code 2024");
Console.WriteLine("--- Day 2: Red-Nosed Reports ---");

var lines = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));
var stopwatch = new Stopwatch();
stopwatch.Start();

int safeReportCount = 0;

foreach (var line in lines)
{    
    var levels = line.Split(' ').Select(int.Parse).ToList();
    bool unsafeReport = false;
    bool isIncreasing = true;
    bool isDecreasing = true;

    for (int i = 1; i < levels.Count; i++)
    {
        int diff = levels[i] - levels[i - 1];
        int absDiff = Math.Abs(diff);
        if (absDiff < 1 || absDiff > 3)
        {
            unsafeReport = true;
            break;
        }

        if (diff > 0)
        {            
            isDecreasing = false;
        }
        if (diff < 0)
        {            
            isIncreasing = false;
        }
    }

    if(unsafeReport)
    {
        Console.WriteLine($"Processing line: {line} - UNSAFE (differs NOT by at least one and at most three.)");
        continue;
    }

    if ((isDecreasing == false && isIncreasing == false))
    {
        Console.WriteLine($"Processing line: {line} - UNSAFE (both increasing and decreasing.)");
        continue;
    }

    Console.WriteLine($"Processing line: {line} - SAFE");
    safeReportCount++;
}

Console.WriteLine($"Part 1 - Number of safe reports: {safeReportCount}");

safeReportCount = 0;
foreach (var line in lines)
{
    var levels = line.Split(' ').Select(int.Parse).ToList();
    bool isSafe = false;

    // Check if the original list is safe
    if (IsSafe(levels))
    {
        isSafe = true;
    }
    else
    {
        // Check if removing one item makes the list safe
        for (int i = 0; i < levels.Count; i++)
        {
            var modifiedLevels = new List<int>(levels);
            modifiedLevels.RemoveAt(i);
            if (IsSafe(modifiedLevels))
            {
                Console.WriteLine($"Remove item at index: {i} with value {levels[i]} to make the list safe.");
                isSafe = true;
                break;
            }
        }
    }

    if (isSafe)
    {
        Console.WriteLine($"Processing line: {line} - SAFE");
        safeReportCount++;
    }
    else
    {
        Console.WriteLine($"Processing line: {line} - UNSAFE");
    }
}

Console.WriteLine($"Part 2 - Number of safe reports: {safeReportCount}");

stopwatch.Stop();
Console.WriteLine($"Calculation performed in: {stopwatch.ElapsedMilliseconds} ms");
Console.ReadKey();

static bool IsSafe(List<int> levels)
{
    bool isIncreasing = true;
    bool isDecreasing = true;

    for (int i = 1; i < levels.Count; i++)
    {
        int diff = levels[i] - levels[i - 1];
        int absDiff = Math.Abs(diff);
        if (absDiff < 1 || absDiff > 3)
        {
            return false;
        }
        if (diff > 0)
        {
            isDecreasing = false;
        }
        if (diff < 0)
        {
            isIncreasing = false;
        }
    }

    return isIncreasing || isDecreasing;
}