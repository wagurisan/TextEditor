using System.Text;

namespace TextEditor
{
    internal class TextEditor
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            //Console.SetWindowSize(200,200);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WelcomeScreen();
            Console.Clear();
            Console.CursorVisible = true;
            Input();//hello

        }

        static List<string> Input()
        {
            List<string> userInputs = new List<string>();

            string CurrentString = "";

            int LineCounter = 1; // variable for the line number

            int CurrentLine = 0;

            string UserChosenFileName = "";

            Load(ref userInputs, ref CurrentString, ref LineCounter, ref CurrentLine, ref UserChosenFileName);

            while (true)
            {
                ConsoleKeyInfo UserInput = Console.ReadKey();
                if (UserInput.Key != ConsoleKey.Enter && UserInput.Key != ConsoleKey.Backspace)
                    CurrentString += UserInput.KeyChar;

                if (UserInput.Key == ConsoleKey.Enter)
                {
                    userInputs.Add(CurrentString.Trim()); // make sure that spaces are not included in the string.
                    LineCounter++; // increment line counter
                    CurrentLine++; // increment currentline counter
                    Console.Write("\n");
                    Console.Write($" {string.Concat(Enumerable.Repeat(" ", 4 - LineCounter.ToString().Length))}{LineCounter}  ❚ "); // write the line number when enter is pressed
                    CurrentString = "";
                }

                BackSpace(ref userInputs, ref UserInput, ref CurrentString, ref CurrentLine, ref LineCounter);

                Save(ref userInputs, ref UserInput, ref CurrentString, ref UserChosenFileName);

            }
        }

        static void Save(ref List<string> userInputs, ref ConsoleKeyInfo UserInput, ref string CurrentString, ref string UserChosenFileName)
        {

            if (UserInput.Key == ConsoleKey.Escape)
            {

                int PreSaveX = Console.CursorLeft;

                int PreSaveY = Console.CursorTop;

                Random SaveTime = new Random();

                int RandomTime = SaveTime.Next(1000, 1500);

                Console.CursorVisible = false;
                Console.SetCursorPosition(100, 0);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("Saving...");
                Thread.Sleep(RandomTime);
                Console.SetCursorPosition(100, 0);
                Console.Write("         ");
                Console.SetCursorPosition(100, 0);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("Saved");
                Thread.Sleep(1000);
                Console.SetCursorPosition(100, 0);
                Console.Write("         ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.SetCursorPosition(PreSaveX, PreSaveY);
                Console.CursorVisible = true;

                try
                {
                    System.IO.File.WriteAllLines(UserChosenFileName, userInputs);
                }

                catch (ArgumentException SaveException)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("-- FILE SAVE ERROR -- ERROR CODE 0x01");
                    Console.WriteLine();
                    Console.WriteLine(SaveException.Message);
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                }

                catch (IOException FileNamingException)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("-- ILLEGAL FILE NAME ERROR -- ERROR CODE 0x03");
                    Console.WriteLine();
                    Console.WriteLine(FileNamingException.Message);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
            }
        }

        static void Load(ref List<string> userInputs, ref string CurrentString, ref int LineCounter,
            ref int CurrentLine, ref string UserChosenFileName)
        {
            Console.WriteLine("Would you like to load a file? (filename.txt) (yes or no)");
            string Answer = Console.ReadLine().Trim();

            if (Answer == "yes")
            {

                try
                {
                    Console.Clear();
                    Console.WriteLine("Whats the File Path?");
                    UserChosenFileName = Console.ReadLine().Trim();
                    userInputs = File.ReadAllLines(UserChosenFileName).ToList();
                    Console.Clear();
                    Console.WriteLine("Loaded...");
                    Thread.Sleep(1000);
                    Console.Clear();

                    foreach (string StringLine in userInputs)
                    {

                        Console.WriteLine(
                            $" {string.Concat(Enumerable.Repeat(" ", 4 - LineCounter.ToString().Length))}{LineCounter}  ❚ {StringLine}"); // write the line number when enter is pressed
                        LineCounter++;

                        Thread.Sleep(10);
                        CurrentLine++;

                    }

                    Console.SetCursorPosition(userInputs[CurrentLine-1].Length + 9, LineCounter - 2);

                }

                catch (ArgumentException FileNamingException)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("-- FILE NAMING ERROR -- ERROR CODE 0x04");
                    Console.WriteLine();
                    Console.WriteLine(FileNamingException.Message);
                    Console.CursorVisible = false;
                    Thread.Sleep(2000);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Press Any Key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.CursorVisible = true;
                    Load(ref userInputs, ref CurrentString, ref LineCounter, ref CurrentLine, ref UserChosenFileName);
                }

                catch (IOException LoadException) //will potentially add an error readme with error details.
                {
                    Console.Clear();
                    Console.ForegroundColor= ConsoleColor.DarkRed;
                    Console.WriteLine("-- FILE LOAD ERROR -- ERROR CODE 0x01");
                    Console.WriteLine();
                    Console.WriteLine(LoadException.Message);
                    Console.CursorVisible = false;
                    Thread.Sleep(2000);
                    Console.WriteLine("Press Any Key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.CursorVisible = true;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Clear();
                    
                    Load(ref userInputs, ref CurrentString, ref LineCounter, ref CurrentLine, ref UserChosenFileName);
                }

            }

            if (Answer != "yes" && Answer != "no")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Please choose from one of the options -- yes or no --");
                Thread.Sleep(2000);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Press Any Key to continue...");
                Console.ReadKey();
                Console.Clear();
                Load(ref userInputs, ref CurrentString, ref LineCounter, ref CurrentLine, ref UserChosenFileName);
            }

            if (Answer == "no")
            {
                string UserChoice = "";
                Console.Clear();
                Console.WriteLine("Would you like to create a new file? (yes or no)");
                UserChoice = Console.ReadLine().Trim();
                Console.Clear();

                if (UserChoice == "yes")
                {
                    Console.WriteLine("Please enter the desired file name...");
                    UserChosenFileName = Console.ReadLine().Trim();
                    Console.Clear();
                    Console.WriteLine("Created...");
                    Thread.Sleep(600);
                    Console.Clear();
                    Console.Write("    1  ❚ "); // displays the current line number
                }

                else if (Answer != "yes")
                {
                    Console.CursorVisible = false;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Please choose from at least one of the options -- Load or Create --");
                    Thread.Sleep(2000);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Press Any Key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.CursorVisible = true;
                    Console.Clear();
                    Load(ref userInputs, ref CurrentString, ref LineCounter, ref CurrentLine, ref UserChosenFileName);
                }
            }
        }

        static void BackSpace(ref List<string> userInputs, ref ConsoleKeyInfo UserInput, ref string CurrentString, ref int CurrentLine, ref int LineCounter)
        {
            int CurrentStringLength = CurrentString.Length;

            if (CurrentStringLength == 0 && CurrentLine != 0)
            {
                if (UserInput.Key == ConsoleKey.Backspace)
                {
                    CurrentLine--;
                    LineCounter--;
                    CurrentStringLength = userInputs[CurrentLine].Length;
                    Console.SetCursorPosition(CurrentStringLength + 9, Console.CursorTop - 1);
                    CurrentString = userInputs[CurrentLine];
                    userInputs[CurrentLine] = CurrentString;
                    Console.SetCursorPosition(0, CurrentLine + 1);
                    Console.Write("        ");
                    Console.SetCursorPosition(CurrentStringLength + 9, CurrentLine);
                    userInputs.RemoveAt(CurrentLine);
                }

            }

            else
            {
                if (UserInput.Key == ConsoleKey.Backspace)

                {
                    if (Console.CursorLeft < 9)
                    {
                        Console.Write(" ");
                    }

                    if (CurrentString.Length > 0) // checking if the cursor is at the leftmost position
                    {
                        CurrentString = CurrentString[..(CurrentString.Length - 1)];
                        Console.Write(" \b");
                    }

                    if (CurrentString.Length > 0) // checking if the cursor is at the leftmost position
                    {
                        Console.Write(" \b");
                    }
                }
            }

        }

        static void WelcomeScreen()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(43, 10);
            Console.Write("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.SetCursorPosition(43, 11);
            Console.Write("┃ Welcome to the Text Editor ┃");
            Console.SetCursorPosition(43, 12);
            Console.Write("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Thread.Sleep(2200);
        }
    }
}