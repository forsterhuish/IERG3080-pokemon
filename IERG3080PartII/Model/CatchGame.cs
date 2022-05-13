using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Threading;
using System.Timers;
using System.Linq;
using IERG3080PartII.Presenter;
using System.Windows.Threading;

// By HO, Tsz Ngong

namespace IERG3080PartII.Model {
    class CatchGame {
        private Random rand;

        private static CatchGame _instance;
        public static CatchGame Instance {
            get {
                if (_instance == null) {
                    _instance = new CatchGame();
                }
                return _instance;
            }
        }

        private User _user;
        private PresenterClass _presenter;

        private Grid catchGrid;
        private Button[] buttons;
        private List<int> pattern;
        private List<int> inputpattern;
        private PokemonTemplate currentPoke;
        private Image PokemonImage;


        public void initCatchGame(Button[] buttons, Grid catchGrid) {
            this.rand = new Random();

            this.buttons = buttons;
            this.catchGrid = catchGrid;
            pattern = new List<int>();
            inputpattern = new List<int>();
            _user = User.Instance;
            _presenter = PresenterClass.Instance;

            // add event handler
            foreach (Button button in this.buttons) {
                button.Background = Brushes.White;
                button.Click += new RoutedEventHandler(seqbutton_click);
            } 
        }

        private async void seqbutton_click(object sender, RoutedEventArgs e) {
            Button buttonClicked = sender as Button;
            int ID = Grid.GetRow(buttonClicked) * 3 + Grid.GetColumn(buttonClicked);
            inputpattern.Add(ID);

            // check correctness
            if (inputpattern.Count == 5) {
                if (inputpattern.SequenceEqual(pattern)) {
                    _user.addPokemon(currentPoke);
                    _user.addXP(150);
                    _user.addItem("coin", 100);
                    _user.addItem(ItemFactory.randomItem(), 1);
                    catchGrid.Background = Brushes.LightGreen;
                }
                else {
                    catchGrid.Background = Brushes.Red;
                }
                foreach (Button button in buttons) {
                    button.IsEnabled = false;
                }
                await Task.Delay(500);
                _user.removeItem("pokeball", 1);
                catchGrid.Background = Brushes.LightBlue;
                catchGrid.Visibility = Visibility.Collapsed;

                catchGrid.Children.Remove(PokemonImage);
                _presenter.restartspawn();
            }
        }

        public async void newCatchGame(PokemonTemplate EnPokemon) {
            // reset
            pattern.Clear();
            inputpattern.Clear();

            // add photo
            this.currentPoke = EnPokemon;
            catchGrid.Visibility = Visibility.Visible;

            PokemonImage = new Image();
            try {
                PokemonImage.Source = new BitmapImage(new Uri("pack://application:,,,/image/" + EnPokemon.getID.ToString().Replace(" ", "") + ".png"));
            } catch (Exception) {
                PokemonImage.Source = new BitmapImage(new Uri("pack://application:,,,/image/pokeball.png"));
            }
            PokemonImage.Height = 150;
            PokemonImage.Width = 150;
            PokemonImage.HorizontalAlignment = HorizontalAlignment.Right;
            PokemonImage.VerticalAlignment = VerticalAlignment.Center;
            catchGrid.Children.Add(PokemonImage);

            // init pattern for this time
            for (int i = 0; i < 5; i++) {
                pattern.Add(rand.Next(9));
            }

            // flash 1 time

            await Task.Delay(2000); // delay before entering the game
            foreach (int i in pattern) {
                BlinkOff(i);
                await Task.Delay(1000);
            }

            // enable button
            foreach (Button button in buttons) {
                button.IsEnabled = true;
            }

        }
        
        private void BlinkOff(int i)
        {
            // Ref:　https://www.youtube.com/watch?v=14hWiT1rMPo
            DoubleAnimation blinkingAnim = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromSeconds(1))
            };
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(blinkingAnim);
            Storyboard.SetTarget(blinkingAnim, buttons[i]);
            Storyboard.SetTargetProperty(blinkingAnim, new PropertyPath("Opacity"));
            storyboard.Begin(buttons[i]);
        }

    }
}
