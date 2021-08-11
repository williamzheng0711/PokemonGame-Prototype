using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IERG3080_POKEMON
{
    /// <summary>
    /// Capture.xaml 的互動邏輯
    /// </summary>
    public partial class Capture : Window
    {
        public bool Success = false;

        public string Answer = "Random";

        public Capture(string name)
        {
            InitializeComponent();
            Answer = name;
            if (name == "Pichu") IMG.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/011Pichu.png", UriKind.Relative));
            else if (name == "Pikachu") IMG.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/012Pikachu.png", UriKind.Relative));
            else if (name == "Raichu") IMG.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/013Raichu.png", UriKind.Relative));
            else if (name == "Zubat") IMG.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/021Zubat.png", UriKind.Relative));
            else if (name == "Golbat") IMG.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/022Golbat.png", UriKind.Relative));
            else if (name == "Magikarp") IMG.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/031Magikarp.png", UriKind.Relative));
            else if (name == "Gyarados") IMG.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/032Gyarados.png", UriKind.Relative));
            else if (name == "Bulbasaur") IMG.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/041Bulbasaur.png", UriKind.Relative));
            else if (name == "Ivysaur") IMG.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/042Ivysaur.png", UriKind.Relative));
            else if (name == "Venusaur") IMG.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/043Venusaur.png", UriKind.Relative));
            namepoke.Content = name;

        }

        private void Leave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CaptureIt_Click(object sender, RoutedEventArgs e)
        {
            if (InputBox.Text == Answer) { Success = true; this.Close(); }
            else { MessageBox.Show(Answer + "fled, too late......"); this.Close(); }
        }
    }
}
