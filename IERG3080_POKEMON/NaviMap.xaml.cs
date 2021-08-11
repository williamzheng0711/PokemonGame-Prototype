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
using System.Timers;

namespace IERG3080_POKEMON
{
    /// <summary>
    /// NaviMap.xaml 的互動邏輯
    /// </summary>
    public partial class NaviMap : Window
    {
        int direction = 2;
        private Image[,] PicMat = new Image[10, 10];
        private Image[,] CharaPosMat = new Image[10, 10]; //where the main chara is, where the value is 1.
        private Image[,] PokemonPosMat = new Image[10, 10]; //where the pokemon is, where the value is 1.
        //public int CharaPosX = 5;
        //public int CharaPosY = 5;

        

        public NaviMap()
        {
            InitializeComponent();
            LinkListBox();
            DrawPokemonCanvas();
            //GivingInitialPokemon();
            
            ImgPoke.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/guess_poke.jpg", UriKind.Relative));

            MapModel.Map.MapChanged += Map_MapChanged1;
            MapModel.MapEvents.ChangeLocation += Map_ChangeLocation;
            MapModel.Map.DisplayPokemon += Map_DisplayPokemon;
            PlayerModel.PlayerEvents.PokemonInventoryChanged += PlayerEvents_PokemonInventoryChanged;
            MapModel.MapEvents.TimerNullify += Map_TimerNullify;
            PlayerModel.PlayerEvents.BattleReadyChanged += PlayerEvents_BattleReadyChanged;

            MapModel.Map map= MapModel.Map.Main;
        }

        private void MapEvents_GymStepped(object source, EventArgs args)
        {
            MessageBox.Show(" Are you ready to have a gym battle?\n The chosen at most three Pokemon will be on stage. \n Winning a gym can gain much experience! Even if you lose you gain some!");
            if (PlayerModel.Player.Main.BattleReadyPokemon.Count < 2)
            {
                MessageBox.Show("You'd prepare at least 2 battle-ready pokemons!");
            }
            else 
            {
                GymBattle newBattle = new GymBattle();
                newBattle.ShowDialog(); // Then the user need to focus on the newBattle windows at this moment.
            }
            MapModel.Map.GymStepped -= MapEvents_GymStepped;
        }

        private void Map_CaveStepped(object source, EventArgs args)
        {
            MessageBox.Show("Unknown cave here... Don't regret if you enter here!!");
            MazeCave newCave = new MazeCave();
            newCave.ShowDialog();
            //throw new NotImplementedException();

            //PokeShowBox.ItemsSource = PlayerModel.Player.Main.PokemonToDisplay;

            MapModel.Map.CaveStepped -= Map_CaveStepped;
        }

        private void DrawPokemonCanvas() 
        {
            WP_PKM.Children.Clear();
            for (int nrow = 0; nrow < 10; nrow++) 
            {
                for (int ncol = 0; ncol < 10; ncol++) 
                {
                    PokemonPosMat[nrow, ncol] = new Image();
                    WP_PKM.Children.Add(PokemonPosMat[nrow, ncol]);        
                }
            }
        }

        private void Map_PokemonStepped(object source, EventArgs args, string name, int[] pos)
        {
            //PokemonPosMat[pos[0], pos[1]].Source = null;
            //MessageBox.Show("U Steped on me!");
            WP_PKM.Children.Clear();
            for (int nrow = 0; nrow < 10; nrow++) // to print the pokemon in its position
            {
                for (int ncol = 0; ncol < 10; ncol++)
                {
                    if (pos[0] == nrow && pos[1] == ncol)
                    {
                        PokemonPosMat[nrow, ncol] = new Image();
                        PokemonPosMat[nrow, ncol].Height = 40;
                        PokemonPosMat[nrow, ncol].Width = 40;
                        PokemonPosMat[nrow, ncol].Opacity = 1;
                        PokemonPosMat[nrow, ncol].Stretch = Stretch.UniformToFill;
                    }
                    WP_PKM.Children.Add(PokemonPosMat[nrow, ncol]);
                }
            }

            Capture newCapture = new Capture(name);
            newCapture.ShowDialog();
            if (newCapture.Success == true)
            {
                PlayerModel.PlayerEvents.PokemonInventoryChanged += PlayerEvents_PokemonInventoryChanged;
                MapModel.Map.OnPokemonCaptured();
            }
        }

        private void Map_TimerNullify(object source, EventArgs args)
        {
            timer.Stop(); timer.Dispose();
            MapModel.Map.OnCanSpawnPokemon();

            MapModel.MapEvents.TimerNullify -= Map_TimerNullify;
        }

        Timer timer = new Timer();
        //DispatcherTimer Timer1 = new DispatcherTimer();
        private void Map_DisplayPokemon(object source, EventArgs args, string name, int[] position, double setTimer)
        {
            //MessageBox.Show("Display!");
            for (int nrow = 0; nrow < 10; nrow++) // to print the pokemon in its position
            {
                for (int ncol = 0; ncol < 10; ncol++)
                {
                    PokemonPosMat[nrow, ncol].Height = 40;
                    PokemonPosMat[nrow, ncol].Width = 40;
                    PokemonPosMat[nrow, ncol].Opacity = 1;
                    PokemonPosMat[nrow, ncol].Stretch = Stretch.UniformToFill;

                    if (position[0] == nrow && position[1] == ncol)
                    {
                        if (name == "Pichu") PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/011Pichu.png", UriKind.Relative));
                        else if (name == "Pikachu") PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/012Pikachu.png", UriKind.Relative));
                        else if (name == "Raichu") PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/013Raichu.png", UriKind.Relative));
                        else if (name == "Zubat") PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/021Zubat.png", UriKind.Relative));
                        else if (name == "Golbat") PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/022Golbat.png", UriKind.Relative));
                        else if (name == "Magikarp") PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/031Magikarp.png", UriKind.Relative));
                        else if (name == "Gyarados") PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/032Gyarados.png", UriKind.Relative));
                        else if (name == "Bulbasaur") PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/041Bulbasaur.png", UriKind.Relative));
                        else if (name == "Ivysaur") PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/042Ivysaur.png", UriKind.Relative));
                        else if (name == "Venusaur") PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/043Venusaur.png", UriKind.Relative));
                        else PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/guess_poke.jpg", UriKind.Relative));
                        //else PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/043Venusaur.png", UriKind.Relative));
                        //PokemonPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/043Venusaur.png", UriKind.Relative));
                    }
                }
            }
            timer.Interval = setTimer;
            MapModel.MapEvents.PokemonStepped -= Map_PokemonStepped;
            MapModel.MapEvents.GymStepped -= MapEvents_GymStepped;
        }

        private void Map_ChangeLocation(object source, EventArgs args, int[] newPos) //Draw the player
        {
            WP1.Children.Clear();//ensure that inside the WP1 there is always only 100 elements. 
            //MessageBox.Show(newMap[2][2].ToString());
            //throw new NotImplementedException();
            for (int nrow = 0; nrow < 10; nrow++)
            {
                for (int ncol = 0; ncol < 10; ncol++)
                {
                    CharaPosMat[nrow, ncol] = new Image();
                    CharaPosMat[nrow, ncol].Height = 40;
                    CharaPosMat[nrow, ncol].Width = 40;
                    CharaPosMat[nrow, ncol].Opacity = 1;
                    if (nrow == newPos[0] && ncol == newPos[1])
                    {
                        if (direction == 1) CharaPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/mainchara/mainchara_up.png", UriKind.Relative));
                        if (direction == 2) CharaPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/mainchara/mainchara_down.png", UriKind.Relative));
                        if (direction == 3) CharaPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/mainchara/mainchara_left.png", UriKind.Relative));
                        if (direction == 4) CharaPosMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/mainchara/mainchara_right.png", UriKind.Relative));
                    }
                    WP1.Children.Add(CharaPosMat[nrow, ncol]);
                }
            }
        }

        private void Map_MapChanged1(object source, EventArgs args, List<List<int>> newMap)
        {
            //MessageBox.Show("draw a map");
            WP.Children.Clear();//ensure that inside the WP there is always only 100 elements. 
            //MessageBox.Show(newMap[2][2].ToString());
            //throw new NotImplementedException();
            DrawPokemonCanvas();
            for (int nrow = 0; nrow < 10; nrow++)
            {
                for (int ncol = 0; ncol < 10; ncol++)
                {
                    PicMat[nrow, ncol] = new Image();
                    PicMat[nrow, ncol].Height = 40;
                    PicMat[nrow, ncol].Width = 40;
                    PicMat[nrow, ncol].Opacity = 1;
                    PicMat[nrow, ncol].Stretch = Stretch.UniformToFill;

                    if (newMap[nrow][ncol] == 2)    PicMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/treebg-1.jpg", UriKind.Relative));
                    else if (newMap[nrow][ncol] == 0)   PicMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/grass_material.png", UriKind.Relative));
                    else if (newMap[nrow][ncol] == 1)   PicMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/road_material.png", UriKind.Relative));
                    else if (newMap[nrow][ncol] == -1)  PicMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/gym.png", UriKind.Relative));
                    else if (newMap[nrow][ncol] == -2)  PicMat[nrow, ncol].Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/cave.jpg", UriKind.Relative));
                    WP.Children.Add(PicMat[nrow, ncol]);
                }
            }
        } //Draw the trees, caves and so on. 

        //movement
        private void Button_Click(object sender, RoutedEventArgs e) // This is W, for upwards. 
        {
            direction = 1;
            MapModel.Map.DisplayPokemon += Map_DisplayPokemon;
            MapModel.Map.PokemonStepped += Map_PokemonStepped;
            MapModel.Map.GymStepped += MapEvents_GymStepped;
            MapModel.Map.CaveStepped += Map_CaveStepped;

            MapModel.Map.OnPlayerMove("up");

            MapModel.Map.DisplayPokemon -= Map_DisplayPokemon;
            MapModel.Map.PokemonStepped -= Map_PokemonStepped;
            MapModel.MapEvents.GymStepped -= MapEvents_GymStepped;
            MapModel.Map.CaveStepped -= Map_CaveStepped;


        }


        private void LEFT_Click(object sender, RoutedEventArgs e)
        {
            direction = 3;
            MapModel.Map.PokemonStepped += Map_PokemonStepped;
            MapModel.Map.DisplayPokemon += Map_DisplayPokemon;
            MapModel.MapEvents.GymStepped += MapEvents_GymStepped;
            MapModel.Map.CaveStepped += Map_CaveStepped;

            MapModel.Map.OnPlayerMove("left");

            MapModel.Map.DisplayPokemon -= Map_DisplayPokemon;
            MapModel.Map.PokemonStepped -= Map_PokemonStepped;
            MapModel.MapEvents.GymStepped -= MapEvents_GymStepped;
            MapModel.Map.CaveStepped -= Map_CaveStepped;


        }

        private void RIGHT_Click(object sender, RoutedEventArgs e)
        {
            direction = 4;
            MapModel.Map.PokemonStepped += Map_PokemonStepped;
            MapModel.Map.DisplayPokemon += Map_DisplayPokemon;
            MapModel.MapEvents.GymStepped += MapEvents_GymStepped;
            MapModel.Map.CaveStepped += Map_CaveStepped;

            MapModel.Map.OnPlayerMove("right");

            MapModel.Map.DisplayPokemon -= Map_DisplayPokemon;
            MapModel.Map.PokemonStepped -= Map_PokemonStepped;
            MapModel.MapEvents.GymStepped -= MapEvents_GymStepped;
            MapModel.Map.CaveStepped -= Map_CaveStepped;


        }

        private void DOWN_Click(object sender, RoutedEventArgs e)
        {
            direction = 2;
            MapModel.Map.PokemonStepped += Map_PokemonStepped;
            MapModel.Map.DisplayPokemon += Map_DisplayPokemon;
            MapModel.MapEvents.GymStepped += MapEvents_GymStepped;
            MapModel.Map.CaveStepped += Map_CaveStepped;

            MapModel.Map.OnPlayerMove("down");

            MapModel.Map.DisplayPokemon -= Map_DisplayPokemon;
            MapModel.Map.PokemonStepped -= Map_PokemonStepped;
            MapModel.MapEvents.GymStepped -= MapEvents_GymStepped;
            MapModel.Map.CaveStepped -= Map_CaveStepped;


        }



        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("dwadwadaw");
            if (PokeShowBox.SelectedItem != null)
            {
                //string SelectedPokemonInfo = PokeShowBox.SelectedItem.ToString();
                //MessageBox.Show(PokeShowBox.SelectedItem.ToString());
                PokemonPresenter.PokemonDisplay BeViewed = (PokemonPresenter.PokemonDisplay)PokeShowBox.SelectedItem;
                Rename renamingwindow = new Rename(BeViewed.ID);
                renamingwindow.Show();
            }
            else
            {
                MessageBox.Show("Please select in the above list a Pokemon you wanna rename.");
            }

        }

        public void LinkListBox() // Initialize the ListBox for choosing pokemon from Pokemons
        {
            PokeShowBox.ItemsSource = PlayerModel.Player.Main.PokemonToDisplay;
            Potion_status.Text = " Potions:" + PlayerModel.Player.Main.expPotions;
            BattleListBox.ItemsSource = PlayerModel.Player.Main.BattleReadyPokemon;
        }


        private void Refresh_Battle_Status_and_EXP(PokemonPresenter.PokemonDisplay BeViewed)
        {
            //sees if the Pokemon selected is a Battle-ready one
            if (PlayerModel.Player.Main.BattleReadyPokemon.Contains(BeViewed))
                Battle_status.Text = "Ready";
            else Battle_status.Text = "Not Ready";

            foreach (KeyValuePair<int, PokemonModel.PokemonCaptured> item in PlayerModel.Player.Main.PokemonModelDict)
            {
                if (item.Key == BeViewed.ID)
                {
                    LevelPoke.Text = " Level:" + item.Value.level.ToString();
                    EXPinfo.Text = "EXP: " + item.Value.exp.ToString() + "/" + item.Value.maxExp.ToString();
                }
            }
        }

        private void ViewPoke_Click(object sender, RoutedEventArgs e) //View the information will not change the list anyway. 
        {
            if (PokeShowBox.SelectedItem != null)
            {
                PokemonPresenter.PokemonDisplay BeViewed = (PokemonPresenter.PokemonDisplay)PokeShowBox.SelectedItem;
                PictureShow(BeViewed.Name);
                NicknamePoke.Text = " " + BeViewed.DisplayName;
                HPPoke.Text = " HP: " + BeViewed.maxHP.ToString();
                LevelPoke.Text = " Level:" + BeViewed.level.ToString();
                EXPinfo.Text = "EXP: " + BeViewed.exp.ToString() + "/" + BeViewed.maxExp.ToString(); 
                Refresh_Battle_Status_and_EXP(BeViewed);
            }
        }

        private void PictureShow(string Name)
        {
            /* To filp a image horizontally.
            ImgPoke.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform flipTrans = new ScaleTransform();
            flipTrans.ScaleX = -1;
            //flipTrans.ScaleY = -1;
            ImgPoke.RenderTransform = flipTrans;
            */
            if (Name == "Pichu") ImgPoke.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/011Pichu.png", UriKind.Relative));
            else if (Name == "Pikachu") ImgPoke.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/012Pikachu.png", UriKind.Relative));
            else if (Name == "Raichu") ImgPoke.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/013Raichu.png", UriKind.Relative));
            else if (Name == "Zubat") ImgPoke.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/021Zubat.png", UriKind.Relative));
            else if (Name == "Golbat") ImgPoke.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/022Golbat.png", UriKind.Relative));
            else if (Name == "Magikarp") ImgPoke.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/031Magikarp.png", UriKind.Relative));
            else if (Name == "Gyarados") ImgPoke.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/032Gyarados.png", UriKind.Relative));
            else if (Name == "Bulbasaur") ImgPoke.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/041Bulbasaur.png", UriKind.Relative));
            else if (Name == "Ivysaur") ImgPoke.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/042Ivysaur.png", UriKind.Relative));
            else if (Name == "Venusaur") ImgPoke.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/043Venusaur.png", UriKind.Relative));
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
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void PokeShowBox_ItemChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewPoke_Click(sender, e);
        }

        private void SellPoke_Click(object sender, RoutedEventArgs e)
        {
            if (PokeShowBox.SelectedItem != null)
            {
                PokemonPresenter.PokemonDisplay P = (PokemonPresenter.PokemonDisplay)PokeShowBox.SelectedItem;
                if (PlayerModel.Player.Main.BattleReadyPokemon[0] != null && PlayerModel.Player.Main.BattleReadyPokemon[0].ID == P.ID)
                { 
                    MessageBox.Show("The only-battle-ready-Pokemon should not be sold!"); 
                }
                else
                {
                    PlayerModel.PlayerEvents.BattleReadyChanged += PlayerEvents_BattleReadyChanged;
                    PlayerModel.PlayerEvents.PokemonInventoryChanged += PlayerEvents_PokemonInventoryChanged;
                    PlayerModel.PlayerEvents.PotionsChanged += PlayerEvents_PotionsChanged;
                    PlayerModel.Player.Main.OnPokemonSold(P.ID);
                }
            }
            else
            {
                MessageBox.Show("Please select in the above list a Pokemon you wanna sell.");
            }
        }

        private void Battle_ready_Click(object sender, RoutedEventArgs e) //Wont change inventory
        {
            if (PokeShowBox.SelectedItem != null)
            {
                PlayerModel.PlayerEvents.BattleReadyChanged += PlayerEvents_BattleReadyChanged;
                PokemonPresenter.PokemonDisplay P = (PokemonPresenter.PokemonDisplay)PokeShowBox.SelectedItem;
                PlayerModel.Player.Main.OnAddBattlePokemon(P.ID);
                //PlayerModel.Player.Main.PokemonToDisplay.RemoveAt(3);
                PictureShow(P.Name);
                NicknamePoke.Text = " " + P.DisplayName;
                HPPoke.Text = " HP: " + P.maxHP.ToString();
                Battle_status.Text = "Ready";
                LevelPoke.Text = " Level:" + P.level.ToString();
                EXPinfo.Text = "EXP: " + P.exp.ToString() + "/" + P.maxExp.ToString();
            }
            else
            {
                MessageBox.Show("Please select a Pokemon you wanna use to battle.");
            }
        }

        private void PlayerEvents_BattleReadyChanged(object source, EventArgs args, List<PokemonPresenter.PokemonDisplay> newList)
        {
            BattleListBox.ItemsSource = null;
            BattleListBox.ItemsSource = newList;
            //throw new NotImplementedException();
        }

        private void PotionPoke_Click(object sender, RoutedEventArgs e)
        {
            //Wait for the implementation code, I think this part is not that difficult
            if (PokeShowBox.SelectedItem != null)
            {
                PlayerModel.PlayerEvents.PotionsChanged += PlayerEvents_PotionsChanged;
                PlayerModel.PlayerEvents.PokemonInventoryChanged += PlayerEvents_PokemonInventoryChanged;
                PokemonPresenter.PokemonDisplay P = (PokemonPresenter.PokemonDisplay)PokeShowBox.SelectedItem;
                PlayerModel.Player.Main.OnDrinkPotion(P.ID);
                
                //Refresh_Battle_Status_and_EXP(BeViewed);
            }
            else
            {
                MessageBox.Show("Please select in the above list a Pokemon you wanna feed.");
            }
        }

        private void PlayerEvents_PokemonInventoryChanged(object source, EventArgs args, List<PokemonPresenter.PokemonDisplay> newList)
        {
            PokeShowBox.ItemsSource = null;
            PokeShowBox.ItemsSource = newList;
        }

        private void PlayerEvents_PotionsChanged(object source, EventArgs args, int newNum)
        {
            Potion_status.Text = " Potions:  " + newNum.ToString();
        }
    }

    
}
