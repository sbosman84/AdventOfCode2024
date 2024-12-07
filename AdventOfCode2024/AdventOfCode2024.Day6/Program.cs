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

var (uniquePositionsCount, obstructionPositions) = CalculateNumberOfPositionsForRoute(grid, startingDirection, startRow, startCol, startingDirection);

Console.WriteLine($"Guard finished the route with {uniquePositionsCount} unique positions!");
Console.WriteLine($"Found {obstructionPositions.Count} obstruction positions!");

stopwatch.Stop();
Console.WriteLine($"Calculation performed in: {stopwatch.ElapsedMilliseconds} ms");
Console.ReadKey();

static (int uniquePositionsCount, HashSet<(int row, int col)> obstructionPositions) CalculateNumberOfPositionsForRoute(
    char[,] grid, char currentDirection, int startX, int startY, char startingDirection)
{
    var uniquePositions = new HashSet<(int row, int col)>();
    var obstructionPositions = new HashSet<(int row, int col)>();
    var directions = new Dictionary<char, (int, int)>
    {
        ['^'] = (-1, 0), // up
        ['>'] = (0, 1),  // right
        ['v'] = (1, 0),  // down
        ['<'] = (0, -1)  // left
    };

    int rowCount = grid.GetLength(0);
    int colCount = grid.GetLength(1);

    int row = startX;
    int col = startY;

    var visitedStates = new HashSet<(int row, int col, char direction)>();
    bool isLoopDetected = false;

    while (true)
    {
        // Add the current position to unique positions
        uniquePositions.Add((row, col));

        // Record the current state (position and direction)
        if (visitedStates.Contains((row, col, currentDirection)))
        {
            isLoopDetected = true;
            break;
        }
        visitedStates.Add((row, col, currentDirection));

        // Calculate next position
        var nextRowPosition = row + directions[currentDirection].Item1;
        var nextColPosition = col + directions[currentDirection].Item2;

        if (!IsInBounds(nextRowPosition, nextColPosition, rowCount, colCount))
        {
            // Guard leaves the mapped area
            break;
        }

        char nextCell = grid[nextRowPosition, nextColPosition];
        if (nextCell == '.' || nextCell == '^' || nextCell == '>' || nextCell == 'v' || nextCell == '<')
        {
            row = nextRowPosition;
            col = nextColPosition;
        }
        else if (nextCell == '#')
        {
            // Rotate right
            currentDirection = RotateRight(currentDirection);
        }
        else
        {
            // Unexpected character
            break;
        }
    }

    // Identify positions where placing an obstruction would cause the guard to get stuck in a loop
    for (int r = 0; r < rowCount; r++)
    {
        for (int c = 0; c < colCount; c++)
        {
            if (grid[r, c] == '.' && !(r == startX && c == startY))
            {
                // Place an obstruction at the current position
                grid[r, c] = '#';

                // Simulate the guard's movement with the obstruction
                if (DoesObstructionCauseLoop(grid, startX, startY, startingDirection, directions))
                {
                    obstructionPositions.Add((r, c));
                }

                // Remove the obstruction
                grid[r, c] = '.';
            }
        }
    }

    return (uniquePositions.Count, obstructionPositions);
}

static bool DoesObstructionCauseLoop(char[,] grid, int startRow, int startCol, char startingDirection, Dictionary<char, (int, int)> directions)
{
    int rowCount = grid.GetLength(0);
    int colCount = grid.GetLength(1);
    var visitedStates = new HashSet<(int row, int col, char direction)>();
    int row = startRow;
    int col = startCol;
    char currentDirection = startingDirection;

    while (true)
    {
        if (visitedStates.Contains((row, col, currentDirection)))
        {
            // Loop detected
            return true;
        }
        visitedStates.Add((row, col, currentDirection));

        // Calculate next position
        int nextRow = row + directions[currentDirection].Item1;
        int nextCol = col + directions[currentDirection].Item2;

        if (!IsInBounds(nextRow, nextCol, rowCount, colCount))
        {
            // Guard leaves the mapped area
            break;
        }

        char nextCell = grid[nextRow, nextCol];
        if (nextCell == '.' || nextCell == '^' || nextCell == '>' || nextCell == 'v' || nextCell == '<')
        {
            // Move forward
            row = nextRow;
            col = nextCol;
        }
        else if (nextCell == '#')
        {
            // Rotate right
            currentDirection = RotateRight(currentDirection);
        }
        else
        {
            // Unexpected character
            break;
        }
    }

    return false; // No loop detected
}

static char RotateRight(char direction)
{
    return direction switch
    {
        '^' => '>',
        '>' => 'v',
        'v' => '<',
        '<' => '^',
        _ => direction
    };
}

static bool IsInBounds(int row, int col, int rowCount, int colCount)
{
    return row >= 0 && row < rowCount && col >= 0 && col < colCount;
}