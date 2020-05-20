using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kys.Web.Business;
using Kys.Web.Helpers;
using Kys.Web.Models;
using Kys.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kys.Web.Controllers
{
    public class UyeController : Controller
    {
        public IUyeServis _uyeServis;

        public UyeController(IUyeServis uyeServis)
        {
            _uyeServis = uyeServis;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> UyeleriGetir(UyeAramaViewModel model)
        {
            var sonuc = await _uyeServis.UyeVeyaUyeleriGetir(model);
            return Json(sonuc);
        }

        public IActionResult UyeKayit(UyeKayitViewModel model)
        {
            if (model != null)
            {
                return PartialView("~/Views/Uye/_UyeKayitPartial.cshtml", model);
            }

            return PartialView("~/Views/Uye/_UyeKayitPartial.cshtml");
        }

        public async Task<SonucModel<UyeKayitViewModel>> UyeKaydet(UyeKayitViewModel model)
        {
            if (model!= null)
            {
                model.Ad = Helper.ToTitleCase(model.Ad);
            }
            var sonuc = await _uyeServis.UyeKaydetGuncelle(model);
            return sonuc;
        }

        public async Task<IActionResult> UyeDetayGetir(int uyeKey)
        {
            var sonuc = await _uyeServis.UyeDetayGetir(uyeKey);

            return PartialView("~/Views/Uye/_UyeDetayPartial.cshtml",sonuc);
        }

        public async Task<IActionResult> OduncVer(int demirbasNo, int uyeKey)
        {
            OduncVerViewModel model = new OduncVerViewModel()
            {
                DemirbasNo = demirbasNo,
                UyeKey = uyeKey
            };
           
            var sonuc = await _uyeServis.OduncVer(model);
            return Json(sonuc);
        }

        public async Task<IActionResult> IadeAl(int oduncKey)
        {
            IadeAlViewModel model = new IadeAlViewModel()
            {
                OduncKey = oduncKey
            };

            var sonuc = await _uyeServis.IadeAl(model);
            return Json(sonuc);
        }

        public async Task<SonucModel<UyeKayitSilViewModel>> UyeSil(UyeKayitSilViewModel model)
        {
            var sonuc = await _uyeServis.UyeSil(model);
            return sonuc;
        }

    }
}
