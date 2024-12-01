using System.Diagnostics;
using System.Reflection;

Console.WriteLine("Advent Of Code 2024");
Console.WriteLine("--- Day 1: Historian Hysteria ---");

var lines = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));
var stopwatch = new Stopwatch();
stopwatch.Start();

List<int> leftList = new List<int>();
List<int> rightList = new List<int>();

foreach (var line in lines)
{
    var lineParts = line.Split("   ");
    leftList.Add(Convert.ToInt32(lineParts[0]));
    rightList.Add(Convert.ToInt32(lineParts[1]));
}

// Sort the lists once
var sortedLeftList = leftList.OrderBy(x => x).ToList();
var sortedRightList = rightList.OrderBy(x => x).ToList();

List<int> differences = new List<int>();
for (int i = 0; i < sortedLeftList.Count; i++)
{
    var difference = Math.Abs(sortedLeftList[i] - sortedRightList[i]);
    differences.Add(difference);
    Console.WriteLine($"Finding nearest number for: {sortedLeftList[i]} on the right list: {sortedRightList[i]} with a difference of: {difference}");
}

Console.WriteLine($"Part 1 - Total difference is: {differences.Sum()}");

List<int> similarityScores = new List<int>();
foreach(var leftItem in leftList)
{
    var appearances = rightList.Count(i => i == leftItem);
    var similarityScore = leftItem * appearances;
    Console.WriteLine($"For item: {leftItem} found {appearances} on the right. Similarity score: {similarityScore}" );
    similarityScores.Add(similarityScore);
}

Console.WriteLine($"Part 2 - Total Similarity Score is: {similarityScores.Sum()}");

stopwatch.Stop();
Console.WriteLine($"Calculation performed in: {stopwatch.ElapsedMilliseconds} ms");
Console.ReadKey();