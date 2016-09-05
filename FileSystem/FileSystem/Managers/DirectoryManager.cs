using FileSystem.Interfaces;
using System.Linq;
using System.IO;
using FileSystem.Models;
using System;

namespace FileSystem.Managers
{
	public class DirectoryManager : IDirectoryManager
	{
		const long FIRST_SIZE = 10000000;
		const long SECOND_SIZE = 50000000;
		const long THIRD_SIZE = 100000000;

		public Models.Directory Read(string currentPath, string pathToBrowse)
		{
			//if (currentPath == "This PC")
			//	return GetDirectory(new DirectoryInfo(pathToBrowse));

			if (pathToBrowse == "backward")
			{
				var directoryInfo = new DirectoryInfo(currentPath);
				if (directoryInfo.Parent != null)
					return GetDirectory(directoryInfo.Parent);
				pathToBrowse = "This PC";
			}

			if (pathToBrowse == "This PC")
			{
				var directory = new Models.Directory();
				directory.Path = "This PC";
				foreach (var item in DriveInfo.GetDrives())
				{
					if (!item.IsReady)
						continue;
					var file = new Models.File();
					if (item.VolumeLabel != "")
						file.Name = item.VolumeLabel + " (" + item.Name.Substring(0, 2) + ")";
					else
						file.Name = "Local Disk (" + item.Name.Substring(0, 2) + ")";
					file.Path = item.Name;
					directory.Directories.Add(file);
				}
				return directory;
			}

			return GetDirectory(new DirectoryInfo(pathToBrowse));
		}

		public Counts GetCounts(string currentPath, string pathToBrowse)
		{
			var counts = new Counts();

			//if (currentPath == "This PC")
			//{
			//	GetCounts(ref counts, new DirectoryInfo(nameToBrowse));
			//	return counts;
			//}

			if (pathToBrowse == "backward")
			{
				var directoryInfo = new DirectoryInfo(currentPath);
				if (directoryInfo.Parent != null)
				{
					GetCounts(ref counts, directoryInfo.Parent);
					return counts;
				}
				pathToBrowse = "This PC";
			}

			if (pathToBrowse == "This PC")
			{
				foreach (var item in DriveInfo.GetDrives())
					GetCounts(ref counts, item.RootDirectory);
				return counts;
			}

			GetCounts(ref counts, new DirectoryInfo(pathToBrowse));
			return counts;
		}

		private Models.Directory GetDirectory(DirectoryInfo directoryInfo)
		{
			var directory = new Models.Directory();
			directory.Path = directoryInfo.FullName;

			try
			{
				foreach (var item in directoryInfo.GetDirectories())
				{
					var file = new Models.File();
					file.Name = item.Name;
					file.Path = item.FullName;
					file.DateModified = item.LastWriteTime.ToString(@"MM-dd-yyyy HH:mm:ss");
					directory.Directories.Add(file);
				}
			}
			catch { }

			try
			{
				foreach (var item in directoryInfo.GetFiles())
				{
					var file = new Models.File();
					file.Name = item.Name;
					file.Path = item.FullName;
					file.DateModified = item.LastWriteTime.ToString(@"MM-dd-yyyy HH:mm:ss");
					file.Size = SetFileSize(item.Length);
					directory.Files.Add(file);
				}
			}
			catch { }

			return directory;
		}

		private void GetCounts(ref Counts counts, DirectoryInfo directoryInfo)
		{
			try
			{
				foreach (var item in directoryInfo.GetDirectories())
					GetCounts(ref counts, item);
			}
			catch { }

			try
			{
				counts.FirstCount += directoryInfo.EnumerateFiles().Where(f => f.Length <= FIRST_SIZE).Count();
			}
			catch { }

			try
			{
				counts.SecondCount += directoryInfo.EnumerateFiles().Where(f => f.Length > FIRST_SIZE && f.Length <= SECOND_SIZE).Count();
			}
			catch { }

			try
			{
				counts.ThirdCount += directoryInfo.EnumerateFiles().Where(f => f.Length > THIRD_SIZE).Count();
			}
			catch { }
		}

		private string SetFileSize(long length)
		{
			double dblSize = length;
			int count = 0;

			while (dblSize >= 1000 && count <= 3)
			{
				dblSize /= 1000;
				count++;
			}

			switch (count)
			{
				case 0:
					return dblSize.ToString() + " B";
				case 1:
					return Math.Round(dblSize, 1).ToString() + " kB";
				case 2:
					return Math.Round(dblSize, 1).ToString() + " MB";
				default:
					return Math.Round(dblSize, 1).ToString() + " GB";
			}
		}
	}
}