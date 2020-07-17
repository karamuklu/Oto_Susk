using INF_OTO_SUSK_Entities;
using NetOpenX50;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.InteropServices;
using INF_OTO_SUSK_DataAccessLayer;
using System.Timers;
using System.IO;
using System.Windows.Forms;

namespace INF_OTO_SUSK_BusinessLayer
{
    public class INF_SUSK_Manager
    {
        public static INF_SUSK_Dal infSusk_Dal = new INF_SUSK_Dal();
        public List<SUSK_LISTESI_ESTAP> ESTAPSUSK_LISTESI()
        {
            return infSusk_Dal.ESTAPSUSK_LISTESI();
        }
        public List<SUSK_LISTESI_MKA> YARIMAMULSUSK_LISTESI_MEK()
        {
            return infSusk_Dal.YARIMAMULSUSK_LISTESI_MEK();
        }
        public List<SUSK_LISTESI_ESTAP> YARIMAMULSUSK_LISTESI_J()
        {
            return infSusk_Dal.YARIMAMULSUSK_LISTESI_J();
        }
        public List<YARIMAMUL> YariMamul_Bul()
        {
            return infSusk_Dal.YariMamul_Bul();
        }
        public List<YARIMAMUL> TransferListesi()
        {
            return infSusk_Dal.TransferListesi();
        }
        public List<YARIMAMUL> AcilacakIsemriListesi()
        {
            return infSusk_Dal.AcilacakIsemriListesi();
        }
        public List<YARIMAMUL> AcilacakIsemriListesi_Mekanik()
        {
            return infSusk_Dal.AcilacakIsemriListesi_Mekanik();
        }
        public List<YARIMAMUL> YARIMAMUL_TRANSFERLISTESI_MEK()
        {
            return infSusk_Dal.YARIMAMUL_TRANSFERLISTESI_MEK();
        }
        public List<SUSK_LISTESI_EKART> YARIMAMULSUSK_LISTESI_EKART()
        {
            return infSusk_Dal.YARIMAMULSUSK_LISTESI_EKART();
        }
        public List<SUSK_LISTESI_GENEL> INFORM_SUSK_LISTESI()
        {
            return infSusk_Dal.INFORM_SUSK_LISTESI();
        }
        public string ISEMRINO_OLUSTUR(string isEmriilkHarf)
        {
            return infSusk_Dal.ISEMRINO_OLUSTUR(isEmriilkHarf);
        }
        public List<SUSK_LISTESI_ESTAP> YARIMAMULSUSK_LISTESI(string mamul_Kodu, string refIsemriNo, decimal isemriMiktar)
        {
            return infSusk_Dal.YARIMAMULSUSK_LISTESI(mamul_Kodu, refIsemriNo, isemriMiktar);
        }
        public List<SUSK_LISTESI_ESTAP> YARIMAMULSUSK_LISTESI_MKA()
        {
            return infSusk_Dal.YARIMAMULSUSK_LISTESI_MKA();
        }
        public void SUSK_Kaydet(SUSK_LISTESI_MKA suskList, int GIRISDEPO, int CIKISDEPO)
        {
            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            SerbestUSK susk = default(SerbestUSK);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                  ConfigurationManager.AppSettings["SIRKET"],
                     "TEMELSET",
                     "",
                     "karamuklu",
                     "12qw",
                     1);

                susk = kernel.yeniSerbestUSK(sirket);
                susk.UretSon_FisNo = susk.SonFisNumarasi("M");
                susk.UretSon_Mamul = suskList.STOK_KODU;
                susk.UretSon_Depo = 118; //giriş depo
                susk.I_Yedek1 = 118;   //çıkış depo
                susk.UretSon_Miktar = (double)suskList.MIKTAR;
                susk.UretSon_Tarih = DateTime.Now.Date;
                susk.BelgeTipi = TBelgeTipi.btIsEmri;
                susk.UretSon_SipNo = suskList.ISEMRINO;
                susk.DepoOnceligi = TDepoOnceligi.doStokDepo;
                //susk.F_Yedek1 = (double)suskList.MIKTAR; //miktar2
                susk.Aciklama = "OTO. SUSK";
                susk.Proje_Kodu = "G";
                susk.S_Yedek1 = suskList.REFISEMRINO;
                //susk.S_Yedek2 = "ekalan2 örneği";
                susk.OTO_YMAM_GIRDI_CIKTI = true;
                susk.OTO_YMAM_STOK_KULLAN = false;
                susk.BAKIYE_DEPO = 0;  //0:verilen_depo 1:tüm_depolar

                if (susk.FisUret() != true)
                {
                    MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş no ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" +
                       susk.HataMesaji, ConfigurationManager.AppSettings["YariMamulSUSKEmail"]);
                    //MessageBox.Show(susk.HataKodu.ToString() + ' ' + susk.HataMesaji);
                    infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                    //infSusk_Dal.IsemriKapat(suskList.ISEMRINO);
                }


                if (susk.Kaydet() != true)
                {
                    MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş no ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" + susk.HataMesaji, ConfigurationManager.AppSettings["YariMamulSUSKEmail"]);
                    infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);

                }
                else
                {
                    MailGonder("Bilgi... SUSK Yapılan İşemri " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş numarası ile  otomatik olarak SUSK yapılmıştır.", ConfigurationManager.AppSettings["YariMamulSUSKEmail"]);//oto mail gönder ,bora.demirkol@inform.com.tr,hakan.sari@inform.com.tr
                    //infSusk_Dal.IsemriKapat(suskList.ISEMRINO);
                }
            }
            catch (Exception ex)
            {
                MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş no ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" +
                   ex.Message, ConfigurationManager.AppSettings["YariMamulSUSKEmail"]);
                infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(susk);
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }

        }
        public void SUSK_Kaydet(SUSK_LISTESI_ESTAP suskList, int GIRISDEPO, int CIKISDEPO)
        {
            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            SerbestUSK susk = default(SerbestUSK);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                   ConfigurationManager.AppSettings["SIRKET"],
                     "TEMELSET",
                     "",
                     "karamuklu",
                     "12qw",
                     1);

                susk = kernel.yeniSerbestUSK(sirket);
                susk.UretSon_FisNo = susk.SonFisNumarasi("S");
                susk.UretSon_Mamul = suskList.STOK_KODU;
                susk.UretSon_Depo = GIRISDEPO; //giriş depo
                susk.I_Yedek1 = CIKISDEPO;   //çıkış depo
                susk.UretSon_Miktar = (double)suskList.MIKTAR;
                susk.UretSon_Tarih = DateTime.Now.Date;
                susk.BelgeTipi = TBelgeTipi.btIsEmri;
                susk.UretSon_SipNo = suskList.ISEMRINO;
                susk.DepoOnceligi = TDepoOnceligi.doStokDepo;
                //susk.F_Yedek1 = (double)suskList.MIKTAR; //miktar2
                susk.Aciklama = "OTO. SUSK";
                susk.Proje_Kodu = "G";
                //susk.S_Yedek1 = "ekalan1 örneği";
                //susk.S_Yedek2 = "ekalan2 örneği";
                susk.OTO_YMAM_GIRDI_CIKTI = true;
                susk.OTO_YMAM_STOK_KULLAN = false;
                susk.BAKIYE_DEPO = 0;  //0:verilen_depo 1:tüm_depolar

                if (susk.FisUret() != true)
                {
                    MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş no ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" + susk.HataMesaji, ConfigurationManager.AppSettings["AnaMamulSUSKEmail"]);
                    infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                }


                if (susk.Kaydet() != true)
                {
                    MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş no ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" + susk.HataMesaji, ConfigurationManager.AppSettings["AnaMamulSUSKEmail"]);
                    infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                    // infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                }


                //MessageBox.Show(susk.HataKodu.ToString() + ' ' + susk.HataMesaji);
                //AutoClosingMessageBox.Show(suskList.ISEMRINO + " nolu işemri SUSK yapılmıştır.", "SUSK Kontrol", 1000);


                else
                {
                    MailGonder("Bilgi... SUSK Yapılan İşemri " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş numarası ile  otomatik olarak SUSK yapılmıştır.", ConfigurationManager.AppSettings["AnaMamulSUSKEmail"]);//oto mail gönder ,bora.demirkol@inform.com.tr,hakan.sari@inform.com.tr
                    //infSusk_Dal.IsemriKapat(suskList.ISEMRINO);
                }

            }
            catch (Exception ex)
            {
                MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş no ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" +
                    ex.Message, ConfigurationManager.AppSettings["AnaMamulSUSKEmail"]);
                infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(susk);
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }

        }
        public void SUSK_Kaydet(SUSK_LISTESI_EKART suskList, int GIRISDEPO, int CIKISDEPO)
        {
            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            SerbestUSK susk = default(SerbestUSK);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                   ConfigurationManager.AppSettings["SIRKET"],
                     "TEMELSET",
                     "",
                     "karamuklu",
                     "12qw",
                     1);

                susk = kernel.yeniSerbestUSK(sirket);
                susk.UretSon_FisNo = susk.SonFisNumarasi("M");
                susk.UretSon_Mamul = suskList.STOK_KODU;
                susk.UretSon_Depo = GIRISDEPO; //giriş depo
                susk.I_Yedek1 = CIKISDEPO;   //çıkış depo
                susk.UretSon_Miktar = (double)suskList.MIKTAR;
                susk.UretSon_Tarih = DateTime.Now.Date;
                susk.BelgeTipi = TBelgeTipi.btIsEmri;
                susk.UretSon_SipNo = suskList.ISEMRINO;
                susk.DepoOnceligi = TDepoOnceligi.doStokDepo;
                //susk.F_Yedek1 = (double)suskList.MIKTAR; //miktar2
                susk.Aciklama = "OTO. SUSK";
                susk.Proje_Kodu = "G";
                //susk.S_Yedek1 = "ekalan1 örneği";
                //susk.S_Yedek2 = "ekalan2 örneği";
                susk.OTO_YMAM_GIRDI_CIKTI = true;
                susk.OTO_YMAM_STOK_KULLAN = false;
                susk.BAKIYE_DEPO = 0;  //0:verilen_depo 1:tüm_depolar

                if (susk.FisUret() != true)
                {
                    //MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş numarası ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" +
                    //   susk.HataMesaji, ConfigurationManager.AppSettings["EkartSUSKEmail"]);
                    ////MessageBox.Show(susk.HataKodu.ToString() + ' ' + susk.HataMesaji);
                    //// infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                }

                if (susk.Kaydet() != true)
                {
                    MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş numarası ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" + susk.HataMesaji, ConfigurationManager.AppSettings["EkartSUSKEmail"]);
                    // infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                }
                else
                {
                    MailGonder("Bilgi... SUSK Yapılan İşemri " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş numarası ile  otomatik olarak SUSK yapılmıştır.", ConfigurationManager.AppSettings["EkartSUSKEmail"]);//oto mail gönder ,bora.demirkol@inform.com.tr,hakan.sari@inform.com.tr
                    //infSusk_Dal.IsemriKapat(suskList.ISEMRINO); //Parçalı SUSK yapıldığı için tam olarak istenilen şekilde çalışmıyor.

                }
            }
            catch (Exception ex)
            {
                MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş numarası ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" +
                   ex.Message, ConfigurationManager.AppSettings["EkartSUSKEmail"]);
                //infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(susk);
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }

        }
        public void SUSK_Kaydet(SUSK_LISTESI_GENEL suskList, int GIRISDEPO, int CIKISDEPO)
        {
            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            SerbestUSK susk = default(SerbestUSK);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                   ConfigurationManager.AppSettings["SIRKET"],
                     "TEMELSET",
                     "",
                     "karamuklu",
                     "12qw",
                     1);

                susk = kernel.yeniSerbestUSK(sirket);
                susk.UretSon_FisNo = susk.SonFisNumarasi("M");
                susk.UretSon_Mamul = suskList.STOK_KODU;
                susk.UretSon_Depo = GIRISDEPO; //giriş depo
                susk.I_Yedek1 = CIKISDEPO;   //çıkış depo
                susk.UretSon_Miktar = (double)suskList.MIKTAR;
                susk.UretSon_Tarih = DateTime.Now.Date;
                susk.BelgeTipi = TBelgeTipi.btIsEmri;
                susk.UretSon_SipNo = suskList.ISEMRINO;
                susk.DepoOnceligi = TDepoOnceligi.doStokDepo;
                //susk.F_Yedek1 = (double)suskList.MIKTAR; //miktar2
                susk.Aciklama = "OTO. SUSK";
                susk.Proje_Kodu = "G";
                //susk.S_Yedek1 = "ekalan1 örneği";
                //susk.S_Yedek2 = "ekalan2 örneği";
                susk.OTO_YMAM_GIRDI_CIKTI = true;
                susk.OTO_YMAM_STOK_KULLAN = false;
                susk.BAKIYE_DEPO = 0;  //0:verilen_depo 1:tüm_depolar

                if (susk.FisUret() != true)
                {
                    MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş numarası ile SUSK fiş numarası ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" +
                       susk.HataMesaji, ConfigurationManager.AppSettings["InformSUSKEmail"]);
                    //MessageBox.Show(susk.HataKodu.ToString() + ' ' + susk.HataMesaji);
                    infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                    //infSusk_Dal.IsemriKapat(suskList.ISEMRINO);
                }

                if (susk.Kaydet() != true)
                {
                    MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş numarası ile SUSK fiş numarası ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" + susk.HataMesaji, ConfigurationManager.AppSettings["InformSUSKEmail"]);
                    infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                }

                else
                {
                    MailGonder("Bilgi... SUSK Yapılan İşemri " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş numarası ile  otomatik olarak SUSK yapılmıştır.", ConfigurationManager.AppSettings["InformSUSKEmail"]);//oto mail gönder ,bora.demirkol@inform.com.tr,hakan.sari@inform.com.tr
                }
            }
            catch (Exception ex)
            {
                MailGonder("Uyarı... Eksik Malzeme - SUSK YAPILAMAYAN İŞEMRİ " + suskList.ISEMRINO + "  " + suskList.STOK_KODU, suskList.STOK_KODU + " stok kodu için açılmış olan " + suskList.ISEMRINO + " nolu işemri " + susk.UretSon_FisNo + " fiş numarası ile SUSK fiş numarası ile SUSK yapılırken eksik malzemeden dolayı TAMAMLANAMAMIŞTIR..." + " " + "" +
                   ex.Message, ConfigurationManager.AppSettings["InformSUSKEmail"]);
                infSusk_Dal.IsEmriUSK_STATUSGuncelle(suskList.ISEMRINO);
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(susk);
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }
        }
        public static bool MailGonder(string konu, string aciklama, string kime)
        {
            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential = new NetworkCredential("mustafa.karamuklu@inform.com.tr", "Password19");
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress("noreply@legrand.com");
            smtpClient.Host = "SMTP.LIMOUSIN.FR.GRPLEG.COM";
            smtpClient.Port = 25; //Gönderici portudur.
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;
            smtpClient.EnableSsl = false;
            message.From = fromAddress;
            message.Subject = konu;
            message.IsBodyHtml = true; // HTML içeriğine izin verir
            message.Body = aciklama; // İçeriği oluşturmaktadır.
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            message.To.Add(kime);
            message.Bcc.Add("mustafa.karamuklu@inform.com.tr");
            smtpClient.Send(message);
            return true;
        }
        public void LokalDepolarArasiTransferBelgesi(List<YARIMAMUL> transferList)
        {
            //aktarım için lazım olan kalemler, hammadde kodu, transfer miktar
            //Üst bilgiler sabit, kalem bilgilerini foreach ile dönüp aktarımı yaptırabiliriz.

            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string configFile = System.IO.Path.Combine(appPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe.config");

            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            Fatura fatura = default(Fatura);
            FatUst fatUst = default(FatUst);
            FatKalem fatKalem = default(FatKalem);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                              ConfigurationManager.AppSettings["SIRKET"],
                                              "TEMELSET",
                                              "",
                                              "karamuklu",
                                              "12qw",
                                              1);
                fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);
                fatUst = fatura.Ust();
                fatUst.FATIRS_NO = fatura.YeniNumara("X");
                fatUst.KOD2 = "D";
                //fatUst.CariKod = "1";
                fatUst.CARI_KOD2 = "1";
                fatUst.TIPI = TFaturaTipi.ft_Bos;
                fatUst.AMBHARTUR = TAmbarHarTur.htDepolar;
                fatUst.Tarih = DateTime.Now.Date;
                fatUst.FiiliTarih = DateTime.Now.Date;
                fatUst.PLA_KODU = "D";
                fatUst.Proje_Kodu = "G";
                fatUst.Aciklama = "OTO. AKTARIM";
                fatUst.EFatOzelKod = 'D';
                fatUst.KDV_DAHILMI = true;
                fatUst.EKACK1 = " Otomatik Aktarım - Depo Transfer Fişi";

                foreach (var item in transferList)
                {
                    fatKalem = fatura.kalemYeni(item.ALTMAMUL);//stok kodu lazım
                    fatKalem.Gir_Depo_Kodu = 118;               //Depo Kodu lazım Giriş
                    fatKalem.DEPO_KODU = 117;                   //Depo Kodu lazım Çıkış
                    fatKalem.STra_GCMIK = (double)item.TRANSFERMIKTAR;
                    fatKalem.ProjeKodu = "G";
                    fatKalem.ReferansKodu = "1Y3321";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir
                    //Sadece 114 depodan çekeceğimiz zaman Dinamik depo hareketlerini yaptırabilmemiz lazım. program doğru şekilde çalışıyor.

                    //if (item.TRANSFERMIKTAR > item.BAKIYE115)
                    //{
                    //      //Miktar Lazım
                    //}
                    //fatKalem.DEPO_KODU = 114;
                    //fatKalem.STra_BF = 1;
                    fatKalem.Irsaliyeno = "OTO AKTARIM";
                }
                fatura.kayitYeni();
                MailGonder("Otomatik oluşturulan " + fatUst.FATIRS_NO + " no'lu Depo Transfer Fişi", fatUst.FATIRS_NO + " numaralı Depo Transfer Fişi sistemde oluşturulmuştur.", ConfigurationManager.AppSettings["DepoTransferEmail"]);//,bora.demirkol@inform.com.tr
            }
            finally
            {
                Marshal.ReleaseComObject(fatKalem);
                Marshal.ReleaseComObject(fatUst);
                Marshal.ReleaseComObject(fatura);
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }
        }
        public void IsEmriKaydet(YARIMAMUL acilacakIsemri)//İşemri Açma NetOpenx
        {
            //var sirketNedir = ConfigurationManager.AppSettings["SIRKET"];
            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            IsEmri Isemri = default(IsEmri);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                           ConfigurationManager.AppSettings["SIRKET"],
                                           "TEMELSET",
                                           "",
                                           "karamuklu",
                                           "12qw",
                                           1);
                Isemri = kernel.yeniIsEmri(sirket);
                Isemri.IsEmriNo = infSusk_Dal.ISEMRINO_OLUSTUR("J");
                Isemri.Tarih = DateTime.Now.Date;
                Isemri.StokKodu = acilacakIsemri.ALTMAMUL;
                Isemri.Miktar = (double)acilacakIsemri.TRANSFERMIKTAR;//depo adetine bak, fark kadar aç
                //Isemri.SiparisNo = acilacakIsemri.SIPARIS_NO;  // Müşteri sipariş no
                Isemri.TeslimTarihi = DateTime.Now.Date;
                //Isemri.RefIsEmriNo = "MKA000000000001";
                Isemri.Aciklama = "B";
                Isemri.ProjeKodu = "G";

                Isemri.kayitYeni();

                string isemri = infSusk_Dal.ISEMRINO_OLUSTUR("J");
                MailGonder("Bilgi... Açılmayan Alt İşemirleri", isemri + " nolu işemri ile " + acilacakIsemri.ALTMAMUL + " kodlu ürün sistemde " + Convert.ToInt32(acilacakIsemri.TRANSFERMIKTAR) + " adet olarak açılmıştır.", ConfigurationManager.AppSettings["IsemriEmail"]);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                Marshal.ReleaseComObject(Isemri);
                Marshal.ReleaseComObject(sirket);
                kernel.FreeNetsisLibrary();
                Marshal.ReleaseComObject(kernel);
            }
        }
        public void LokalDepolarArasiTransferBelgesi(List<EKART_TRANSFER> transferList)
        {
            //aktarım için lazım olan kalemler, hammadde kodu, transfer miktar
            //Üst bilgiler sabit, kalem bilgilerini foreach ile dönüp aktarımı yaptırabiliriz.

            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string configFile = System.IO.Path.Combine(appPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe.config");

            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            Fatura fatura = default(Fatura);
            FatUst fatUst = default(FatUst);
            FatKalem fatKalem = default(FatKalem);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                              ConfigurationManager.AppSettings["SIRKET"],
                                              "TEMELSET",
                                              "",
                                              "karamuklu",
                                              "12qw",
                                              1);
                fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);
                fatUst = fatura.Ust();
                fatUst.FATIRS_NO = fatura.YeniNumara("X");
                fatUst.KOD2 = "D";
                //fatUst.CariKod = "1";
                fatUst.CARI_KOD2 = "1";
                fatUst.TIPI = TFaturaTipi.ft_Bos;
                fatUst.AMBHARTUR = TAmbarHarTur.htDepolar;
                fatUst.Tarih = DateTime.Now.Date;
                fatUst.FiiliTarih = DateTime.Now.Date;
                fatUst.PLA_KODU = "D";
                fatUst.Proje_Kodu = "G";
                fatUst.Aciklama = "OTO. AKTARIM";
                fatUst.EFatOzelKod = 'D';
                fatUst.KDV_DAHILMI = true;
                fatUst.EKACK1 = "E-KART - Otomatik Aktarım - Depo Transfer Fişi";

                //Dinamik depoya STHARINC bilgisi atayabilmek için lazım oldu.
                //string inc_HAMKODU = "";
                //decimal inc_ILKTRANSFERMIKTAR = 0;
                //string inc_FISNO = "";


                var depharList = new List<TBLDEPHAR>();

                foreach (var item in transferList)
                {
                    if ((double)item.ILKTRANSFERMIKTAR != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 115;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.ILKTRANSFER_DEPO;                   //Depo Kodu lazım Çıkış
                        fatKalem.STra_GCMIK = (double)item.ILKTRANSFERMIKTAR;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U2120";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir

                        TBLDEPHAR dephar = new TBLDEPHAR();
                        dephar.STOKKODU = item.HAM_KODU;
                        dephar.NETMIKTAR = item.ILKTRANSFERMIKTAR;
                        dephar.DEPOHARFISNO = fatUst.FATIRS_NO;
                        depharList.Add(dephar);

                        //inc_HAMKODU = item.HAM_KODU;
                        //inc_ILKTRANSFERMIKTAR = item.ILKTRANSFERMIKTAR;
                        //inc_FISNO = fatUst.FATIRS_NO;


                    }
                    if ((double)item.IKINCITRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 115;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.IKINCITRANSFER_DEPO;
                        fatKalem.STra_GCMIK = (double)item.IKINCITRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U2120";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir
                        //fatKalem.Irsaliyeno = "OTO AKTARIM";

                    }

                    if ((double)item.UCUNCUTRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);
                        fatKalem.Gir_Depo_Kodu = 115;
                        fatKalem.DEPO_KODU = item.UCUNCUTRANSFER_DEPO;
                        fatKalem.STra_GCMIK = (double)item.UCUNCUTRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U2120";
                        //fatKalem.Irsaliyeno = "OTO AKTARIM";
                    }

                    if ((double)item.DORDUNCUTRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 115;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.DORDUNCUTRANSFER_DEPO;                   //Depo Kodu lazım Çıkış
                        fatKalem.STra_GCMIK = (double)item.DORDUNCUTRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U2120";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir
                        //fatKalem.Irsaliyeno = "OTO AKTARIM";
                    }
                }
                if (fatura.KalemAdedi != 0)
                {
                    fatura.kayitYeni();
                    //Dinamik Depo Hareketi atan kod
                    if (depharList.Count != 0)
                    {
                        foreach (var item in depharList)
                        {
                            infSusk_Dal.HUCREHAREKETKAYIT(item.STOKKODU, item.NETMIKTAR, item.DEPOHARFISNO);
                        }
                    }
                    MailGonder("Otomatik oluşturulan " + fatUst.FATIRS_NO + " no'lu Depo Transfer Fişi", fatUst.FATIRS_NO + " numaralı Depo Transfer Fişi sistemde oluşturulmuştur.", ConfigurationManager.AppSettings["DepoTransferEmail"]);
                }
            }
            finally
            {
                try
                {
                    Marshal.ReleaseComObject(fatKalem);
                    Marshal.ReleaseComObject(fatUst);
                    Marshal.ReleaseComObject(fatura);
                    Marshal.ReleaseComObject(sirket);
                    kernel.FreeNetsisLibrary();
                    Marshal.ReleaseComObject(kernel);
                }
                catch (Exception)
                {
                    AutoClosingMessageBox.Show("Transfer edilecek kayıt bulunamadı, Sonraki işleme geçilecektir.", "Depo Transfer Kontrol", 2000);
                }
            }
        }
        public void LokalDepolarArasiTransferBelgesi(List<MEKANIK_TRANSFER> transferList)
        {
            //aktarım için lazım olan kalemler, hammadde kodu, transfer miktar
            //Üst bilgiler sabit, kalem bilgilerini foreach ile dönüp aktarımı yaptırabiliriz.

            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string configFile = System.IO.Path.Combine(appPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe.config");

            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            Fatura fatura = default(Fatura);
            FatUst fatUst = default(FatUst);
            FatKalem fatKalem = default(FatKalem);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                              ConfigurationManager.AppSettings["SIRKET"],
                                              "TEMELSET",
                                              "",
                                              "karamuklu",
                                              "12qw",
                                              1);
                fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);
                fatUst = fatura.Ust();
                fatUst.FATIRS_NO = fatura.YeniNumara("X");
                fatUst.KOD2 = "D";
                //fatUst.CariKod = "1";
                fatUst.CARI_KOD2 = "1";
                fatUst.TIPI = TFaturaTipi.ft_Bos;
                fatUst.AMBHARTUR = TAmbarHarTur.htDepolar;
                fatUst.Tarih = DateTime.Now.Date;
                fatUst.FiiliTarih = DateTime.Now.Date;
                fatUst.PLA_KODU = "D";
                fatUst.Proje_Kodu = "G";
                fatUst.Aciklama = "OTO. AKTARIM";
                fatUst.EFatOzelKod = 'D';
                fatUst.KDV_DAHILMI = true;
                fatUst.EKACK1 = "MEKANIK - Otomatik Aktarım - Depo Transfer Fişi";

                
                var depharList = new List<TBLDEPHAR>();

                foreach (var item in transferList)
                {
                    if ((double)item.ILKTRANSFERMIKTAR != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 115;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.ILKTRANSFER_DEPO;                   //Depo Kodu lazım Çıkış
                        fatKalem.STra_GCMIK = (double)item.ILKTRANSFERMIKTAR;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U2120";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir

                        TBLDEPHAR dephar = new TBLDEPHAR();
                        dephar.STOKKODU = item.HAM_KODU;
                        dephar.NETMIKTAR = (decimal)item.ILKTRANSFERMIKTAR;
                        dephar.DEPOHARFISNO = fatUst.FATIRS_NO;
                        depharList.Add(dephar);

                        //inc_HAMKODU = item.HAM_KODU;
                        //inc_ILKTRANSFERMIKTAR = item.ILKTRANSFERMIKTAR;
                        //inc_FISNO = fatUst.FATIRS_NO;

                    }
                    if ((double)item.IKINCITRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 118;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.IKINCITRANSFER_DEPO;
                        fatKalem.STra_GCMIK = (double)item.IKINCITRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U1114";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir
                        fatKalem.Sira = 2;
                    }
                }

                if (fatura.KalemAdedi != 0)
                {
                    fatura.kayitYeni();
                    //Dinamik Depo Hareketi atan kod
                    if (depharList.Count != 0)
                    {
                        foreach (var item in depharList)
                        {
                            infSusk_Dal.HUCREHAREKETKAYIT(item.STOKKODU, item.NETMIKTAR, item.DEPOHARFISNO);
                        }
                    }
                    MailGonder("Otomatik oluşturulan " + fatUst.FATIRS_NO + " no'lu Depo Transfer Fişi", fatUst.FATIRS_NO + " numaralı Depo Transfer Fişi sistemde oluşturulmuştur.", ConfigurationManager.AppSettings["DepoTransferEmail"]);
                }
            }

            finally
            {
                try
                {
                    Marshal.ReleaseComObject(fatKalem);
                    Marshal.ReleaseComObject(fatUst);
                    Marshal.ReleaseComObject(fatura);
                    Marshal.ReleaseComObject(sirket);
                    kernel.FreeNetsisLibrary();
                    Marshal.ReleaseComObject(kernel);
                }
                catch (Exception)
                {
                    AutoClosingMessageBox.Show("Transfer edilecek kayıt bulunamadı, Sonraki işleme geçilecektir.","Depo Transfer Kontrol",2000); 
                }
            }
        }
        public void LokalDepolarArasiTransferBelgesi(List<WIPCIHAZ_TRANSFER> transferList)
        {
            //aktarım için lazım olan kalemler, hammadde kodu, transfer miktar
            //Üst bilgiler sabit, kalem bilgilerini foreach ile dönüp aktarımı yaptırabiliriz.

            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string configFile = System.IO.Path.Combine(appPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe.config");

            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            Fatura fatura = default(Fatura);
            FatUst fatUst = default(FatUst);
            FatKalem fatKalem = default(FatKalem);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                              ConfigurationManager.AppSettings["SIRKET"],
                                              "TEMELSET",
                                              "",
                                              "karamuklu",
                                              "12qw",
                                              1);
                fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);
                fatUst = fatura.Ust();
                fatUst.FATIRS_NO = fatura.YeniNumara("X");
                fatUst.KOD2 = "D";
                //fatUst.CariKod = "1";
                fatUst.CARI_KOD2 = "1";
                fatUst.TIPI = TFaturaTipi.ft_Bos;
                fatUst.AMBHARTUR = TAmbarHarTur.htDepolar;
                fatUst.Tarih = DateTime.Now.Date;
                fatUst.FiiliTarih = DateTime.Now.Date;
                fatUst.PLA_KODU = "D";
                fatUst.Proje_Kodu = "G";
                fatUst.Aciklama = "OTO. AKTARIM";
                fatUst.EFatOzelKod = 'D';
                fatUst.KDV_DAHILMI = true;
                fatUst.EKACK1 = "WIP CIHAZ - Otomatik Aktarım - Depo Transfer Fişi";

                //string inc_HAMKODU = "";
                //decimal inc_ILKTRANSFERMIKTAR = 0;
                //string inc_FISNO = "";

                var depharList = new List<TBLDEPHAR>();

                foreach (var item in transferList)
                {
                    if ((double)item.ILKTRANSFERMIKTAR != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 115;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.ILKTRANSFER_DEPO;                   //Depo Kodu lazım Çıkış
                        fatKalem.STra_GCMIK = (double)item.ILKTRANSFERMIKTAR;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U2120";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir

                        TBLDEPHAR dephar = new TBLDEPHAR();
                        dephar.STOKKODU = item.HAM_KODU;
                        dephar.NETMIKTAR = item.ILKTRANSFERMIKTAR;
                        dephar.DEPOHARFISNO = fatUst.FATIRS_NO;
                        depharList.Add(dephar);

                        //inc_HAMKODU = item.HAM_KODU;
                        //inc_ILKTRANSFERMIKTAR = item.ILKTRANSFERMIKTAR;
                        //inc_FISNO = fatUst.FATIRS_NO;
                    }

                    if ((double)item.IKINCITRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 115;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.IKINCITRANSFER_DEPO;
                        fatKalem.STra_GCMIK = (double)item.IKINCITRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U1151";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir
                        //fatKalem.Irsaliyeno = "OTO AKTARIM";


                    }//Depo Kodu lazım Çıkış

                    if ((double)item.UCUNCUTRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);
                        fatKalem.Gir_Depo_Kodu = 115;
                        fatKalem.DEPO_KODU = item.UCUNCUTRANSFER_DEPO;
                        fatKalem.STra_GCMIK = (double)item.UCUNCUTRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U1151";
                        //fatKalem.Irsaliyeno = "OTO AKTARIM";
                    }

                    if ((double)item.DORDUNCUTRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 115;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.DORDUNCUTRANSFER_DEPO;                   //Depo Kodu lazım Çıkış
                        fatKalem.STra_GCMIK = (double)item.DORDUNCUTRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U1151";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir
                        //fatKalem.Irsaliyeno = "OTO AKTARIM";
                    }

                }
                if (fatura.KalemAdedi != 0)
                {
                    fatura.kayitYeni();
                    //Dinamik Depo Hareketi atan kod
                    if (depharList.Count != 0)
                    {
                        foreach (var item in depharList)
                        {
                            infSusk_Dal.HUCREHAREKETKAYIT(item.STOKKODU, item.NETMIKTAR, item.DEPOHARFISNO);
                        }
                    }
                    MailGonder("Otomatik oluşturulan " + fatUst.FATIRS_NO + " no'lu Depo Transfer Fişi", fatUst.FATIRS_NO + " numaralı Depo Transfer Fişi sistemde oluşturulmuştur.", ConfigurationManager.AppSettings["DepoTransferEmail"]);
                }


            }
            finally
            {
                try
                {
                    Marshal.ReleaseComObject(fatKalem);
                    Marshal.ReleaseComObject(fatUst);
                    Marshal.ReleaseComObject(fatura);
                    Marshal.ReleaseComObject(sirket);
                    kernel.FreeNetsisLibrary();
                    Marshal.ReleaseComObject(kernel);
                }
                catch (Exception)
                {
                    AutoClosingMessageBox.Show("Transfer edilecek kayıt bulunamadı, Sonraki işleme geçilecektir.", "Depo Transfer Kontrol", 2000);
                }
            }
        }
        public void LokalDepolarArasiTransferBelgesi(List<ESTAP_TRANSFER> transferList)
        {
            //aktarım için lazım olan kalemler, hammadde kodu, transfer miktar
            //Üst bilgiler sabit, kalem bilgilerini foreach ile dönüp aktarımı yaptırabiliriz.

            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string configFile = System.IO.Path.Combine(appPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe.config");

            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            Fatura fatura = default(Fatura);
            FatUst fatUst = default(FatUst);
            FatKalem fatKalem = default(FatKalem);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                              ConfigurationManager.AppSettings["SIRKET"],
                                              "TEMELSET",
                                              "",
                                              "karamuklu",
                                              "12qw",
                                              1);
                fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);
                fatUst = fatura.Ust();
                fatUst.FATIRS_NO = fatura.YeniNumara("X");
                fatUst.KOD2 = "D";
                //fatUst.CariKod = "1";
                fatUst.CARI_KOD2 = "1";
                fatUst.TIPI = TFaturaTipi.ft_Bos;
                fatUst.AMBHARTUR = TAmbarHarTur.htDepolar;
                fatUst.Tarih = DateTime.Now.Date;
                fatUst.FiiliTarih = DateTime.Now.Date;
                fatUst.PLA_KODU = "D";
                fatUst.Proje_Kodu = "G";
                fatUst.Aciklama = "OTO. AKTARIM";
                fatUst.EFatOzelKod = 'D';
                fatUst.KDV_DAHILMI = true;
                fatUst.EKACK1 = "ESTAP ANAMAMUL - Otomatik Aktarım - Depo Transfer Fişi";

                //string inc_HAMKODU = "";
                //decimal inc_ILKTRANSFERMIKTAR = 0;
                //string inc_FISNO = "";

                var depharList = new List<TBLDEPHAR>();

                foreach (var item in transferList)
                {
                    if ((double)item.ILKTRANSFERMIKTAR != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 115;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.ILKTRANSFER_DEPO;                   //Depo Kodu lazım Çıkış
                        fatKalem.STra_GCMIK = (double)item.ILKTRANSFERMIKTAR;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U2120";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir

                        TBLDEPHAR dephar = new TBLDEPHAR();
                        dephar.STOKKODU = item.HAM_KODU;
                        dephar.NETMIKTAR = item.ILKTRANSFERMIKTAR;
                        dephar.DEPOHARFISNO = fatUst.FATIRS_NO;
                        depharList.Add(dephar);

                        //inc_HAMKODU = item.HAM_KODU;
                        //inc_ILKTRANSFERMIKTAR = item.ILKTRANSFERMIKTAR;
                        //inc_FISNO = fatUst.FATIRS_NO;
                    }

                    if ((double)item.IKINCITRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 118;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.IKINCITRANSFER_DEPO;
                        fatKalem.STra_GCMIK = (double)item.IKINCITRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U0250";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir


                    }//Depo Kodu lazım Çıkış

                    if ((double)item.UCUNCUTRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);
                        fatKalem.Gir_Depo_Kodu = 118;
                        fatKalem.DEPO_KODU = item.UCUNCUTRANSFER_DEPO;
                        fatKalem.STra_GCMIK = (double)item.UCUNCUTRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U0250";
                        //fatKalem.Irsaliyeno = "OTO AKTARIM";
                    }

                    if ((double)item.DORDUNCUTRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 118;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.DORDUNCUTRANSFER_DEPO;                   //Depo Kodu lazım Çıkış
                        fatKalem.STra_GCMIK = (double)item.DORDUNCUTRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U0250";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir
                        //fatKalem.Irsaliyeno = "OTO AKTARIM";
                    }
                }
              

                if (fatura.KalemAdedi != 0)
                {
                    fatura.kayitYeni();
                    //Dinamik Depo Hareketi atan kod
                    if (depharList.Count != 0)
                    {
                        foreach (var item in depharList)
                        {
                            infSusk_Dal.HUCREHAREKETKAYIT(item.STOKKODU, item.NETMIKTAR, item.DEPOHARFISNO);
                        }
                    }
                    MailGonder("Otomatik oluşturulan " + fatUst.FATIRS_NO + " no'lu Depo Transfer Fişi", fatUst.FATIRS_NO + " numaralı Depo Transfer Fişi sistemde oluşturulmuştur.", ConfigurationManager.AppSettings["DepoTransferEmail"]);
                }
            }
            finally
            {
                try
                {
                    Marshal.ReleaseComObject(fatKalem);
                    Marshal.ReleaseComObject(fatUst);
                    Marshal.ReleaseComObject(fatura);
                    Marshal.ReleaseComObject(sirket);
                    kernel.FreeNetsisLibrary();
                    Marshal.ReleaseComObject(kernel);
                }
                catch (Exception)
                {
                    AutoClosingMessageBox.Show("Transfer edilecek kayıt bulunamadı, Sonraki işleme geçilecektir.", "Depo Transfer Kontrol", 2000);
                }
            }
        }
        public void LokalDepolarArasiTransferBelgesi(List<CIHAZ_TRANSFER> transferList)
        {
            //aktarım için lazım olan kalemler, hammadde kodu, transfer miktar
            //Üst bilgiler sabit, kalem bilgilerini foreach ile dönüp aktarımı yaptırabiliriz.

            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string configFile = System.IO.Path.Combine(appPath, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe.config");

            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            Kernel kernel = new Kernel();
            Sirket sirket = default(Sirket);
            Fatura fatura = default(Fatura);
            FatUst fatUst = default(FatUst);
            FatKalem fatKalem = default(FatKalem);
            try
            {
                sirket = kernel.yeniSirket(TVTTipi.vtMSSQL,
                                              ConfigurationManager.AppSettings["SIRKET"],
                                              "TEMELSET",
                                              "",
                                              "karamuklu",
                                              "12qw",
                                              1);
                fatura = kernel.yeniFatura(sirket, TFaturaTip.ftLokalDepo);
                fatUst = fatura.Ust();
                fatUst.FATIRS_NO = fatura.YeniNumara("X");
                fatUst.KOD2 = "D";
                //fatUst.CariKod = "1";
                fatUst.CARI_KOD2 = "1";
                fatUst.TIPI = TFaturaTipi.ft_Bos;
                fatUst.AMBHARTUR = TAmbarHarTur.htDepolar;
                fatUst.Tarih = DateTime.Now.Date;
                fatUst.FiiliTarih = DateTime.Now.Date;
                fatUst.PLA_KODU = "D";
                fatUst.Proje_Kodu = "G";
                fatUst.Aciklama = "OTO. AKTARIM";
                fatUst.EFatOzelKod = 'D';
                fatUst.KDV_DAHILMI = true;
                fatUst.EKACK1 = "CIHAZ ANAMAMUL - Otomatik Aktarım - Depo Transfer Fişi";

                //string inc_HAMKODU = "";
                //decimal inc_ILKTRANSFERMIKTAR = 0;
                //string inc_FISNO = "";

                var depharList = new List<TBLDEPHAR>();

                foreach (var item in transferList)
                {
                    if ((double)item.ILKTRANSFERMIKTAR != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 115;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.ILKTRANSFER_DEPO;                   //Depo Kodu lazım Çıkış
                        fatKalem.STra_GCMIK = (double)item.ILKTRANSFERMIKTAR;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U1151";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir

                        TBLDEPHAR dephar = new TBLDEPHAR();
                        dephar.STOKKODU = item.HAM_KODU;
                        dephar.NETMIKTAR = item.ILKTRANSFERMIKTAR;
                        dephar.DEPOHARFISNO = fatUst.FATIRS_NO;
                        depharList.Add(dephar);

                        //inc_HAMKODU = item.HAM_KODU;
                        //inc_ILKTRANSFERMIKTAR = item.ILKTRANSFERMIKTAR;
                        //inc_FISNO = fatUst.FATIRS_NO;
                    }

                    if ((double)item.IKINCITRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 115;               //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.IKINCITRANSFER_DEPO;
                        fatKalem.STra_GCMIK = (double)item.IKINCITRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U1151";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir


                    }//Depo Kodu lazım Çıkış

                    if ((double)item.UCUNCUTRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);
                        fatKalem.Gir_Depo_Kodu = 115;
                        fatKalem.DEPO_KODU = item.UCUNCUTRANSFER_DEPO;
                        fatKalem.STra_GCMIK = (double)item.UCUNCUTRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U1151";
                        //fatKalem.Irsaliyeno = "OTO AKTARIM";
                    }

                    if ((double)item.DORDUNCUTRANSFER != 0)
                    {
                        fatKalem = fatura.kalemYeni(item.HAM_KODU);//stok kodu lazım
                        fatKalem.Gir_Depo_Kodu = 115; //Depo Kodu lazım Giriş
                        fatKalem.DEPO_KODU = item.DORDUNCUTRANSFER_DEPO;                   //Depo Kodu lazım Çıkış
                        fatKalem.STra_GCMIK = (double)item.DORDUNCUTRANSFER;
                        fatKalem.ProjeKodu = "G";
                        fatKalem.ReferansKodu = "1U1151";// Bu kısımda önemli.Bağlı siparişe bakıp çekilebilir
                        //fatKalem.Irsaliyeno = "OTO AKTARIM";
                    }
                }


                if (fatura.KalemAdedi != 0)
                {
                    fatura.kayitYeni();
                    //Dinamik Depo Hareketi atan kod
                    if (depharList.Count != 0)
                    {
                        foreach (var item in depharList)
                        {
                            infSusk_Dal.HUCREHAREKETKAYIT(item.STOKKODU, item.NETMIKTAR, item.DEPOHARFISNO);
                        }
                    }
                    MailGonder("Otomatik oluşturulan " + fatUst.FATIRS_NO + " no'lu Depo Transfer Fişi", fatUst.FATIRS_NO + " numaralı Depo Transfer Fişi sistemde oluşturulmuştur. Cihazlar için SUSK işlemine başlayabilirsiniz...", ConfigurationManager.AppSettings["CihazTransferEmail"]);
                }
            }
            finally
            {
                try
                {
                    Marshal.ReleaseComObject(fatKalem);
                    Marshal.ReleaseComObject(fatUst);
                    Marshal.ReleaseComObject(fatura);
                    Marshal.ReleaseComObject(sirket);
                    kernel.FreeNetsisLibrary();
                    Marshal.ReleaseComObject(kernel);
                }
                catch (Exception)
                {
                    AutoClosingMessageBox.Show("Transfer edilecek kayıt bulunamadı, Sonraki işleme geçilecektir.", "Depo Transfer Kontrol", 2000);
                }
            }
        }



        public List<EKART_TRANSFER> EKART_TRANSFERLIST()
        {
            return infSusk_Dal.EKART_TRANSFERLIST();
        }
        public List<MEKANIK_TRANSFER> MEKANIK_TRANSFERLIST()
        {
            return infSusk_Dal.MEKANIK_TRANSFERLIST();
        }
        public List<ESTAP_TRANSFER> ESTAP_TRANSFERLIST()
        {
            return infSusk_Dal.ESTAP_TRANSFERLIST();
        }
        public List<WIPCIHAZ_TRANSFER> WIPCIHAZ_TRANSFERLIST()
        {
            return infSusk_Dal.WIPCIHAZ_TRANSFER();
        }
        public List<CIHAZ_TRANSFER> CIHAZ_TRANSFERLIST()
        {
            return infSusk_Dal.CIHAZ_TRANSFERLIST();
        }
        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                using (_timeoutTimer)
                    MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }

    }

    public class InnerOperation
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        public void Start()
        {

            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 3600000;
            timer.Enabled = true;
        }
        public void Stop()
        {
            WriteToFile("Service Durduruldu " + DateTime.Now);
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service Message ! Servis Çalıştı :    " + DateTime.Now);

            INF_SUSK_Manager infSusk_Manager = new INF_SUSK_Manager();



            //GIRIS20 tablosundaki işemirlerinin eksikleri transfer ediliyor.
            var mekanik_list = infSusk_Manager.MEKANIK_TRANSFERLIST();
            if (mekanik_list.Count != 0)
            {
                infSusk_Manager.LokalDepolarArasiTransferBelgesi(mekanik_list);
            }

            //GIRIS20 tablosundaki Mekanik Yarımamul işemirleri toplanıp, SUSK yapılıyor
            List<SUSK_LISTESI_MKA> yariMamulMekList = new List<SUSK_LISTESI_MKA>();
            yariMamulMekList = infSusk_Manager.YARIMAMULSUSK_LISTESI_MEK();
            foreach (var item in yariMamulMekList)
            {
                infSusk_Manager.SUSK_Kaydet(item, 118, 118);
            }

            //ElektronikKart SUsk yapılıyor.
            List<SUSK_LISTESI_EKART> SuskListEkart = new List<SUSK_LISTESI_EKART>();
            SuskListEkart = infSusk_Manager.YARIMAMULSUSK_LISTESI_EKART();
            var list = infSusk_Manager.EKART_TRANSFERLIST();
            if (list.Count != 0)
            {
                infSusk_Manager.LokalDepolarArasiTransferBelgesi(list);
            }

            if (SuskListEkart.Count != 0)
            {
                foreach (var item in SuskListEkart)
                {
                    infSusk_Manager.SUSK_Kaydet(item, 131, 115);
                }
            }

            //inform P'li işemirleri için eksikler kontrol ediliyor
            var wipCihaz_list = infSusk_Manager.WIPCIHAZ_TRANSFERLIST();
            if (wipCihaz_list.Count != 0)
            {
                infSusk_Manager.LokalDepolarArasiTransferBelgesi(wipCihaz_list);
            }

            //inform P'li işemirleri SUSK yapılıyor.
            List<SUSK_LISTESI_GENEL> SuskListInform = new List<SUSK_LISTESI_GENEL>();
            SuskListInform = infSusk_Manager.INFORM_SUSK_LISTESI();
            foreach (var item in SuskListInform)
            {
                infSusk_Manager.SUSK_Kaydet(item, 115, 115);
            }


            //Hakanın anamamül listesinin eksikleri transfer ediliyor
            var Estap_list = infSusk_Manager.ESTAP_TRANSFERLIST();
            if (Estap_list.Count != 0)
            {
                infSusk_Manager.LokalDepolarArasiTransferBelgesi(Estap_list);
            }

            //Hakanın anamamül listesi alınıyor
            List<SUSK_LISTESI_ESTAP> estapSuskList = new List<SUSK_LISTESI_ESTAP>();
            estapSuskList = infSusk_Manager.ESTAPSUSK_LISTESI();//Estap Anamamuller Bulunur


            //Hakanın anamamül listesi SUSK yapılıyor.
            List<SUSK_LISTESI_ESTAP> estapAnamamulSuskList = new List<SUSK_LISTESI_ESTAP>();
            estapAnamamulSuskList = infSusk_Manager.ESTAPSUSK_LISTESI();
            foreach (var item in estapAnamamulSuskList)
            {
                infSusk_Manager.SUSK_Kaydet(item, 128, 118);
            }



        }
    }

}
