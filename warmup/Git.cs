// Copyright 2007-2010 The Apache Software Foundation.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace warmup
{
    using System;
    using System.Diagnostics;
    using settings;

    public class Git :
        IExporter
    {
        public static void Clone(Uri sourceLocation, TargetDir target)
        {
            var separationCharacters = new[] {".git"};
            string[] piecesOfPath = sourceLocation.ToString().Split(separationCharacters, StringSplitOptions.RemoveEmptyEntries);
            if (piecesOfPath != null && piecesOfPath.Length > 0)
            {
                string sourceLocationToGit = piecesOfPath[0] + ".git";

                var psi = new ProcessStartInfo("cmd",
                                               string.Format(" /c git clone {0} {1}", sourceLocationToGit, target.FullPath));

                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;

                //todo: better error handling
                Console.WriteLine("Running: {0} {1}", psi.FileName, psi.Arguments);
                string output, error = "";
                using (Process p = Process.Start(psi))
                {
                    output = p.StandardOutput.ReadToEnd();
                    error = p.StandardError.ReadToEnd();
                }

                Console.WriteLine(output);
                Console.WriteLine(error);


                //string git_directory = Path.Combine(target.FullPath, ".git");
                //if (Directory.Exists(git_directory))
                //{
                //    Console.WriteLine("Deleting {0} directory", git_directory);
                //    Directory.Delete(git_directory, true);
                //}
            }
        }

        public void Export(string sourceControlWarmupLocation, string templateName, TargetDir targetDir)
        {
            var baseUri = new Uri(WarmupConfiguration.settings.SourceControlWarmupLocation + templateName);
            Console.WriteLine("svn exporting to: {0}", targetDir.FullPath);
            Clone(baseUri, targetDir);
        }
    }
}