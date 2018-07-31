using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FileReader
{
    class ReadFromFile
    {
        static void Main()
        {
            Clear();
            Console.WriteLine("Type in the directory of a text file. Submit nothing for an excerpt from 'To Kill a Mocking Bird.'");
            string file = Console.ReadLine();
            if (file == "") { file = @"C:\Users\ekuehl\Documents\textSample.txt"; }
            //A test file would be at C:\Users\ekuehl\Documents\test.txt
            string text = System.IO.File.ReadAllText(file);
            text = text.Replace("Ms.", "Ms/").Replace("Mrs.", "Mrs/").Replace("Mr.", "Mr/").Replace(".", ".*").Replace("!", "!*").Replace("?", "?*");
            var finaltext = splitting(text);
            Count(text, finaltext);
        }

        static string[] splitting(string text)
        {
            string newSentence;
            List<string> ret = new List<string>();
            string[] splitter = text.Split('*');
            foreach (string sentence in splitter)
            {
                newSentence = sentence;
                newSentence = newSentence.Replace("/", ".").Replace("\n", " ").Replace("\r", " ");
                ret.Add(newSentence);
                char[] sentenceList = newSentence.ToCharArray();
                try
                {
                    while (sentenceList[0] == ' ')
                    {
                        string str = new string(sentenceList);
                        str = str.Remove(0, 1);
                        sentenceList = str.ToCharArray();
                    }
                }
                catch { }

                newSentence = String.Join("", sentenceList);
                Console.WriteLine(newSentence);
                AddSentence(newSentence);
            }
            string[] stuff = ret.ToArray();
            return stuff;
        }

        static void Count(string text, string[] finaltext)
        {
            int sentences = 0;
            int words = 0;
            string[] notdot = text.Split(new String[] { "ms.", "mrs.", "mr." }, StringSplitOptions.None);
            foreach (string split in notdot) { sentences--; }
            string[] sentence = text.Split(new Char[] { '.', '?', '!' });
            foreach (string split in sentence) { sentences++; }
            Console.WriteLine("\nThere are {0} sentences in this passage.", sentences);

            while (true)
            {
                words = -1;
                Console.WriteLine("\nWhat word count would you like to look for in the passage? Type !q to quit.");
                string what = Console.ReadLine();
                if (what == "!q") { break; }
                string joinedText = String.Join(" ", finaltext);
                string[] split_text = joinedText.Split(new string[] { what }, StringSplitOptions.None);
                foreach (string split in split_text) { words++; }
                Console.WriteLine("\nThere are {0} '{1}'s in the passage.", words, what);
            }
        }

        static void AddSentence(string Sentence)
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = "Server=.;Database=practice1;Trusted_Connection=true";
                con.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO Sentences VALUES(@Sentence)", con))
                {
                    try
                    {
                        command.Parameters.Add(new SqlParameter("Sentence", Sentence));
                        command.ExecuteNonQuery();
                    }
                    catch
                    { Console.WriteLine("*Sentence not inserted.*"); }
                }
            }
        }

        static void Clear()
        {
            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = "Server=.;Database=practice1;Trusted_Connection=true";
                con.Open();
                try
                {
                    SqlCommand command = new SqlCommand("TRUNCATE TABLE Sentences", con);
                    command.ExecuteNonQuery();
                }
                catch
                { Console.WriteLine("Sentences not deleted."); }
            }
        }
    }
}
