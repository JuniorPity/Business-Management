using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace BusinessManagement.Models
{
    public class ImageProcessor
    {
        public string ImageToBinary(Image img)
        {
            string base64string = "";

            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                byte[] imgBytes = ms.ToArray();

                base64string = Convert.ToBase64String(imgBytes);
            }

            return base64string;
        }

        public Image BinaryToImage(string binary)
        {
            byte[] bytes = Convert.FromBase64String(binary);
            Image image;

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }
    }
}