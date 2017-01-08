using System;

class UIConsoleManager
{
    private static ConsoleColor originalColor;

    public static void WriteLine(ConsoleColor color, string message, params object[] p)
    {
        originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message, p);
        Console.ForegroundColor = originalColor;
    }

    public static void WriteLineInRed(string message, params object[] p)
    {
        WriteLine(ConsoleColor.Red, message, p);
    }

    public static void WriteLineInDarkRed(string message, params object[] p)
    {
        WriteLine(ConsoleColor.DarkRed, message, p);
    }

    public static void WriteLineInGreen(string message, params object[] p)
    {
        WriteLine(ConsoleColor.Green, message, p);
    }

    public static void WriteLineInGray(string message, params object[] p)
    {
        WriteLine(ConsoleColor.Gray, message, p);
    }

    public static void WriteLineInYellow(string message, params object[] p)
    {
        WriteLine(ConsoleColor.Yellow, message, p);
    }

    public static void WriteLineInCyan(string message, params object[] p)
    {
        WriteLine(ConsoleColor.Cyan, message, p);
    }

    public static void WriteLineInMagenta(string message, params object[] p)
    {
        WriteLine(ConsoleColor.Magenta, message, p);
    }
}