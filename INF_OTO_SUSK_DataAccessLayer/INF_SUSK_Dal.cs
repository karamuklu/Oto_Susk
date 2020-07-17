using INF_OTO_SUSK_Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INF_OTO_SUSK_DataAccessLayer
{
    public class INF_SUSK_Dal
    {
        public List<SUSK_LISTESI_MKA> YARIMAMULSUSK_LISTESI_MEK()
        {
            string sqlCumle = "SELECT TARIH,ISEMRINO,STOK_KODU,MIKTAR,REFISEMRINO FROM SUSK_LISTESI_MKA";

            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<SUSK_LISTESI_MKA>(sqlCumle).ToList();
        }
        public List<SUSK_LISTESI_ESTAP> ESTAPSUSK_LISTESI()//Hakanın Listesi
        {
            string sqlCumle = "SELECT A.TARIH,A.ISEMRINO,A.STOK_KODU, A.MIKTAR FROM (SELECT i.TESLIM_TARIHI TARIH, i.ISEMRINO, i.STOK_KODU, i.URETIM_ADET - isnull(susk.SUSKMIKTAR, 0) MIKTAR FROM _SG_..INF_MONTAJ_EKRAN_GECICI i LEFT OUTER JOIN(SELECT sum(uretson_miktar) SUSKMIKTAR, t.URETSON_SIPNO FROM TBLSTOKURS t GROUP BY t.URETSON_SIPNO)SUSK ON i.ISEMRINO = susk.URETSON_SIPNO WHERE i.URETIM_ADET - isnull(susk.SUSKMIKTAR, 0) > 0 )A";

            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<SUSK_LISTESI_ESTAP>(sqlCumle).ToList();
        }
        public List<SUSK_LISTESI_ESTAP> YARIMAMULSUSK_LISTESI(string mamul_Kodu, string refIsemriNo, decimal isemriMiktar)
        {
            string sqlCumle = "SELECT SEVIYE,isemri.ISEMRINO,isnull(susk.URETSON_FISNO,'SUSK YAPILMAMIS')SUSK_NO, isemri.TARIH, isemri.STOK_KODU, isemri.MIKTAR FROM INFORM20..TBLISEMRI isemri LEFT OUTER JOIN INFORM20..TBLSTOKURS susk ON isemri.ISEMRINO = susk.URETSON_SIPNO AND isemri.STOK_KODU = susk.URETSON_MAMUL CROSS APPLY(SELECT * from INFORM20..SUSKONCESI_HAZIRLIK_MKA('" + mamul_Kodu + "','" + refIsemriNo + "'," + isemriMiktar + ",'E',''))A WHERE isemri.REFISEMRINO = '" + refIsemriNo + "' AND a.stok_kodu = isemri.STOK_KODU AND isemri.MIKTAR > isnull(susk.URETSON_MIKTAR, 0) ORDER BY seviye DESC";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<SUSK_LISTESI_ESTAP>(sqlCumle).ToList();
        }
        public List<YARIMAMUL> YariMamul_Bul()
        {
            //int day = tarih.Day;
            //int month = tarih.Month;
            //int year = tarih.Year;
            //string tamTarih = month + "-" + day + "-" + year;
            string sqlCumle = "SELECT B.STOK_KODU ALTMAMUL,Sum(TRANSFERMIKTAR)TRANSFERMIKTAR, ISNULL((SELECT t.TOP_GIRIS_MIK - t.TOP_CIKIS_MIK AS BAKIYE FROM INFORM20..TBLSTOKPH t WHERE t.DEPO_KODU = '117' AND t.STOK_KODU = B.STOK_KODU),0) BAKIYE117 FROM(SELECT i.TESLIM_TARIHI TARIH, i.ISEMRINO, i.STOK_KODU, i.URETIM_ADET - isnull(susk.SUSKMIKTAR, 0) MIKTAR FROM INF_MONTAJ_EKRAN_GECICI i LEFT OUTER JOIN(SELECT sum(uretson_miktar) SUSKMIKTAR, t.URETSON_SIPNO FROM INFORM20..TBLSTOKURS t GROUP BY t.URETSON_SIPNO)SUSK ON i.ISEMRINO = susk.URETSON_SIPNO WHERE i.URETIM_ADET - isnull(susk.SUSKMIKTAR, 0) > 0)A CROSS APPLY INFORM20..SUSKONCESI_HAZIRLIK_MKA(a.stok_kodu,a.isemrino,a.miktar,'E','2-17-2020') B where b.refisemrino=a.isemrino GROUP BY B.STOK_KODU";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<YARIMAMUL>(sqlCumle).ToList();
        }
        public List<YARIMAMUL> TransferListesi()
        {
            //int day = tarih.Day;
            //int month = tarih.Month;
            //int year = tarih.Year;
            //string tamTarih = month + "-" + day + "-" + year;
            string sqlCumle = "SELECT D.ALTMAMUL, ISNULL(D.TRANSFERMIKTAR,0) TRANSFERMIKTAR FROM (SELECT C.ALTMAMUL,CASE WHEN C.BAKIYE117 > 0 AND C.TRANSFERMIKTAR >= C.BAKIYE117 THEN C.BAKIYE117 WHEN C.BAKIYE117 > 0 AND C.TRANSFERMIKTAR < C.BAKIYE117 THEN C.TRANSFERMIKTAR END TRANSFERMIKTAR FROM(         SELECT B.STOK_KODU ALTMAMUL, Sum(TRANSFERMIKTAR) TRANSFERMIKTAR , ISNULL((SELECT t.TOP_GIRIS_MIK -t.TOP_CIKIS_MIK AS BAKIYE FROM INFORM20..TBLSTOKPH t WHERE t.DEPO_KODU = '117' AND t.STOK_KODU = B.STOK_KODU),0) BAKIYE117 FROM (SELECT i.TESLIM_TARIHI TARIH, i.ISEMRINO, i.STOK_KODU, i.URETIM_ADET - isnull(susk.SUSKMIKTAR, 0) MIKTAR FROM INF_MONTAJ_EKRAN_GECICI i LEFT OUTER JOIN(SELECT sum(uretson_miktar) SUSKMIKTAR, t.URETSON_SIPNO FROM INFORM20..TBLSTOKURS t GROUP BY t.URETSON_SIPNO)SUSK ON i.ISEMRINO = susk.URETSON_SIPNO WHERE i.URETIM_ADET - isnull(susk.SUSKMIKTAR, 0) > 0)A CROSS APPLY INFORM20..SUSKONCESI_HAZIRLIK_MKA(a.stok_kodu, a.isemrino, a.miktar, 'E', '') B where b.refisemrino = a.isemrino GROUP BY B.STOK_KODU)C)D WHERE D.TRANSFERMIKTAR>0 ORDER BY 1";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<YARIMAMUL>(sqlCumle).ToList();
        }
        public List<YARIMAMUL> AcilacakIsemriListesi()
        {
            //int day = tarih.Day;
            //int month = tarih.Month;
            //int year = tarih.Year;
            //string tamTarih = month + "-" + day + "-" + year;
            string sqlCumle = "SELECT D.ALTMAMUL, ISNULL(D.ISEMRIMIKTAR,0) TRANSFERMIKTAR FROM (SELECT C.ALTMAMUL,C.TRANSFERMIKTAR,C.BAKIYE118,CASE WHEN C.BAKIYE118 > C.TRANSFERMIKTAR THEN C.TRANSFERMIKTAR WHEN C.TRANSFERMIKTAR > C.BAKIYE118 THEN C.TRANSFERMIKTAR - C.BAKIYE118     WHEN C.BAKIYE118 = C.TRANSFERMIKTAR THEN 0   END ISEMRIMIKTAR FROM(SELECT B.STOK_KODU ALTMAMUL, Sum(TRANSFERMIKTAR)TRANSFERMIKTAR, ISNULL((SELECT t.TOP_GIRIS_MIK - t.TOP_CIKIS_MIK AS BAKIYE FROM INFORM20..TBLSTOKPH t WHERE t.DEPO_KODU = '118' AND t.STOK_KODU = B.STOK_KODU), 0) BAKIYE118 FROM ( SELECT i.TESLIM_TARIHI TARIH, i.ISEMRINO, i.STOK_KODU, i.URETIM_ADET - isnull(susk.SUSKMIKTAR, 0) MIKTAR FROM INF_MONTAJ_EKRAN_GECICI i LEFT OUTER JOIN(SELECT sum(uretson_miktar) SUSKMIKTAR, t.URETSON_SIPNO FROM INFORM20..TBLSTOKURS t GROUP BY t.URETSON_SIPNO)SUSK ON i.ISEMRINO = susk.URETSON_SIPNO WHERE i.URETIM_ADET - isnull(susk.SUSKMIKTAR, 0) > 0)A CROSS APPLY INFORM20..SUSKONCESI_HAZIRLIK_MKA(a.stok_kodu, a.isemrino, a.miktar, 'E', '') B where b.refisemrino IS null GROUP BY B.STOK_KODU)C ) D WHERE D.ISEMRIMIKTAR > 0 ORDER BY 1";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<YARIMAMUL>(sqlCumle).ToList();
        }
        public List<SUSK_LISTESI_ESTAP> YARIMAMULSUSK_LISTESI_J()
        {
            string sqlCumle = "SELECT ISEMRINO,STOK_KODU,MIKTAR,TARIH  FROM INFORM20..TBLISEMRI t LEFT OUTER JOIN(SELECT sum(uretson_miktar) SUSKMIKTAR, t.URETSON_SIPNO FROM INFORM20..TBLSTOKURS t GROUP BY t.URETSON_SIPNO)SUSK ON t.ISEMRINO = susk.URETSON_SIPNO WHERE t.ISEMRINO LIKE 'J%' AND t.MIKTAR - ISNULL(SUSK.SUSKMIKTAR, 0) > 0";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<SUSK_LISTESI_ESTAP>(sqlCumle).ToList();
        }
        public List<SUSK_LISTESI_ESTAP> YARIMAMULSUSK_LISTESI_MKA()
        {
            //SELECT ISEMRINO, STOK_KODU, MIKTAR, TARIH  FROM INFORM20..TBLISEMRI t LEFT OUTER JOIN(SELECT sum(uretson_miktar) SUSKMIKTAR, t.URETSON_SIPNO FROM INFORM20..TBLSTOKURS t GROUP BY t.URETSON_SIPNO)SUSK ON t.ISEMRINO = susk.URETSON_SIPNO WHERE t.ISEMRINO LIKE 'J%' AND t.MIKTAR - ISNULL(SUSK.SUSKMIKTAR, 0) > 0
            string sqlCumle = "SELECT b.stok_kodu,b.transfermiktar miktar,isemri.TARIH,isemri.ISEMRINO FROM (SELECT i.TESLIM_TARIHI TARIH, i.ISEMRINO, i.STOK_KODU, i.URETIM_ADET - isnull(susk.SUSKMIKTAR, 0) MIKTAR FROM INF_MONTAJ_EKRAN_GECICI i LEFT OUTER JOIN(SELECT sum(uretson_miktar) SUSKMIKTAR, t.URETSON_SIPNO FROM INFORM20..TBLSTOKURS t GROUP BY t.URETSON_SIPNO)SUSK ON i.ISEMRINO = susk.URETSON_SIPNO WHERE i.URETIM_ADET - isnull(susk.SUSKMIKTAR, 0) > 0 )A CROSS APPLY INFORM20..SUSKONCESI_HAZIRLIK_MKA(a.stok_kodu, a.isemrino, a.miktar, 'E', '2-17-2020') B     LEFT OUTER JOIN INFORM20..TBLISEMRI isemri ON a.isemrino = isemri.refisemrino AND b.stok_kodu = isemri.stok_kodu LEFT OUTER JOIN(SELECT sum(uretson_miktar) SUSKMIKTAR, t.URETSON_SIPNO FROM INFORM20..TBLSTOKURS t GROUP BY t.URETSON_SIPNO)SUSK ON isemri.ISEMRINO = susk.URETSON_SIPNO where isemri.isemrino IS NOT NULL AND isemri.miktar - isnull(susk.SUSKMIKTAR, 0) > 0 AND(isemri.ISEMRINO LIKE 'S%' OR isemri.ISEMRINO LIKE 'H%' OR isemri.ISEMRINO LIKE 'N%' OR isemri.ISEMRINO LIKE 'U%')";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<SUSK_LISTESI_ESTAP>(sqlCumle).ToList();
        }
        public List<YARIMAMUL> AcilacakIsemriListesi_Mekanik()
        {
            string sqlCumle = "SELECT altmamul,C.IHTIYACMIKTAR TRANSFERMIKTAR  FROM( SELECT altmamul, sum(A.IHTIYACMIKTAR) IHTIYACMIKTAR, ISNULL((SELECT t.TOP_GIRIS_MIK - t.TOP_CIKIS_MIK AS BAKIYE FROM INFORM20..TBLSTOKPH t WHERE t.DEPO_KODU = '117' AND t.STOK_KODU = A.altmamul), 0) BAKIYE117 FROM(SELECT B.STOK_KODU ALTMAMUL, b.transfermiktar IHTIYACMIKTAR FROM dbo.SUSK_LISTESI_MKA s CROSS APPLY INFORM20..SUSKONCESI_HAZIRLIK_MKA(s.stok_kodu, s.isemrino, s.miktar, 'E', '2-17-2020') B WHERE b.stok_kodu NOT LIKE '143%')A GROUP BY altmamul)C WHERE C.BAKIYE117 = 0";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<YARIMAMUL>(sqlCumle).ToList();
        }
        public List<YARIMAMUL> YARIMAMUL_TRANSFERLISTESI_MEK()
        {
            string sqlCumle = "select* from(SELECT altmamul,CASE WHEN C.BAKIYE117 > C.IHTIYACMIKTAR then C.IHTIYACMIKTAR WHEN C.BAKIYE117 < c.IHTIYACMIKTAR THEN C.BAKIYE117 END TRANSFERMIKTAR FROM(    SELECT altmamul, sum(A.IHTIYACMIKTAR) IHTIYACMIKTAR, ISNULL((SELECT t.TOP_GIRIS_MIK -t.TOP_CIKIS_MIK AS BAKIYE FROM INFORM20..TBLSTOKPH t WHERE t.DEPO_KODU = '117' AND t.STOK_KODU = A.altmamul),0) BAKIYE117   FROM(  SELECT B.STOK_KODU ALTMAMUL, b.transfermiktar IHTIYACMIKTAR FROM dbo.SUSK_LISTESI_MKA s   CROSS APPLY INFORM20..SUSKONCESI_HAZIRLIK_MKA(s.stok_kodu, s.isemrino, s.miktar, 'E', '2-17-2020') B     WHERE b.stok_kodu NOT LIKE '143%')A GROUP BY altmamul)C ) D WHERE D.TRANSFERMIKTAR > 0";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<YARIMAMUL>(sqlCumle).ToList();
        }
        public List<SUSK_LISTESI_EKART> YARIMAMULSUSK_LISTESI_EKART()
        {
            string sqlCumle = "SELECT convert(date,getdate()) TARIH,Ekart.isemrino, Ekart.StokNo STOK_KODU,(Ekart.miktar-isnull((SUSK.SUSKMIKTAR),0))miktar FROM (SELECT isemrino, StokNo, count(*) Miktar FROM(select isemrino, t.StokNo, t.SeriNo,  case when count(*)> 1 THEN 1 else count(*) end miktar from EKartUretim..tblEkartUretimTakip t   WHERE t.IstasyonAdi = 'FCT' AND year(tarih)= 2020 GROUP BY StokNo,isemrino,t.SeriNo )A GROUP BY a.isemrino,A.StokNo)Ekart LEFT OUTER JOIN(SELECT sum(uretson_miktar) SUSKMIKTAR, t.URETSON_SIPNO FROM INFORM20..TBLSTOKURS t GROUP BY t.URETSON_SIPNO)SUSK ON ekart.ISEMRINO = susk.URETSON_SIPNO WHERE Ekart.miktar - isnull((SUSK.SUSKMIKTAR), 0) > 0 AND Ekart.isemrino NOT LIKE 'KAYIP%' AND Ekart.isemrino  IN(SELECT isemrino FROM INFORM20..TBLISEMRI where kapali='H')";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<SUSK_LISTESI_EKART>(sqlCumle).ToList();
        }
        public List<EKART_TRANSFER> EKART_TRANSFERLIST()
        {
            string sqlCumle = " SELECT * FROM EKARTTRANSFERLIST_MKA  ";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<EKART_TRANSFER>(sqlCumle).ToList();
        }

        public List<WIPCIHAZ_TRANSFER> WIPCIHAZ_TRANSFER()
        {
            string sqlCumle = " SELECT * FROM WIPSUSK_TRANSFERLIST_MKA  ";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<WIPCIHAZ_TRANSFER>(sqlCumle).ToList();
        }
        public List<MEKANIK_TRANSFER> MEKANIK_TRANSFERLIST()
        {
            string sqlCumle = "  SELECT * FROM MEKANIK_TRANSFERLIST_MKA ";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<MEKANIK_TRANSFER>(sqlCumle).ToList();
        }
        public List<ESTAP_TRANSFER> ESTAP_TRANSFERLIST()
        {
            string sqlCumle = " SELECT * FROM ESTAP_TRANSFERLIST_MKA  ";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<ESTAP_TRANSFER>(sqlCumle).ToList();
        }
        public List<CIHAZ_TRANSFER> CIHAZ_TRANSFERLIST()
        {
            string sqlCumle = " SELECT * FROM CIHAZSUSK_TRANSFERLIST_MKA  ";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<CIHAZ_TRANSFER>(sqlCumle).ToList();
        }
        public List<SUSK_LISTESI_GENEL> INFORM_SUSK_LISTESI()
        {
            string sqlCumle = "SELECT t.TARIH,t.ISEMRINO,t.STOK_KODU,miktar - isnull(susk.URETSON_MIKTAR, 0) MIKTAR FROM dbo.TBLISEMRI t LEFT OUTER JOIN(SELECT sum(URETSON_MIKTAR) URETSON_MIKTAR, URETSON_MAMUL, URETSON_SIPNO FROM INFORM20..TBLSTOKURS GROUP BY URETSON_MAMUL, URETSON_SIPNO)susk ON t.ISEMRINO = susk.URETSON_SIPNO AND t.STOK_KODU = susk.URETSON_MAMUL LEFT OUTER JOIN TBLISEMRIEK ek ON t.ISEMRINO = ek.ISEMRI WHERE t.KAPALI = 'H' and t.STOK_KODU LIKE '19%' AND miktar-isnull(susk.URETSON_MIKTAR, 0) > 0  AND ek.KT_URT_DURUM='SUSK'";
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            return context.Database.SqlQuery<SUSK_LISTESI_GENEL>(sqlCumle).ToList();
        }
        public string ISEMRINO_OLUSTUR(string isEmriilkHarf)
        {
            isEmriilkHarf = "J";

            string sonIsemriNo = "";
            int isno = 0;
            using (INF_OTOSUSK_Context context = new INF_OTOSUSK_Context())
            {
                sonIsemriNo = context.TBLISEMRI.Where(i => i.ISEMRINO.StartsWith(isEmriilkHarf + "0000000")).OrderByDescending(i => i.ISEMRINO).FirstOrDefault().ISEMRINO;
                isno = Convert.ToInt32(sonIsemriNo.Substring(6)) + 1;
            }
            string isemriNo = isno.ToString();

            if (isemriNo.Length < 15)
            {
                if (isemriNo.Length == 1)
                {
                    isemriNo = isEmriilkHarf + "0000000000000" + isemriNo;
                }
                else if (isemriNo.Length == 2)
                {
                    isemriNo = isEmriilkHarf + "000000000000" + isemriNo;
                }
                else if (isemriNo.Length == 3)
                {
                    isemriNo = isEmriilkHarf + "00000000000" + isemriNo;
                }
                else if (isemriNo.Length == 4)
                {
                    isemriNo = isEmriilkHarf + "0000000000" + isemriNo;
                }
                else if (isemriNo.Length == 5)
                {
                    isemriNo = isEmriilkHarf + "000000000" + isemriNo;
                }
                else if (isemriNo.Length == 6)
                {
                    isemriNo = isEmriilkHarf + "00000000" + isemriNo;
                }
                else if (isemriNo.Length == 7)
                {
                    isemriNo = isEmriilkHarf + "0000000" + isemriNo;
                }
                else if (isemriNo.Length == 8)
                {
                    isemriNo = isEmriilkHarf + "000000" + isemriNo;
                }
                else if (isemriNo.Length == 9)
                {
                    isemriNo = isEmriilkHarf + "00000" + isemriNo;
                }
            }
            //isemriNo = isEmriilkHarf+"00000000" + "" + isemriNo;
            return isemriNo;
        }
        public TBLISEMRI IsEmriUSK_STATUSGuncelle(string isemriNo)
        {
            try
            {
                using (INF_OTOSUSK_Context context = new INF_OTOSUSK_Context())
                {
                    var isemri = context.TBLISEMRI.Where(i => i.ISEMRINO == isemriNo).FirstOrDefault();
                    isemri.USK_STATUS = "Y";
                    isemri.KAPALI = "H";
                    context.SaveChanges();
                    return isemri;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public TBLISEMRI IsemriKapat(string isemriNo)
        {
            using (INF_OTOSUSK_Context context = new INF_OTOSUSK_Context())
            {
                var isemri = context.TBLISEMRI.Where(i => i.ISEMRINO == isemriNo).FirstOrDefault();
                isemri.KAPALI = "E";
                context.SaveChanges();
                return isemri;
            }
        }

        public int HUCREHAREKETKAYIT(string stok_kodu,decimal miktar,string fisno)
        {
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            var depUrun = context.DEPOLOKASYONDURUM_MKA.Where(a => a.STOKKODU == stok_kodu && a.DEPO_KODU == 114 && a.NETBAKIYE > 0).FirstOrDefault();
            var urun = new TBLDEPHAR();
            urun.STOKKODU = depUrun.STOKKODU;
            urun.HUCREKODU = depUrun.HUCREKODU;
            urun.NETMIKTAR = miktar;
            urun.HTUR = "B";
            urun.GC = "C";
            urun.DEPOHARFISNO = fisno;
            urun.TARIH = DateTime.Now.Date;
            urun.KAYITTARIHI = DateTime.Now.Date;
            urun.KAYITYAPANKUL = "KARAMUKLU";
            urun.STHARINC = INCKEYBUL(fisno,depUrun.STOKKODU) ;
            context.TBLDEPHAR.Add(urun);

            if (context.SaveChanges() == 1)
            {
                //MessageBox.Show("İşlem başarılı");
                var depDurum = context.DEPOLOKASYONDURUM_MKA.Where(a => a.STOKKODU == stok_kodu && a.DEPO_KODU == 114 && a.NETBAKIYE > 0).ToList();
            }
            return 1;
        }
        public int INCKEYBUL (string fisno,string stok_kodu)
        {
            INF_OTOSUSK_Context context = new INF_OTOSUSK_Context();
            var inckey = context.TBLSTHAR.Where(i => i.FISNO == fisno && i.STOK_KODU == stok_kodu && i.DEPO_KODU==114).FirstOrDefault();
            return inckey.INCKEYNO;
        }
    }
}










