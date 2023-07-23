using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PathfindingAlgorithms {
    public class UiNode : Button {

        public UiNode[] Neighbors { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public GridField ContainerField { get; private set; }
        public NodeState State { get; private set; }

        public UiNode(int x, int y, UiNode[] neighbors, GridField field) {
            Neighbors = neighbors;
            X = x;
            Y = y;
            ContainerField = field;
            
            SetState(NodeState.Normal);
            
            BorderThickness = new(0.5);
        }

        public void SetState(NodeState state) {
            State = state;
            Background = NodeStateColor(state);
        }

        public static Brush NodeStateColor(NodeState state) => state switch {
            NodeState.Normal => Brushes.LightGray,
            NodeState.Destination => Brushes.Red,
            NodeState.Wall => Brushes.Black,
            NodeState.Visited => Brushes.Gray,
            NodeState.Path => Brushes.CornflowerBlue,
            NodeState.Start => Brushes.Blue,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }

    public enum NodeState {
        Normal,
        Wall,
        Visited,
        Path,
        Destination,
        Start
    }
}
