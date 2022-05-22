using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace LibraryNET6Pages
{
	static class ImageController
	{
		public static class Convert
		{
			public static byte[] ImageToByteArray(Image image)
			{
				var ms = new MemoryStream();

				image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

				return ms.ToArray();
			}

			public static Image ByteArrayToImage(byte[] binaryData)
				=> Image.FromStream(new MemoryStream(binaryData));

			public static byte[] WpfImageToByteArray(System.Windows.Controls.Image image)
			{
				MemoryStream memStream = new MemoryStream();

				var encoder = new PngBitmapEncoder();

				encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
				encoder.Save(memStream);

				return memStream.ToArray();
			}

			public static byte[] WpfImageToByteArray(BitmapImage image)
				=> ((MemoryStream)(image.StreamSource)).ToArray();

			public static System.Windows.Controls.Image ByteArrayToWpfImage(byte[] binaryData)
			{
				var bi = new BitmapImage();
				bi.BeginInit();
				bi.StreamSource = new MemoryStream(binaryData);
				bi.EndInit();

				return new System.Windows.Controls.Image() { Source = bi };
			}
		}
	}
}
