using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaphraserApp
{
    public class InstaniateParaphraser
    {
        public static string excute_python()
        {
            var process = new ProcessStartInfo();
            process.FileName = @"C:\Users\Omar Farag\AppData\Local\Programs\Python\Python36.exe";

            var script = @"C:\Users\Omar Farag\Desktop\paraphrasev2.py";

            String[] output = new string[5];

       
            process.Arguments = string.Format("{0}", script);
            Console.WriteLine(process.Arguments);
            process.UseShellExecute = false;
            process.CreateNoWindow = true;

            var results = "";

            using (var exec = Process.Start(process))
            {
                results = exec.StandardOutput.ReadToEnd();
            }

            return results;
        }

    }
}
