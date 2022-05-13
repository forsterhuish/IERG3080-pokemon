using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace IERG3080PartII.Model
{
    public class PaperScissorsStone
    {
        enum Choices { Paper, Scissors, Stone};

        private static PaperScissorsStone Instance;
        private static PokemonTemplate user, enemy;
        private static String winner;
        private static Choices userChoice, enemyChoice;

        private PaperScissorsStone(PokemonTemplate p1, PokemonTemplate p2) 
        {
            user = p1;
            enemy = p2;
        }

        public static PaperScissorsStone initGame(PokemonTemplate userPokemon1, PokemonTemplate userPokemon2)
        {
            if (Instance == null)
                Instance = new PaperScissorsStone(userPokemon1, userPokemon2);
            return Instance;
        }

        public static void GetUserChoice(String input)
        {
            switch (input)
            {
                case "Paper":
                    userChoice = Choices.Paper;
                    break;
                case "Scissors":
                    userChoice = Choices.Scissors;
                    break;
                case "Stone":
                    userChoice = Choices.Stone;
                    break;
            }
        }

        private static void GetEnemyChoice()
        {
            Random rnd = new Random();
            switch (rnd.Next(0,3))
            {
                case 0:
                    enemyChoice = Choices.Paper;
                    break;
                case 1:
                    enemyChoice = Choices.Scissors;
                    break;
                case 2:
                    enemyChoice = Choices.Stone;
                    break;
            }
        }

        public String GetWinner()
        {
            do
            {
                GetEnemyChoice();
            }
            while (userChoice == enemyChoice); // simplified implementation: forbid case of tie
            winner = "";
            if (userChoice == Choices.Paper)
            {
                if (enemyChoice == Choices.Scissors)
                {
                    winner = enemy.getName;
                }
                else
                {
                    winner = user.getName;
                }
            }

            else if (userChoice == Choices.Scissors)
            {
                if (enemyChoice == Choices.Paper)
                {
                    winner = user.getName;
                }
                else
                {
                    winner = enemy.getName;
                }
            }

            else // userChoice == Choices.Stone
            {
                if (enemyChoice == Choices.Paper)
                {
                    winner = enemy.getName;
                }
                else
                {
                    winner = user.getName;
                }
            }
            MessageBox.Show("User Choice: " + userChoice.ToString() + "\nEnemy Choice: " + enemyChoice.ToString());
            MessageBox.Show(winner + " wins!");
            Instance = null;
            return winner;
        }
    }
}
