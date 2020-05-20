using Kys.Web.Models;
using Kys.Web.Repository;
using Kys.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Business
{
    public interface IKategoriServis
    {
        Task<IEnumerable<KategoriViewModel>> KategorileriGetir();
        Task<SonucModel<KategoriKayitViewModel>> KategoriKaydetGuncelle(KategoriKayitViewModel model);
        Task<SonucModel<KategoriSilViewModel>> KategoriSil(KategoriSilViewModel model);
    }


    public class KategoriServis : IKategoriServis
    {
        private readonly IKategoriRepository _kategoriRepository;

        public KategoriServis(IKategoriRepository kategoriRepository)
        {
            _kategoriRepository = kategoriRepository;
        }

        public async Task<SonucModel<KategoriKayitViewModel>> KategoriKaydetGuncelle(KategoriKayitViewModel model)
        {
            var sonuc = new SonucModel<KategoriKayitViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Kategori Kaydetme İşlemi Gerçekleştirilmedi."
            };

            if (model.KategoriKey > 0 )
            {
                var guncellenecekKategori = await _kategoriRepository.GetByIdAsync(model.KategoriKey);
                string eskiAd = guncellenecekKategori.Ad;
                guncellenecekKategori.Ad = model.Ad;
                var guncellemeSonuc = await _kategoriRepository.UpdateAsync(guncellenecekKategori);
                if (guncellemeSonuc> 0)
                {
                    sonuc.HataMesaji = $"{eskiAd} Kategorisi, {model.Ad} Olarak Güncellenmiştir.";
                    sonuc.Tip = SonucTip.Basarili;
                    sonuc.Data = model;
                    return sonuc;
                }
            }
            else
            {
                Kategori kategori = new Kategori()
                {
                    Ad = model.Ad
                };

                var eklemeSonuc = await _kategoriRepository.InsertAsync(kategori);
                var eklenenKategori = await _kategoriRepository.FindAsync(x => x.Ad == model.Ad);

                if (eklemeSonuc > 0)
                {
                    sonuc.HataMesaji = $"{model.Ad} Kategori Olarak Eklenmiştir.";
                    sonuc.Tip = SonucTip.Basarili;
                    sonuc.Data = new KategoriKayitViewModel()
                    {
                        KategoriKey = eklenenKategori.KategoriKey,
                        Ad = eklenenKategori.Ad
                    };

                    return sonuc;
                }
            }

            return sonuc;            
        }

        public async Task<IEnumerable<KategoriViewModel>> KategorileriGetir()
        {
            List<KategoriViewModel> sonuc = new List<KategoriViewModel>();

            var kategoriler = await _kategoriRepository.GetAllAsync();

            kategoriler.ToList().ForEach(k =>
            {
                var kategori = new KategoriViewModel()
                {
                    KategoriKey = k.KategoriKey,
                    Ad = k.Ad
                };
                sonuc.Add(kategori);
            });

            return sonuc;
        }

        public async Task<SonucModel<KategoriSilViewModel>> KategoriSil(KategoriSilViewModel model)
        {
            var sonuc = new SonucModel<KategoriSilViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Kategori Kaydetme İşlemi Gerçekleştirilmedi."
            };

            Kategori silenecekKategori = new Kategori()
            {
                KategoriKey = model.KategoriKey,
                Ad = model.Ad
            };

            var silmeSonuc = await _kategoriRepository.DeleteAsync(silenecekKategori);

            if (silmeSonuc >  0)
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
