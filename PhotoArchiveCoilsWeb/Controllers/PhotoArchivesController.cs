using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhotoArchiveCoilsWeb.Data;
using PhotoArchiveCoilsWeb.Models;
using System.Net;
using Microsoft.Extensions.Options;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace PhotoArchiveCoilsWeb.Controllers
{
    public class PhotoArchivesController : Controller
    {
        private readonly FileContext _context;
        private readonly IOptions<ApplicationConfiguration> _optionsApplicationConfiguration;



        public PhotoArchivesController(FileContext context, IOptions<ApplicationConfiguration> o, IConfiguration config)
        {
            _context = context;
            _optionsApplicationConfiguration = o;

        }


        public IActionResult IndexCams()
        {
            //for (int i = 0; i < _optionsApplicationConfiguration.Value.Cams.Length; i++)
            //{
            //    if (ImageSave.Instance().connection != null)
            //    {
            //        //await ImageSave.Instance().ImageSaveAsync(_optionsApplicationConfiguration.Value.Cams[i]);

            //    }
            //}

            return View(_optionsApplicationConfiguration.Value.Cams);
        }

        public async Task<IEnumerable<PhotoArchiveShort>> GetAllFiles()
        {
            //var items = _context.PhotoArchives.OrderByDescending(u => u.CreatedTimestamp).Take(2);
            //_context.PhotoArchives.Select

            return await _context.PhotoArchives.OrderByDescending(u => u.CreatedTimestamp).Take(100).Select(
                    photoArchive => new PhotoArchiveShort
                    {
                        Id = photoArchive.Id,
                        Parent = photoArchive.Parent,
                        Cam = photoArchive.Cam,
                        Code = photoArchive.Code,
                        Description = photoArchive.Description,
                        CreatedTimestamp = photoArchive.CreatedTimestamp,
                        UpdatedTimestamp = photoArchive.UpdatedTimestamp
                    }).ToListAsync();



        }

        // GET: PhotoArchives
        public async Task<IActionResult> Index()
        {
            // return View(await _context.PhotoArchives.ToListAsync());

            //var model = new AllUploadedFiles { PhotoArchiveShorts = GetAllFiles().ToList() };
            //return View(model);
            //return View(GetAllFiles().ToList());
            //return View(await _context.PhotoArchivesShort.ToListAsync());
            return View(await GetAllFiles());
        }

        public FileResult GetFileFromBytes(byte[] bytesIn)
        {
            return File(bytesIn, "image/jpg");
        }

        [HttpGet]
        public async Task<IActionResult> GetUserImageFile(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photoArchive = await _context.PhotoArchives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photoArchive == null)
            {
                return NotFound();
            }
            /*
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return null;
            }
            */
            //FileResult imageUserFile = GetFileFromBytes(photoArchive.File);
            //return imageUserFile;
            return File(photoArchive.File, photoArchive.ContentType);
        }
        public IActionResult GetLoadImage(String id)
        {
            //Console.WriteLine("cerco cam con url "+ HttpUtility.UrlDecode(id));
            var cam = new Cam();
            for (int i = 0; i < _optionsApplicationConfiguration.Value.Cams.Length; i++)
            {
                //Console.WriteLine("controllo cam con url " + _optionsApplicationConfiguration.Value.Cams[i].Url);
                if (_optionsApplicationConfiguration.Value.Cams[i].Url == HttpUtility.UrlDecode(id))
                {
                    cam = _optionsApplicationConfiguration.Value.Cams[i];
                    break;
                }
            }

            if (cam != null)
            {
                WebRequest req = WebRequest.Create(cam.Url);
                req.Credentials = new NetworkCredential(cam.UserName, cam.Password);
                WebResponse resp = req.GetResponse();
                string contentType = "";
                if (resp != null)
                {
                    Stream img = resp.GetResponseStream();
                    contentType = resp.ContentType;
                    return File(img, contentType);
                }
            }
            return null;
        }

        // GET: PhotoArchives/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photoArchive = await _context.PhotoArchives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photoArchive == null)
            {
                return NotFound();
            }
            PhotoArchiveShort photoArchiveShort = new PhotoArchiveShort();
            photoArchiveShort.Id = photoArchive.Id;
            photoArchiveShort.Parent = photoArchive.Parent;
            photoArchiveShort.Cam = photoArchive.Cam;
            photoArchiveShort.Code = photoArchive.Code;
            photoArchiveShort.Description = photoArchive.Description;
            photoArchiveShort.CreatedTimestamp = photoArchive.CreatedTimestamp;
            photoArchiveShort.UpdatedTimestamp = photoArchive.UpdatedTimestamp;
            return View(photoArchiveShort);
        }

        // GET: PhotoArchives/DetailsCode/5
        [HttpPost]
        public async Task<IActionResult> DetailsCode(string Code)
        {
            Console.WriteLine("Details code: " + Code);
            if (Code == null)
            {
                return NotFound();
            }

            var photoArchive = await _context.PhotoArchives
                .FirstOrDefaultAsync(m => m.Code == Code);
            if (photoArchive == null)
            {
                return NotFound();
            }
            PhotoArchiveShort photoArchiveShort = new PhotoArchiveShort();
            photoArchiveShort.Id = photoArchive.Id;
            photoArchiveShort.Parent = photoArchive.Parent;
            photoArchiveShort.Cam = photoArchive.Cam;
            photoArchiveShort.Code = photoArchive.Code;
            photoArchiveShort.Description = photoArchive.Description;
            photoArchiveShort.CreatedTimestamp = photoArchive.CreatedTimestamp;
            photoArchiveShort.UpdatedTimestamp = photoArchive.UpdatedTimestamp;
            //return View(photoArchiveShort);
            return View("Details", photoArchiveShort);
        }

        // GET: PhotoArchives/Create
        public async Task<IActionResult> Create()
        {
            Console.WriteLine("Numero di telecamere configurate: " + _optionsApplicationConfiguration.Value.Cams.Length);

            for (int i = 0; i < _optionsApplicationConfiguration.Value.Cams.Length; i++)
            {

                await ImageSave.Instance().ImageSaveAsync(_optionsApplicationConfiguration.Value.Cams[i]);


            }
            //return View();
            return RedirectToAction(nameof(Index));
        }

        /*
                // POST: PhotoArchives/Create
                // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
                // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("Id,Cam,File,Code,Parent,Description,CreatedTimestamp,UpdatedTimestamp")] PhotoArchive photoArchive)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(photoArchive);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    return View(photoArchive);
                }
                */

        // GET: PhotoArchives/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photoArchive = await _context.PhotoArchives.FindAsync(id);
            if (photoArchive == null)
            {
                return NotFound();
            }

            //return View(photoArchive);
            PhotoArchiveEdit photoArchiveEdit = new PhotoArchiveEdit();
            photoArchiveEdit.Id = photoArchive.Id;
            photoArchiveEdit.Description = photoArchive.Description;

            return View(photoArchiveEdit);
        }

        // POST: PhotoArchives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(Guid id, [Bind("Id,Description")] PhotoArchiveEdit photoArchiveEdit)
        {
            if (id != photoArchiveEdit.Id)
            {
                return NotFound();
            }
            /*
            PhotoArchive photoArchive2 = new PhotoArchive();
            photoArchive2 = await _context.PhotoArchives.Where(p => p.Id == id).AsNoTracking().ToList;
            photoArchive.File = photoArchive2.File;
            photoArchive.CreatedTimestamp = photoArchive2.CreatedTimestamp;
            */



            if (ModelState.IsValid)
            {

                PhotoArchive photoArchive = new PhotoArchive();


                try
                {
                    photoArchive.Id = photoArchiveEdit.Id;
                    photoArchive.Description = photoArchiveEdit.Description;
                    photoArchive.UpdatedTimestamp = DateTime.Now;
                    _context.PhotoArchives.Attach(photoArchive);
                    _context.Entry(photoArchive).Property(x => x.Description).IsModified = true;
                    //_context.SaveChanges();
                    //_context.Update(photoArchive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoArchiveExists(photoArchive.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(photoArchiveEdit);
        }


        /*
        // GET: PhotoArchives/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photoArchive = await _context.PhotoArchives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photoArchive == null)
            {
                return NotFound();
            }

            return View(photoArchive);
        }
        

        // POST: PhotoArchives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photoArchive = await _context.PhotoArchives.FindAsync(id);
            _context.PhotoArchives.Remove(photoArchive);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        private bool PhotoArchiveExists(Guid id)
        {
            return _context.PhotoArchives.Any(e => e.Id == id);
        }
    }
}
