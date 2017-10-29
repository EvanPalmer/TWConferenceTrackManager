using System.Collections.Generic;
using System.IO;

namespace ThoughtWorks.ConferenceTrackManager.Access
{
    public interface IFileSystem
    {
        List<string> ReadFileAsStringListOrEmptyList(string inputFileNameWithPath);
    }

    public class FileSystem : IFileSystem
    {
        readonly IOutputWriter _outputWriter;

        public FileSystem(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public List<string> ReadFileAsStringListOrEmptyList(string inputFileNameWithPath)
        {
            List<string> fileContentsList = new List<string>();

            try
            {
                var fileContentsArray = File.ReadAllLines(inputFileNameWithPath);

                if (fileContentsArray != null && fileContentsArray.Length > 0)
                {
                    fileContentsList = new List<string>(fileContentsArray);
                }
            }catch{
                _outputWriter.WriteLine($"Error reading file: {inputFileNameWithPath}");
            }
            return fileContentsList;
        }
    }
}
