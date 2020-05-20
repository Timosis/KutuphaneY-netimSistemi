using Kys.Web.Models;
using Kys.Web.Repository;
using Kys.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Kys.Web.Business
{
    public interface IKitapServis
    {
        Task<IEnumerable<KitapAramaSonucViewModel>> KitaplariVeyaKitapGetir(KitapAramaViewModel model);
        Task<SonucModel<KitapKayitViewModel>> KitapKaydetGuncelle(KitapKayitViewModel model);
        Task<SonucModel<KitapKayitViewModel>> KitapGetir(KitapKayitGuncelleViewModel model);

        Task<SonucModel<KitapKayitSilViewModel>> KitapSil(KitapKayitSilViewModel model);
    }

    public class KitapServis : IKitapServis
    {
        private readonly IKitapRepository _kitapRepository;
        private readonly IYazarRepository _yazarRepository;
        private readonly IYayineviRepository _yayineviRepository;
        public KitapServis(IKitapRepository kitapRepository, IYayineviRepository yayineviRepository, IYazarRepository yazarRepository)
        {
            _kitapRepository = kitapRepository;
            _yazarRepository = yazarRepository;
            _yayineviRepository = yayineviRepository;
        }
        public async Task<SonucModel<KitapKayitViewModel>> KitapKaydetGuncelle(KitapKayitViewModel model)
        {
            var sonuc = new SonucModel<KitapKayitViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Kitap Kaydetme İşlemi Gerçekleştirilmedi."
            };

            var yazar = await _yazarRepository.FindAsync(x => x.Ad == model.Yazar.Ad);
            var yayinevi = await _yayineviRepository.FindAsync(x => x.Ad == model.Yayinevi.Ad);

            // Eğer autocomplete'te olmayan bir yazar veya yayinevi girildiyse ekle.
            if (yazar == null)
            {
                yazar = new Yazar
                {
                    Ad = model.Yazar.Ad
                };
                await _yazarRepository.InsertAsync(yazar);
                yazar = await _yazarRepository.FindAsync(x => x.Ad == model.Yazar.Ad);
            }

            if (yayinevi == null)
            {
                yayinevi = new Yayinevi
                {
                    Ad = model.Yayinevi.Ad
                };
                await _yayineviRepository.InsertAsync(yayinevi);
                yayinevi = await _yayineviRepository.FindAsync(x => x.Ad == model.Yayinevi.Ad);
            }

            if (model.KitapKey > 0)
            {
                var guncellenecekKitap = await _kitapRepository.GetByIdAsync(model.KitapKey);
                guncellenecekKitap.Ad = model.Ad;
                guncellenecekKitap.Isbn = model.Isbn;
                guncellenecekKitap.YazarKey = yazar.YazarKey;
                guncellenecekKitap.YayineviKey = yayinevi.YayineviKey;
                guncellenecekKitap.KitapHakkindaOzet = model.KitapHakkindaOzet;
                guncellenecekKitap.SayfaSayisi = Convert.ToInt32(model.SayfaSayisi);
                var guncellemeSonuc = await _kitapRepository.UpdateAsync(guncellenecekKitap);
                if (guncellemeSonuc > 0)
                {
                    sonuc.HataMesaji = $"Kitap Bilgileri Güncellenmiştir.";
                    sonuc.Tip = SonucTip.Basarili;
                    sonuc.Data = model;
                    return sonuc;
                }
            }
            else
            {
                Kitap kitap = new Kitap()
                {
                    Ad = model.Ad,
                    KitapDurumKey = (int) KitapDurum.Kutuphanede,
                    Isbn = model.Isbn,
                    KitapHakkindaOzet = model.KitapHakkindaOzet,
                    SayfaSayisi = Convert.ToInt32(model.SayfaSayisi),
                    YazarKey = yazar.YazarKey,
                    YayineviKey = yayinevi.YayineviKey,
                };

                var eklemeSonuc = await _kitapRepository.InsertAsync(kitap);
                var eklenenKitap = await _kitapRepository.FindAsync(x => x.Ad == model.Ad);

                if (eklemeSonuc > 0)
                {
                    eklenenKitap.DemirbasNo = DemirbasNoUret(eklenenKitap.KitapKey);

                    var eklenenKitabiGuncelle = await _kitapRepository.UpdateAsync(eklenenKitap);

                    if (eklenenKitabiGuncelle > 0)
                    {
                        sonuc.HataMesaji = $"{model.Ad} Kitap Olarak Eklenmiştir.";
                        sonuc.Tip = SonucTip.Basarili;
                        sonuc.Data = new KitapKayitViewModel()
                        {
                            KitapKey = eklenenKitap.KitapKey,
                            Ad = eklenenKitap.Ad,
                            Isbn = eklenenKitap.Isbn,
                            SayfaSayisi = Convert.ToString(eklenenKitap.SayfaSayisi),
                            KitapHakkindaOzet = eklenenKitap.KitapHakkindaOzet,
                            KitapDurumKey = eklenenKitap.KitapDurumKey
                        };
                    }
                    
                    return sonuc;
                }
            }

            return sonuc;
        }
        public int DemirbasNoUret(int kitapKey)
        {
            return kitapKey + 100;
        }

        public async Task<SonucModel<KitapKayitViewModel>> KitapGetir(KitapKayitGuncelleViewModel model)
        {
            var sonuc = new SonucModel<KitapKayitViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Kitap Guncelleme İşlemi Gerçekleştirilmedi."
            };

            var kitap = _kitapRepository.SelectIncludeMany(y => y.Yazar, x => x.Yayinevi).Where(k => k.KitapKey == model.KitapKey).FirstOrDefault();

            if (kitap != null)
            {
                sonuc.Tip = SonucTip.Basarili;
                sonuc.HataMesaji = "Güncellenecek Kitap Bulundu";
                sonuc.Data = new KitapKayitViewModel
                {
                    Ad = kitap.Ad,
                    Isbn = kitap.Isbn,
                    KitapDurumKey = kitap.KitapDurumKey,
                    KitapHakkindaOzet = kitap.KitapHakkindaOzet,
                    KitapKey = kitap.KitapKey,
                    SayfaSayisi = Convert.ToString(kitap.SayfaSayisi),
                    Yayinevi = new YayineviViewModel
                    {
                        Ad = kitap.Yayinevi.Ad,
                        YayineviKey = kitap.Yayinevi.YayineviKey
                    },
                    Yazar = new YazarViewModel
                    {
                        Ad = kitap.Yazar.Ad,
                        Hakkinda = kitap.Yazar.YazarHakkinda,
                        YazarKey = kitap.Yazar.YazarKey
                    }
                };
                return sonuc;
            }

            return sonuc;
        }


        public async Task<IEnumerable<KitapAramaSonucViewModel>> KitaplariVeyaKitapGetir(KitapAramaViewModel model)
        {
            List<KitapAramaSonucViewModel> sonuc = new List<KitapAramaSonucViewModel>();

            var kitaplar = _kitapRepository.SelectIncludeMany(d => d.Yazar, e => e.Yayinevi);

            if (model.AramaAd != null)
            {
                kitaplar = kitaplar.Where(k => k.Ad == model.AramaAd);
            }

            if (model.AramaIsbn != null)
            {
                kitaplar = kitaplar.Where(k => k.Isbn == model.AramaIsbn);
            }

            if (model.AramaYazar != null)
            {
                kitaplar = kitaplar.Where(k => k.Yazar.Ad == model.AramaYazar);
            }

            if (model.AramaYayinevi != null)
            {
                kitaplar = kitaplar.Where(k => k.Yayinevi.Ad == model.AramaYayinevi);
            }

            kitaplar.ToList().ForEach(k =>
            {
                var kitap = new KitapAramaSonucViewModel()
                {
                    KitapKey = k.KitapKey,
                    Ad = k.Ad,
                    Yayinevi = new YayineviViewModel { 
                        YayineviKey = k.Yayinevi.YayineviKey,
                        Ad = k.Yayinevi.Ad
                    },
                    Yazar = new YazarViewModel { 
                        Ad = k.Yazar.Ad,
                        YazarKey = k.Yazar.YazarKey,
                        Hakkinda = k.Yazar.YazarHakkinda
                    },
                    DemirbasNo = k.DemirbasNo,
                    Isbn = k.Isbn,
                    SayfaSayisi = k.SayfaSayisi,
                    KitapDurumKey =(int) KitapDurum.Kutuphanede,
                    KitapHakkindaOzet = k.KitapHakkindaOzet                                       
                };
                sonuc.Add(kitap);
            });

            return sonuc;
        }
        public async Task<SonucModel<KitapKayitSilViewModel>> KitapSil(KitapKayitSilViewModel model)
        {
            var sonuc = new SonucModel<KitapKayitSilViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Kitap Silme İşlemi Gerçekleştirilemedi."
            };

            Kitap silenecekYayinevi = new Kitap()
            {
                KitapKey = model.KitapKey,
                Ad = model.Ad
            };

            var silmeSonuc = await _kitapRepository.DeleteAsync(silenecekYayinevi);

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
