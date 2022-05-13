using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using IERG3080PartII.Presenter;

namespace IERG3080PartII {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        PresenterClass Presenter;

        public MainWindow() {
            Presenter = PresenterClass.Instance;
            
        }

        public bool[,] adjArray = { 
            { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
            { false, true, true, false, false, false, true, false, false, false, false, false, false, false, false },
            { false, true, true, true, false, false, false, true, false, false, false, false, false, false, false },
            { false, false, true, true, true, false, false, false, true, false, false, false, false, false, false },
            { false, false, false, true, true, false, false, false, true, false,false, false, false, false, false },
            { false, false, false, false, false, true, true, false, false, false, true, false, false, false, false },
            { false, true, false, false, false, true, true, true, false, false, false, true, false, false, false },
            { false, false, true, false, false, false, true, true, true, false, false, false, true, false, false },
            { false, false, false, true, true, false, false, true, true, false, false, false, false, true, false },
            { false, false, false, false, false, false, false, false, false, true, false, false, false, false, false },
            { false, false, false, false, false, true, false, false, false, false, true, true, false, false, false },
            { false, false, false, false, false, false, true, false, false, false, true, true, true, false, false },
            { false, false, false, false, false, false, false, true, false, false,false, true, true, true, false },
            { false, false, false, false, false, false, false, false, true, false, false, false, true, true, false },
            { false, false, false, false, false, false, false, false, false, false, false, false, false, false, true },
        };

        public void StartButton_Click(object sender, RoutedEventArgs e) {
            RadioButton[] nodes = new RadioButton[adjArray.GetLength(0)];
            for (int i = 0; i < adjArray.GetLength(0); i++) {
                if (NodeGrid.Children[i] is RadioButton) {
                    nodes[i] = NodeGrid.Children[i] as RadioButton;
                }
            }
            Presenter.initMap(nodes, adjArray);
            StartButton.IsEnabled = false;
            StartGrid.Children.Remove(StartButton);
            Grid parentGrid = StartGrid.Parent as Grid;
            parentGrid.Children.Remove(StartGrid);

            Presenter.setUser();

            Button[] seqButtons = new Button[9];
            for (int i = 0; i < 9; i++) {
                seqButtons[i] = GamePanel.Children[i] as Button;
            }
            Presenter.SeqButtons = seqButtons;

            Presenter.InvGrid = InventoryGrid;
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e) {
            Presenter.ShowInventory();
        }

    }

}
