using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace VideoRotator
{

	public class Video : Media
	{
		// Fields
		static string[] validFormats = new string[] {".mp4", ".avi", ".ogg", ".flv"};

		public Video(string path) : base(path, System.IO.Path.GetExtension(path))
		{
			if (!IsVideo(path))
				throw new Exception("Invalid video format.");
		}

		public Video(string path, bool temporary) : this(path)
		{
			_isTemporary = temporary;
		}

		/// <summary>
		/// This function generates a Thumbnail (preview image) of the video.
		/// </summary>
		/// <returns>Thumbnail of the video</returns>
		public Thumbnail GetThumbnail()
		{
			string tempFileName = System.IO.Path.ChangeExtension(System.IO.Path.GetRandomFileName(), "png");
			string tempFilePath = System.IO.Path.Combine(MainWindow.tempFolderPath, tempFileName);

			// Init process information
			ProcessStartInfo processStartInfo = new ProcessStartInfo()
			{
				FileName = "ffmpeg",
				Arguments = string.Format("-y -i \"{0}\" -ss 00:00:01 -vframes {1} \"{2}\"", Path, 1, tempFilePath),
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Hidden
			};

			// Start the process to get the thumbnail
			Process.Start(processStartInfo).WaitForExit();

			return new Thumbnail(tempFilePath);
		}

		/// <summary>
		/// Returns <see langword="true"/> if the file in <paramref name="path"/> is an Video, else returns <see langword="false"/>
		/// </summary>
		/// <param name="path">The path to the file to check</param>
		/// <returns></returns>
		public static bool IsVideo(string path)
		{
			return path != null && validFormats.Contains(System.IO.Path.GetExtension(path));
		}
	}
}
