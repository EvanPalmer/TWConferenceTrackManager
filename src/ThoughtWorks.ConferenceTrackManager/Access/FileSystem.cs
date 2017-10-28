using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ThoughtWorks.ConferenceTrackManager.Access
{
    public interface IFileSystem
    {
        List<string> ReadFileAsStringListOrEmptyList(string inputFileNameWithPath);
    }

    public class FileSystem : IFileSystem
    {
        public List<string> ReadFileAsStringListOrEmptyList(string inputFileNameWithPath)
        {
            var fileContentsArray = File.ReadAllLines(inputFileNameWithPath);

			List<string> fileContentsList = new List<string>() ;
            if (fileContentsArray != null && fileContentsArray.Length > 0)
            {
                fileContentsList = new List<string>(fileContentsArray);
            }

            return fileContentsList;
        }
    }
}
