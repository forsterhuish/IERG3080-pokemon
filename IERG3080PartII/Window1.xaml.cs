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
using IERG3080PartII.Model;

namespace IERG3080PartII
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class battleWindow : Window
    {
        PokemonTemplate user, enemy;
        Battle battle1;
        AI newAI;
        userSelectionWindow userSelection;

        public battleWindow()
        {
            InitializeComponent();

            openUserSelection();

            battle1 = Battle.initBattle(user, enemy);
            newAI = new AI(enemy, user);
            ChoiceBox.Items.Add("Paper");
            ChoiceBox.Items.Add("Scissors");
            ChoiceBox.Items.Add("Stone");

            battleInfo();
            foreach (var item in user.getAttacks)
            {
                AttackChoice.Items.Add(item.Method.Name);
            }

            foreach (var item in user.getIdles)
            {
                IdleChoice.Items.Add(item.Method.Name);
            }
            Closing += BattleWindow_Closing;
            attackEnabled(false);
        }

        private void openUserSelection()
        {
            // Define a selection window
            userSelection = new userSelectionWindow();
            userSelection.userPokemonConfirm.Click += UserPokemonConfirm_Click;
            userSelection.enemyConfirm.Click += EnemyConfirm_Click;
            userSelection.userPokemonConfirm.Click += new RoutedEventHandler(Ready);
            userSelection.enemyConfirm.Click += new RoutedEventHandler(Ready);
            userSelection.Closing += UserSelection_Closing;
            userSelection.ShowDialog();
        }

        private void BattleWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (user.GetHP > 0 && enemy.GetHP > 0) // game has not ended, prevent quiting game in middle
            {
                e.Cancel = true;
            }
            else
            {
                user = null;
                enemy = null;
                userSelection = null;
                battle1.Reset();
            }
        }

        private void UserSelection_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (user == null || enemy == null)
                e.Cancel = true;
        }

        private void Ready(object sender, RoutedEventArgs e)
        {
            if (user != null && enemy != null)
            {
                userSelection.Close();
            }
        }

        private void EnemyConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (userSelection.EnemyCollection.SelectedValue == null)
                return;
            PokemonTemplate temp = userSelection.getEnemy(userSelection.EnemyCollection.SelectedValue.ToString());
            dynamicPokemonCreation(temp.GetType().Name, temp, ref enemy);
        }

        private void UserPokemonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (userSelection.UserPokemonCollection.SelectedValue == null)
                return;
            PokemonTemplate temp = userSelection.getUserPokemon(userSelection.UserPokemonCollection.SelectedValue.ToString());
            dynamicPokemonCreation(temp.GetType().Name, temp, ref user);
        }

        private void dynamicPokemonCreation(String name, PokemonTemplate input, ref PokemonTemplate output)
        {
            switch (name)
            {
                case "Pikachu":
                    output = input as Pikachu;
                    break;
                case "IERG_Student":
                    output = input as IERG_Student;
                    break;
                case "FireDragon":
                    output = input as FireDragon;
                    break;
                case "Elsa":
                    output = input as Elsa;
                    break;
                case "Killer":
                    output = input as Killer;
                    break;
                default:
                    throw new Exception("Error");
            }
        }

        private void User_Choice_Click(object sender, RoutedEventArgs e)
        {
            if (ChoiceBox.SelectedItem == null)
                return;
            PaperScissorsStone.GetUserChoice(ChoiceBox.SelectedItem.ToString());
            gameEnabled(false);
            if (Battle.getWinner().Equals(user))
            {
                attackEnabled(true);
            }
            else
            {
                newAI.Actions();
                battleInfo();
                gameEnabled(true);
            }
        }

        private void attackConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (AttackChoice.SelectedValue == null && IdleChoice.SelectedValue == null)
                return;
            else if (AttackChoice.SelectedValue != null && IdleChoice.SelectedValue != null)
            {
                MessageBox.Show("Only one action per move!");
                AttackChoice.SelectedValue = null;
                IdleChoice.SelectedValue = null;
                return;
            }

            else if (AttackChoice.SelectedValue != null)
            {
                foreach (var item in user.getAttacks)
                {
                    if (item.Method.Name == AttackChoice.SelectedValue.ToString())
                    {
                        MessageBox.Show(user.getName + " attacks" + enemy.getName + " using" + item.Method.Name + "!");
                        battle1.initAttack(item, enemy);
                        battleInfo();
                    }
                }
            }

            else if (IdleChoice.SelectedValue != null)
            {
                foreach (var item in user.getIdles)
                {
                    if (item.Method.Name == IdleChoice.SelectedValue.ToString())
                    {
                        MessageBox.Show(user.getName + " uses " + item.Method.Name + "!");
                        battle1.initIdle(item, user);
                        battleInfo();
                    }
                }
            }

            AttackChoice.SelectedValue = null;
            IdleChoice.SelectedValue = null;
            attackEnabled(false);
            gameEnabled(true);
        }

        private void battleInfo()
        {
            PokemonInfo.Text = "Pokemon Name: " + user.getName + "\nHP: " + user.GetHP + "\nMP: " + user.GetMP;
            EnemyInfo.Text = "Enemy Name: " + enemy.getName + "\nHP: " + enemy.GetHP + "\nMP: " + enemy.GetMP;
        }

        private void gameEnabled(bool status)
        {
            ChoiceBox.IsEnabled = status;
            User_Choice.IsEnabled = status;
        }

        private void attackEnabled(bool status)
        {
            AttackChoice.IsEnabled = status;
            IdleChoice.IsEnabled = status;
            actionConfirm.IsEnabled = status;
        }
    }
}
