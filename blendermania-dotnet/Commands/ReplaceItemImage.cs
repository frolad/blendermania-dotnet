using Pfim;
using GBX.NET;
using GBX.NET.Engines.GameData;
using GBX.NET.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace blendermania_dotnet
{
    class ReplaceItemImage
    {
        public string ItemPath { get; set; } = "";
        public string ImagePath { get; set; } = "";

        async public Task Exec()
        {
            if (ItemPath.Length == 0)
            {
                throw new Exception("ItemPath is empty");
            }
            else if (ImagePath.Length == 0)
            {
                throw new Exception("ItemPath is empty");
            }
            else if (!ImagePath.EndsWith(".tga"))
            {
                throw new Exception("Only .tga image supported");
            }

            var item = GameBox.ParseNodeHeader<CGameItemModel>(ItemPath, readRawBody: true);
            var bitmap = TgaToBitmap(ImagePath);
            var resized = new Bitmap(bitmap, new Size(64, 64));
            var colors = BitMapToColor(resized);

            item.Icon = colors;
            item.IconWebP = null;

            await item.SaveAsync();
        }

        public Bitmap TgaToBitmap(string Path)
        {
            var tga = Pfimage.FromFile(Path);
            PixelFormat format;

            switch (tga.Format)
            {
                case Pfim.ImageFormat.Rgba32:
                    format = PixelFormat.Format32bppArgb;
                    break;
                default:
                    // see the sample for more details
                    throw new NotImplementedException();
            }

            var data = Marshal.UnsafeAddrOfPinnedArrayElement(tga.Data, 0);
            return new Bitmap(tga.Width, tga.Height, tga.Stride, format, data);
        }

        public Color[,] BitMapToColor(Bitmap bm)
        {
            var colors = new Color[bm.Width, bm.Height];
            for (int i = 0; i < bm.Height; i++)
            {
                for (int j = 0; j < bm.Width; j++)
                {
                    var mediacolor = bm.GetPixel(i, j);
                    var drawingcolor = Color.FromArgb(mediacolor.A, mediacolor.R, mediacolor.G, mediacolor.B);
                    colors[i, j] = drawingcolor;
                }
            }

            return colors;
        }
    }
}