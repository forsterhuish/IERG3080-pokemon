using System;
using System.Collections.Generic;
using System.Text;
using IERG3080PartII.Model;

// By HO, Tsz Ngong

namespace IERG3080PartII.Model {
    public class User {
        static private User _instance;
        private int _Lv;
        private int _XP;

        private Dictionary<string, int> _Inventory = new Dictionary<string, int>();
        private List<PokemonTemplate> _PokemonInventory = new List<PokemonTemplate>();

        //private Dictionary<Item, int> _Inventory = new Dictionary<Item, int>();
        //Item coin = new Item();

        // private Dictionary<Item, int> _Inventory = new Dictionary<Item, int>();
        private User() {
            _Lv = 1;
            _XP = 0;

        }

        static public User Instance {
            get {
                if (_instance == null) {
                    _instance = new User();
                }
                return _instance;
            }
        }
        public Dictionary<string, int> getInventory { get { return _Inventory; } }

        public List<PokemonTemplate> getPokemonOwned
        {
            get
            {
                return this._PokemonInventory;
            }
        }

        public void addPokemon(PokemonTemplate pokemon) {
            bool check = true;
            foreach (PokemonTemplate Ownpokemon in this._PokemonInventory) {
                if (Ownpokemon.getID == pokemon.getID) {
                    check = false;
                }
            }
            if (check) {
                this._PokemonInventory.Add(pokemon);
            }
            else {
                addItem("coin", 100);
            }
            // pokemon.owner?
            PokemonTemplate.CapturePokemon(_instance, pokemon);
        }

        public void releasePokemon(PokemonTemplate pokemon) {
            if (this._PokemonInventory.Contains(pokemon)) {
                this._PokemonInventory.Remove(pokemon);
            }
            PokemonTemplate.ReleasePokemon(pokemon);
        }

        public void addXP(int i) {
            this._XP += i;
            while (this._XP > Math.Pow(2, this._Lv + 2)) {
                this._Lv += 1;
            }
        }

        public int getXP() {
            return _XP;
        }
        public int getLV() {
            return _Lv;
        }

        public void addItem(string item, int number) {
            if (this._Inventory.ContainsKey(item)) {
                this._Inventory[item] += number;
            }
            else {
                this._Inventory.Add(item, number);
            }
            
        }

        public void removeItem(string item, int number) {
            if (this._Inventory.ContainsKey(item)) {
                if (number > this._Inventory[item]) {

                }
                else {
                    this._Inventory[item] -= number;
                    if (this._Inventory[item] == 0) {
                        this._Inventory.Remove(item);
                    }
                }
            }
        }
    }
}
