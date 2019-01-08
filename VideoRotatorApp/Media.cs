using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace VideoRotator
{
	public enum MediaRotationDirection { Right = 1, Left };
	public enum MediaFormats { Mp4, Avi, Ogg, Flv, Png, Jpg, Jpeg };

	public class Media
	{
		// Properties
		public string Path { get; set; }
		public string Format { get; set; }
		
		// Statics & Enums
		public static string[] mediaFormats;

		// Fields
		protected bool _isTemporary;

		public Media(string path, string format)
		{
			Path = path;
			Format = format;
			_isTemporary = false;
		}

		public Media(string path, string format, bool temporary) : this(path, format)
		{
			_isTemporary = temporary;
		}
	}
}
