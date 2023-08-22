using System;
using System.Text;

class ConsoleInputWithNavigation
{
    private StringBuilder inputBuffer = new StringBuilder();
    private int cursorPosition = 0;

    public static string[] text;
    public static string predicted = "";
    public string GetInput()
    {
        ConsoleKeyInfo keyInfo;
        do
        {
            keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.Backspace:
                    if (cursorPosition > 0)
                    {
                        inputBuffer.Remove(cursorPosition - 1, 1);
                        cursorPosition--;
                        RefreshConsole();
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (cursorPosition > 0)
                    {
                        cursorPosition--;
                        RefreshConsole();
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (cursorPosition < inputBuffer.Length)
                    {
                        cursorPosition++;
                        RefreshConsole();
                    }
                    break;
                case ConsoleKey.Spacebar:
                    text = inputBuffer.ToString().Split(" ");
                    predicted = wordPrediction.PredictNextWord(text[text.Length - 1]);
                    inputBuffer.Insert(cursorPosition, keyInfo.KeyChar);
                    cursorPosition++;
                    RefreshConsole();
                    break;
                case ConsoleKey.Tab:
                    text = inputBuffer.ToString().Split(" ");
                    string lastWordOfText = text[text.Length - 1];
                    string addText = predicted.Substring(lastWordOfText.Length);
                    foreach (char c in addText)
                    {
                        inputBuffer.Insert(cursorPosition, c);
                        cursorPosition++;
                    }
                    break;
                default:
                    if (!char.IsControl(keyInfo.KeyChar))
                    {
                        inputBuffer.Insert(cursorPosition, keyInfo.KeyChar);
                        cursorPosition++;
                        RefreshConsole();
                    }
                    break;
            }
        } while (keyInfo.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return inputBuffer.ToString();
    }

    WordPrediction wordPrediction = new WordPrediction();
    
    // Refreshes the console display with input buffer and predicted text
    private void RefreshConsole()
    {
        text = inputBuffer.ToString().Split(" ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(cursorPosition - text[text.Length - 1].Length , Console.CursorTop);
        Console.Write(predicted + " ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(inputBuffer);
    }
}
