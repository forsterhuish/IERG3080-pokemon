using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using IERG3080PartII.Presenter;

// By HO, Tsz Ngong

namespace IERG3080PartII.Model {
    class PokemonSpawner : PokemonFactory {
        private PresenterClass Presenter = PresenterClass.Instance;
        static private PokemonSpawner _instance;
        static private System.Windows.Threading.DispatcherTimer SpawnTimer;
        private Random rand;

        //private ArrayList OnScreenPokemon;
        private List<PokemonTemplate> OnScreenPokemon;
        public List<PokemonTemplate> getOnScreenPoke {
            get {
                return OnScreenPokemon;
            }
        }

        public string getName {
            get {
                string con = "";
                foreach (PokemonTemplate poke in OnScreenPokemon) {
                    con += poke.getName + " ";
                }
                return con;
            }
        }

        private int SpawnRate = 60;
        private int DespawnRate = 30;

        static public PokemonSpawner Instance {
            get {
                if (_instance == null) {
                    _instance = new PokemonSpawner();
                }
                return _instance;
            }
        }

        private PokemonSpawner() {
            OnScreenPokemon = new List<PokemonTemplate>();
            rand = new Random();
            SpawnTimer = new System.Windows.Threading.DispatcherTimer();
            SpawnTimer.Tick += SpawnTimer_Tick;
            SpawnTimer.Interval = TimeSpan.FromSeconds(4);

            startSpawner();
        }

        public void startSpawner() {
            SpawnTimer.Start();
        }

        public void pauseSpawner() {
            SpawnTimer.Stop();
        }

        private void SpawnTimer_Tick(object sender, EventArgs e) {
            if (OnScreenPokemon.Count > 0) {    // Spawn if 1-5 pokemon on screen
                despawnPokemon();
            }
            if (OnScreenPokemon.Count < 6) {    // Spawn if 1-5 pokemon on screen
                spawnPokemon();
            }
        }

        private void spawnPokemon() {
            if (rand.Next(0, 100) < this.SpawnRate) {
                var NewPokemon = PokemonFactory.PokemonDatabase[rand.Next(PokemonFactory.PokemonDatabase.Count)].Clone() as PokemonTemplate;
                OnScreenPokemon.Add(NewPokemon);
                PokemonVisualize();
            }
        }

        private void despawnPokemon() {
            if (rand.Next(0, 100) < this.DespawnRate) { // Natual despawn
                OnScreenPokemon.RemoveAt(rand.Next(OnScreenPokemon.Count));
                PokemonDevisualizeRandom();
            }
        }

        public void despawnPokemon(object sender) {    // After fighting
            OnScreenPokemon.RemoveAt(rand.Next(OnScreenPokemon.Count));
            PokemonDevisualize(sender);
        }

        private void PokemonVisualize() {
            // Add Button randomly
            Presenter.SpawnButton("Pokemon");
        }

        private void PokemonDevisualizeRandom() {
            // Romove random Button
            Presenter.RemoveButton();
        }
        private void PokemonDevisualize(object sender) {
            
            Presenter.RemoveButton(sender);
        }



        
    }
}
