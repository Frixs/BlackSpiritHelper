using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Handles reading/writing and querying the file system.
    /// </summary>
    public class FileManager : IFileManager
    {
        /// <summary>
        /// Writes the text to the specified file.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="path">The path of the file to write to.</param>
        /// <param name="append">If true, writes the text to the end of the file, otherwise overrides any existing file.</param>
        /// <returns></returns>
        public async Task WriteTextToFileAsync(string text, string path, bool append = false)
        {
            // Normalize path.
            path = NormalizedPath(path);

            // Resolve to absolute path.
            path = ResolvePath(path);

            // Lock the task.
            await AsyncAwaiter.AwaitAsync(nameof(WriteTextToFileAsync) + path, async () =>
            {
                // Run the synchronous file access as a new task.
                await IoC.Task.Run(() =>
                {
                    // Write the log message to file.
                    using (var writer = (TextWriter)new StreamWriter(File.Open(path, append ? FileMode.Append : FileMode.Create)))
                        writer.Write(text);
                });
            });
        }

        /// <summary>
        /// Count lines in the specified file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public long LineCount(string path)
        {
            long lineCount = 0;

            // Normalize path.
            path = NormalizedPath(path);

            // Resolve to absolute path.
            path = ResolvePath(path);

            // Count lines.
            using (var reader = new StreamReader(path))
            {
                while (reader.ReadLine() != null)
                    lineCount++;
            }

            return lineCount;
        }

        /// <summary>
        /// Read all lines from specified file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<string> ReadLines(string path)
        {
            var lines = new List<string>();

            // Normalize path.
            path = NormalizedPath(path);

            // Resolve to absolute path.
            path = ResolvePath(path);

            // Get lines.
            using (var reader = new StreamReader(path))
            {
                var line = reader.ReadLine();
                var c = 0;
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                    c++;
                }
            }

            return lines;
        }

        /// <summary>
        /// Check if the file is in use or not.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool IsInUse(FileInfo file)
        {
            FileStream stream = null;
            bool isOpened = false;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                // The file is unavailable because it is:
                //  - Still being written to.
                //  - Or being processed by another thread.
                //  - Or does not exist (has already been processed).
                isOpened = true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            // File is not in use.
            return isOpened;
        }

        /// <summary>
        /// Normalizing a path based on the current operating system.
        /// </summary>
        /// <param name="path">The path to normalize.</param>
        /// <returns></returns>
        public string NormalizedPath(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return path?.Replace('/', '\\').Trim();
            else
                return path?.Replace('\\', '/').Trim();
        }

        /// <summary>
        /// Resolves any relative elements of the path to absolute.
        /// </summary>
        /// <param name="path">The path to resolve.</param>
        /// <returns></returns>
        public string ResolvePath(string path)
        {
            // Resolve the path to absolute.
            return Path.GetFullPath(path);
        }
    }
}
