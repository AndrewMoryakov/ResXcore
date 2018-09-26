using System;
using System.IO;

namespace ResXcore
{
    public static class FileValidator
    {
		internal static void ThrowIfStringNull(string line, string errMess)
		{
			if (string.IsNullOrEmpty(line))
				throw new ArgumentNullException(errMess);
		}

		internal static void ThrowIfEmpty(string line, string errMess)
		{
			if (string.IsNullOrEmpty(line))
				throw new ArgumentException(errMess);
		}

		internal static void ThrowIfFileNotExists(string filePath, string mess)
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException(mess);
		}

		internal static void ThrowIfFileNotResx(string filePath, string mess)
		{
			if (Path.GetExtension(filePath) != ".resx")
				throw new ArgumentException(mess);
		}
	}
}