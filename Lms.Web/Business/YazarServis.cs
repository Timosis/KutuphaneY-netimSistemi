using Kys.Web.Models;
using Kys.Web.Repository;
using Kys.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Business
{
    public interface IYazarServis
    {
        Task<List<YazarViewModel>> YazarlariGetir();
        Task<SonucModel<YazarKayitViewModel>> YazarKaydetGuncelle(YazarKayitViewModel model);
        Task<SonucModel<YazarSilViewModel>> YazarSil(YazarSilViewModel model);
    }


    public class YazarServis : IYazarServis
    {
        private readonly IYazarRepository _yazarRepository;

        public YazarServis(IYazarRepository yazarRepository)
        {
            _yazarRepository = yazarRepository;
        }

        public async Task<SonucModel<YazarKayitViewModel>> YazarKaydetGuncelle(YazarKayitViewModel model)
        {
            var sonuc = new SonucModel<YazarKayitViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Kategori Kaydetme İşlemi Gerçekleştirilmedi."
            };

            if (model.YazarKey > 0)
            {
                var guncellenecekYazar = await _yazarRepository.GetByIdAsync(model.YazarKey);                
                guncellenecekYazar.Ad = model.Ad;
                guncellenecekYazar.YazarHakkinda = model.Hakkinda;
                var guncellemeSonuc = await _yazarRepository.UpdateAsync(guncellenecekYazar);
                if (guncellemeSonuc > 0)
                {
                    sonuc.HataMesaji = $"Yazar Bilgileri Güncellenmiştir.";
                    sonuc.Tip = SonucTip.Basarili;
                    sonuc.Data = model;
                    return sonuc;
                }
            }
            else
            {
                Yazar yazar = new Yazar()
                {
                    Ad = model.Ad,
                    YazarHakkinda = model.Hakkinda
                };

                var eklemeSonuc = await _yazarRepository.InsertAsync(yazar);
                var eklenenYazar = await _yazarRepository.FindAsync(x => x.Ad == model.Ad);

                if (eklemeSonuc > 0)
                {
                    sonuc.HataMesaji = $"{model.Ad} Yazar Olarak Eklenmiştir.";
                    sonuc.Tip = SonucTip.Basarili;
                    sonuc.Data = new YazarKayitViewModel()
                    {
                        YazarKey = eklenenYazar.YazarKey,
                        Ad = eklenenYazar.Ad,
                        Hakkinda = eklenenYazar.YazarHakkinda
                    };

                    return sonuc;
                }
            }

            return sonuc;
        }

        public async Task<List<YazarViewModel>> YazarlariGetir()
        {
            List<YazarViewModel> sonuc = new List<YazarViewModel>();

            var yazarlar = await _yazarRepository.GetAllAsync();

            yazarlar.ToList().ForEach(k =>
            {
                var yazar = new YazarViewModel()
                {
                    YazarKey = k.YazarKey,
                    Ad = k.Ad,
                    Hakkinda = k.YazarHakkinda
                    
                };
                sonuc.Add(yazar);
            });

            return sonuc;
        }

        public async Task<SonucModel<YazarSilViewModel>> YazarSil(YazarSilViewModel model)
        {
            var sonuc = new SonucModel<YazarSilViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Yazar Silme İşlemi Gerçekleştirilmedi."
            };

            Yazar silenecekYazar = new Yazar()
            {
                YazarKey = model.YazarKey,
                Ad = model.Ad,
                YazarHakkinda = model.YazarHakkinda
            };

            var silmeSonuc = await _yazarRepository.DeleteAsync(silenecekYazar);

            if (silmeSonuc > 0)
            {
                sonuc.Tip = SonucTip.Basarili;
                sonuc.HataMesaji = "Silme İşlemi Başarı İle Gerçekleşti";
                sonuc.Data = model;
                return sonuc;
            }
            return sonuc;
        }
    }
}
