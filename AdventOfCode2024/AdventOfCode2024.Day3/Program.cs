using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

Console.WriteLine("Advent Of Code 2024");
Console.WriteLine("--- Day 3: Mull It Over ---");

var lines = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));
var stopwatch = new Stopwatch();
stopwatch.Start();

string pattern = @"mul\((\d+),(\d+)\)";
string doPattern = @"do\(\)";
string dontPattern = @"don't\(\)";

StringBuilder concatLines = new StringBuilder();

List<int> resultsPart1 = new List<int>();
List<int> resultsPart2 = new List<int>();

SortedDictionary<int, string> doAndDont = new SortedDictionary<int, string>();

foreach (var line in lines)
{
    concatLines.Append(line);
}

var doMatches = Regex.Matches(concatLines.ToString(), doPattern);
var dontMatches = Regex.Matches(concatLines.ToString(), dontPattern);

foreach (Match match in doMatches)
{
    Console.WriteLine($"Found 'do()' at position {match.Index}");
    doAndDont.Add(match.Index, "do()");
}

foreach (Match match in dontMatches)
{
    Console.WriteLine($"Found 'don't()' at position {match.Index}");
    doAndDont.Add(match.Index, "don't()");
}

var matches = Regex.Matches(concatLines.ToString(), pattern);
foreach (Match match in matches)
{
    Console.WriteLine($"Found {match.Value}");
    if (match.Groups.Count == 3)
    {
        int firstNumber = int.Parse(match.Groups[1].Value);
        int secondNumber = int.Parse(match.Groups[2].Value);
        int result = firstNumber * secondNumber;

        Console.WriteLine($"First number: {firstNumber}, Second number: {secondNumber} with result: {result}");
        resultsPart1.Add(result);

        if(!doAndDont.Any(x => x.Key < match.Index))
        {
            Console.WriteLine($"Item is before a do() or don't() so add it!");
            resultsPart2.Add(result);
        }
        else if (doAndDont.Where(x => x.Key < match.Index).LastOrDefault().Value == "do()")
        {
            Console.WriteLine($"Item is after a do() so add it!");
            resultsPart2.Add(result);
        }
        else
        {
            Console.WriteLine($"Item is after a don't() so don't add it...");
        }
    }
}


Console.WriteLine($"Part 1 - Sum of all results: {resultsPart1.Sum()}");
Console.WriteLine($"Part 2 - Sum of all results: {resultsPart2.Sum()}");

stopwatch.Stop();
Console.WriteLine($"Calculation performed in: {stopwatch.ElapsedMilliseconds} ms");
Console.ReadKey();