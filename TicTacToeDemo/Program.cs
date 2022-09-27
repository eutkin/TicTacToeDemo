// See https://aka.ms/new-console-template for more information

/*
 
   1   2   3 
1  o | x | o
  ---+---+---
2  o | x | o 
  ---+---+---
3  x | o | x
  
  
  X = 2, O = 1
 
 */

using System.Text.RegularExpressions;


var moveCount = 0;

var win = 0; // Принимает значения: 0 - продолжаем, 1 – кто-то выиграл, 2 – ничья

const int fieldSize = 3;

var field = new[]
{
    new int[fieldSize],
    new int[fieldSize],
    new int[fieldSize]
};

while (win == 0)
{
    PrintField(field, fieldSize);
    var (coordX, coordY) = ReadMove(field);

    moveCount++;
    bool firstPlayer = moveCount % 2 == 1;
    field[coordX][coordY] = firstPlayer ? 2 : 1;
    win = CheckWin(fieldSize, moveCount, field);
}


switch (win)
{
    case 1:
        Console.WriteLine(moveCount % 2 == 1 ? "Выиграл первый игрок" : "Выиграл второй игрок");
        break;
    default:
        Console.WriteLine("Ничья");
        break;
}


/**
 * 0 - игра продолжается
 * 1 - выигрыш
 * 2 - ничья
 */
static int CheckWin(int fieldSize, int moveCount, int[][] field)
{
    if (moveCount < fieldSize * 2 - 1)
    {
        return 0;
    }

    var win = CheckVertical(fieldSize, field);
    if (win != 0)
    {
        return win;
    }

    win = CheckHorizontal(fieldSize, field);
    if (win != 0)
    {
        return win;
    }

    win = CheckLeftDiagonal(fieldSize, field);
    if (win != 0)
    {
        return win;
    }

    win = CheckLeftDiagonal(fieldSize, field);
    if (win != 0)
    {
        return win;
    }

    win = CheckRightDiagonal(fieldSize, field);

    if (win != 0)
    {
        return win;
    }

    return CheckDraw(fieldSize, field);
}

static int CheckVertical(int fieldSize, int[][] field)
{
    for (var x = 0; x < fieldSize; x++)
    {
        var y = 1;
        while (field[x][y - 1] == field[x][y])
        {
            if (y == fieldSize - 1)
            {
                return 1;
            }

            y++;
        }
    }

    return 0;
}

static int CheckHorizontal(int fieldSize, int[][] field)
{
    for (var y = 0; y < fieldSize; y++)
    {
        var x = 1;
        while (field[x - 1][y] == field[x][y])
        {
            if (x == fieldSize - 1)
            {
                return 1;
            }

            x++;
        }
    }

    return 0;
}

static int CheckLeftDiagonal(int fieldSize, int[][] field)
{
    var result = false;
    for (var x = 1; x < fieldSize; x++)
    {
        result = field[x - 1][x - 1] == field[x][x];
    }

    return result ? 1 : 0;
}

static int CheckRightDiagonal(int fieldSize, int[][] field)
{
    var result = false;
    for (var x = 1; x < fieldSize; x++)
    {
        result = field[x - 1][fieldSize - x] == field[x][fieldSize - x - 1];
    }

    return result ? 1 : 0;
}

static int CheckDraw(int fieldSize, int[][] field)
{
    for (var x = 0; x < fieldSize; x++)
    {
        for (var y = 0; y < fieldSize; y++)
        {
            if (field[x][y] == 0)
            {
                return 0;
            }
        }
    }

    return 2;
}

static (int coordX, int coordY) ReadMove(int[][] field)
{
    while (true)
    {
        Console.WriteLine("Введите координаты по горизонтали и вертикали (от 1 до 3) через пробел:");

        var rawPlayerMove = Console.ReadLine();

        if (rawPlayerMove != null)
        {
            var matches = Regex.Match(rawPlayerMove, "(\\d) (\\d)");
            if (matches.Success)
            {
                var coordX = int.Parse(matches.Groups[2].Value) - 1;
                var coordY = int.Parse(matches.Groups[1].Value) - 1;

                if ((coordX is >= 0 and < fieldSize) && (coordY is >= 0 and < fieldSize))
                {
                    if (field[coordX][coordY] != 0)
                    {
                        Console.WriteLine("Клетка занята");
                    }
                    else
                    {
                        return (coordX, coordY);
                    }
                }
            }
        }

        Console.WriteLine("Попробуйте еще");
    }
}

static void PrintField(int[][] field, int fieldSize)
{
    Console.WriteLine("   1   2   3 ");
    for (var y = 0; y < fieldSize; y++)
    {
        Console.Write((y + 1) + "  ");
        for (var x = 0; x < fieldSize; x++)
        {
            var symbol = field[x][y] switch
            {
                2 => "X",
                1 => "O",
                _ => " "
            };
            Console.Write(symbol);
            if (x + 1 != fieldSize)
            {
                Console.Write(" | ");
            }
        }

        Console.WriteLine();
        if (y + 1 != fieldSize)
        {
            Console.WriteLine("  ---+---+---");
        }
    }

    // Console.WriteLine("1    |   |   ");
    // Console.WriteLine("  ---+---+---");
    // Console.WriteLine("2    |   |   ");
    // Console.WriteLine("  ---+---+---");
    // Console.WriteLine("3    |   |   ");
}