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

namespace IERG3080PartII {
    /// <summary>
    /// By HO, Tsz Ngong
    /// </summary>
    public partial class renameWin : Window {
        private PokemonTemplate _pokemon;

        public renameWin(PokemonTemplate pokemon) {
            InitializeComponent();
            this._pokemon = pokemon;
            initText();
        }

        private void initText() {
            OldName_Block.Text = _pokemon.getName;
            Input_Box.Text = _pokemon.getName;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e) {
            _pokemon.Rename(Input_Box.Text);
            Close();
        }
    }
}
