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
    public class YayineviController : Controller
    {
        public IYayineviServis _yayineviServis;

        public YayineviController(IYayineviServis yayineviServis)
        {
            _yayineviServis = yayineviServis;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> YayineviGetir()
        {
            var sonuc = await _yayineviServis.YayinevleriGetir();
            return Json(sonuc);
        }

        public IActionResult YayineviKayit(YayineviKayitViewModel model)
        {
            if (model.YayineviKey != null)
            {
                return PartialView("~/Views/Yayinevi/_YayineviKayitPartial.cshtml", model);
            }

            return PartialView("~/Views/Yayinevi/_YayineviKayitPartial.cshtml");
        }

        public async Task<SonucModel<YayineviKayitViewModel>> YayineviKaydet(YayineviKayitViewModel model)
        {
            if (model.Ad != null)
            {
                model.Ad = Helper.ToTitleCase(model.Ad);
            }
            var sonuc = await _yayineviServis.YayineviKaydetGuncelle(model);
            return sonuc;
        }

        public async Task<SonucModel<YayineviSilViewModel>> YayineviSil(YayineviSilViewModel model)
        {
            var sonuc = await _yayineviServis.YayineviSil(model);
            return sonuc;
        }
    }
}