using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region Excercise2 Detail
//There is a text file with 1M lines of 5 characters long alphanumerical strings.
//Write a console app which asks the user for an input of a 5 characters long alphanumerical string, searches it in the file(or any preloaded in-memory data structure). 
//The equality ignores characters order but takes characters appearances count into consideration.
//
//For example:
//ABCDD
//and
//DCDAB
//should be considered equal, but
//ABCDD
//and
//ABCCD
//are not because in the first string D appears twice and in the second string C appears twice.


//The app should display the result of the search on the screen (the input and the match).
//The search algorithm should run as fast as possible(initial load time can as be as long as needed).

#endregion
namespace Excercise2
{
    class Program
    {
        static List<string> list5;
        const string filename = "list1M.txt"; // source file
        static bool noCase = false; // if true, then A=a , so convert all to uppercase

        static int Main(string[] args)
        {
            // check if needed to normalize to no case difference (-nocase)
            foreach (string arg in args)
            {
                if (arg.ToLowerInvariant() == "-nocase")    // found nocase directive
                {
                    noCase = true;
                    Console.WriteLine("Searching without CASE difference");
                }
            }

            // allocate list
            list5 = new List<string>(1000000);
            // upload file
            if (list5 != null)  // make sure list was allocated
            {
                if (UploadFile(filename)) // file was uploaded correctly
                {
                    // sort the list
                    list5 = list5.OrderBy(x => x).ToList();
                    
                    bool stop = false;//controls the loop
                    
                    while (!stop)
                    {
                        Console.WriteLine();
                        Console.Write("Enter a 5 character string, (to exit press enter): ");
                        // get the user input
                        string queryString = Console.ReadLine();
                        // check if empty
                        if (string.IsNullOrWhiteSpace(queryString))
                            stop = true; 
                        // check if alphaNumerical
                        else if (!queryString.All(char.IsLetterOrDigit))  // could have done it with Regex                            
                            Console.WriteLine("Only Alpha numeric digits allowed!");
                        else
                        {
                            // search for it; BinarySearch is the fastest way on an ordered list

                            string searchFor = new string(queryString.OrderBy(c => c).ToArray()); // normalize like stored string

                            // if do not differentiate between case (A = a); 
                            if (noCase)
                                searchFor = searchFor.ToUpperInvariant(); // normalize to Upper

                            int found = list5.BinarySearch(searchFor); // do the search
                            if (found >= 0) // if found
                                Console.WriteLine("String {0} found at position {1} as {2}", queryString, found, searchFor);
                            else Console.WriteLine("String {0} NOT FOUND!", queryString);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Exiting due to failure in UploadFile ({0})", filename);
                    return (-1);
                }
            }

            return (0);

        }
        /// <summary>
        /// Iploads a file; normalizes to character order and to upper case if requested
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>true if succeeded, otherwise false</returns>
        private static bool UploadFile(string filename)
        {
            bool bRet = false;
            try
            {
                if (!File.Exists(filename)) // check if file exists
                {
                    Console.WriteLine("File {0} is not accesible", filename);
                    return bRet = false; ;
                }
                else
                {
                    // open text file as a stream
                    using (StreamReader sr = File.OpenText(filename)) 
                    {
                        string lineInFile = null; // get each line; checks if EOF
                        while ((lineInFile = sr.ReadLine()) != null)
                        {
                            // normalize
                            string normalizedString = null;
                            if (!string.IsNullOrWhiteSpace(lineInFile)) // if not EOF
                                normalizedString = new string (lineInFile.OrderBy(c => c).ToArray()); // order characters

                            // if do not differentiate between case (A = a)
                            if (noCase)
                                normalizedString = normalizedString.ToUpperInvariant();

                            // add to list
                            if (!string.IsNullOrWhiteSpace(normalizedString)) // if not empty string
                                list5.Add(normalizedString);
                        }
                    }
                    bRet = true;
                }
            }
            catch (Exception)
            {
                bRet = false;
            }

            return bRet;

        }
    }
}
