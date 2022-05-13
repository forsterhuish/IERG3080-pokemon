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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class userSelectionWindow : Window
    {
        public userSelectionWindow()
        {
            InitializeComponent();
            initWindow();
        }

        User newUser;
        List<PokemonTemplate> userPokemonList, enemyList;

        private void initWindow()
        {
            newUser = User.Instance;
            userPokemonList = newUser.getPokemonOwned;
            enemyList = PokemonFactory.accessDatabase;
            foreach (var item in userPokemonList)
            {
                UserPokemonCollection.Items.Add(item.ToString());
            }

            foreach (var item in enemyList)
            {
                EnemyCollection.Items.Add(item.ToString());
            }
        }

        public PokemonTemplate getUserPokemon(String pokemonName)
        {
            PokemonTemplate pokemon = null;
            foreach (var item in userPokemonList)
            {
                if (item.ToString() == pokemonName)
                {
                    pokemon = item.Clone() as PokemonTemplate;
                    MessageBox.Show("Pokemon name: " + pokemon.getName);
                }
            }
            return pokemon;
        }

        public PokemonTemplate getEnemy(String pokemonName)
        {
            PokemonTemplate enemy = null;
            foreach (var item in enemyList)
            {
                if (item.ToString() == pokemonName)
                {
                    enemy = item.Clone() as PokemonTemplate;
                    MessageBox.Show("Pokemon name: " + enemy.getName);
                }
            }
            return enemy;
        }
    }
}
