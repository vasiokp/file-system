using FileSystem.Models;

namespace FileSystem.Interfaces
{
	public interface IDirectoryManager
	{
		Directory Read(string currentPath, string pathToBrowse);

		Counts GetCounts(string currentPath, string pathToBrowse);
	}
}