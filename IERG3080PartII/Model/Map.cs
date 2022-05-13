using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

// By HO, Tsz Ngong

namespace IERG3080PartII.Model {
    public class Map {
        static private Map _instance;
        private RadioButton[] nodes;
        private bool[,] adjNodes;
        private int currentNodeID;

        public RadioButton LastNode {
            get {
                return (nodes[nodes.Length - 1]);
            }
        }

        static public Map Instance {
            get {
                if (_instance == null) {
                    _instance = new Map();
                }
                return _instance;
            }
        }

        public void initMap(RadioButton[] nodes, bool[,] adjNodes) {
            this.nodes = nodes;
            this.adjNodes = adjNodes;
            foreach (RadioButton node in nodes) {
                node.IsChecked = false;
                node.IsEnabled = false;
            }
        }

        public void setCurrent(int nodeID) {
            this.currentNodeID = nodeID;
            this.nodes[this.currentNodeID].IsEnabled = true;
            this.nodes[this.currentNodeID].IsChecked = true;
            for (int i = 0; i < nodes.Length; i++) {
                this.nodes[i].IsEnabled = adjNodes[this.currentNodeID, i];
            }
        }

        public int getCurrent() {
            return this.currentNodeID;
        }
    }
}
