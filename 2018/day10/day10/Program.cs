using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace day10
{
    class Program
    {
        public const int SIZE = 200;

        public const int OFFSET_X = -20;

        public const int OFFSET_Y = -20;

        public const float NORMALIZE_Y = 1;
        public const float NORMALIZE_X = 1;
        public const float NORMALIZE_VELOCITY_X = 1;
        public const float NORMALIZE_VELOCITY_Y = 1;

        public static List<Star> Stars { get; set; }
        
        public static char[][] StarView { get; set; }

        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("../../../input.txt"))
            {
                var inputString = sr.ReadToEnd();

                Stars = new List<Star>();
                StarView = new char[SIZE][];

                for (int i = 0; i < StarView.Length; i++)
                {
                    StarView[i] = Enumerable.Repeat(' ', SIZE).ToArray();
                }

                var starString = inputString.Split(Environment.NewLine);

                foreach (var item in starString)
                {
                    var star = new Star();
                    var pos = item.Substring("position=<".Length, item.IndexOf('>') - "position=<".Length).Split(',');
                    var vel = item.Substring(item.LastIndexOf('<') + 1, item.Length - (item.LastIndexOf('<') + 1)).Replace(">",string.Empty).Split(',');
                    star.PositionX = float.Parse(pos[0].Trim()) / NORMALIZE_X + (float)OFFSET_X;
                    star.PositionY = float.Parse(pos[1].Trim()) / NORMALIZE_Y + (float)OFFSET_Y;
                    star.VelocityX = float.Parse(vel[0].Trim()) / NORMALIZE_VELOCITY_X;
                    star.VelocityY = float.Parse(vel[1].Trim()) / NORMALIZE_VELOCITY_Y;
                    Stars.Add(star);
                }
            }

            for (int i = 0; i < 13000; i++)
            {                
                foreach (var star in Stars)
                {
                    star.PositionX = star.PositionX + star.VelocityX;
                    star.PositionY = star.PositionY + star.VelocityY;

                    if(i > 10570) { 
                        if (star.PositionY >= 0 && star.PositionX >= 0 && star.PositionX < SIZE && star.PositionY < SIZE) {
                        
                            StarView[(int)Math.Floor(star.PositionY)][(int)Math.Floor(star.PositionX)] = '#';
                        }
                    }
                }
                
                //CreateImage(i);
                if(i > 10570) {
                    Console.Clear();
                    for (int r = 0; r < StarView.Length; r++)
                    {
                        for (int p = 0; p < StarView[r].Length; p++)
                        {
                            Console.Write(StarView[r][p]);
                        }
                        Console.WriteLine();
                    }
                    
                    StarView = new char[SIZE][];
                    for (int a = 0; a < StarView.Length; a++)
                    {
                        StarView[a] = Enumerable.Repeat(' ', SIZE).ToArray();
                    }
                }
            }

            Console.ReadLine();
        }
     
        
        private static void CreateImage(int i)
        {
            using (var image = new Bitmap(SIZE, SIZE))
            {
                using (var graphics = Graphics.FromImage(image))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, SIZE, SIZE);

                    using (SolidBrush brush = new SolidBrush(Color.Black))
                    {
                        graphics.FillRectangle(brush, 0, 0, SIZE, SIZE);
                    }

                    foreach (var star in Stars)
                    {
                        if (star.PositionY >= 0 && star.PositionX >= 0 && star.PositionX < SIZE && star.PositionY < SIZE)
                        {
                            image.SetPixel((int)Math.Floor(star.PositionX), (int)Math.Floor(star.PositionY), Color.White);
                        }
                    }                    
                    using (var output = File.Open($"D:/w/AoC/2018/day10/images/{i}.jpg", FileMode.Create))
                    {
                        var qualityParamId = Encoder.Quality;
                        var encoderParameters = new EncoderParameters();
                        encoderParameters.Param[0] = new EncoderParameter(qualityParamId, 75L);
                        var codec = ImageCodecInfo.GetImageDecoders()
                            .FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);

                        image.Save(output, codec, encoderParameters);
                    }
                }
            }
        }
    }

    class Star
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
    }
}
