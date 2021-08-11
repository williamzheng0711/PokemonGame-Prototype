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
    /// MazeCave.xaml 的互動邏輯
    /// </summary>
    public partial class MazeCave : Window
    {
        private Image[,] Maze = new Image[18, 18];
        private Image[,] Chara = new Image[18, 18];
        int direction = 2;

        public MazeCave()
        {
            InitializeComponent();
            DrawBeginPlayer();
            CaveModel.Cave.MazeDraw += Cave_MazeDraw;
            CaveModel.Cave.OnCaveStart();//Generate a new maze. 
        }

        public void DrawBeginPlayer() 
        {
            for (int nrow = 0; nrow < 18; nrow++)
            {
                for (int ncol = 0; ncol < 18; ncol++)
                {
                    Chara[nrow, ncol] = new Image();
                    Chara[nrow, ncol].Height = 25;
                    Chara[nrow, ncol].Width = 25;
                    Chara[nrow, ncol].Stretch = Stretch.Uniform;
                    if (nrow == 1 && ncol == 1) Chara[nrow,ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/mainchara/mainchara_down.png", UriKind.Relative));
                    WP_Player.Children.Add(Chara[nrow, ncol]);
                }
            }
        }

        private void Cave_MazeDraw(object source, EventArgs args, List<List<int>> newList)
        {
            WP_Maze.Children.Clear();
            for (int nrow = 0; nrow < 18; nrow++)
            {
                for (int ncol = 0; ncol < 18; ncol++)
                {
                    Maze[nrow, ncol] = new Image();
                    Maze[nrow, ncol].Height = 25;
                    Maze[nrow, ncol].Width = 25;
                    Maze[nrow, ncol].Stretch = Stretch.UniformToFill;

                    if (newList[nrow][ncol]==0) Maze[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/road_material.png", UriKind.Relative));
                    else if (newList[nrow][ncol] == 1) Maze[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/stone_cave.jpg", UriKind.Relative));

                    if (nrow == ncol && nrow == 16) Maze[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/pokeball.jpg", UriKind.Relative));
                    WP_Maze.Children.Add(Maze[nrow, ncol]);
                }
            }
        }

        private void Cave_MovePlayer(object source, EventArgs args, int[] newPos)
        {
            WP_Player.Children.Clear();
            for (int nrow = 0; nrow < 18; nrow++)
            {
                for (int ncol = 0; ncol < 18; ncol++)
                {
                    Chara[nrow, ncol] = new Image();
                    Chara[nrow, ncol].Height = 25;
                    Chara[nrow, ncol].Width = 25;
                    Chara[nrow, ncol].Stretch = Stretch.Uniform;
                    if (nrow == newPos[0] && ncol == newPos[1])
                    {
                        if (direction == 2) Chara[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/mainchara/mainchara_down.png", UriKind.Relative));
                        if (direction == 1) Chara[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/mainchara/mainchara_up.png", UriKind.Relative));
                        if (direction == 3) Chara[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/mainchara/mainchara_left.png", UriKind.Relative));
                        if (direction == 4) Chara[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/mainchara/mainchara_right.png", UriKind.Relative));
                    }
                    WP_Player.Children.Add(Chara[nrow, ncol]);
                }
            }
            CaveModel.Cave.MovePlayer -= Cave_MovePlayer;
        }

        private void Cave_EndCave(object source, EventArgs args, string name)
        {
            if (name == "null")
            {
                MessageBox.Show("You escaped from the cave.");
                this.Close();
            }
            else
            {
                Reward reward = new Reward();
                if (name == "Pichu") reward.PokeShow.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/011Pichu.png", UriKind.Relative));
                else if (name == "Pikachu") reward.PokeShow.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/012Pikachu.png", UriKind.Relative));
                else if (name == "Raichu") reward.PokeShow.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/013Raichu.png", UriKind.Relative));
                else if (name == "Zubat") reward.PokeShow.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/021Zubat.png", UriKind.Relative));
                else if (name == "Golbat") reward.PokeShow.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/022Golbat.png", UriKind.Relative));
                else if (name == "Magikarp") reward.PokeShow.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/031Magikarp.png", UriKind.Relative));
                else if (name == "Gyarados") reward.PokeShow.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/032Gyarados.png", UriKind.Relative));
                else if (name == "Bulbasaur") reward.PokeShow.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/041Bulbasaur.png", UriKind.Relative));
                else if (name == "Ivysaur") reward.PokeShow.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/042Ivysaur.png", UriKind.Relative));
                else if (name == "Venusaur") reward.PokeShow.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/043Venusaur.png", UriKind.Relative));

                reward.Pokemon_Name.Content = name;
                reward.Show();
                this.Close();
            }
        }

        private void MainWindows_Keydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S)
                DOWN_Click(sender, e);
            if (e.Key == Key.W)
                Button_Click(sender, e);
            if (e.Key == Key.A)
                LEFT_Click(sender, e);
            if (e.Key == Key.D)
                RIGHT_Click(sender, e);
        }
            private void Button_Click(object sender, RoutedEventArgs e) //up
        {
            direction = 1;
            CaveModel.Cave.MovePlayer += Cave_MovePlayer;
            CaveModel.Cave.EndCave += Cave_EndCave;

            CaveModel.Cave.OnPlayerMove("up");

            CaveModel.Cave.MovePlayer -= Cave_MovePlayer;
            CaveModel.Cave.EndCave -= Cave_EndCave;

        }

        private void LEFT_Click(object sender, RoutedEventArgs e)
        {
            direction = 3;
            CaveModel.Cave.MovePlayer += Cave_MovePlayer;
            CaveModel.Cave.EndCave += Cave_EndCave;

            CaveModel.Cave.OnPlayerMove("left");

            CaveModel.Cave.MovePlayer -= Cave_MovePlayer;
            CaveModel.Cave.EndCave -= Cave_EndCave;

        }

        private void RIGHT_Click(object sender, RoutedEventArgs e)
        {
            direction = 4;
            CaveModel.Cave.MovePlayer += Cave_MovePlayer;
            CaveModel.Cave.EndCave += Cave_EndCave;
            //PlayerModel.PlayerEvents.PokemonInventoryChanged += PlayerEvents_PokemonInventoryChanged; ;

            CaveModel.Cave.OnPlayerMove("right");

            CaveModel.Cave.MovePlayer -= Cave_MovePlayer;
            CaveModel.Cave.EndCave -= Cave_EndCave;

        }

        private void DOWN_Click(object sender, RoutedEventArgs e)
        {
            direction = 2;
            CaveModel.Cave.MovePlayer += Cave_MovePlayer;
            CaveModel.Cave.EndCave += Cave_EndCave;
            //PlayerModel.PlayerEvents.PokemonInventoryChanged += PlayerEvents_PokemonInventoryChanged;

            CaveModel.Cave.OnPlayerMove("down");

            CaveModel.Cave.MovePlayer -= Cave_MovePlayer;
            CaveModel.Cave.EndCave -= Cave_EndCave;

        }

        private void ClicktoEscape_Click(object sender, RoutedEventArgs e)
        {
            CaveModel.Cave.EndCave += Cave_EndCave;

            CaveModel.Cave.OnPlayerLeft();

            CaveModel.Cave.EndCave -= Cave_EndCave;
        }
    }
}
