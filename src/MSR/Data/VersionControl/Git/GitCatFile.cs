using System;
using System.Collections.Generic;
using System.IO;

namespace MSR.Data.VersionControl.Git
{
    /// <summary>
    /// Keeps the revision last modified each line.///////////////////
    /// </summary>
    public class GitCatFile
    {
        /// <summary>
        /// Parse catFileCommit stream.
        /// </summary>
        /// <param name="catFileData">Git catFile in incremental format.</param> ///////
        /// <returns>Hash tree.</returns> ///////////
        public static string ParseCommit(Stream catFileData)
        {
            TextReader reader = new StreamReader(catFileData);
            string line;
            string hashTree="";

            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');
                if (parts[0] == "tree")
                    hashTree = parts[1];
            }

            return hashTree;
        }

        /// <summary>
        /// Parse catFileTree stream.
        /// </summary>
        /// <param name="catFileData">Git catFile in incremental format.</param> ///////
        /// <returns>Hash blob.</returns> ///////////
        public static string ParseTree(Stream catFileData, string filePath)
        {
            TextReader reader = new StreamReader(catFileData);
            string line;
            string hash = "";

            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains(filePath))
                {
                    string[] parts = line.Split(' ');
                    string[] separators = { "\t" };
                    string[] parts2 = parts[2].Split(separators, StringSplitOptions.None);
                    hash = parts2[0];
                }
            }

            return hash;
        }

        /// <summary>
        /// Parse catFileBlob stream.
        /// </summary>
        /// <param name="catFileData">Git catFile in incremental format.</param> ///////
        /// <returns>Comment lines number.</returns> ///////////
        public static int ParseBlob(Stream catFileData, int[] lines, int linesNumber)
        {
            TextReader reader = new StreamReader(catFileData);
            string line;
            int numberLine=1;
            int i = 0;
            int commentLinesNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                if (numberLine == lines[i])
                {
                    string lineWithoutSpaces = line.Trim();
                    if (lineWithoutSpaces.IndexOf('%') == 0)
                    {
                        commentLinesNumber++;
                    }
                    if (i == linesNumber - 1)
                    {
                        break;
                    }
                    i++;
                }
                numberLine++;
            }

            return commentLinesNumber;
        }
    }
}
