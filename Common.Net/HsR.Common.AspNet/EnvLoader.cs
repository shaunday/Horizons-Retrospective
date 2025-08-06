using System;
using System.Collections.Generic;
using System.IO;
using DotNetEnv;

namespace HsR.Common.AspNet
{
    public static class EnvLoader
    {
        /// <summary>
        /// Loads the specified .env files after verifying they exist.
        /// Throws FileNotFoundException if any file is missing.
        /// </summary>
        /// <param name="envFilePaths">List of full paths to .env files.</param>
        public static void LoadEnvFiles(IEnumerable<string> envFilePaths)
        {
            foreach (var path in envFilePaths)
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Missing .env file: {path}");
                }

                Env.Load(path);
            }
        }
    }
}
