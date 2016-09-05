using System.Collections.Generic;

namespace FileSystem.Models
{
	public class Directory
	{
		public string Path { get; set; }

		public List<File> Directories { get; set; }

		public List<File> Files { get; set; }

		public Directory ()
		{
			Directories = new List<File>();
			Files = new List<File>();
		}
	}
}