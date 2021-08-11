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
    /// Rename.xaml 的互動邏輯
    /// </summary>
    public partial class Rename : Window
    {
        public Rename(int ID)
        {
            InitializeComponent();
            Num = ID;
        }

        public bool named = false;
        public string returnname;
        public int Num;

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (newname.Text != null) 
            { 
                named = true; 
                returnname = newname.Text; 
                PlayerModel.Player.Main.OnPokemonRenamed(Num, returnname); 
            }
            this.Close();
        }
    }
}
