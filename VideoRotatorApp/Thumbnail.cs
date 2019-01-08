using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;

namespace VideoRotator
{
	public class Thumbnail : Media
	{
		// Fields
		static string[] validFormats = new string[] { ".png", ".jpg", ".jpeg" };
		public Image ThumbnailImage { get; private set; }

		public Thumbnail(string path) : base(path, System.IO.Path.GetExtension(path), true)
		{
			if (!IsThumbnail(path))
				throw new Exception("Invalid thumbnail format.");
			
			// Save the Thumbnail to the image
			BitmapImage bmp = new BitmapImage(new Uri(path));
			ThumbnailImage = new Image() { Source = bmp };
		}

		public Thumbnail(string path, bool temporary) : this(path)
		{
			_isTemporary = temporary;
		}

		/// <summary>
		/// Returns <see langword="true"/> if the file in <paramref name="path"/> is an Image, else returns <see langword="false"/>
		/// </summary>
		/// <param name="path">The path to the file to check</param>
		/// <returns></returns>
		public static bool IsThumbnail(string path)
		{
			return path != null && validFormats.Contains(System.IO.Path.GetExtension(path));
		}
	}
}
