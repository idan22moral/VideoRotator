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
using System.Threading;

namespace VideoRotator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public readonly static string tempFolderPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "MediaRotatorTmp");

		public Video OriginalVideo { get; set; }
		public Video CurrentVideo { get; set; }
		public Thumbnail VideoThumbnail { get; set; }
		public List<string> TempFiles = new List<string>();

		bool _showRotator;

		public MainWindow()
		{
			InitializeComponent();

			// Load the Drag & Drop page for the first time
			Loaded += (object sender, RoutedEventArgs e) =>
			{
				// Create folder for temporary files
				Directory.CreateDirectory(tempFolderPath);

				// Create navigator and navigate to the Drag & Drop page
				MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
				MainFrame.Navigate(new DragDropPage());
			};
		}

		~MainWindow()
		{
			string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), tempFolderPath);

			// Delete the temporary folder
			if (Directory.Exists(path))
			{
				while (true)
				{
					try
					{
						foreach (string file in Directory.GetFiles(path))
						{
							File.Delete(file);
						}
						Directory.Delete(path);
						break;
					}
					catch (Exception) { }
				}
			}
		}

		public void SwitchPages()
		{
			_showRotator = !_showRotator;
			if (_showRotator)
			{
				if (CurrentVideo == null)
					throw new Exception("No video loaded and the program switched to the rotator.");
				VideoThumbnail = CurrentVideo.GetThumbnail();
				TempFiles.Add(VideoThumbnail.Path);
				MainFrame.NavigationService.Navigate(new RotatorPage());
			}
			else
			{
				ClearTemporaryData();
				MainFrame.NavigationService.Navigate(new DragDropPage());
			}
		}

		public void ClearTemporaryData()
		{
			OriginalVideo = null;
			VideoThumbnail = null;
			CurrentVideo = null;
		}
	}
}
