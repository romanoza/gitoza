﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gitoza.BusinessLogic
{
    // http://chrisparnin.github.io/articles/2013/09/parse-git-log-output-in-c/
    public static class DomainFacade
    {
        private static async Task<string> listShaWithFiles(string path) {
            string output = await runProcess(string.Format(" --git-dir=\"{0}/.git\"  --work-tree=\"{1}\" log --name-status --date=iso", path.Replace("\\", "/"), path.Replace("\\", "/")));
            return output;
        }

        private static async Task<string> runProcess(string command) {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;

            string gitExecutable = Properties.Settings.Default.GitExecutable;
            if (!File.Exists(gitExecutable))
                throw new FileNotFoundException(string.Format("Git executable at {0} wasn't found.", gitExecutable));
            p.StartInfo.FileName = gitExecutable;
            
            p.StartInfo.Arguments = command;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.ErrorDataReceived += p_ErrorDataReceived;
            p.Start();
            p.BeginErrorReadLine();
            string output = await p.StandardOutput.ReadToEndAsync();
            p.WaitForExit();
            return output;
        }

        static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if(e.Data != null)
                MessageBox.Show(e.Data);
        }

        public static async Task<int[,]> GetCommitCounts(string repoPath) {
            if (string.IsNullOrEmpty(repoPath))
                throw new Exception("The path is not set.");

            Task<string> t = listShaWithFiles(repoPath);

            // string output = listShaWithFiles(repoPath);
            ParseGitLog parser = new ParseGitLog();
            int[,] res = new int[7, 24];

            string output = await t;
            List<GitCommit> commits = await parser.Parse(output);
            IEnumerable<string> datesAsString = commits.Select(c => c.Headers["Date"]);
            
            var counts = datesAsString.Select(str => DateTime.Parse(str))
                .GroupBy(d => new { d.DayOfWeek, d.Hour })
                .Select(g => new { g.Key.DayOfWeek, g.Key.Hour, Count = g.Count() });
            
            
            foreach (var c in counts)
                res[(int)c.DayOfWeek, c.Hour] = c.Count;
            return res;
        }
    }
}
