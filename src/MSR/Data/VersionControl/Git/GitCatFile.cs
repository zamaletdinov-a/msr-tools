using System;
using System.Collections.Generic;
using System.IO;

namespace MSR.Data.VersionControl.Git
{
    /// <summary>
    /// Keeps the revision last modified each line.///////////////////
    /// </summary>
    public class GitCatFile : string
    {
        /// <summary>
        /// Parse catFileCommit stream.
        /// </summary>
        /// <param name="catFileData">Git catFile in incremental format.</param> ///////
        /// <returns>Dictionary of line numbers (from 1) with revisions.</returns> ///////////
        public static string ParseCommit(Stream catFileData)
        {
            TextReader reader = new StreamReader(catFileData);
            //GitCatFile catFile = new GitCatFile();
            string line="";

            /*while ((line = reader.ReadLine()) != null)
            {
                if ((line.Length >= 46) && (line.Length < 100))
                {
                    string[] parts = line.Split(' ');
                    if ((parts.Length == 4) && (parts[0].Length == 40))
                    {
                        int lines = Convert.ToInt32(parts[3]);
                        int startLine = Convert.ToInt32(parts[2]);
                        for (int i = 0; i < lines; i++)
                        {
                            catFile.Add(startLine + i, parts[0]);
                        }
                    }
                }
            }*/

            return line;
        }

        /// <summary>
        /// Parse catFileCommit stream.
        /// </summary>
        /// <param name="catFileData">Git catFile in incremental format.</param> ///////
        /// <returns>Dictionary of line numbers (from 1) with revisions.</returns> ///////////
        public static string ParseTree(Stream catFileData, string filePath)
        {
            TextReader reader = new StreamReader(catFileData);
            //GitCatFile catFile = new GitCatFile();
            string line = "";

            /*while ((line = reader.ReadLine()) != null)
            {
                if ((line.Length >= 46) && (line.Length < 100))
                {
                    string[] parts = line.Split(' ');
                    if ((parts.Length == 4) && (parts[0].Length == 40))
                    {
                        int lines = Convert.ToInt32(parts[3]);
                        int startLine = Convert.ToInt32(parts[2]);
                        for (int i = 0; i < lines; i++)
                        {
                            catFile.Add(startLine + i, parts[0]);
                        }
                    }
                }
            }*/

            return line;
        }

        /// <summary>
        /// Parse catFileCommit stream.
        /// </summary>
        /// <param name="catFileData">Git catFile in incremental format.</param> ///////
        /// <returns>Dictionary of line numbers (from 1) with revisions.</returns> ///////////
        public static int ParseBlob(Stream catFileData, int[] lines)
        {
            TextReader reader = new StreamReader(catFileData);
            //GitCatFile catFile = new GitCatFile();
            int line = 0;

            /*while ((line = reader.ReadLine()) != null)
            {
                if ((line.Length >= 46) && (line.Length < 100))
                {
                    string[] parts = line.Split(' ');
                    if ((parts.Length == 4) && (parts[0].Length == 40))
                    {
                        int lines = Convert.ToInt32(parts[3]);
                        int startLine = Convert.ToInt32(parts[2]);
                        for (int i = 0; i < lines; i++)
                        {
                            catFile.Add(startLine + i, parts[0]);
                        }
                    }
                }
            }*/

            return line;
        }
    }
}
