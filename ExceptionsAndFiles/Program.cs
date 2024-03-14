namespace ExceptionsAndFiles;

class Program
{
    class CustomException : Exception
    {
        public CustomException(string message) : base(message) { }
    }
    static void Main()
    {
        //EXCEPTIONS:
        Console.WriteLine("Exceptions:");
        //try catch finally example
        try
        {
            //try to access an index that doesn't exist
            int[] numbers = { 1, 2, 3 };
            Console.WriteLine(numbers[10]);
        }
        catch (IndexOutOfRangeException ex)
        {
            //catch the exception and output a message so we dont crash
            Console.WriteLine("Index out of range");
            Console.WriteLine(ex);
        }
        //finally block will always execute
        //effectively the same as just writing code after the try-catch block, but good for strcture and readability
        finally
        {
            Console.WriteLine("try-catch finished");
            Console.WriteLine();
        }

        int x = int.MaxValue; //0111 1111 1111 1111 1111 1111 1111 1111 = 2147483647
        try
        {
            //checked keyword will throw an exception if an overflow occurs
            //otherwise it will wrap around to the lowest value and continue as if nothing went wrong
            checked
            {
                x++; //1000 0000 0000 0000 0000 0000 0000 0000 = -2147483648
                Console.WriteLine(x);
            }
            //unchecked would allow the overflow to occur and wrap around
        }
        catch (OverflowException ex)
        {
            Console.WriteLine("Overflow");
            Console.WriteLine(ex);
        }

        //custom exception example
        try
        {
            throw new CustomException("That won't work");
        }
        catch (CustomException ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.WriteLine();



        //FILES:
        Console.WriteLine("File Handling:");
        string fileName = "test.txt";
        string content = """
            This is a test file.
            It's not very interesting.
            But it does have multiple lines!
            I made this in C#.
            """;

        //create a file with the given name and content
        CreateFile(fileName, content);
        Console.WriteLine();

        //try to overwrite the file, and fail
        CreateFile(fileName, "This is a new file");
        Console.WriteLine();

        //read the file and output the content
        ReadFile(fileName);
        Console.WriteLine();

        //copying file
        string destinationFile = "default.txt";
        CopyFile(fileName, destinationFile);
        Console.WriteLine();

        //read the destination file and output the content
        ReadFile(destinationFile);
        Console.WriteLine();

        //and delete it
        DeleteFile(destinationFile);
        Console.WriteLine();

        //verify that it was deleted
        ReadFile(destinationFile);
        Console.WriteLine();

        //move file
        //we can't just drop a file in the root of C:\ in Windows without modifying permissions, so need to first make a folder for it to go into
        MakeDirectory(@"C:\temp\");
        MoveFile(fileName, @"C:\temp\moved.txt");
        Console.WriteLine();

        /*check the file was moved and not copied
        note: the first ReadFile call should always succeed, and on a clean run the second should fail.
        on a subsequent run, both should succeed since a new file was created and couldn't be moved because the destination already existed*/
        ReadFile(@"C:\temp\moved.txt");
        Console.WriteLine();

        ReadFile(fileName);
        Console.WriteLine();

        //delete files to do a clean run next time - comment this out to see the files
        /*DeleteFile(fileName);
        DeleteFile(destinationFile);
        DeleteFile(@"C:\temp\moved.txt");*/
    }

    private static void CreateFile(string path, string content)
    {
        /*create file for demo purposes
        if a file with the name already exists, it will not be overwritten
        we avoid the need for an exception by checking if the file exists first*/
        if(!File.Exists(path))
        {
            //output a message and create the file
            Console.WriteLine("File does not exist. Creating file...");
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(content);
            }
            Console.WriteLine("File created");
        }
        else
        {
            Console.WriteLine("Cannot write! File already exists");
        }
    }

    private static void ReadFile(string fileName)
    {
        //if the file does not exist, we cannot read it
        if (!File.Exists(fileName))
        {
            Console.WriteLine("Cannot read! File does not exist");
            return;
        }
        else
        {
            //read the file and output the content, line by line
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s;
                Console.WriteLine("Reading file...");
                //pragma to suppress warning about possible null value
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                Console.WriteLine("Finished reading");
            }
        }
    }


    private static void DeleteFile(string fileName)
    {
        //if the file does not exist, we cannot delete it
        if (!File.Exists(fileName))
        {
            Console.WriteLine("Cannot delete! File does not exist");
        }
        //otherwise, delete the file - should NOT do it this way in a real program
        //check with the user that the file should be deleted
        else
        {
            Console.WriteLine("File exists. Deleting file...");
            File.Delete(fileName);
            Console.WriteLine("File deleted");
        }
    }

    private static void CopyFile(string fileName, string destination)
    {
        //if the file does not exist, we cannot copy it
        if (!File.Exists(fileName))
        {
            Console.WriteLine("Cannot copy! File does not exist");
            return;
        }
        //or if the destination file already exists, we cannot copy it
        else
        {
            if (File.Exists(destination))
            {
                Console.WriteLine("Cannot copy! Destination file already exists");
                return;
            }
            //if we can copy the file, do so
            else
            {
                Console.WriteLine("Copying file...");
                File.Copy(fileName, destination);
                Console.WriteLine("File copied");
            }
        }
    }

    private static void MoveFile(string fileName, string destination)
    {
        //if the file does not exist, we cannot move it
        if (!File.Exists(fileName))
        {
            Console.WriteLine("Cannot move! File does not exist");
            return;
        }
        //or if the destination file already exists, we cannot move it
        else
        {
            if (File.Exists(destination))
            {
                Console.WriteLine("Cannot move! Destination file already exists");
                return;
            }
            //if we can move the file, do so
            else
            {
                Console.WriteLine("Moving file...");
                File.Move(fileName, destination);
                Console.WriteLine("File moved");
            }
        }
    }

    private static void MakeDirectory(string path)
    {
        //check if the directory exists
        if (!Directory.Exists(path))
        {
            //if it does not, create it
            Directory.CreateDirectory(path);
            Console.WriteLine("Directory created");
        }
        //if it does, output a message
        else
        {
            Console.WriteLine("Directory already exists");
        }
    }
}