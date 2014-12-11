using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
namespace generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker worker;
        private List<string> imiona;
        private List<string> nazwiska;
        private List<string> przedmioty = new List<string>();
        private List<KeyValuePair<string, List<string>>> katedry = new List<KeyValuePair<string, List<string>>>();

        private List<KeyValuePair<string, List<KeyValuePair<string, List<string>>>>> wydzialy =
            new List<KeyValuePair<string, List<KeyValuePair<string, List<string>>>>>();

        private Random rand = new Random((int) DateTime.Now.Ticks);
        private int iloscProwadzacych = 5;
        private int id = 0;
        private List<Prowadzacy> wylosowaniProwadzacy = new List<Prowadzacy>();
        private List<Przedmiot> wylosowanePrzedmioty = new List<Przedmiot>();
        private List<SkladowaPrzedmiotu> wylosowaneSkladowePrzedmiotu = new List<SkladowaPrzedmiotu>();
        private List<ProwadzacySkladowych> wylosowaniProwadzacySkladowych = new List<ProwadzacySkladowych>();
        private List<Wyniki> wylosowaneWyniki = new List<Wyniki>();
        private List<Studenci> wylosowaniStudenci = new List<Studenci>();
        private List<RodzajSkladowych> wylosowaneRodzajeSkladowych;
        private List<Arkusz1> wylosowaneWierszeArkuszu1 = new List<Arkusz1>();
        private List<Arkusz2> wylosowaneWierszeArkuszu2 = new List<Arkusz2>();

        private void initwydzialy()
        {
            string tmp = System.IO.File.ReadAllText("przedmioty.txt", Encoding.GetEncoding("windows-1250"));
            string[] kat = tmp.Split('&');
            foreach (string k in kat)
            {
                if (k != "")
                {
                    string[] przed = k.Split(new char[] {'\n', '\r', '\t'});
                    katedry.Add(new KeyValuePair<string, List<string>>(przed[0],
                        przed.ToList().Where((x) => !x.Contains("Katedra") && x != "").ToList()));
                    przedmioty.AddRange(przed.ToList().Where((x) => !x.Contains("Katedra") && x != ""));
                }
            }
            wydzialy.Add(new KeyValuePair<string, List<KeyValuePair<string, List<string>>>>("Wydział Architektury",
                new List<KeyValuePair<string, List<string>>>(katedry.GetRange(0, 4))));
            wydzialy.Add(
                new KeyValuePair<string, List<KeyValuePair<string, List<string>>>>(
                    "Wydział Elektroniki, Telekomunikacji i Informatyki",
                    new List<KeyValuePair<string, List<string>>>(katedry.GetRange(4, 4))));
            wydzialy.Add(
                new KeyValuePair<string, List<KeyValuePair<string, List<string>>>>("Wydział Zarządzania i Ekonomii",
                    new List<KeyValuePair<string, List<string>>>(katedry.GetRange(8, 3))));
        }

        private void initImionaNazwiska()
        {
            string tmp = System.IO.File.ReadAllText("imiona.txt", Encoding.GetEncoding("windows-1250"));
            imiona = tmp.Split(new char[] {'\n', '\r', '\t'}).Where((x) => x != "").ToList();
            tmp = System.IO.File.ReadAllText("nazwiska.txt", Encoding.GetEncoding("windows-1250"));
            nazwiska = tmp.Split(new char[] {'\n', '\r', '\t'}).Where((x) => x != "").ToList();
        }

        public MainWindow()
        {
            InitializeComponent();
            initwydzialy();
            initImionaNazwiska();
            losujRodzajeSkladowych();
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = false;
            worker.DoWork += zapisz;
            worker.ProgressChanged += (x, y) =>
            {
                bar1.Value = y.ProgressPercentage;
            };
            worker.RunWorkerCompleted += (x, y) =>
            {
                button.IsEnabled = true;
            };
            button.Content = "Generuj";
        }

        private void losujProwadzacych()
        {
            string[] tytuly = new string[3] {"mgr", "dr", "prof"};
            for (int i = 0; i < iloscProwadzacych; i++)
            {
                int wiek = rand.Next(30, 60);
                var tmp = wydzialy.ToArray()[rand.Next(0, wydzialy.Count)];
                string wydzial = tmp.Key;
                string katedra = tmp.Value.ToArray()[rand.Next(0, tmp.Value.Count)].Key;
                string pesel = rand.Next(10, 99) + "0" + rand.Next(1, 10) + "" + rand.Next(10, 28) + "" + rand.Next(10000, 99999).ToString();
                wylosowaniProwadzacy.Add(new Prowadzacy(id, pesel, tytuly[rand.Next(0, 3)], wiek - 30, wiek,
                    imiona.ToArray()[rand.Next(0, imiona.Count)], nazwiska.ToArray()[rand.Next(0, nazwiska.Count)],
                    katedra, wydzial));
                id++;
            }
        }

        private void losujPrzedmioty()
        {
            foreach (var przed in przedmioty)
            {
                if(!wylosowanePrzedmioty.Exists((x)=>x.nazwa==przed))
                    wylosowanePrzedmioty.Add(new Przedmiot(przed, rand.Next(1, 11), 15*rand.Next(1, 5), rand.Next(1, 7),
                        wylosowaniProwadzacy.ToArray()[rand.Next(0, wylosowaniProwadzacy.Count)].id));
            }
        }

        private void losujRodzajeSkladowych()
        {
            wylosowaneRodzajeSkladowych = new List<RodzajSkladowych>();
            wylosowaneRodzajeSkladowych.Add(new RodzajSkladowych("projekt"));
            wylosowaneRodzajeSkladowych.Add(new RodzajSkladowych("laboratorium"));
            wylosowaneRodzajeSkladowych.Add(new RodzajSkladowych("ćwiczenia"));
            wylosowaneRodzajeSkladowych.Add(new RodzajSkladowych("wykład"));
        }

        private void losujSkladowePrzedmiotu()
        {
            foreach (var przedmiot in wylosowanePrzedmioty)
            {
                int iloscgodzin = przedmiot.godziny;
                for (int i = 0; i < 4 && iloscgodzin != 0; i++)
                {
                    int tmp = (iloscgodzin > 15) ? rand.Next(1,1+ iloscgodzin/15)*15 : iloscgodzin;
                    iloscgodzin -= tmp;
                    wylosowaneSkladowePrzedmiotu.Add(new SkladowaPrzedmiotu(id, przedmiot.nazwa,
                        wylosowaneRodzajeSkladowych.ToArray()[rand.Next(0, 4)].nazwa, tmp,
                        wylosowaniProwadzacy.ToArray()[rand.Next(0, wylosowaniProwadzacy.Count)].id));
                    id++;
                }
            }
        }

        private void losujProwadzacychSkladowych()
        {
            foreach (var prowadzacy in wylosowaniProwadzacy)
            {
                for (int i = 0; i < 10; i++)
                {
                    var tmp=new ProwadzacySkladowych(wylosowaneSkladowePrzedmiotu.ToArray()[rand.Next(0, wylosowaneSkladowePrzedmiotu.Count)].idskladowej, prowadzacy.id);
                    if(!wylosowaniProwadzacySkladowych.Exists((x)=>x.idprowadzacego==tmp.idprowadzacego&&x.idskladowej==tmp.idskladowej))
                        wylosowaniProwadzacySkladowych.Add(tmp);
                }
            }
        }

        private void losujStudentow()
        {
            for (int i = 0; i < 100*iloscProwadzacych; i++)
            {
                int rok = rand.Next(1, 5);
                int semestr = (rok - 1)*2 + rand.Next(1, 2);
                int dlug = rand.Next(0, 20);
                string pesel = rand.Next(10, 99)+"0" + rand.Next(1, 10)+"" + rand.Next(10, 28)+""+ rand.Next(10000, 99999).ToString();
                wylosowaniStudenci.Add(new Studenci(id, pesel, imiona.ToArray()[rand.Next(0, imiona.Count)],
                    nazwiska.ToArray()[rand.Next(0, nazwiska.Count)], rok, semestr, dlug));
                id++;
            }
        }

        private void losujWyniki()
        {
            foreach (var studenci in wylosowaniStudenci)
            {
                for (int i = 0; i < studenci.semestr*5; i++)
                {
                    var tmp=new Wyniki(wylosowanePrzedmioty.ToArray()[rand.Next(0, wylosowanePrzedmioty.Count)].nazwa,
                            studenci.nrindeksu, rand.Next(4, 12)*0.5f);
                    if(wylosowaneWyniki.Exists((x)=>x.indeksstudenta==tmp.indeksstudenta&&x.nazwaprzedmiotu==tmp.nazwaprzedmiotu))
                        wylosowaneWyniki.Find((x)=>x.indeksstudenta==tmp.indeksstudenta&&x.nazwaprzedmiotu==tmp.nazwaprzedmiotu).wynik=tmp.wynik;
                    else
                        wylosowaneWyniki.Add(tmp);
                }
            }
        }

        private void losujProtokoly1()
        {
            foreach (var prowadzacySkladowych in wylosowaniProwadzacySkladowych)
            {
                int tmp = rand.Next(1, 5);
                for (int i = 0; i < tmp; i++)
                {
                    wylosowaneWierszeArkuszu1.Add(new Arkusz1(id, prowadzacySkladowych.idskladowej,
                        prowadzacySkladowych.idprowadzacego, rand.Next(1,23)));
                    id++;
                }
            }
        }

        private void losujProtokoly2()
        {
            var studenci = new List<Studenci>(wylosowaniStudenci);
            foreach (var arkusz1 in wylosowaneWierszeArkuszu1)
            {
                SkladowaPrzedmiotu tmp = wylosowaneSkladowePrzedmiotu.Find((x) => x.idskladowej == arkusz1.idskladowej);
                int ilosczajec = (int) (tmp.iloscgodzin/1.5f);
                string[] arrayterminy = new string[ilosczajec];
                float[] uzyskanewyniki = new float[ilosczajec];
                float[] mozliwepunkty = new float[ilosczajec];        
                for (int j = 0; j < 10 && studenci.Count > 0; j++)
                {
                    var student = studenci.ToArray()[rand.Next(0, studenci.Count)];
                    for (int i = 0; i < arrayterminy.Length; i++)
                    {
                        int rok = rand.Next(12, 14);
                        int dzien = rand.Next(1, 28);
                        string data = "20" + rok + "-" +
                                      (rok == 12 ? rand.Next(10, 13).ToString() : ("0" + rand.Next(1, 7))) + "-" +
                                      (dzien < 10 ? "0" + dzien:dzien.ToString())+" 00:00:00.000";
                        arrayterminy[i] = rand.Next(0, 10) != 0 ? "TAK" : "NIE";
                        mozliwepunkty[i] = (float) rand.NextDouble()*10.0f;
                        uzyskanewyniki[i] = rand.Next(30,100)*0.01f*mozliwepunkty[i];
                        wylosowaneWierszeArkuszu2.Add(new Arkusz2(student.nrindeksu, arkusz1.idterminu, arrayterminy[i],
                        uzyskanewyniki[i], mozliwepunkty[i], data));
                    }
                    
                    studenci.Remove(student);
                }
            }
        }

        private void aktualizujProwadzacych()
        {
            for (int i = 0; i < rand.Next(0, wylosowaniProwadzacy.Count/10); i++)
            {
                var prowadzacy = wylosowaniProwadzacy.ToArray()[rand.Next(0, wylosowaniProwadzacy.Count)];
                switch (prowadzacy.tytul)
                {
                    case "mgr":
                        prowadzacy.tytul = "dr";
                        break;
                    case "dr":
                        prowadzacy.tytul = "prof";
                        break;
                }
            }
            for (int i = 0; i < rand.Next(0, wylosowaniProwadzacy.Count / 10); i++)
            {
                var prowadzacy = wylosowaniProwadzacy.ToArray()[rand.Next(0, wylosowaniProwadzacy.Count)];
                prowadzacy.nazwisko = nazwiska.ToArray()[rand.Next(0, nazwiska.Count)];
            }
            for (int i = 0; i < rand.Next(0, wylosowaniProwadzacy.Count / 10); i++)
            {
                var prowadzacy = wylosowaniProwadzacy.ToArray()[rand.Next(0, wylosowaniProwadzacy.Count)];
                prowadzacy.wiek += 1;
                prowadzacy.staz += 1; 
            }
            for (int i = 0; i < rand.Next(0, wylosowaniProwadzacy.Count / 20); i++)
            {
                var prowadzacy = wylosowaniProwadzacy.ToArray()[rand.Next(0, wylosowaniProwadzacy.Count)];
                wylosowaniProwadzacy.Remove((prowadzacy));
            }
        }

        private void aktualizujProwadzacychSkladowych()
        {
            for (int i = 0; i < rand.Next(0, wylosowaniProwadzacySkladowych.Count / 10); i++)
            {
                var prowadzacy = wylosowaniProwadzacySkladowych.ToArray()[rand.Next(0, wylosowaniProwadzacySkladowych.Count)];
                prowadzacy.idskladowej = wylosowaneSkladowePrzedmiotu.ToArray()[rand.Next(0, wylosowaneSkladowePrzedmiotu.Count)].idskladowej;

            }
        }

        private void aktualizujStudentow()
        {
            foreach (var wynik in wylosowaneWyniki.FindAll((x) => x.wynik < 3.0f))
            {
                var student = wylosowaniStudenci.Find((x) => x.nrindeksu == wynik.indeksstudenta);
                student.dlugects += wylosowanePrzedmioty.Find((x) => x.nazwa == wynik.nazwaprzedmiotu).ects;
            }
            foreach (var studenci in wylosowaniStudenci)
            {
                if (studenci.semestr < 10)
                {
                    studenci.semestr ++;
                    studenci.rok = studenci.semestr/2 + 1;
                }
            }
            wylosowaniStudenci.RemoveAll((x) => x.dlugects > 20);
        }

        private void onClick(object sender, RoutedEventArgs e)
        {
            if (button.Content == "Generuj")
                button.Content = "Generuj drugi raz";
            else
                button.Content = "Generuj";
            button.IsEnabled = false;
            iloscProwadzacych = Convert.ToInt32(textbox.Text);
            worker.RunWorkerAsync();
            aktualizujProwadzacych();
            aktualizujProwadzacychSkladowych();
            aktualizujStudentow();
        }

        private void zapisz(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker w = sender as BackgroundWorker;
            losujProwadzacych();
            losujPrzedmioty();
            
            losujSkladowePrzedmiotu();
            losujProwadzacychSkladowych();
            losujStudentow();
            losujWyniki();
            losujProtokoly1();
            losujProtokoly2();
            w.ReportProgress(10);
            Thread.Sleep(10);

            float progress = wylosowanePrzedmioty.Count + wylosowaniProwadzacy.Count + wylosowaneRodzajeSkladowych.Count +
                           wylosowaneSkladowePrzedmiotu.Count + wylosowaneWyniki.Count +
                           wylosowaniProwadzacySkladowych.Count + wylosowaniStudenci.Count +
                           wylosowaneWierszeArkuszu1.Count + wylosowaneWierszeArkuszu2.Count;
            float tmpprogress = 0;
            string text = wylosowaniProwadzacy.Aggregate("", (current, x) => current + x.toString2());
            System.IO.File.WriteAllText("prowadzacy.bulk", text);
            tmpprogress += wylosowaniProwadzacy.Count;
            w.ReportProgress(10+(int)((tmpprogress/progress)*90));
            Thread.Sleep(10);

            text = wylosowanePrzedmioty.Aggregate("", (current, x) => current + x.toString2());
            System.IO.File.WriteAllText("przedmioty.bulk", text);
            tmpprogress += wylosowanePrzedmioty.Count;
            w.ReportProgress(10 + (int)((tmpprogress / progress) * 90.0));
            Thread.Sleep(10);

            text = wylosowaneRodzajeSkladowych.Aggregate("", (current, x) => current + x.toString2());
            System.IO.File.WriteAllText("rodzaje_skladowych.bulk", text);
            tmpprogress += wylosowaneRodzajeSkladowych.Count;
            w.ReportProgress(10 + (int)((tmpprogress / progress) * 90));
            Thread.Sleep(10);

            text = wylosowaneSkladowePrzedmiotu.Aggregate("", (current, x) => current + x.toString2());
            System.IO.File.WriteAllText("skladowe_przedmiotow.bulk", text);
            tmpprogress += wylosowaneSkladowePrzedmiotu.Count;
            w.ReportProgress(10 + (int)((tmpprogress / progress) * 90));
            Thread.Sleep(10);

            text = wylosowaneWyniki.Aggregate("", (current, x) => current + x.toString2());
            System.IO.File.WriteAllText("wyniki.bulk", text);
            tmpprogress += wylosowaneWyniki.Count;
            w.ReportProgress(10 + (int)((tmpprogress / progress) * 90));
            Thread.Sleep(10);

            text = wylosowaniProwadzacySkladowych.Aggregate("", (current, x) => current + x.toString2());
            System.IO.File.WriteAllText("prowadzacy_skladowych_czesci.bulk", text);
            tmpprogress += wylosowaniProwadzacySkladowych.Count;
            w.ReportProgress(10 + (int)((tmpprogress / progress) * 90));
            Thread.Sleep(10);

            text = wylosowaniStudenci.Aggregate("", (current, x) => current + x.toString2());
            System.IO.File.WriteAllText("studenci.bulk", text);
            tmpprogress += wylosowaniStudenci.Count;
            w.ReportProgress(10 + (int)((tmpprogress / progress) * 90));
            Thread.Sleep(10);

            text = wylosowaneWierszeArkuszu1.Aggregate("", (current, x) => current + x.toString2());
            System.IO.File.WriteAllText("arkusz1.csv", text);
            tmpprogress += wylosowaneWierszeArkuszu1.Count;
            w.ReportProgress(10 + (int)((tmpprogress / progress) * 90));
            Thread.Sleep(10);

            text = wylosowaneWierszeArkuszu2.Aggregate("", (current, x) => current + x.toString2());
            System.IO.File.WriteAllText("arkusz2.csv", text);
            tmpprogress += wylosowaneWierszeArkuszu2.Count;
            w.ReportProgress(10 + (int)((tmpprogress / progress) * 90));
            Thread.Sleep(10);
        }
    }
}
