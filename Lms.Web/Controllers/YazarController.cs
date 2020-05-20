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
    public class YazarController : Controller
    {
        public IYazarServis _yazarServis;

        public YazarController(IYazarServis yazarServis)
        {
            _yazarServis = yazarServis;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> YazarlariGetir()
        {
            var sonuc = await _yazarServis.YazarlariGetir();
            return Json(sonuc);
        }

        public IActionResult YazarKayit(YazarKayitViewModel model)
        {
            if (model.YazarKey != null)
            {
                return PartialView("~/Views/Yazar/_YazarKayitPartial.cshtml", model);
            }

            return PartialView("~/Views/Yazar/_YazarKayitPartial.cshtml");
        }

        public async Task<SonucModel<YazarKayitViewModel>> YazarKaydet(YazarKayitViewModel model)
        {
            if (model.Ad != null)
            {
                model.Ad = Helper.ToTitleCase(model.Ad);
            }
            var sonuc = await _yazarServis.YazarKaydetGuncelle(model);
            return sonuc;
        }

        public async Task<SonucModel<YazarSilViewModel>> YazarSil(YazarSilViewModel model)
        {
            var sonuc = await _yazarServis.YazarSil(model);
            return sonuc;
        }
    }
}