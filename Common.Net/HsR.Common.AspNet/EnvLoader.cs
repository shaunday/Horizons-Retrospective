using System;
using System.Collections.Generic;
using System.IO;
using DotNetEnv;

namespace HsR.Common.AspNet
{
    public static class EnvLoader
    {
        public static void LoadEnvFiles(IEnumerable<string> envFiles)
        {
            foreach (var FileName in envFiles)
            {
                string tFilePath = Path.Combine("E:\\documents\\git\\HsR-Configs", FileName); //dev files
                if (!File.Exists(tFilePath))
                {
                    throw new FileNotFoundException($"Missing .env file: {tFilePath}");
                }

                Env.Load(tFilePath);
            }
        }
    }
}
