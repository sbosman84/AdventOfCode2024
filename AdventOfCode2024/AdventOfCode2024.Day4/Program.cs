using System.Diagnostics;
using System.Reflection;
using System.Text;

Console.WriteLine("Advent Of Code 2024");
Console.WriteLine("--- Day 4: Ceres Search ---");

var lines = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));
var stopwatch = new Stopwatch();
stopwatch.Start();

int rowCount = lines.Length;
int colCount = lines[0].Length;
char[,] grid = new char[rowCount, colCount];

for (int i = 0; i < rowCount; i++)
{
    for (int j = 0; j < colCount; j++)
    {
        grid[i, j] = lines[i][j];
    }
}

int countPart1 = 0;
int countPart2 = 0;
string word = "XMAS";

for (int i = 0; i < rowCount; i++)
{
    for (int j = 0; j < colCount; j++)
    {
        countPart1 += CountWord(grid, i, j, word);
        if (grid[i, j] == 'A')
        {            
            if(CountXMASPattern(grid, i, j))
            {
                countPart2++;
            }
        }
    }
}

Console.WriteLine($"Part 1 - Total occurrences of '{word}': {countPart1}");
Console.WriteLine($"Part 2 - Total occurrences of 'X-MAS': {countPart2}");

stopwatch.Stop();
Console.WriteLine($"Calculation performed in: {stopwatch.ElapsedMilliseconds} ms");
Console.ReadKey();

static int CountWord(char[,] grid, int row, int col, string word)
{
    int count = 0;
    int wordLength = word.Length;
    int rowCount = grid.GetLength(0);
    int colCount = grid.GetLength(1);

    // Directions: right, down, down-right, down-left, left, up, up-right, up-left
    int[][] directions = new int[][]
    {
        [0, 1], // right
        [1, 0], // down
        [1, 1], // down-right
        [1, -1], // down-left
        [0, -1], // left
        [-1, 0], // up
        [-1, 1], // up-right
        [-1, -1] // up-left
    };

    foreach (var direction in directions)
    {
        int newRow = row;
        int newCol = col;
        bool match = true;

        for (int k = 0; k < wordLength; k++)
        {
            if (newRow < 0 || newRow >= rowCount || newCol < 0 || newCol >= colCount || grid[newRow, newCol] != word[k])
            {
                match = false;
                break;
            }
            newRow += direction[0];
            newCol += direction[1];
        }

        if (match)
        {
            count++;
        }
    }

    return count;
}

static bool CountXMASPattern(char[,] grid, int row, int col)
{
    int count = 0;
    int rowCount = grid.GetLength(0);
    int colCount = grid.GetLength(1);

    // Check for the X-MAS pattern in all possible orientations
    if (!IsXMASPattern(grid, row, col, -1, -1, 1, 1)) count++; // Top-left and Down-Right
    if (!IsXMASPattern(grid, row, col, 1, 1, -1, -1)) count++; // Down-right and Top-Left
    if (!IsXMASPattern(grid, row, col, -1, 1, 1, -1)) count++; // Top-right and down-left
    if (!IsXMASPattern(grid, row, col, 1, -1, -1, 1)) count++; // Up-left and up-right

    // Check for the X-MAS pattern in two directions
    return count == 2;
}

static bool IsXMASPattern(char[,] grid, int row, int col, int dir1Row, int dir1Col, int dir2Row, int dir2Col)
{
    int rowCount = grid.GetLength(0);
    int colCount = grid.GetLength(1);

    // Check the M
    if (!IsInBounds(row + dir1Row, col + dir1Col, rowCount, colCount) || grid[row + dir1Row, col + dir1Col] != 'M') return false;

    // Check the S
    if (!IsInBounds(row + dir2Row, col + dir2Col, rowCount, colCount) || grid[row + dir2Row, col + dir2Col] != 'S') return false;

    return true;
}

static bool IsInBounds(int row, int col, int rowCount, int colCount)
{
    return row >= 0 && row < rowCount && col >= 0 && col < colCount;
}