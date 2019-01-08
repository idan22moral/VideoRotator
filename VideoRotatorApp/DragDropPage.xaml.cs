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

namespace VideoRotator
{
	/// <summary>
	/// Interaction logic for DragDropPage.xaml
	/// </summary>
	public partial class DragDropPage : Page
	{
		const string DRAG_DROP_WAIT_TEXT = "Drag & Drop Video";
		const string DRAG_DROP_ENTER_TEXT = "Cool, Just Drop It!";

		MainWindow _mainWindow;

		public DragDropPage()
		{
			InitializeComponent();
			_mainWindow = (MainWindow)Application.Current.MainWindow;
		}

		private void Page_DragEnter(object sender, DragEventArgs e)
		{
			DragDropLabel.Content = DRAG_DROP_ENTER_TEXT;
		}

		private void Page_DragLeave(object sender, DragEventArgs e)
		{
			DragDropLabel.Content = DRAG_DROP_WAIT_TEXT;
		}

		private void Page_Drop(object sender, DragEventArgs e)
		{
			DragDropLabel.Content = DRAG_DROP_WAIT_TEXT;
			var path = e.Data.GetData(DataFormats.FileDrop);

			if (path == null)
			{
				MessageBox.Show("Invalid Data Type!\n Try Again.", "Try Again", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}
			else
			{
				// If the file is Video
				if (Video.IsVideo((path as string[])[0]))
				{
					// Save the video
					_mainWindow.CurrentVideo = new Video((path as string[])[0]);
					// Show the proper GUI
					_mainWindow.SwitchPages();
				}
				else
					MessageBox.Show("The file's format is incorrect.\n Try Again.");
			}
		}
	}
}
