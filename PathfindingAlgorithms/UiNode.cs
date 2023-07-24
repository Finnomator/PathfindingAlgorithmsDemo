using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace PathfindingAlgorithms {
    public class UiNode : Button {

        public UiNode[] Neighbors { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public (int x, int y) Position => (X, Y);
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

        public double EuclideanDistance(UiNode node) => Math.Sqrt(SquaredEuclideanDistance(node));
        public double SquaredEuclideanDistance(UiNode node) => Math.Pow(X - node.X, 2) + Math.Pow(Y - node.Y, 2);
        public int ManhattanDistance(UiNode node) => Math.Abs(X - node.X) + Math.Abs(Y - node.Y);

        public static Brush NodeStateColor(NodeState state) => state switch {
            NodeState.Normal => Brushes.LightGray,
            NodeState.Destination => Brushes.Red,
            NodeState.Wall => Brushes.Black,
            NodeState.Visited => Brushes.Gray,
            NodeState.Path => Brushes.CornflowerBlue,
            NodeState.Start => Brushes.Blue,
            NodeState.SearchHead => Brushes.Lime,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }

    public enum NodeState {
        Normal,
        Wall,
        Visited,
        Path,
        Destination,
        Start,
        SearchHead
    }
}
