using System.Text;

namespace TextEditor
{
    
    internal class TextEditor
    {
        
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetWindowSize(160,40);
            //Console.SetBufferSize(9999,9999);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WelcomeScreen();
            Console.Clear();
            Console.CursorVisible = true;
            Input();

        }

        static List<string> Input()
        {
            List<string> userInputs = new List<string>();

            string CurrentString = "";

            string Message = "";
            
            int BoxDrawPositionY = 0;

            int LineCounter = 1; // variable for the line number

            int CurrentLine = 0;

            string UserChosenFileName = "";

            Load(ref userInputs, ref CurrentString, ref LineCounter, ref CurrentLine, ref UserChosenFileName, ref  Message, ref  BoxDrawPositionY);
            
            Console.CursorVisible = true;

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
                
                UserMovement(ref userInputs, ref UserInput, ref CurrentString, ref CurrentLine, ref LineCounter);

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

                int RandomTime = SaveTime.Next(1000, 1300);

                Console.CursorVisible = false;
                Console.SetCursorPosition(150, 0);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("Saving...");
                Thread.Sleep(RandomTime);
                Console.SetCursorPosition(150, 0);
                Console.Write("         ");
                Console.SetCursorPosition(150, 0);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("Saved");
                Thread.Sleep(750);
                Console.SetCursorPosition(150, 0);
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

        static void Load(ref List<string> userInputs, ref string CurrentString, ref int LineCounter, ref int CurrentLine, ref string UserChosenFileName, ref string Message, ref int BoxDrawPositionY)
        {
            Message = " Would you like to load a file? (yes/no) ";
            UserLoadUI(ref Message, ref BoxDrawPositionY);
            
            UserInputUI(ref BoxDrawPositionY);
            string Answer = Console.ReadLine().Trim();
            

            if (Answer == "yes")
            {

                try
                {
                    Console.Clear();
                    
                    Message = " Whats the File Path? ";
                    UserLoadUI(ref Message, ref BoxDrawPositionY);
                    
                    UserInputUI(ref BoxDrawPositionY);
                    UserChosenFileName = Console.ReadLine().Trim();
                    userInputs = File.ReadAllLines(UserChosenFileName).ToList();
                    Console.Clear();

                    Message = " Loaded... " ;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    UserLoadUI(ref Message, ref BoxDrawPositionY);
                    
                    Thread.Sleep(750);
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
                    Thread.Sleep(1200);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Press Any Key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.CursorVisible = true;
                    Load(ref userInputs, ref CurrentString, ref LineCounter, ref CurrentLine, ref UserChosenFileName, ref Message, ref BoxDrawPositionY);
                }

                catch (IOException LoadException) //will potentially add an error readme with error details.
                {
                    Console.Clear();
                    Console.ForegroundColor= ConsoleColor.DarkRed;
                    Console.WriteLine("-- FILE LOAD ERROR -- ERROR CODE 0x01");
                    Console.WriteLine();
                    Console.WriteLine(LoadException.Message);
                    Console.CursorVisible = false;
                    Console.WriteLine("");
                    Thread.Sleep(1200);
                    Console.WriteLine("Press Any Key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.CursorVisible = true;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Clear();
                    
                    Load(ref userInputs, ref CurrentString, ref LineCounter, ref CurrentLine, ref UserChosenFileName, ref Message, ref BoxDrawPositionY);
                }

            }

            if (Answer != "yes" && Answer != "no")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Message = " Please choose from one of the options -- yes or no -- ";
                UserLoadUI(ref Message, ref BoxDrawPositionY);
                Thread.Sleep(1000);
                
                Console.ForegroundColor = ConsoleColor.DarkGray;

                Message = " Press Any Key to continue... ";
                BoxDrawPositionY = 5;
                UserLoadUI(ref Message, ref BoxDrawPositionY);
                
                Console.ReadKey();
                Console.Clear();
                Load(ref userInputs, ref CurrentString, ref LineCounter, ref CurrentLine, ref UserChosenFileName, ref Message, ref BoxDrawPositionY);
            }

            if (Answer == "no")
            {
                string UserChoice = "";
                Console.Clear();

                Message = " Would you like to create a new file? (yes or no) ";
                UserLoadUI(ref Message, ref BoxDrawPositionY);
                UserInputUI(ref BoxDrawPositionY);
                UserChoice = Console.ReadLine().Trim();
                Console.Clear();

                if (UserChoice == "yes")
                {
                    Message = " Please enter the desired file name... ";
                    UserLoadUI(ref Message, ref BoxDrawPositionY);
                    UserInputUI(ref BoxDrawPositionY);
                    UserChosenFileName = Console.ReadLine().Trim();
                    Console.Clear();

                    Message = " Created... ";
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    UserLoadUI(ref Message, ref BoxDrawPositionY);
                    
                    Thread.Sleep(400);
                    Console.Clear();
                    Console.Write("    1  ❚ "); // displays the current line number
                }

                else if (Answer != "yes")
                {
                    Console.CursorVisible = false;
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    Message = " Please choose from at least one of the options -- Load or Create -- ";
                    UserLoadUI(ref Message, ref BoxDrawPositionY);
                    
                    Thread.Sleep(1000);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    
                    Message = " Press Any Key to continue... ";
                    BoxDrawPositionY = 5;
                    UserLoadUI(ref Message, ref BoxDrawPositionY);
                    
                    Console.ReadKey();
                    Console.Clear();
                    Console.CursorVisible = true;
                    Console.Clear();
                    Load(ref userInputs, ref CurrentString, ref LineCounter, ref CurrentLine, ref UserChosenFileName, ref Message, ref BoxDrawPositionY);
                }
            }
        }
        
        static void UserLoadUI(ref string Message, ref int BoxDrawPositionY) //nearly finished
        {
            
            //string Message = Message; // spaces to account for the foreach loop
            
            Console.CursorVisible = false;
            
            Console.SetCursorPosition((160-Message.Length)/2,BoxDrawPositionY+8);
            Console.Write("┏");
            
            foreach (char character in Message)
            {
                Console.Write("━");
            }
            Console.Write("┓");
            
            Console.SetCursorPosition((160-Message.Length)/2,BoxDrawPositionY+9);
            Console.Write($"┃{Message}┃");
            
            Console.SetCursorPosition((160-Message.Length)/2, BoxDrawPositionY+10);
            Console.Write("┗");
            
            BoxDrawPositionY = 0;
            
            foreach (char character in Message)
            {
                Console.Write("━");
            }
            Console.Write("┛");
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            
            
        }

        static void UserInputUI(ref int BoxDrawPositionY)
        {
            Console.CursorVisible = false;
            
            Console.SetCursorPosition((160-20)/2,BoxDrawPositionY+13);
            Console.Write(" ┏━━━━━━━━━━━━━━━━━━━┓ ");
            
            Console.SetCursorPosition((160-20)/2,BoxDrawPositionY+14);
            Console.Write(" ┃                   ┃ ");
            
            Console.SetCursorPosition((160-20)/2, BoxDrawPositionY+15);
            Console.Write(" ┗━━━━━━━━━━━━━━━━━━━┛");
            
            Console.SetCursorPosition(((160-20)/2)+2, BoxDrawPositionY+14);
            Console.CursorVisible = true;
            
            BoxDrawPositionY = 0;
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
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
            Console.SetCursorPosition(65,13);
            Console.Write("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.SetCursorPosition(65,14);
            Console.Write("┃ Welcome to the Text Editor ┃");
            Console.SetCursorPosition(65, 15);
            Console.Write("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            Thread.Sleep(2000);
        }

        static void UserMovement(ref List<string> userInputs, ref ConsoleKeyInfo UserInput, ref string CurrentString, ref int CurrentLine, ref int LineCounter) //in progress
        {
            //int CurrentCharacterPosition = userInputs[CurrentString.Length].Length;
            
            if (Console.CursorLeft != 9)
            {
                if (UserInput.Key == ConsoleKey.LeftArrow)
                {
                    //CurrentCharacterPosition--;
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
            }

            if (UserInput.Key == ConsoleKey.RightArrow)
            {
                Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
            }

        }

        static void DisplayPosition() //in progress
        {
            
        }
        
    }
}