using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using WebProjesi1.Models;
using WebProjesi1.Utility;

namespace WebProjesi1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KiralamaController : Controller
    {
        private readonly IKiralamaRepository _kiralamaRepository;
        private readonly IKitapRepository _kitapRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KiralamaController(IKiralamaRepository kiralamaRepository, IKitapRepository kitapRepository, IWebHostEnvironment webHostEnvironment)
        {
            _kiralamaRepository = kiralamaRepository;
            _kitapRepository = kitapRepository;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {

           
            List<Kiralama> objKiralamaList = _kiralamaRepository.GetAll(includeProps:"Kitap").ToList();

            return View(objKiralamaList);
        }

        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll().Select(k => new SelectListItem
            {

                Text = k.KitapAdi,
                Value = k.Id.ToString()
            });

            ViewBag.KitapList = KitapList;
            //controllerdan viewa veri aktarır
            if(id == null ||id==0)
            {
                //ekleme kısmı
                return View();
            }
            else
            {
                //guncelleme
                Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id);//(Expression<Func<T, bool>> filtre)
                if (kiralamaVt == null)
                {
                    return NotFound();
                }
                return View(kiralamaVt);
            }
            
        }
        //burda
        [HttpPost]
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string kitapPath = Path.Combine(wwwRootPath, @"img");

                if (kiralama.Id == 0)
                {
                    _kiralamaRepository.Ekle(kiralama);

                    // Stoktan düşürme yapılışı dikkat
                    var kitap = _kitapRepository.Get(u => u.Id == kiralama.KitapId);
                    if (kitap != null && kitap.StockQuantity >= kiralama.kitapadedi)
                    {
                        kitap.StockQuantity -= kiralama.kitapadedi;
                        _kitapRepository.Guncelle(kitap);
                        TempData["basarili"] = "Yeni Kiralama Kaydı Başarıyla Oluşturuldu!";
                    }
                  
                }
                else
                {
                    _kiralamaRepository.Guncelle(kiralama);

                    
                    var kitap = _kitapRepository.Get(u => u.Id == kiralama.KitapId);
                    if (kitap != null && kitap.StockQuantity >= kiralama.kitapadedi)
                    {
                        kitap.StockQuantity -= kiralama.kitapadedi;
                        _kitapRepository.Guncelle(kitap);
                        TempData["basarili"] = " Kiralama Kaydı Güncelleme Tamamlandı!";
                    }
                  
                }

                _kiralamaRepository.Kaydet();

                return RedirectToAction("Index", "Kiralama");
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

        public IActionResult Sil(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepository.GetAll().Select(k => new SelectListItem
            {

                Text = k.KitapAdi,
                Value = k.Id.ToString()
            });

            ViewBag.KitapList = KitapList;

            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id); ;
            if (kiralamaVt == null)
            {
                return NotFound();
            }
            return View(kiralamaVt);
        }


        [HttpPost,ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {

            Kiralama kiralama = _kiralamaRepository.Get(u => u.Id == id); ;
            if(kiralama == null)
            {
                return NotFound();
            }
            _kiralamaRepository.Sil(kiralama);
            _kiralamaRepository.Kaydet();
            TempData["basarili"] = " Kiralama Kaydı Başarıyla Silindi!";
            return RedirectToAction("Index", "Kiralama");

        }





    }
}
