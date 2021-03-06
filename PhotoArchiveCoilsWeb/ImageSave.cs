using Microsoft.EntityFrameworkCore;
using PhotoArchiveCoilsWeb.Data;
using PhotoArchiveCoilsWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace PhotoArchiveCoilsWeb
{
    public class ImageSave
    {
        //public FileContext context { get; set; }
        public string connection { get; set; }
        private static ImageSave _instance;

        public static ImageSave Instance()
        {
            // Uses lazy initialization.

            // Note: this is not thread safe.

            if (_instance == null)
            {
                _instance = new ImageSave();
            }

            return _instance;
        }


        public async Task ImageSaveAsync(Cam cam)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FileContext>();
            optionsBuilder.UseSqlServer(connection);
            
            FileContext context = new FileContext(optionsBuilder.Options);
            
            if (context != null)
            {
                PhotoArchive photoArchive = new PhotoArchive();
                String imageUrl = cam.Url;
                photoArchive.Cam = cam.Name;
                photoArchive.Parent = cam.Parent;
                photoArchive.CreatedTimestamp = DateTime.Now;
                photoArchive.UpdatedTimestamp = DateTime.Now;
                photoArchive.Description = "";
                Random random = new Random();
                photoArchive.Code = Convert.ToString(random.Next(1000));

                try
                {
                    WebRequest req = WebRequest.Create(imageUrl);
                    req.Credentials = new NetworkCredential(cam.UserName, cam.Password);
                    WebResponse resp = req.GetResponse();
                    


                    string contentType = "";
                    if (resp != null)
                    {
                        Stream img = resp.GetResponseStream();
                        contentType = resp.ContentType;
                        Console.WriteLine(" Content type: " + contentType);
                        //img.Save("prova.jpg");
                        MemoryStream ms = new MemoryStream();

                        using (var bitmap = new Bitmap(img))
                        {
                            //draw the image object using a Graphics object
                            Graphics graphicsImage = Graphics.FromImage(bitmap);

                            //Set the alignment based on the coordinates   
                            StringFormat stringformat = new StringFormat();
                            stringformat.Alignment = StringAlignment.Near;
                            stringformat.LineAlignment = StringAlignment.Near;

                            StringFormat stringformat2 = new StringFormat();
                            stringformat2.Alignment = StringAlignment.Near;
                            stringformat2.LineAlignment = StringAlignment.Near;

                            //Set the font color/format/size etc..  
                            Color StringColor = System.Drawing.ColorTranslator.FromHtml("#933eea");//direct color adding
                            //Color StringColor2 = System.Drawing.ColorTranslator.FromHtml("#e80c88");//customise color adding
                            string Str_TextOnImage = photoArchive.Cam+" "+ photoArchive.Code;//Your Text On Image
                            //string Str_TextOnImage2 = photoArchive.Code;//Your Text On Image
                            var misura_font = (int)(bitmap.Width * 0.01)>10?(int)(bitmap.Width * 0.01):10;
                            var font = new Font("arial", misura_font, FontStyle.Regular);
                            SizeF misuratesto = graphicsImage.MeasureString(Str_TextOnImage, font);
                            SolidBrush opaqueBrush = new SolidBrush(Color.FromArgb(128, 255, 255, 255));
                            var x = (int)(bitmap.Width*0.01);
                            var y = (int)(bitmap.Height * 0.1);

                            graphicsImage.FillRectangle(opaqueBrush, new Rectangle(x, y, (int)Math.Round(misuratesto.Width), (int)Math.Round(misuratesto.Height)));
                            graphicsImage.DrawString(Str_TextOnImage, font, new SolidBrush(StringColor), new Point(x, y), stringformat);
                            //Response.ContentType = "image/jpeg";

                            //graphicsImage.DrawString(Str_TextOnImage2, new Font("Edwardian Script ITC", 10, FontStyle.Bold), new SolidBrush(StringColor2), new Point(10, 100), stringformat2);

                            //Response.ContentType = "image/jpeg";

                            bitmap.Save(ms, ImageFormat.Jpeg);
                        }

                        

                        //img.CopyTo(ms);
                        byte[] bytes = ms.ToArray();
                        photoArchive.File = bytes;
                        photoArchive.ContentType = contentType;



                        context.Add(photoArchive);
                        Console.WriteLine("acquisisco cam: " + cam.Name);
                        await context.SaveChangesAsync();
                    }
                }
                catch
                {
                    Console.WriteLine("impossibile acquisire cam: " + cam.Name);
                }


                await context.DisposeAsync();
            }
            
        }
    }
}
