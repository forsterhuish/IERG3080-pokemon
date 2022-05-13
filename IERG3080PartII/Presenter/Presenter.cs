using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using IERG3080PartII.Model;
using IERG3080PartII;

namespace IERG3080PartII.Presenter
{
    public class PresenterClass 
    {
        private static PresenterClass _instance;

        public string setInfo() {   // tmp
            return pSpawner.getName;
        }

        public static PresenterClass Instance {
            get {
                if (_instance == null) {
                    _instance = new PresenterClass();
                }
                return _instance;
            }
        }

        private static User user1;
        private static Map map1;
        private static Grid crossGrid;
        private static Random rand;
        private static PokemonSpawner pSpawner;
        private static Inventory inventoryModel;

        private static CatchGame seqGame;
        private static Button[] seqButtons;
        
        public Button[] SeqButtons {
            set {
                seqButtons = value;
            }
            get {
                return seqButtons;
            }
        }

        private PresenterClass() {
            rand = new Random();
        }

        public void setUser()
        {
            user1 = User.Instance;

            /* TESTING */
            user1.addItem("pokeball", 10);
            user1.addPokemon(new Elsa());
            user1.addPokemon(new Pikachu());
            user1.addPokemon(new FireDragon());

            user1.addItem("pokeball", 10);
            user1.addItem("coin", 2000);
            user1.addItem("PowerUpPotion", 3);
            user1.addItem("HPPotion", 3);
            user1.addItem("MPPotion", 3);
        }

        public void initMap(RadioButton[] nodes, bool[,] adjNodes) {

            map1 = Map.Instance;
            map1.initMap(nodes, adjNodes);
            map1.setCurrent(10);

            foreach (RadioButton node in nodes) {
                node.Checked += new RoutedEventHandler(Move);
            }

            PokemonFactory.InitDatabase();
            crossGrid = map1.LastNode.Parent as Grid;
            pSpawner = PokemonSpawner.Instance;
            inventoryModel = Inventory.Instance;

            SpawnButton("Battle", 0, 4);
        }

        public void Move(object sender, RoutedEventArgs e) {
            RadioButton nodePressed = sender as RadioButton;
            int RowIndex = Grid.GetRow(nodePressed);
            int ColumnIndex = Grid.GetColumn(nodePressed);
            int maxColumn = Grid.GetColumn(map1.LastNode) + 1;
            int nodeID = (RowIndex * maxColumn) + ColumnIndex;
            onMove(nodeID);
        }
        
        public void onMove(int nodeID) {
            map1.setCurrent(nodeID);
        }

        /*---------- For spawner ---------*/
        public void SpawnButton(string ButtonType, int x = 0, int y = 0) {
            Button wildButton = new Button();
            Image img = new Image();
            if (ButtonType == "Pokemon") {
                wildButton.Name = "wildPokemonButton";
                img.Source = new BitmapImage(new Uri("pack://application:,,,/image/pokeball.png"));
                wildButton.Click += new RoutedEventHandler(encounter);

                x = rand.Next(Grid.GetRow(map1.LastNode) + 1);
                y = rand.Next(Grid.GetColumn(map1.LastNode) + 1);
            }
            else if (ButtonType == "Battle") {
                wildButton.Name = "battle";
                img.Source = new BitmapImage(new Uri("pack://application:,,,/image/fight.png"));
                wildButton.Click += new RoutedEventHandler(newBattle);
            }

            Grid.SetRow(wildButton, x);
            Grid.SetColumn(wildButton, y);

            StackPanel sPan1 = new StackPanel();
            sPan1.Children.Add(img);
            wildButton.Content = sPan1;

            wildButton.Height = 20;
            wildButton.Width = 20;
            Thickness margin = wildButton.Margin;
            margin.Left = rand.Next(100 - (int)wildButton.Width);
            margin.Top = rand.Next(100 - (int)wildButton.Height);
            wildButton.Margin = margin;
            
            // find which grid block to add in
            crossGrid.Children.Add(wildButton);
        }

        // For battle
        private void newBattle(object sender, RoutedEventArgs e)
        {
            if (Grid.GetRow(sender as Button) * (Grid.GetColumn(Map.Instance.LastNode) + 1) + Grid.GetColumn(sender as Button) == Map.Instance.getCurrent()) {
                battleWindow w = new battleWindow();
                w.Show();
            }
        }

        // Remove specific Button
        public void RemoveButton(object sender) {
            crossGrid.Children.Remove(sender as Button);
        }

        // Remove random pokemon button (natural remove)
        public void RemoveButton() {
            List<Button> actPokemon = new List<Button>();
            for (int i = 0; i < crossGrid.Children.Count; i++) {
                if (!(crossGrid.Children[i] is RadioButton)) {
                    Button tmp = (Button)crossGrid.Children[i];
                    if (tmp.Name == "wildPokemonButton") {
                        actPokemon.Add(tmp);
                    }
                }
            }
            crossGrid.Children.Remove(actPokemon[rand.Next(actPokemon.Count)]);
        }

        private void encounter(object sender, RoutedEventArgs e) {
            if (Grid.GetRow(sender as Button) * (Grid.GetColumn(Map.Instance.LastNode) + 1) + Grid.GetColumn(sender as Button) == Map.Instance.getCurrent()) {
                if (user1.getInventory.ContainsKey("pokeball")) {
                    pSpawner.pauseSpawner();
                    // Catch
                    StartCatchGame();
                    // restart timer

                    pSpawner.despawnPokemon(sender);
                }
            }
        }

        private void StartCatchGame() {
            if (seqGame == null) {
                Grid catchPanel = seqButtons[0].Parent as Grid;
                Grid catchGrid = catchPanel.Parent as Grid;

                seqGame = CatchGame.Instance;
                seqGame.initCatchGame(seqButtons, catchGrid);
            }
            seqGame.newCatchGame(pSpawner.getOnScreenPoke[rand.Next(pSpawner.getOnScreenPoke.Count)]);
        }

        // Purely for other class to ccess pSpawn
        public void restartspawn() {
            pSpawner.startSpawner();
        }

        // Inventory
        private Grid _InvenGrid;
        public Grid InvGrid {
            get {
                return _InvenGrid;
            }
            set {
                _InvenGrid = value;
            }
        }

        public void ShowInventory() {
            inventoryModel.showInventory(InvGrid);
        }


        /*----------------------------------- TEMP ------------------------------*/
        public void increaseXP(int value) {
            user1.addXP(value);
        }

        public string getInfo() {
            string info = user1.getXP() + "\n" + user1.getLV();
            return info;
        }
    }
}
