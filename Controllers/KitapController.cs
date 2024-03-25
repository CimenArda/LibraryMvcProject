using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using WebProjesi1.Models;
using WebProjesi1.Utility;

namespace WebProjesi1.Controllers
{
    
    public class KitapController : Controller
    {
        private readonly IKitapRepository _kitapRepository;
        private readonly IKitapTuruRepository _kitapTuruRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KitapController(IKitapRepository kitapRepository, IKitapTuruRepository kitapTuruRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kitapRepository = kitapRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles ="Admin,Ogrenci")]
        public IActionResult Index(string ara)
        {

            // List<Kitap> objKitapList = _kitapRepository.GetAll().ToList();
            List<Kitap> objKitapList = _kitapRepository.GetAll(includeProps:"KitapTuru").ToList();
            if (!string.IsNullOrEmpty(ara))
            {
                objKitapList = objKitapList.Where(x => x.KitapAdi.ToLower().Contains(ara)).ToList();

            }

            return View(objKitapList);
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.GetAll().Select(k => new SelectListItem
            {

                Text = k.Ad,
                Value = k.Id.ToString()
            });

            ViewBag.KitapTuruList = KitapTuruList;
            //controllerdan viewa veri aktarır
            if(id == null ||id==0)
            {
                //ekleme kısmı
                return View();
            }
            else
            {
                //guncelleme
                Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id);//(Expression<Func<T, bool>> filtre)
                if (kitapVt == null)
                {
                    return NotFound();
                }
                return View(kitapVt);
            }
            
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(Kitap kitap, IFormFile? file )
        {
            //modelstate kaynaklı hatayı yakalamak icin kullanılıyor 
           // var errors = ModelState.Values.SelectMany(x => x.Errors);   

            if(ModelState.IsValid)  
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string kitapPath = Path.Combine(wwwRootPath, @"img");

                if(file!= null)
                {

              
                using (var fileStream = new FileStream(Path.Combine(kitapPath,file.FileName ),FileMode.Create))
                {
                    file.CopyTo(fileStream);

                }
                kitap.ResimUrl = @"\img\" + file.FileName;
                }

                if (kitap.Id == 0)
                {
                    _kitapRepository.Ekle(kitap);
                    TempData["basarili"] = "Yeni Kitap Başarıyla Oluşturuldu!";
                }
                else
                {
                    _kitapRepository.Guncelle(kitap);
                    TempData["basarili"] = " Kitap Güncelleme Tamamlandı!";
                }
                
                _kitapRepository.Kaydet();
                
                return RedirectToAction("Index", "Kitap");

            }
            return View();
           
        }
        /*
        public IActionResult Guncelle(int? id)
        {
            if (id == null || id==0)
            {
                return NotFound();
            }
           
        }*/

        /*
        [HttpPost]
        public IActionResult Guncelle(Kitap kitap)
        {
            if (ModelState.IsValid)
            {

                _kitapRepository.Guncelle(kitap);
                _kitapRepository.Kaydet();
                TempData["basarili"] = "Yeni Kitap  Başarıyla Güncellendi!";
                return RedirectToAction("Index", "Kitap");

            }
            return View();

        }*/
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kitap? kitapVt = _kitapRepository.Get(u => u.Id == id); ;
            if (kitapVt == null)
            {
                return NotFound();
            }
            return View(kitapVt);
        }


        [HttpPost,ActionName("Sil")]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult SilPOST(int? id)
        {

            Kitap? kitap = _kitapRepository.Get(u => u.Id == id); ;
            if(kitap == null)
            {
                return NotFound();
            }
            _kitapRepository.Sil(kitap);
            _kitapRepository.Kaydet();
            TempData["basarili"] = "Kitap Başarıyla Silindi!";
            return RedirectToAction("Index", "Kitap");

        }





    }
}
