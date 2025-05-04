using System;
using System.Collections.Generic;
using System.Linq;

// Класс Квадрат
public class Square
{
    public ConsoleColor Color { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsVisible { get; private set; }

    public Square(int x, int y, ConsoleColor color)
    {
        X = x;
        Y = y;
        Color = color;
        IsVisible = true;
    }

    public void Display()
    {
        if (!IsVisible) return;

        Console.ForegroundColor = Color;
        Console.SetCursorPosition(X * 2, Y); // Умножаем на 2 чтобы было лучше видно
        Console.Write("■");
        Console.ResetColor();
    }

    public void Hide()
    {
        if (!IsVisible) return;

        Console.SetCursorPosition(X * 2, Y);
        Console.Write("  ");
        IsVisible = false;
    }

    public void Show()
    {
        IsVisible = true;
        Display();
    }
}

// Класс Фигура
public class Figure
{
    public string Type { get; }
    public List<Square> Squares { get; }
    public Square CenterSquare { get; }
    private GameField _field;

    public Figure(string type, List<Square> squares, Square centerSquare, GameField field)
    {
        Type = type;
        Squares = squares;
        CenterSquare = centerSquare;
        _field = field;
    }

    public bool Rotate()
    {
        // Проверяем возможность поворота
        foreach (var square in Squares)
        {
            if (square == CenterSquare) continue;

            int dx = square.X - CenterSquare.X;
            int dy = square.Y - CenterSquare.Y;

            // Поворот на 90 градусов по часовой стрелке: (x, y) → (y, -x)
            int newX = CenterSquare.X + dy;
            int newY = CenterSquare.Y - dx;

            // Проверяем, не выходит ли за границы или не пересекается с другими квадратами
            if (newX < 0 || newX >= _field.Width || newY < 0 || newY >= _field.Height ||
                _field.IsOccupied(newX, newY))
            {
                return false;
            }
        }

        // Выполняем поворот
        foreach (var square in Squares)
        {
            if (square == CenterSquare) continue;

            int dx = square.X - CenterSquare.X;
            int dy = square.Y - CenterSquare.Y;

            // Поворот на 90 градусов по часовой стрелке
            square.X = CenterSquare.X + dy;
            square.Y = CenterSquare.Y - dx;
        }

        return true;
    }

    public bool MoveLeft()
    {
        // Проверяем возможность движения влево
        foreach (var square in Squares)
        {
            if (square.X - 1 < 0 || _field.IsOccupied(square.X - 1, square.Y))
            {
                return false;
            }
        }

        // Двигаем влево
        foreach (var square in Squares)
        {
            square.X--;
        }

        return true;
    }

    public bool MoveRight()
    {
        // Проверяем возможность движения вправо
        foreach (var square in Squares)
        {
            if (square.X + 1 >= _field.Width || _field.IsOccupied(square.X + 1, square.Y))
            {
                return false;
            }
        }

        // Двигаем на право
        foreach (var square in Squares)
        {
            square.X++;
        }

        return true;
    }

    public bool MoveDown()
    {
        // Проверяем возможность движения вниз
        foreach (var square in Squares)
        {
            if (square.Y + 1 >= _field.Height || _field.IsOccupied(square.X, square.Y + 1))
            {
                return false;
            }
        }

        // Двигаем вниз
        foreach (var square in Squares)
        {
            square.Y++;
        }

        return true;
    }
}

// Класс Игровое поле
public class GameField
{
    public int Width { get; }
    public int Height { get; }
    public Figure CurrentFigure { get; private set; }
    public List<Square> FallenSquares { get; } = new List<Square>();

    public GameField(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public bool IsOccupied(int x, int y)
    {
        return FallenSquares.Any(s => s.X == x && s.Y == y);
    }

    public void newFugure()
    {
        Random random = new Random();
        string[] figureTypes = { "I", "J", "L", "O", "S", "T", "Z" }; //тут создаётся случайная фигура
        string type = figureTypes[random.Next(figureTypes.Length)];

        List<Square> squares = new List<Square>();
        Square centerSquare = null;
        ConsoleColor color = (ConsoleColor)random.Next(1, 16);

        // Начальная позиция в верхней части поля (по сентру)
        int startX = Width / 2;
        int startY = 0;

        switch (type)
        {
            case "I":
                squares.Add(new Square(startX, startY, color));
                squares.Add(new Square(startX, startY + 1, color));
                squares.Add(new Square(startX, startY + 2, color));
                squares.Add(new Square(startX, startY + 3, color));
                centerSquare = squares[1];
                break;
            case "J":
                squares.Add(new Square(startX, startY, color));
                squares.Add(new Square(startX, startY + 1, color));
                squares.Add(new Square(startX - 1, startY + 1, color));
                squares.Add(new Square(startX - 2, startY + 1, color));
                centerSquare = squares[1];
                break;
            case "L":
                squares.Add(new Square(startX, startY, color));
                squares.Add(new Square(startX, startY + 1, color));
                squares.Add(new Square(startX + 1, startY + 1, color));
                squares.Add(new Square(startX + 2, startY + 1, color));
                centerSquare = squares[1];
                break;
            case "O":
                squares.Add(new Square(startX, startY, color));
                squares.Add(new Square(startX + 1, startY, color));
                squares.Add(new Square(startX, startY + 1, color));
                squares.Add(new Square(startX + 1, startY + 1, color));
                centerSquare = squares[0];
                break;
            case "S":
                squares.Add(new Square(startX, startY + 1, color));
                squares.Add(new Square(startX + 1, startY + 1, color));
                squares.Add(new Square(startX - 1, startY, color));
                squares.Add(new Square(startX, startY, color));
                centerSquare = squares[1];
                break;
            case "T":
                squares.Add(new Square(startX, startY, color));
                squares.Add(new Square(startX - 1, startY + 1, color));
                squares.Add(new Square(startX, startY + 1, color));
                squares.Add(new Square(startX + 1, startY + 1, color));
                centerSquare = squares[2];
                break;
            case "Z":
                squares.Add(new Square(startX, startY, color));
                squares.Add(new Square(startX + 1, startY, color));
                squares.Add(new Square(startX, startY + 1, color));
                squares.Add(new Square(startX - 1, startY + 1, color));
                centerSquare = squares[0];
                break;
        }

        CurrentFigure = new Figure(type, squares, centerSquare, this);
    }

    public void checkerDown()
    {
        for (int y = Height - 1; y >= 0; y--)
        {
            var lineSquares = FallenSquares.Where(s => s.Y == y).ToList();
            if (lineSquares.Count == Width)
            {
                // Удаляем квадраты этой линии
                foreach (var square in lineSquares)
                {
                    square.Hide();
                    FallenSquares.Remove(square);
                }

                // Сдвигаем все квадраты выше вниз
                foreach (var square in FallenSquares.Where(s => s.Y < y))
                {
                    square.Y++;
                }
                // После удаления линии нужно проверить снова эту же позицию,
                // так как квадраты сверху сдвинулись вниз
                y++;
            }
        }
    }

    public void GameLoop()
    {
        Console.CursorVisible = false;
        newFugure();

        while (true)
        {
            // Отрисовка
            Console.Clear();
            DrawBorder();
            DrawFallenSquares();
            DrawCurrentFigure();

            // Обработка ввода
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        CurrentFigure.MoveLeft();
                        break;
                    case ConsoleKey.RightArrow:
                        CurrentFigure.MoveRight();
                        break;
                    case ConsoleKey.DownArrow:
                        if (!CurrentFigure.MoveDown())
                        {
                            // Фигура достигла дна или другой фигуры
                            FallenSquares.AddRange(CurrentFigure.Squares);
                            checkerDown();
                            newFugure();
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        CurrentFigure.Rotate();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }

            // Автоматическое движение вниз
            if (!CurrentFigure.MoveDown())
            {
                // Фигура достигла дна или другой фигуры
                FallenSquares.AddRange(CurrentFigure.Squares);
                checkerDown();
                newFugure();
            }

            // Проверка окончания игры
            if (FallenSquares.Any(s => s.Y == 0))
            {
                Console.Clear();
                Console.SetCursorPosition(Width, Height / 2);
                Console.WriteLine("Game Over!");
                break;
            }

            System.Threading.Thread.Sleep(300);
        }
    }

    private void DrawBorder()
    {
        Console.ForegroundColor = ConsoleColor.White;
        for (int y = 0; y < Height; y++)
        {
            Console.SetCursorPosition(0, y);
            Console.Write("|");
            Console.SetCursorPosition(Width * 2, y);
            Console.Write("|");
        }

        for (int x = 0; x <= Width * 2; x++)
        {
            Console.SetCursorPosition(x, Height);
            Console.Write("-");
        }
    }

    private void DrawFallenSquares()
    {
        foreach (var square in FallenSquares)
        {
            square.Display();
        }
    }

    private void DrawCurrentFigure()
    {
        foreach (var square in CurrentFigure.Squares)
        {
            square.Display();
        }
    }
}

// Главный класс программы
class Program
{
    static void Main(string[] args)
    {
        GameField field = new GameField(10, 20);
        field.GameLoop();

        Console.SetCursorPosition(0, field.Height + 1);
        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}