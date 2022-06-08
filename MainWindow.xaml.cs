using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace WpfApp10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public class KolekcjaDanych
        {
            [XmlArray("Ubrania"), XmlArrayItem("Pozycja")]
            static List<Ubrania> listaUbran;
            Ubrania[] tablicaUbran = null;

            public KolekcjaDanych(List<Ubrania> list)
            {
                listaUbran = list;
                tablicaUbran = list.ToArray();
            }
            public KolekcjaDanych(Ubrania[] arr)
            {
                listaUbran = new List<Ubrania>(arr);
                tablicaUbran = arr;
            }

            public KolekcjaDanych() { }


            public static List<Ubrania> FromCSVToList(string path)
            {
                string[] fileContent = null;
                List<Ubrania> nowaLista = new List<Ubrania>();
                try
                {
                    fileContent = File.ReadAllLines(path);
                    // Z pominięciem nagłówka
                    for (int i = 1; i < fileContent.Length; i++)
                    {
                        string[] tab = fileContent[i].Split(';');
                        if (int.TryParse(tab[0], out int id) && Double.TryParse(tab[3], out double cena))
                        {
                            string plec = (tab[4].IndexOf('M') == 0) ? "Męskie" : "Damskie";
                            nowaLista.Add(new Ubrania { Id = id, Kod = tab[1], Producent = tab [2], Cena = double.Parse(tab[3]), Kategoria = plec, Podkategoria = tab[5] });
                            //MessageBox.Show("Udalo sie");
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                listaUbran = nowaLista;
                return nowaLista;
            }

            public static Ubrania[] FromCSVToArr(string path)
            {
                string[] fileContent = null;
                Ubrania[] nowaTablica = { };
                List<Ubrania> temp = new List<Ubrania>();
                try
                {
                    fileContent = File.ReadAllLines(path);
                    // Z pominięciem nagłówka
                    for (int i = 1; i < fileContent.Length; i++)
                    {
                        string[] tab = fileContent[i].Split(';');
                        if (int.TryParse(tab[0], out int id) && Double.TryParse(tab[3], out double cena))
                        {
                            string plec = (tab[4].IndexOf('M') == 0) ? "Męskie" : "Damskie";
                            temp.Add(new Ubrania { Id = id, Kod = tab[1], Producent = tab[2], Cena = double.Parse(tab[3]), Kategoria = plec, Podkategoria = tab[5] });
                            //nowaTablica[i] = new Ubrania { Id = id, Kod = tab[1], Producent = tab[2], Cena = cena, Kategoria = tab[4], Podkategoria = tab[5] };
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                nowaTablica = temp.ToArray();
                return nowaTablica;
            }


            public static void ToCSV() 
            {
                
                using (FileStream fs = new FileStream(@"CSVFromList.csv", FileMode.Create, FileAccess.Write)) { }
                using (StreamWriter sw = new StreamWriter(@"CSVFromList.csv"))
                {
                    foreach(Ubrania u1 in listaUbran)
                    {
                        string line = $"{u1.Id};{u1.Kod};{u1.Producent};{u1.Cena};{u1.Kategoria};{u1.Podkategoria}";
                        sw.WriteLine(line);
                        sw.Flush();
                    }
                }
            }


            public static void ToXML<T>(T obj)
            {
                //zapisLista.Save(@"ToXML.xml");

                string path = @"ToXML.xml";
                XmlSerializer xSer = new XmlSerializer(typeof(T));
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    xSer.Serialize(fs, obj);
                }

                
            }


        }
        //public static XElement zapisLista;
        public MainWindow()
        {
            InitializeComponent();
            InitBinding();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Podglad.ItemsSource);

            //view.Filter = UserFilter;

            FrameworkElement a = (FrameworkElement)Ubrania.DataContext;
            
        }

        public void InitBinding()
        {
            // Plik CSV
            string path = @"ubrania.csv";

            //List toList 
            List<Ubrania> nowaLista = KolekcjaDanych.FromCSVToList(path);

            //Array toArray
            //Ubrania[] nowaTablica = KolekcjaDanych.FromCSVToArr(path);

            //To CSV
            KolekcjaDanych.ToCSV();
            KolekcjaDanych.ToXML<Ubrania>();



            Ubrania.ItemsSource = nowaLista;
            //Ubrania.ItemsSource = nowaTablica;
            //Podglad.ItemsSource = nowaTablica;


        }

        private bool UserFilter(object item)
        {
            if (cbKat.SelectedIndex == -1)
                return true;
            else 
            { 
                ComboBoxItem it = (ComboBoxItem)cbKat.SelectedItem;
                string kat = it.Content.ToString();


                return ((item as Ubrania).Kategoria.IndexOf(kat, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void btnDane_Click(object sender, RoutedEventArgs e)
        {
            //CollectionViewSource.GetDefaultView(Podglad.ItemsSource).Refresh();
          
        }

        public void btnZapisCSV_Click(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = new FileStream(@"CSVFromList.csv", FileMode.Create, FileAccess.Write)) { }
            using (StreamWriter sw = new StreamWriter(@"CSVFromList.csv"))wi    
            {
                foreach (Ubrania u1 in Ubrania.ItemsSource)
                {
                    string line = $"{u1.Id};{u1.Kod};{u1.Producent};{u1.Cena};{u1.Kategoria};{u1.Podkategoria}";
                    sw.WriteLine(line);
                    sw.Flush();
                }
            }
        }

        public void btnZapisXML_Click(object sender, RoutedEventArgs e)
        {
            //zapisLista.Save(@"ToXML.xml");
        }


    }
}
