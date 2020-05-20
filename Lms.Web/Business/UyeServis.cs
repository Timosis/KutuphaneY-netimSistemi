using Kys.Web.Models;
using Kys.Web.Repository;
using Kys.Web.ViewModels;
using Syncfusion.EJ2.DropDowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Kys.Web.Business
{
    public interface IUyeServis
    {
        Task<IEnumerable<UyeViewModel>> UyeVeyaUyeleriGetir(UyeAramaViewModel model);
        Task<UyeDetayViewModel> UyeDetayGetir(int uyeKey);
        Task<SonucModel<UyeKayitViewModel>> UyeKaydetGuncelle(UyeKayitViewModel model);
        Task<SonucModel<OduncSonucViewModel>> OduncVer(OduncVerViewModel model);
        Task<SonucModel<IadeAlSonucViewModel>> IadeAl(IadeAlViewModel model);
        Task<SonucModel<UyeKayitSilViewModel>> UyeSil(UyeKayitSilViewModel model);
    }

    public class UyeServis : IUyeServis
    {
        private readonly IUyeRepository _uyeRepository;
        private readonly IKitapOduncRepository _kitapOduncRepository;
        private readonly IKitapRepository _kitapRepository;

        public UyeServis(IUyeRepository uyeRepository,IKitapRepository kitapRepository, IKitapOduncRepository kitapOduncRepository)
        {
            _uyeRepository = uyeRepository;
            _kitapOduncRepository = kitapOduncRepository;
            _kitapRepository = kitapRepository;
        }
        public async Task<SonucModel<IadeAlSonucViewModel>> IadeAl(IadeAlViewModel model)
        {
            var oduncKey = model.OduncKey;

            var sonuc = new SonucModel<IadeAlSonucViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Iade Alma İşlemi Gerçekleşemedi"
            };

            var oduncAlinmisKitap = _kitapOduncRepository.SelectInclude(k => k.Kitap.Yazar).Where(o => o.KitapOduncKey == oduncKey).FirstOrDefault();

            if (oduncAlinmisKitap == null)
            {
                sonuc.HataMesaji = "Ödünç Alınmış Kitap Bulunamadı";
                sonuc.Tip = SonucTip.Basarisiz;
                return sonuc;
            }

            var uye = await _uyeRepository.FindAsync(u => u.UyeKey == oduncAlinmisKitap.UyeKey);

            if (uye == null)
            {
                sonuc.HataMesaji = "Uye Bulunamadı";
                sonuc.Tip = SonucTip.Basarisiz;
                return sonuc;
            }

            var kitap = await _kitapRepository.FindAsync(k => k.KitapKey == oduncAlinmisKitap.KitapKey);

            if (kitap == null)
            {
                sonuc.HataMesaji = "Kitap Bulunamadı";
                sonuc.Tip = SonucTip.Basarisiz;
                return sonuc;
            }

            oduncAlinmisKitap.KitapOduncDurum =(int) KitapDurum.Kutuphanede;
            oduncAlinmisKitap.GetirisTarihi = DateTime.Now;

            var guncellemeSonuc = await _kitapOduncRepository.UpdateAsync(oduncAlinmisKitap);

            if (guncellemeSonuc > 0)
            {
                kitap.KitapDurumKey = (int)KitapDurum.Kutuphanede;

                var kitapDurumGuncelle = await _kitapRepository.UpdateAsync(kitap);
                if (kitapDurumGuncelle > 0)
                {
                    sonuc.HataMesaji = $"{kitap.Ad} kitabı {uye.Ad} {uye.Soyad}'den iade alınmıştır.";
                    sonuc.Tip = SonucTip.Basarili;
                    sonuc.Data = new IadeAlSonucViewModel()
                    { 
                        DemirbasNo = oduncAlinmisKitap.Kitap.DemirbasNo,
                        KitapAd = oduncAlinmisKitap.Kitap.Ad,
                        KitapOduncKey = oduncAlinmisKitap.KitapOduncKey,
                        Yazar = oduncAlinmisKitap.Kitap.Yazar.Ad

                    };
                    return sonuc;
                }
            }
            return sonuc;
        }
        public async Task<SonucModel<OduncSonucViewModel>> OduncVer(OduncVerViewModel model)
        {
            var sonuc = new SonucModel<OduncSonucViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Ödünç Verme İşlemi Başarısız"
            };

            var uye = await _uyeRepository.FindAsync(u => u.UyeKey == model.UyeKey);

            if (uye == null)
            {
                sonuc.HataMesaji = "Üye Bulunamadı";
                sonuc.Tip = SonucTip.Basarisiz;
                return sonuc;
            }

            var kitap = await _kitapRepository.FindAsync(k => k.DemirbasNo == model.DemirbasNo);

            if (kitap == null)
            {
                sonuc.HataMesaji = "Kitap Bulunamadı";
                sonuc.Tip = SonucTip.Basarisiz;
                return sonuc;
            }

            if (kitap.KitapDurumKey == (int)KitapDurum.Okuyucuda)
            {
                sonuc.HataMesaji = "Ödünç Verilmek İstenen Kitap Okuyucuda Bulunmaktadır";
                sonuc.Tip = SonucTip.Basarisiz;
                return sonuc;
            }


            KitapOdunc odunc = new KitapOdunc()
            {
                KitapKey = kitap.KitapKey,
                UyeKey = uye.UyeKey,
                KitapOduncDurum = (int)KitapDurum.Okuyucuda,
                AlisTarihi = DateTime.Now
            };

            var kayitSonuc = await _kitapOduncRepository.InsertAsync(odunc);

            if (kayitSonuc > 0)
            {
                var oduncVerilen = _kitapOduncRepository.SelectIncludeMany(k => k.Kitap.Yazar).Where(o => o.KitapKey == kitap.KitapKey).FirstOrDefault();
                kitap.KitapDurumKey = (int)KitapDurum.Okuyucuda;
                var guncellemeSonuc = await _kitapRepository.UpdateAsync(kitap);
                if (guncellemeSonuc > 0)
                {
                    sonuc.HataMesaji = $"{kitap.Ad} kitabı {uye.Ad} {uye.Soyad}'e ödünç verilmiştir";
                    sonuc.Tip = SonucTip.Basarili;
                    sonuc.Data = new OduncSonucViewModel()
                    {
                        DemirbasNo = oduncVerilen.Kitap.DemirbasNo,
                        KitapAd = oduncVerilen.Kitap.Ad,
                        OduncKey = oduncVerilen.KitapOduncKey,
                        Yazar = oduncVerilen.Kitap.Yazar.Ad,
                    };

                    return sonuc;
                }
            }
            return sonuc;
        }        
        public async Task<IEnumerable<UyeViewModel>> UyeVeyaUyeleriGetir(UyeAramaViewModel model)
        {
            List<UyeViewModel> sonuc = new List<UyeViewModel>();

            var uyeler = await _uyeRepository.GetAllAsync();

            if (model.KimlikNo != null)
            {
                uyeler = uyeler.Where(k => k.KimlikNo == model.KimlikNo);
            }

            uyeler.ToList().ForEach(k =>
            {
                var uye = new UyeViewModel()
                {
                    UyeKey = k.UyeKey,
                    Ad = k.Ad,
                    Soyad = k.Soyad,
                    Email = k.Email,
                    KimlikNo = k.KimlikNo,
                    Telefon = k.Telefon
                };
                sonuc.Add(uye);
            });

            return sonuc;
        }
        public async Task<SonucModel<UyeKayitViewModel>> UyeKaydetGuncelle(UyeKayitViewModel model)
        {
            var sonuc = new SonucModel<UyeKayitViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Üyelik Kaydetme İşlemi Gerçekleştirilmedi."
            };

            if (model.UyeKey > 0)
            {
                var guncellenecekUye = await _uyeRepository.GetByIdAsync(model.UyeKey);
                guncellenecekUye.Ad = model.Ad;
                guncellenecekUye.Soyad = model.Soyad;
                guncellenecekUye.Email = model.Email;
                guncellenecekUye.KimlikNo = model.KimlikNo;
                guncellenecekUye.Telefon = model.Telefon;

                var guncellemeSonuc = await _uyeRepository.UpdateAsync(guncellenecekUye);
                if (guncellemeSonuc > 0)
                {
                    sonuc.HataMesaji = $"Üyelik Bilgileri Güncellenmiştir.";
                    sonuc.Tip = SonucTip.Basarili;
                    sonuc.Data = model;
                    return sonuc;
                }
            }
            else
            {
                Uye kategori = new Uye()
                {
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    Email = model.Email,
                    KimlikNo = model.KimlikNo,
                    Telefon = model.Telefon                    
                };

                var eklemeSonuc = await _uyeRepository.InsertAsync(kategori);
                var eklenenUye = await _uyeRepository.FindAsync(x => x.Ad == model.Ad);

                if (eklemeSonuc > 0)
                {
                    sonuc.HataMesaji = $"{model.Ad} Kategori Olarak Eklenmiştir.";
                    sonuc.Tip = SonucTip.Basarili;
                    sonuc.Data = new UyeKayitViewModel()
                    {
                        UyeKey = eklenenUye.UyeKey,
                        Ad = eklenenUye.Ad,
                        Soyad = eklenenUye.Soyad,
                        Email = eklenenUye.Email,
                        KimlikNo = eklenenUye.KimlikNo,
                        Telefon = eklenenUye.Telefon
                    };
                    return sonuc;
                }
            }

            return sonuc;
        }
        public async Task<SonucModel<UyeKayitSilViewModel>> UyeSil(UyeKayitSilViewModel model)
        {
            var sonuc = new SonucModel<UyeKayitSilViewModel>
            {
                Tip = SonucTip.Belirsiz,
                HataMesaji = "Kategori Kaydetme İşlemi Gerçekleştirilmedi."
            };

            Uye silenecekUye = new Uye()
            {
                UyeKey = model.UyeKey          
            };

            var silmeSonuc = await _uyeRepository.DeleteAsync(silenecekUye);

            if (silmeSonuc > 0)
            {
                sonuc.Tip = SonucTip.Basarili;
                sonuc.HataMesaji = "Silme İşlemi Başarı İle Gerçekleşti";
                sonuc.Data = model;
                return sonuc;
            }
            return sonuc;
        }
        public async Task<UyeDetayViewModel> UyeDetayGetir(int uyeKey)
        {
            UyeDetayViewModel sonuc = new UyeDetayViewModel();

            var oduncler = _kitapOduncRepository.SelectIncludeMany(u => u.Uye, k => k.Kitap, y => y.Kitap.Yazar).Where(x => x.UyeKey == uyeKey);

            var uye = oduncler.Select(x => x.Uye).FirstOrDefault();

            if (uye == null)
            {
                uye = await _uyeRepository.FindAsync(x => x.UyeKey == uyeKey);
            }

            oduncler.ToList().ForEach(o =>
            {
                var odunc = new UyeOduncViewModel()
                {
                    OduncKey = o.KitapOduncKey,
                    OduncKitap = new KitapViewModel()
                    {
                        KitapKey = o.Kitap.KitapKey,
                        Ad = o.Kitap.Ad,
                        DemirbasNo = o.Kitap.DemirbasNo,
                        Yazar = o .Kitap.Yazar.Ad
                    },
                    OduncKitapDurum = (KitapDurum)o.Kitap.KitapDurumKey
                };
                sonuc.Oduncler.Add(odunc);
            });

            sonuc.Uye = new UyeViewModel()
            {
                UyeKey = uye.UyeKey,
                Ad = uye.Ad,
                Email = uye.Email,
                KimlikNo = uye.KimlikNo,
                Soyad = uye.KimlikNo,
                Telefon = uye.Telefon
            };

            return sonuc;
        }
    }
}
