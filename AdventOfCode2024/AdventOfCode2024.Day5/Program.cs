using System.Diagnostics;
using System.Reflection;

Console.WriteLine("Advent Of Code 2024");
Console.WriteLine("--- Day 5: Print Queue ---");

var lines = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));
var stopwatch = new Stopwatch();
stopwatch.Start();

var rules = new List<(int, int)>();
var updates = new List<List<int>>();
bool parsingRules = true;

foreach (var line in lines)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        parsingRules = false;
        continue;
    }

    if (parsingRules)
    {
        var parts = line.Split('|');
        rules.Add((int.Parse(parts[0]), int.Parse(parts[1])));
    }
    else
    {
        var update = line.Split(',').Select(int.Parse).ToList();
        updates.Add(update);
    }
}

int sumOfMiddlePages = 0;
int sumOfMiddlePagesIncorrect = 0;

foreach (var update in updates)
{
    if (IsUpdateInCorrectOrder(update, rules))
    {
        int middleIndex = update.Count / 2;
        sumOfMiddlePages += update[middleIndex];
    }
    else
    {
        var correctedUpdate = CorrectOrder(update, rules);
        int middleIndex = correctedUpdate.Count / 2;
        sumOfMiddlePagesIncorrect += correctedUpdate[middleIndex];
    }
}

Console.WriteLine($"Part 1 - Sum of middle pages of correctly-ordered updates: {sumOfMiddlePages}");
Console.WriteLine($"Part 2 - Sum of middle pages of corrected updates: {sumOfMiddlePagesIncorrect}");

stopwatch.Stop();
Console.WriteLine($"Calculation performed in: {stopwatch.ElapsedMilliseconds} ms");
Console.ReadKey();

static bool IsUpdateInCorrectOrder(List<int> update, List<(int, int)> rules)
{
    var indexMap = update.Select((value, index) => new { value, index }).ToDictionary(x => x.value, x => x.index);

    foreach (var rule in rules)
    {
        if (indexMap.ContainsKey(rule.Item1) && indexMap.ContainsKey(rule.Item2))
        {
            if (indexMap[rule.Item1] > indexMap[rule.Item2])
            {
                return false;
            }
        }
    }

    return true;
}

static List<int> CorrectOrder(List<int> update, List<(int, int)> rules)
{
    var graph = new Dictionary<int, List<int>>();
    var inDegree = new Dictionary<int, int>();

    foreach (var page in update)
    {
        graph[page] = new List<int>();
        inDegree[page] = 0;
    }

    foreach (var rule in rules)
    {
        if (graph.ContainsKey(rule.Item1) && graph.ContainsKey(rule.Item2))
        {
            graph[rule.Item1].Add(rule.Item2);
            inDegree[rule.Item2]++;
        }
    }

    var queue = new Queue<int>();
    foreach (var page in update)
    {
        if (inDegree[page] == 0)
        {
            queue.Enqueue(page);
        }
    }

    var sortedUpdate = new List<int>();
    while (queue.Count > 0)
    {
        var page = queue.Dequeue();
        sortedUpdate.Add(page);

        foreach (var neighbor in graph[page])
        {
            inDegree[neighbor]--;
            if (inDegree[neighbor] == 0)
            {
                queue.Enqueue(neighbor);
            }
        }
    }

    return sortedUpdate;
}