﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace word_count
{
    class Scanner
    {
        public string RootDirectory { get; set; }
        private IEnumerable<string> _filePath;
        public IEnumerable<string> FilePaths {
            get
            {
                if(_filePath == null)
                {
                    _filePath = GetFiles(RootDirectory, "*.txt");
                }
                return _filePath;
            }
        }

        public Dictionary<string,int> WordDict { get; set; }

        public Scanner(string root)
        {
            if (Directory.Exists(root))
            {
                RootDirectory = root;
            }
            else
            {
                throw new FileNotFoundException("Error Directory!");
            }
        }

        public IEnumerable<string> GetFiles(string root,string searchPattern)
        {
            List<string> paths = new List<string>();
            Stack<string> pending = new Stack<string>();
            pending.Push(root);
            while(pending.Count != 0)
            {
                var path = pending.Pop();
                string[] next = null;
                try
                {
                    next = Directory.GetFiles(path, searchPattern);
                }
                catch { }
                if (next != null && next.Length != 0)
                    foreach (var n in next) yield return n;
                try
                {
                    next = Directory.GetDirectories(path);
                    foreach (var subDir in next) pending.Push(subDir);
                }
                catch { }
            }
        }

        public void CountWord()
        {
            Stopwatch Watch = new Stopwatch();
            Watch.Start();

            WordDict = new Dictionary<string, int>();            
            Watch.Start();

            Parallel.ForEach(FilePaths, sFile =>
            {
                try
                {
                    Dictionary<string, int> localWordDict = new Dictionary<string, int>();
                    string allLine = File.ReadAllText(sFile);
                    allLine = Regex.Replace(allLine, "[^a-zA-Z0-9]", " ");
                    string[] splits = allLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in splits)
                    {
                        if (localWordDict.ContainsKey(word))
                            localWordDict[word]++;
                        else
                            localWordDict.Add(word, 1);
                    }
                    lock (WordDict)
                    {
                        foreach (var pair in localWordDict)
                        {
                            if (WordDict.ContainsKey(pair.Key))
                                WordDict[pair.Key] += localWordDict[pair.Key];
                            else
                                WordDict.Add(pair.Key, pair.Value);
                        }
                    }
                }
                catch { }
            });
            WordDict = WordDict.OrderByDescending(o => o.Value).ToDictionary(p => p.Key, o => o.Value);
            int index = 0;
            foreach(var pair in WordDict)
            {
                if (index == 100)
                {
                    break;
                }
                index++;
                Console.WriteLine("Key:{0},Value:{1}", pair.Key, pair.Value);
            }

            Watch.Stop();
            Console.WriteLine("Spent time:{0}", Watch.ElapsedMilliseconds);
        }

    }
}