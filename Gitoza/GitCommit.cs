﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitoza
{
    // http://chrisparnin.github.io/articles/2013/09/parse-git-log-output-in-c/
    public class GitCommit
    {
        public GitCommit() {
            Headers = new Dictionary<string, string>();
            Files = new List<GitFileStatus>();
            Message = "";
        }

        public Dictionary<string, string> Headers { get; set; }
        public string Sha { get; set; }
        public string Message { get; set; }
        public List<GitFileStatus> Files { get; set; }

        public void Print() {
            Console.WriteLine("commit " + Sha);
            foreach (var key in Headers.Keys) {
                Console.WriteLine(key + ":" + Headers[key]);
            }
            Console.WriteLine();
            Console.WriteLine(Message);
            Console.WriteLine();
            foreach (var file in Files) {
                Console.WriteLine(file.Status + "\t" + file.File);
            }
        }
    }
}