using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
using System.Windows;
using IERG3080PartII.Model;

// By HUI Sze Ho, Forster

namespace IERG3080PartII.Model
{
    public class Battle // Battle class represents a battle started
        // Battle between user Pokemon and CPU Pokemon
    {
        private static PokemonTemplate userPokemon, enemyPokemon;
        private static PokemonTemplate currMove;
        private static Battle battleInstance;
        private static DispatcherTimer battleTimer;
        private static PaperScissorsStone game;

        private Battle(PokemonTemplate p1, PokemonTemplate p2)
        {
            if (userPokemon == null && enemyPokemon == null)
            {
                userPokemon = p1;
                enemyPokemon = p2;
            }
            battleTimer = new DispatcherTimer();
            game = PaperScissorsStone.initGame(userPokemon, enemyPokemon);
            MessageBox.Show("User: " + userPokemon.getName + "\nEnemy: " + enemyPokemon.getName);
            battleTimer.Tick += BattleTimer_Tick;
            battleTimer.Interval = TimeSpan.FromMilliseconds(1);
            startBattle();
        }
        ~Battle()
        {
            userPokemon = null;
            enemyPokemon = null;
        }

        public static Battle initBattle(PokemonTemplate userPokemon1, PokemonTemplate userPokemon2)
        {
            if (battleInstance == null)
                battleInstance = new Battle(userPokemon1, userPokemon2);
            return battleInstance;
        }

        public void Reset()
        {
            endBattle();
            userPokemon = null;
            enemyPokemon = null;
            battleInstance = null;
        }

        private void BattleTimer_Tick(object sender, EventArgs e)
        {
            if (userPokemon == null || enemyPokemon == null)
                return;
            if (userPokemon.GetHP <= 0 || enemyPokemon.GetHP <= 0)
            {
                if (userPokemon.GetHP <= 0)
                {
                    MessageBox.Show(enemyPokemon.ToString() + " wins!");
                }
                else if (enemyPokemon.GetHP <= 0)
                {
                    MessageBox.Show(userPokemon.ToString() + " wins!");
                }
                endBattle();
            }
        }

        private static void startBattle()
        {
            battleTimer.Start();
        }

        private static void endBattle()
        {
            battleTimer.Stop();
        }

        public static PokemonTemplate getWinner()
        {
            if (game.GetWinner() == userPokemon.ToString())
                currMove = userPokemon;
            else
                currMove = enemyPokemon;
            return currMove;
        }

        public void initAttack(PokemonTemplate.AttackAction a1, PokemonTemplate target)
        {
            a1(target);
        }

        public void initIdle(PokemonTemplate.IdleAction i1, PokemonTemplate target)
        {
            i1(target);
        }
    }

    class AI
    {
        private static PokemonTemplate ai_pokemon, user_Pokemon;
        private static List<PokemonTemplate.AttackAction> attacks = new List<PokemonTemplate.AttackAction>();
        private static List<PokemonTemplate.IdleAction> idles = new List<PokemonTemplate.IdleAction>();
        protected static Random rnd = new Random();
        private static Battle newBattle = Battle.initBattle(ai_pokemon, user_Pokemon);

        public AI(PokemonTemplate AI_Pokemon, PokemonTemplate user)
        {
            ai_pokemon = AI_Pokemon;
            user_Pokemon = user;

            attacks = AI_Pokemon.getAttacks;
            idles = AI_Pokemon.getIdles;
        }

        private PokemonTemplate.AttackAction chooseAttacks
        {
            get
            {
                try 
                { 
                    return attacks[rnd.Next(0, attacks.Count)]; 
                }
                catch (IndexOutOfRangeException)
                {
                    return attacks[0];
                }
            }
        }

        private PokemonTemplate.IdleAction chooseIdles
        {
            get
            {
                try
                {
                    return idles[rnd.Next(0, idles.Count)];
                }
                catch (IndexOutOfRangeException)
                {
                    return idles[0];
                }
            }
        }

        private void attackUser()
        {
            MessageBox.Show(ai_pokemon.ToString() + " attacks " + user_Pokemon.ToString() + " by " + chooseAttacks.Method.Name + "!");
            newBattle.initAttack(chooseAttacks, user_Pokemon);
        }

        private void idleActions()
        {
            MessageBox.Show(ai_pokemon.ToString() + " uses " + chooseIdles.Method.Name + "!");
            newBattle.initIdle(chooseIdles, ai_pokemon);
        }

        public void Actions()
        {
            if (attacks.Count == 0)
                idleActions();
            else if (idles.Count == 0)
                attackUser();
            else
            {
                if (rnd.Next(0, 100) > 50) // simple logic: choosing based on probability
                    attackUser();
                else
                    idleActions();
            }
        }
    }
}
