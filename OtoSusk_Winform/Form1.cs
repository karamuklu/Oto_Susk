using INF_OTO_SUSK_BusinessLayer;
using INF_OTO_SUSK_Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtoSusk_Winform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public INF_SUSK_Manager infSusk_Manager = new INF_SUSK_Manager();
        private void button1_Click(object sender, EventArgs e)
        {
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

            var Cihaz_list = infSusk_Manager.CIHAZ_TRANSFERLIST();
            if (Cihaz_list.Count != 0)
            {
                infSusk_Manager.LokalDepolarArasiTransferBelgesi(Cihaz_list);
            }



        }

        
    }
}
