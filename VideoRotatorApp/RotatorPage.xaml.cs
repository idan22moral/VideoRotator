using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace VideoRotator
{
	/// <summary>
	/// Interaction logic for RotatorPage.xaml
	/// </summary>

	public partial class RotatorPage : Page
	{
		public MainWindow _mainWindow;
		public MediaRotator rotator = new MediaRotator();

		public RotatorPage()
		{
			InitializeComponent();
			_mainWindow = (MainWindow)Application.Current.MainWindow;
			ThumbnailImage.Source = _mainWindow.VideoThumbnail.ThumbnailImage.Source;
		}
		
		#region UtilityFunctions

		/// <summary>
		/// Rotates the video in the given <see cref="MediaRotationDirection"/>.
		/// </summary>
		/// <param name="dir">The direction of rotation</param>
		void RotateOnEvent(MediaRotationDirection dir)
		{
			// Rotate the video
			Video video = (Video)rotator.Rotate(_mainWindow.CurrentVideo, dir);

			if (_mainWindow.OriginalVideo == null)
				_mainWindow.OriginalVideo = (Video)video;
			_mainWindow.CurrentVideo = video;

			// Rotate the thumbnail
			_mainWindow.VideoThumbnail = (Thumbnail)rotator.Rotate(_mainWindow.VideoThumbnail, dir);
			ThumbnailImage.Source = _mainWindow.VideoThumbnail.ThumbnailImage.Source;
		}

		/// <summary>
		/// This function moves the file in <paramref name="newFile"/> to the location of <paramref name="originalFile"/> and deletes <paramref name="originalFile"/>
		/// </summary>
		/// <param name="originalFile">The old file that should be replaced</param>
		/// <param name="newFile">The new file to replace</param>
		private void ReplaceFile(string originalFile, string newFile)
		{
			// Delete the old file
			File.Delete(originalFile);

			// Move the new file from its place to the old file's location
			File.Replace(newFile, originalFile, originalFile);
		}

		/// <summary>
		/// This function opens a dialog for the user, where he can choose path and file name to save a video.
		/// </summary>
		private void SaveVideoUsingDialog()
		{
			// Create the file path selection dialog
			CommonSaveFileDialog dialog = new CommonSaveFileDialog()
			{
				EnsureValidNames = true,
				EnsurePathExists = true,
				OverwritePrompt = true,
				DefaultExtension = "mp4",
				DefaultFileName = "RotatedVideoName",
				AlwaysAppendDefaultExtension = true,
				IsExpandedMode = true
			};

			// Show the dialog and save the user choice
			CommonFileDialogResult userChoice = dialog.ShowDialog();

			// Do things according to the user choice
			switch (userChoice)
			{
				// In case the user canceled the saving process, do nothing
				case CommonFileDialogResult.None:
				case CommonFileDialogResult.Cancel:
					return;

				// In case the user selected a file name, save a copy of the file in the selected path
				case CommonFileDialogResult.Ok:
					File.Copy(_mainWindow.CurrentVideo.Path, dialog.FileName, true);
					break;

				default:
					break;
			}
		}

		#endregion

		#region EventHandlers

		/// <summary>
		/// Save the changes that the user made to the video.
		/// </summary>
		private void SaveChanges(object sender, MouseButtonEventArgs e)
		{
			// Ask the user if he wants to replace the original video
			MessageBoxResult deleteOriginal = MessageBox.Show(
				messageBoxText: "Replace The Original Video?",
				caption: "Test",
				button: MessageBoxButton.YesNoCancel,
				icon: MessageBoxImage.Question,
				defaultResult: MessageBoxResult.Cancel
			);

			// Do things according to the user choice
			switch (deleteOriginal)
			{
				// In case of cancelation, do nothing
				case MessageBoxResult.None:
				case MessageBoxResult.Cancel:
					return;
				
				// If the user wants to replace the original video, do it.
				case MessageBoxResult.Yes:
					ReplaceFile(_mainWindow.OriginalVideo.Path, _mainWindow.CurrentVideo.Path);
					break;

				// If the user wants to save the video in a seperate file, do it.
				case MessageBoxResult.No:
					SaveVideoUsingDialog();
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Cancel the changes that the user made to the video and switch to the DragDrop Page.
		/// </summary>
		private void CancelChanges(object sender, MouseButtonEventArgs e)
		{
			ThumbnailImage.Source = null;
			ThumbnailImage = null;
			_mainWindow.ClearTemporaryData();
			_mainWindow.SwitchPages();
		}

		private void RotateRightImage_MouseUp(object sender, MouseButtonEventArgs e)
		{
			RotateOnEvent(MediaRotationDirection.Right);
		}

		private void RotateLeftImage_MouseUp(object sender, MouseButtonEventArgs e)
		{
			RotateOnEvent(MediaRotationDirection.Left);
		}

		#endregion
	}
}
