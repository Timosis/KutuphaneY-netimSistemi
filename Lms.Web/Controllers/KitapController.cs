using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kys.Web.Business;
using Kys.Web.Helpers;
using Kys.Web.Models;
using Kys.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lms.Web.Controllers
{
    public class KitapController : Controller
    {
        public IKitapServis _kitapServis;
        public IYayineviServis _yayineviServis;
        public IYazarServis _yazarServis;

        public KitapController(IKitapServis kitapServis, IYayineviServis yayineviServis, IYazarServis yazarServis)
        {
            _kitapServis = kitapServis;
            _yazarServis = yazarServis;
            _yayineviServis = yayineviServis;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.Yazarlar = "";
            ViewBag.Yayinevleri = "";

            return View();
        }

        public async Task<JsonResult> KitaplariGetir(KitapAramaViewModel model)
        {
            var sonuc = await _kitapServis.KitaplariVeyaKitapGetir(model);
            return Json(sonuc);
        }

        public async Task<IActionResult> KitapKayit(KitapKayitViewModel model)
        {
            var yayinevleri = await _yayineviServis.YayinevleriGetir();
            var yazarlar = await _yazarServis.YazarlariGetir();
            ViewBag.yazarlar = yazarlar;
            ViewBag.yayinevleri = yayinevleri;

            if (model.KitapKey.HasValue)
            {
                try
                {
                    return PartialView("~/Views/Kitap/_KitapKayitPartial.cshtml", model);
                }
                catch (Exception ex)
                {

                    throw ex;
                }                
            }

            return PartialView("~/Views/Kitap/_KitapKayitPartial.cshtml");
        }

        public async Task<IActionResult> KitapKayitGuncelle(KitapKayitGuncelleViewModel model)
        {
            var yayinevleri = await _yayineviServis.YayinevleriGetir();
            var yazarlar = await _yazarServis.YazarlariGetir();
            ViewBag.yazarlar = yazarlar;
            ViewBag.yayinevleri = yayinevleri;

            var kitap = await _kitapServis.KitapGetir(model);
            return PartialView("~/Views/Kitap/_KitapKayitGuncellePartial.cshtml",kitap.Data);
        }

        public async Task<SonucModel<KitapKayitViewModel>> KitapKaydet(KitapKayitViewModel model)
        {
            if (model != null)
            {
                model.Ad = Helper.ToTitleCase(model.Ad);
            }
            var sonuc = await _kitapServis.KitapKaydetGuncelle(model);
            return sonuc;
        }

        public async Task<SonucModel<KitapKayitSilViewModel>> KitapSil(KitapKayitSilViewModel model)
        {
            var sonuc = await _kitapServis.KitapSil(model);
            return sonuc;
        }
    }
}
