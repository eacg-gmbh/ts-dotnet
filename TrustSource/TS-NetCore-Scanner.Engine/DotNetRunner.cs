﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_NetCore_Scanner.Engine
{
    internal class DotNetRunner
    {
        private readonly string fileName;
        private readonly bool useDotnet;

        public DotNetRunner(string fileName = "dotnet", bool useDotnetToExecute = false)
        {
            this.fileName = fileName;
            this.useDotnet = useDotnetToExecute;
        }

        public RunStatus Run(string workingDirectory, string[] arguments)
        {
            var fileName = useDotnet ? "dotnet" : this.fileName;
            var args = (useDotnet ? this.fileName + " " : "") + string.Join(" ", arguments);

            var psi = new ProcessStartInfo(fileName, args)
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var p = new Process();
            try
            {
                p.StartInfo = psi;
                p.Start();

                var output = new StringBuilder();
                var errors = new StringBuilder();
				var outputTask = Task.Run(() => ConsumeStreamReaderAsync(p.StandardOutput, output));
				var errorTask = Task.Run(() => ConsumeStreamReaderAsync(p.StandardError, errors));
				var processExited = p.WaitForExit(20000);

                if (processExited == false)
                {
                    p.Kill();

                    return new RunStatus(output.ToString(), errors.ToString(), exitCode: -1);
                }

                Task.WaitAll(outputTask, errorTask);

                return new RunStatus(output.ToString(), errors.ToString(), p.ExitCode);
            }
            finally
            {
                p.Dispose();
            }
        }

        private static async Task ConsumeStreamReaderAsync(StreamReader reader, StringBuilder lines)
        {
            await Task.Yield();

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                lines.AppendLine(line);
            }
        }
    }

    internal class RunStatus
    {
        public string Output { get; }
        public string Errors { get; }
        public int ExitCode { get; }

        public bool IsSuccess => ExitCode == 0;

        public RunStatus(string output, string errors, int exitCode)
        {
            Output = output;
            Errors = errors;
            ExitCode = exitCode;
        }

    }
}
