using System.Diagnostics;
using System.Reflection;

Console.WriteLine("Advent Of Code 2024");
Console.WriteLine("--- Day 6: Guard Gallivant ---");

var lines = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));
var stopwatch = new Stopwatch();
stopwatch.Start();

int rowCount = lines.Length;
int colCount = lines[0].Length;
char[,] grid = new char[rowCount, colCount];

char startingDirection = 'X';
int startRow = 0;
int startCol = 0;

for (int row = 0; row < rowCount; row++)
{
    for (int col = 0; col < colCount; col++)
    {
        grid[row, col] = lines[row][col];

        if (grid[row, col] == '^' || grid[row, col] == '>' || grid[row, col] == 'v' || grid[row, col] == '<')
        {
            startingDirection = grid[row, col];
            startRow = row;
            startCol = col;

            Console.WriteLine($"Guard found at: {row},{col} with direction: '{startingDirection}'");            
        }
    }
}

if(startingDirection == 'X')
{
    Console.WriteLine("No guard found in the mapped area!");
    return;
}

var posititons = CalculateNumberOfPositionsForRoute(grid, '^', startRow, startCol, startingDirection);

Console.WriteLine($"Guard finished the route with {posititons} unique positions!");

stopwatch.Stop();
Console.WriteLine($"Calculation performed in: {stopwatch.ElapsedMilliseconds} ms");
Console.ReadKey();

static int CalculateNumberOfPositionsForRoute(char[,] grid, char currentDirection, int startX, int startY, char startingDirection)
{
    var uniquePositions = new HashSet<(int row, int col)>();
    var directions = new Dictionary<char, (int, int)>
    {
        ['^'] = (-1, 0), // up
        ['>'] = (0, 1), // right
        ['v'] = (1, 0), // down
        ['<'] = (0, -1) // left
    };

    int rowCount = grid.GetLength(0);  
    int colCount = grid.GetLength(1);

    int row = startX;   
    int col = startY;

    while (true)
    {
        //First of all don't forget to add the current position to the unique positions
        uniquePositions.Add((row, col));

        //Then itterate through the route
        if (ExecuteStep(grid, ref currentDirection, directions, rowCount, colCount, ref row, ref col, startingDirection, ref uniquePositions))
        {
            break;
        };
    }

    static bool ExecuteStep(char[,] grid, ref char currentDirection, Dictionary<char, (int, int)> directions, int rowCount, int colCount, ref int row, ref int col, char startingDirection, ref HashSet<(int row, int col)> uniquePositions)
    {
        var nextRowPosition = row + directions[currentDirection].Item1;
        var nextColPosition = col + directions[currentDirection].Item2;

        if (!IsInBounds(nextRowPosition, nextColPosition, rowCount, colCount))
        {            
            Console.WriteLine("Guard leaves the mapped area!");
            return true;
        }

        char nextCell = grid[nextRowPosition, nextColPosition];
        if (nextCell == '.' || nextCell == startingDirection)
        {            
            row = nextRowPosition;
            col = nextColPosition;

            uniquePositions.Add((row, col));
        }
        else if (nextCell == '#')
        {
            switch (currentDirection)
            {
                case '^':
                    currentDirection = '>';
                    break;
                case '>':
                    currentDirection = 'v';
                    break;
                case 'v':
                    currentDirection = '<';
                    break;
                case '<':
                    currentDirection = '^';
                    break;
            }

            Console.WriteLine($"New direction: {currentDirection} at position: {row}, {col}");
            ExecuteStep(grid, ref currentDirection, directions, rowCount, colCount, ref row, ref col, startingDirection, ref uniquePositions);
        }
        else
        {
            Console.WriteLine($"Strange character on the route: {nextCell}!");            
        }        

        return false;
    }

    return uniquePositions.Count;
}

static bool IsInBounds(int row, int col, int rowCount, int colCount)
{
    return row >= 0 && row < rowCount && col >= 0 && col < colCount;
}