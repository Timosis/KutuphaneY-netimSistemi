using Kys.Web.Models;
using Kys.Web.Repository;
using Kys.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Business
{
    public interface IYayineviServis
    {
        Task<List<YayineviViewModel>> YayinevleriGetir();
        Task<SonucModel<YayineviKayitViewModel>> YayineviKaydetGuncelle(YayineviKayitViewModel model);
        Task<SonucModel<YayineviSilViewModel>> YayineviSil(YayineviSilViewModel model);
    }


    public class YayineviServis : IYayineviServis
    {
        private readonly IYayineviRepository _yayineviRepository;
        public YayineviServis(IYayineviRepository yayineviRepository)
        {
            _yayineviRepository = yayineviRepository;
        }

        public async Task<SonucModel<YayineviKayitViewModel>> YayineviKaydetGuncelle(YayineviKayitViewModel model)
        {
            var sonuc = new SonucModel<YayineviKayitViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Yayinevi Kaydetme İşlemi Gerçekleştirilmedi."
            };

            if (model.YayineviKey > 0)
            {
                var guncellenecekYayinevi = await _yayineviRepository.GetByIdAsync(model.YayineviKey);
                string eskiAd = guncellenecekYayinevi.Ad;
                guncellenecekYayinevi.Ad = model.Ad;
                var guncellemeSonuc = await _yayineviRepository.UpdateAsync(guncellenecekYayinevi);
                if (guncellemeSonuc > 0)
                {
                    sonuc.HataMesaji = $"{eskiAd} Yayinevi, {model.Ad} Olarak Güncellenmiştir.";
                    sonuc.Tip = SonucTip.Basarili;
                    sonuc.Data = model;
                    return sonuc;
                }
            }
            else
            {
                Yayinevi kategori = new Yayinevi()
                {
                    Ad = model.Ad
                };

                var eklemeSonuc = await _yayineviRepository.InsertAsync(kategori);
                var eklenenYayinevi = await _yayineviRepository.FindAsync(x => x.Ad == model.Ad);

                if (eklemeSonuc > 0)
                {
                    sonuc.HataMesaji = $"{model.Ad} Kategori Olarak Eklenmiştir.";
                    sonuc.Tip = SonucTip.Basarili;
                    sonuc.Data = new YayineviKayitViewModel()
                    {
                        YayineviKey = eklenenYayinevi.YayineviKey,
                        Ad = eklenenYayinevi.Ad
                    };

                    return sonuc;
                }
            }

            return sonuc;
        }

        public async Task<SonucModel<YayineviSilViewModel>> YayineviSil(YayineviSilViewModel model)
        {
            var sonuc = new SonucModel<YayineviSilViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Yayınevi Silme İşlemi Gerçekleştirilemedi."
            };

            Yayinevi silenecekYayinevi = new Yayinevi()
            {
                YayineviKey = model.YayineviKey,
                Ad = model.Ad
            };

            var silmeSonuc = await _yayineviRepository.DeleteAsync(silenecekYayinevi);

            if (silmeSonuc > 0)
            {
                sonuc.Tip = SonucTip.Basarili;
                sonuc.HataMesaji = "Silme İşlemi Başarı İle Gerçekleşti";
                sonuc.Data = model;
                return sonuc;
            }
            return sonuc;
        }

        public async Task<List<YayineviViewModel>> YayinevleriGetir()
        {
            List<YayineviViewModel> sonuc = new List<YayineviViewModel>();

            var kategoriler = await _yayineviRepository.GetAllAsync();

            kategoriler.ToList().ForEach(k =>
            {
                var yayinevi = new YayineviViewModel()
                {
                    YayineviKey = k.YayineviKey,
                    Ad = k.Ad
                };
                sonuc.Add(yayinevi);
            });

            return sonuc;
        }
    }
}
