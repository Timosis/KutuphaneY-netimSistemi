using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kys.Web.Business;
using Kys.Web.Helpers;
using Kys.Web.Models;
using Kys.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lms.Web.Controllers
{
    public class KategoriController : Controller
    {
        public IKategoriServis _kategoriServis;

        public KategoriController(IKategoriServis kategoriServis)
        {
            _kategoriServis = kategoriServis;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> KategorileriGetir()
        {
            var sonuc = await _kategoriServis.KategorileriGetir();
            return Json(sonuc);
        }

        public IActionResult KategoriKayit(KategoriKayitViewModel model)
        {
            if (model != null)
            {
                return PartialView("~/Views/Kategori/_KategoriKayitPartial.cshtml",model);
            }

            return PartialView("~/Views/Kategori/_KategoriKayitPartial.cshtml");
        }

        public async Task<SonucModel<KategoriKayitViewModel>> KategoriKaydet(KategoriKayitViewModel model)
        {
            if (model.Ad != null)
            {
                model.Ad = Helper.ToTitleCase(model.Ad);
            }            
            var sonuc = await _kategoriServis.KategoriKaydetGuncelle(model);
            return sonuc;
        }

        public async Task<SonucModel<KategoriSilViewModel>> KategoriSil(KategoriSilViewModel model)
        {           
            var sonuc = await _kategoriServis.KategoriSil(model);
            return sonuc;
        }

    }
}