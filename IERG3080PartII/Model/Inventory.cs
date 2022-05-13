using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using IERG3080PartII.Model;

// By HO, Tsz Ngong

namespace IERG3080PartII.Model {
    class Inventory {
        static private Inventory _instance;
        static public Inventory Instance {
            get {
                if (_instance == null) {
                    _instance = new Inventory();
                }
                return _instance;
            }
        }

        private User user1 = User.Instance;

        private Button exit = new Button();
        private TextBlock info = new TextBlock();

        private StackPanel InvList = new StackPanel();
        public StackPanel DetailList = new StackPanel();
        private Grid _InvGrid;
        
        private List<string> InterList = new List<string> { "Item", "Pokemon", "Potions", "Trade" };
        
        public Inventory() {
            InvList.Orientation = Orientation.Vertical;
            InvList.HorizontalAlignment = HorizontalAlignment.Left;
            InvList.Width = 100;
            InitInvList();

            DetailList.Orientation = Orientation.Vertical;
            DetailList.HorizontalAlignment = HorizontalAlignment.Right;
            DetailList.Width = 500;
        }

        public void showInventory(Grid InvGrid) {
            if (this._InvGrid == null) {
                this._InvGrid = InvGrid;
                this._InvGrid.Children.Add(InvList);
                this._InvGrid.Children.Add(DetailList);

            }
            info.Text = String.Format("XP: {0}\nLv: {1}", user1.getXP(), user1.getLV());
            _InvGrid.Visibility = Visibility.Visible;
        }

        private void InitInvList() {
            foreach(string item in InterList) {
                Button SelButton = new Button() { Content = item };
                SelButton.Click += new RoutedEventHandler(ChangePage);
                InvList.Children.Add(SelButton);
            }
            exit.Click += new RoutedEventHandler(exitInventory);
            exit.Content = "Exit";
            InvList.Children.Add(exit);

            info.Text = String.Format("XP: {0}\nLv: {1}", user1.getXP(), user1.getLV());
            InvList.Children.Add(info);
        }

        private void exitInventory(object sender, RoutedEventArgs e) {
            DetailList.Children.Clear();
            _InvGrid.Visibility = Visibility.Collapsed;
        }
        
        private void ChangePage(object sender, RoutedEventArgs e) {
            Button Selected = sender as Button;
            string dest = Selected.Content as string;
            showDetail(dest);
        }

        private void showDetail(string view) {
            if (view == "Item") {
                DetailItem();
            }
            else if (view == "Pokemon") {
                DetailPokemon();
            }
            else if (view == "Potions") {
                DetailPotions();
            }
            else if (view == "Trade") {
                DetailTrade();
            }
        }

        private void DetailItem() {
            DetailList.Children.Clear();

            foreach (KeyValuePair<string, int> item in user1.getInventory) {
                string text = "Item: " + item.Key.PadRight(30, ' ') + "Count: " + item.Value.ToString();
                
                TextBlock newBlock = new TextBlock();
                newBlock.Height = 40;
                newBlock.Text = text;

                DetailList.Children.Add(newBlock);
            }
            Button buyBall = new Button();
            buyBall.Content = "Buy Ball with 10 coin";
            buyBall.HorizontalAlignment = HorizontalAlignment.Left;
            buyBall.Width = 450;
            buyBall.Click += new RoutedEventHandler(buyOneBall);
            DetailList.Children.Add(buyBall);
        }

        private void buyOneBall(object sender, RoutedEventArgs e) {
            user1.addItem("pokeball", 1);
            user1.removeItem("coin", 10);
            DetailItem();
        }

        private void DetailPokemon() {
            DetailList.Children.Clear();
            foreach (PokemonTemplate pokemon in user1.getPokemonOwned) {
                StackPanel PokePanel = new StackPanel();
                PokePanel.Orientation = Orientation.Horizontal;

                Image img = new Image();
                try {
                    img.Source = new BitmapImage(new Uri("pack://application:,,,/image/" + pokemon.getID.ToString().Replace(" ", "") + ".png"));
                }
                catch (Exception) {
                    img.Source = new BitmapImage(new Uri("pack://application:,,,/image/pokeball.png"));
                }
                img.Height = 40;
                img.Width = 40;
                PokePanel.Children.Add(img);

                string text = String.Format("Name: {0}, Type: {1}\nHP: {2}/{3}, MP: {4}/{5}", pokemon.getName.PadRight(15, ' '), pokemon.getType.PadRight(10, ' '), pokemon.GetHP, pokemon.GetMaxHP, pokemon.GetMP, pokemon.GetMaxMP);
                TextBlock newBlock = new TextBlock();
                newBlock.Height = 40;
                newBlock.Width = 220;
                newBlock.Text = text;
                PokePanel.Children.Add(newBlock);

                Button renameButton = new Button();
                renameButton.Click += new RoutedEventHandler((sender, e) => rename(sender, e, pokemon));
                renameButton.Content = "Rename";
                PokePanel.Children.Add(renameButton);

                Button evolveButton = new Button();
                evolveButton.Click += new RoutedEventHandler((sender, e) => evolve(sender, e, pokemon));
                evolveButton.Content = "Evolve\n-100 coin";
                PokePanel.Children.Add(evolveButton);

                Button sellButton = new Button();
                sellButton.Click += new RoutedEventHandler((sender, e) => sell(sender, e, pokemon));
                sellButton.Content = "Sell\n+100 coin";
                PokePanel.Children.Add(sellButton);

                DetailList.Children.Add(PokePanel);
            }
            Button refresh = new Button();
            refresh.Content = "Refresh";
            refresh.HorizontalAlignment = HorizontalAlignment.Left;
            refresh.Width = 450;
            refresh.Click += new RoutedEventHandler(refreshPokemonView);
            DetailList.Children.Add(refresh);
        }

        private void rename(object sender, RoutedEventArgs e, PokemonTemplate pokemon) {
            renameWin window = new renameWin(pokemon);
            window.Show();
        }

        private void evolve(object sender, RoutedEventArgs e, PokemonTemplate pokemon) {
            pokemon.Evolve();
            user1.removeItem("coin", 100);
            refreshPokemonView(sender, e);
        }

        private void sell(object sender, RoutedEventArgs e, PokemonTemplate pokemon) {
            user1.releasePokemon(pokemon);
            user1.addItem("coin", 100);
            refreshPokemonView(sender, e);
        }

        private void refreshPokemonView(object sender, RoutedEventArgs e) {
            DetailPokemon();
        }

        private void DetailPotions() {
            DetailList.Children.Clear();
            foreach (PokemonTemplate pokemon in user1.getPokemonOwned) {
                StackPanel PokePanel = new StackPanel();
                PokePanel.Orientation = Orientation.Horizontal;

                Image img = new Image();
                try {
                    img.Source = new BitmapImage(new Uri("pack://application:,,,/image/" + pokemon.getID.ToString().Replace(" ", "") + ".png"));
                }
                catch (Exception) {
                    img.Source = new BitmapImage(new Uri("pack://application:,,,/image/pokeball.png"));
                }
                img.Height = 40;
                img.Width = 40;
                PokePanel.Children.Add(img);

                string text = String.Format("Name: {0}, Type: {1}\nHP: {2}/{3}, MP: {4}/{5}", pokemon.getName.PadRight(15, ' '), pokemon.getType.PadRight(10, ' '), pokemon.GetHP, pokemon.GetMaxHP, pokemon.GetMP, pokemon.GetMaxMP);
                TextBlock newBlock = new TextBlock();
                newBlock.Height = 40;
                newBlock.Width = 220;
                newBlock.Text = text;
                PokePanel.Children.Add(newBlock);

                Button pupButton = new Button();
                pupButton.Click += new RoutedEventHandler((sender, e) => powerup(sender, e, pokemon));
                pupButton.Width = 100;
                pupButton.Content = "PowerUp\n-1 Potion";
                PokePanel.Children.Add(pupButton);

                Button HPButton = new Button();
                HPButton.Click += new RoutedEventHandler((sender, e) => HPPotion(sender, e, pokemon));
                HPButton.Width = 50;
                HPButton.Content = "HP\n+50";
                PokePanel.Children.Add(HPButton);

                Button MPButton = new Button();
                MPButton.Click += new RoutedEventHandler((sender, e) => MPPotion(sender, e, pokemon));
                MPButton.Width = 50;
                MPButton.Content = "MP\n+50";
                PokePanel.Children.Add(MPButton);

                DetailList.Children.Add(PokePanel);
            }
        }

        private void powerup(object sender, RoutedEventArgs e, PokemonTemplate pokemon) {
            if (user1.getInventory.ContainsKey("PowerUpPotion")) {
                pokemon.PowerUp();
                user1.removeItem("PowerUpPotion", 1);
                refreshPotionView();
            }
        }

        private void HPPotion(object sender, RoutedEventArgs e, PokemonTemplate pokemon) {
            if (user1.getInventory.ContainsKey("HPPotion")) {
                pokemon.RecoverHP(50);
                user1.removeItem("HPPotion", 1);
                refreshPotionView();
            }
        }

        private void MPPotion(object sender, RoutedEventArgs e, PokemonTemplate pokemon) {
            if (user1.getInventory.ContainsKey("MPPotion")) {
                pokemon.RecoverMP(50);
                user1.removeItem("HPPotion", 1);
                refreshPotionView();
            }
        }

        public void refreshPotionView() {
            DetailPotions();
        }

        private void DetailTrade() {
            DetailList.Children.Clear();

            TextBlock warning = new TextBlock();
            warning.Text = "DUPLICATION MIGHT HAPPEN, TRADE AT YOUR OWN RISK (why not just sell)";
            warning.Height = 20;
            DetailList.Children.Add(warning);

            TextBlock P1_Block = new TextBlock();
            P1_Block.Text = "Pokemon 1: ";
            P1_Block.Height = 20;
            DetailList.Children.Add(P1_Block);

            ComboBox S1_Combo = new ComboBox();
            S1_Combo.Height = 20;
            S1_Combo.Width = 200;
            S1_Combo.HorizontalAlignment = HorizontalAlignment.Left;
            S1_Combo.ItemsSource = user1.getPokemonOwned;
            DetailList.Children.Add(S1_Combo);

            TextBlock P2_Block = new TextBlock();
            P2_Block.Text = "Pokemon 2: ";
            P2_Block.Height = 20;
            DetailList.Children.Add(P2_Block);

            ComboBox S2_Combo = new ComboBox();
            S2_Combo.Height = 20;
            S2_Combo.Width = 200;
            S2_Combo.HorizontalAlignment = HorizontalAlignment.Left;
            S2_Combo.ItemsSource = user1.getPokemonOwned;
            DetailList.Children.Add(S2_Combo);

            
            Button proceed = new Button();
            proceed.Content = "Proceed";
            proceed.Width = 200;
            proceed.HorizontalAlignment = HorizontalAlignment.Left;
            proceed.Click += new RoutedEventHandler((sender, e)=>startReforge(sender, e, S1_Combo.SelectedItem as PokemonTemplate, S2_Combo.SelectedItem as PokemonTemplate));
            DetailList.Children.Add(proceed);
        }


        private void startReforge(object sender, RoutedEventArgs e, PokemonTemplate P1, PokemonTemplate P2) {
            if (P1 != P2) {
                ReforgeGame _reforgeGame = ReforgeGame.Instance;
                _reforgeGame.startGame(P1, P2);
            }
        }
        
        public void refreshTrade() {
            DetailTrade();
        }


    }

    class ReforgeGame {
        private static ReforgeGame _instance;
        public static ReforgeGame Instance {
            get {
                if (_instance == null) {
                    _instance = new ReforgeGame();
                }
                return _instance;
            }
        }

        private StackPanel selection = new StackPanel();

        private Random rand = new Random();
        private Inventory inv = Inventory.Instance;
        private User user = User.Instance;
        
        private int goal;
        private int current;
        private TextBlock perc = new TextBlock();
        public ReforgeGame() {
            selection.Orientation = Orientation.Horizontal;
            
        }

        private PokemonTemplate P1;
        private PokemonTemplate P2;

        public void startGame(PokemonTemplate P1, PokemonTemplate P2) {
            this.P1 = P1;
            this.P2 = P2;
            selection.Children.Clear();
            
            Button Plus2 = new Button();
            Plus2.Content = "+2";
            Plus2.Width = 30;
            Plus2.Click += new RoutedEventHandler((sender, e) => add(sender, e, 2));
            Button Plus3 = new Button();
            Plus3.Content = "+3";
            Plus3.Width = 30;
            Plus3.Click += new RoutedEventHandler((sender, e) => add(sender, e, 3));
            Button Plus7 = new Button();
            Plus7.Content = "+7";
            Plus7.Width = 30;
            Plus7.Click += new RoutedEventHandler((sender, e) => add(sender, e, 7));
            Button Reset = new Button();
            Reset.Content = "R";
            Reset.Width = 30;
            Reset.Click += new RoutedEventHandler((sender, e) => add(sender, e, -1));
            selection.Children.Add(Plus2);
            selection.Children.Add(Plus3);
            selection.Children.Add(Plus7);
            selection.Children.Add(Reset);

            current = 0;
            goal = rand.Next(12, 51);

            perc.Text = "0 / " + goal.ToString();

            selection.Children.Add(perc);

            inv.DetailList.Children.Add(selection);

        }

        private void add(object sender, RoutedEventArgs e, int i) {
            if (i < 0) {
                this.current = 0;
            }
            else {
                this.current += i;
            }
            perc.Text = current.ToString() + " / " + goal.ToString();

            if (current == goal) {
                user.releasePokemon(P1);
                user.releasePokemon(P2);
                var NewPokemon = PokemonFactory.accessDatabase[rand.Next(PokemonFactory.accessDatabase.Count)].Clone() as PokemonTemplate;
                user.addPokemon(NewPokemon);
                inv.refreshTrade();
            }

        }


    }
}
