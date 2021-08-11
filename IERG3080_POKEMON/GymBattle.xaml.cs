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
    /// GymBattle.xaml 的互動邏輯
    /// </summary>
    public partial class GymBattle : Window
    {
        int StageAllyPokeFullHP=100;
        int StageEnemyPokeFullHP=100;

        public GymBattle()
        {
            InitializeComponent();
        }

        private void Draw_Bar_PokeFig(int HP, int maxHP, bool isAlly)
        {
            double HPportion = (double)(HP*100) / (double)maxHP;
            //Enable the health bar
            MyHpBar.Opacity = 1;
            EnemyHpBar.Opacity = 1;
            if (isAlly == true) { MyHpBar.Value = HPportion; AllyHPShow.Text = "HP: " + HP.ToString() + " / " + maxHP.ToString(); }
            else { EnemyHpBar.Value = HPportion; EnemyHPShow.Text = "HP: " + HP.ToString() + " / " + maxHP.ToString(); }
        } //第二參數是真：畫自己血條， 第二參數是假：畫對方的血條

        private void leave_Click(object sender, RoutedEventArgs e)
        {
            // 測試用的，我在另一個文件裏面也改了他的accesser從private改成了public
            GymModel.GymBattle.inBattle = false;
            this.Close();
        }

        private void START_FIGHT_Click(object sender, RoutedEventArgs e)
        {
            //按下去之後 兩個血條就出來，然後開打。
            START_FIGHT.IsEnabled = false;
            Skill1.IsEnabled = true;
            Skill2.IsEnabled = true;

            GymModel.BattleEvents.PokemonShow += BattleEvents_PokemonShow;
            GymModel.GymBattle.OnBattleStart();
        }

        private void PictureShow(string Name, bool isAlly) 
        {
            if (isAlly == true) 
            {
                if (Name == "Pichu") YourPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/011Pichu.png", UriKind.Relative));
                else if (Name == "Pikachu") YourPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/012Pikachu.png", UriKind.Relative));
                else if (Name == "Raichu") YourPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/013Raichu.png", UriKind.Relative));
                else if (Name == "Zubat") YourPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/021Zubat.png", UriKind.Relative));
                else if (Name == "Golbat") YourPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/022Golbat.png", UriKind.Relative));
                else if (Name == "Magikarp") YourPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/031Magikarp.png", UriKind.Relative));
                else if (Name == "Gyarados") YourPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/032Gyarados.png", UriKind.Relative));
                else if (Name == "Bulbasaur") YourPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/041Bulbasaur.png", UriKind.Relative));
                else if (Name == "Ivysaur") YourPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/042Ivysaur.png", UriKind.Relative));
                else if (Name == "Venusaur") YourPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/043Venusaur.png", UriKind.Relative));
                //MessageBox.Show("Your Pokemon is: " + Name + "!");
            }
            
            else if (isAlly == false)
            {
                if (Name == "Pichu") EnemyPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/011Pichu.png", UriKind.Relative));
                else if (Name == "Pikachu") EnemyPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/012Pikachu.png", UriKind.Relative));
                else if (Name == "Raichu") EnemyPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/013Raichu.png", UriKind.Relative));
                else if (Name == "Zubat") EnemyPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/021Zubat.png", UriKind.Relative));
                else if (Name == "Golbat") EnemyPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/022Golbat.png", UriKind.Relative));
                else if (Name == "Magikarp") EnemyPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/031Magikarp.png", UriKind.Relative));
                else if (Name == "Gyarados") EnemyPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/032Gyarados.png", UriKind.Relative));
                else if (Name == "Bulbasaur") EnemyPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/041Bulbasaur.png", UriKind.Relative));
                else if (Name == "Ivysaur") EnemyPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/042Ivysaur.png", UriKind.Relative));
                else if (Name == "Venusaur") EnemyPokemon.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/043Venusaur.png", UriKind.Relative));

                EnemyPokemon.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform flipTrans = new ScaleTransform();
                flipTrans.ScaleX = -1;
                //flipTrans.ScaleY = -1;
                EnemyPokemon.RenderTransform = flipTrans;
                //MessageBox.Show("Gym Pokemon is: " + Name + "!");
            }
        }  //Show the figures of Pokemons

        private void BattleEvents_PokemonShow(object source, EventArgs args, PokemonPresenter.PokemonBattle pokemon, bool isAlly)
        {
            PictureShow(pokemon.Name, isAlly);
            Draw_Bar_PokeFig(pokemon.HP,pokemon.maxHP, isAlly);
            if (isAlly == true)
            {
                StageAllyPokeFullHP = pokemon.HP;
            }
            else if (isAlly == false)
            {
                StageEnemyPokeFullHP = pokemon.HP;
            }
        }

        private void ChangePokemon_Click(object sender, RoutedEventArgs e)
        {
            //user 要換pokemon了
            GymModel.BattleEvents.ChoiceShow += BattleEvents_ChoiceShow;
            GymModel.GymBattle.OnPokemonChangeClick();
        }

        private void BattleEvents_BattleEnded(object source, EventArgs args, bool state)
        {
            this.Close();
            if (state == true) //Win the battle   
                MessageBox.Show("You win the battle! Every of your pokemon on stage will be gained many experience!");
            else if (state == false)
                MessageBox.Show("You lose the battle. Anyway, every of your pokemon on stage will be gained some experience!");
            //throw new NotImplementedException();
        }

        private void UpdatePokemonChange1(PokemonPresenter.PokemonDisplay P)
        {
            string Name = P.Name;
            Image img = new Image();
            if (Name == "Pichu") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/011Pichu.png", UriKind.Relative));
            else if (Name == "Pikachu") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/012Pikachu.png", UriKind.Relative));
            else if (Name == "Raichu") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/013Raichu.png", UriKind.Relative));
            else if (Name == "Zubat") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/021Zubat.png", UriKind.Relative));
            else if (Name == "Golbat") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/022Golbat.png", UriKind.Relative));
            else if (Name == "Magikarp") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/031Magikarp.png", UriKind.Relative));
            else if (Name == "Gyarados") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/032Gyarados.png", UriKind.Relative));
            else if (Name == "Bulbasaur") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/041Bulbasaur.png", UriKind.Relative));
            else if (Name == "Ivysaur") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/042Ivysaur.png", UriKind.Relative));
            else if (Name == "Venusaur") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/043Venusaur.png", UriKind.Relative));
            Pokemon1.Content = img;
            Pokemon1.IsEnabled = true;
            HPPoke1.Text = "HP: " + P.HP.ToString() + " / " + P.maxHP.ToString();
            LevelPoke1.Text = "Level: " + P.level.ToString();
            Pokemon1.Uid = P.ID.ToString();
        }

        private void UpdatePokemonChange2(PokemonPresenter.PokemonDisplay P)
        {
            string Name = P.Name;
            Image img = new Image();
            if (Name == "Pichu") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/011Pichu.png", UriKind.Relative));
            else if (Name == "Pikachu") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/012Pikachu.png", UriKind.Relative));
            else if (Name == "Raichu") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/013Raichu.png", UriKind.Relative));
            else if (Name == "Zubat") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/021Zubat.png", UriKind.Relative));
            else if (Name == "Golbat") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/022Golbat.png", UriKind.Relative));
            else if (Name == "Magikarp") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/031Magikarp.png", UriKind.Relative));
            else if (Name == "Gyarados") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/032Gyarados.png", UriKind.Relative));
            else if (Name == "Bulbasaur") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/041Bulbasaur.png", UriKind.Relative));
            else if (Name == "Ivysaur") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/042Ivysaur.png", UriKind.Relative));
            else if (Name == "Venusaur") img.Source = new BitmapImage(new Uri("/IERG3080_POKEMON;component/img_navimap/Pokemon_Icon/043Venusaur.png", UriKind.Relative));
            Pokemon2.Content = img;
            Pokemon2.IsEnabled = true;
            HPPoke2.Text = "HP: " + P.HP.ToString() + " / " + P.maxHP.ToString();
            LevelPoke2.Text = "Level: " + P.level.ToString();
            Pokemon2.Uid = P.ID.ToString();
        }

        private void BattleEvents_ChoiceShow(object source, EventArgs args, List<PokemonPresenter.PokemonDisplay> availablePokemon)
        {
            Pokemon1.IsEnabled = false;
            Pokemon2.IsEnabled = false;
            Pokemon1.Content = null;
            Pokemon2.Content = null;

            if (availablePokemon.Count == 2)
            {
                for (int i = 0; i < availablePokemon.Count; i++)
                {
                    if (i == 0) UpdatePokemonChange1(availablePokemon[i]);
                    if (i == 1) UpdatePokemonChange2(availablePokemon[i]);
                }
            }
            else if (availablePokemon.Count == 1)  UpdatePokemonChange1(availablePokemon[0]);
        }

        private void Skill1_Click(object sender, RoutedEventArgs e) //only using attcks may end a battle
        {
            PokemonModel.AttackEvents.AttackPerformed += AttackEvents_AttackPerformed1; ;
            GymModel.BattleEvents.PokemonShow += BattleEvents_PokemonShow;
            GymModel.BattleEvents.ChoiceShow += BattleEvents_ChoiceShow; // Here many be also need to change because Ally Pokemon dies
            GymModel.BattleEvents.BattleEnded += BattleEvents_BattleEnded;

            GymModel.GymBattle.OnAttack("light");
            Pokemon1.IsEnabled = false;
            Pokemon2.IsEnabled = false;
            //MessageBox.Show("I am here.");

            GymModel.BattleEvents.BattleEnded -= BattleEvents_BattleEnded;
        }

        private void AttackEvents_AttackPerformed1(object source, EventArgs args, bool allyIsAttacked, string attackName, PokemonPresenter.PokemonBattle attackerBattle, int attackedNewHP)
        {
            //throw new NotImplementedException();
            if (allyIsAttacked == true) //自己被打，所以重新畫自己的
            {
                //extract enemy's HP. 
                StageEnemyPokeFullHP = attackerBattle.maxHP;
                Draw_Bar_PokeFig(attackedNewHP, StageAllyPokeFullHP, allyIsAttacked);
            }
            else if (allyIsAttacked == false)//對方被打，所以重新畫別人的，也重新計算自己的heavyCharge
            {
                //extract the Full Hp of Ally
                StageAllyPokeFullHP = attackerBattle.maxHP;
                Draw_Bar_PokeFig(attackedNewHP, StageEnemyPokeFullHP, allyIsAttacked);
                //ChargeCalculator.Text = "Heavy Attack Charged Energy: " + newHeavyCharge.ToString();
            }

            if (allyIsAttacked == true && attackedNewHP <= 0)
            {
                Skill1.IsEnabled = false; Skill2.IsEnabled = false;
            }
        }

        private void Skill2_Click(object sender, RoutedEventArgs e)
        {
            PokemonModel.AttackEvents.AttackPerformed += AttackEvents_AttackPerformed1;
            GymModel.BattleEvents.PokemonShow += BattleEvents_PokemonShow;
            GymModel.BattleEvents.ChoiceShow += BattleEvents_ChoiceShow; // Here many be also need to change because Ally Pokemon dies
            GymModel.BattleEvents.BattleEnded += BattleEvents_BattleEnded;

            GymModel.GymBattle.OnAttack("heavy");
            Pokemon1.IsEnabled = false;
            Pokemon2.IsEnabled = false;
            //MessageBox.Show("I am here.");

            GymModel.BattleEvents.BattleEnded -= BattleEvents_BattleEnded;
        }

        private void Pokemon1_Click(object sender, RoutedEventArgs e)
        {
            //GymModel.GymBattle.OnPokemonChanged();
            //MessageBox.Show(Pokemon1.Uid);
            GymModel.BattleEvents.PokemonShow += BattleEvents_PokemonShow;
            GymModel.BattleEvents.ChoiceShow += BattleEvents_ChoiceShow; // Here many be also need to change because Ally Pokemon dies
            int ID = int.Parse(Pokemon1.Uid);
            //MessageBox.Show("Parse: " + ID.ToString());
            GymModel.GymBattle.OnPokemonChanged(ID);
            Skill1.IsEnabled = true;
            Skill2.IsEnabled = true;
            Pokemon1.IsEnabled = false;
            Pokemon2.IsEnabled = false;
        }

        private void Pokemon2_Click(object sender, RoutedEventArgs e)
        {
            GymModel.BattleEvents.PokemonShow += BattleEvents_PokemonShow;
            GymModel.BattleEvents.ChoiceShow += BattleEvents_ChoiceShow;   // Here many be also need to change because Ally Pokemon dies
            int ID = int.Parse(Pokemon2.Uid);
            //MessageBox.Show("Parse: " + ID.ToString());
            GymModel.GymBattle.OnPokemonChanged(ID);
            Skill1.IsEnabled = true;
            Skill2.IsEnabled = true;
            Pokemon1.IsEnabled = false;
            Pokemon2.IsEnabled = false;
        }
    }
}
