using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace VideoRotator
{
	public class MediaRotator
	{
		public static MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;

		public readonly string tempFolderName = MainWindow.tempFolderPath;

		public MediaRotator(){}

		/// <summary>
		/// Rotates a given <seealso cref="Media"/> by a <seealso cref="MediaRotationDirection"/>
		/// </summary>
		/// <param name="media">The media to rotate</param>
		/// <param name="mediaRotation">The rotation direction</param>
		/// <returns>Media that contaion information about the temporary rotated file</returns>
		public Media Rotate(Media media, MediaRotationDirection mediaRotation)
		{
			string tempFileName = Path.ChangeExtension(Path.GetRandomFileName(), media.Format);
			string tempFilePath = Path.Combine(Path.GetTempPath(), tempFolderName, tempFileName);

			// Add the new temporary file's path to the paths list
			_mainWindow.TempFiles.Add(tempFilePath);

			// Init process information
			ProcessStartInfo processStartInfo = new ProcessStartInfo()
			{
				FileName = "ffmpeg",
				Arguments = string.Format("-y -i \"{0}\" -vf \"transpose = {1}\" -c:a copy \"{2}\"", media.Path, (int)mediaRotation, tempFilePath),
				UseShellExecute = true,
				WindowStyle = ProcessWindowStyle.Hidden
			};

			// Start the process to rotate the video
			Process.Start(processStartInfo).WaitForExit();

			// Return media object from the currect type
			if (media is Video)
				return new Video(tempFilePath, true);
			if (media is Thumbnail)
				return new Thumbnail(tempFilePath, true);
			return new Media(tempFilePath, Path.GetExtension(tempFileName), true);
		}
	}
}
