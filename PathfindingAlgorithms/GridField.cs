using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PathfindingAlgorithms {
    public class GridField : UniformGrid {

        public UiNode[][] Nodes { get; }
        public UiNode? StartNode { get; private set; }
        public UiNode? EndNode { get; private set; }
        public NodeState DrawMode { get; set; }

        public GridField() : this(50, 50, 7) {
        }

        public GridField(int width, int height, double nodeSize) {
            Width = width * nodeSize;
            Height = height * nodeSize;
            DrawMode = NodeState.Wall;
            Nodes = new UiNode[height][];
            for (int y = 0; y < height; ++y)
                Nodes[y] = new UiNode[width];

            Rows = height;
            Columns = width;

            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    UiNode uiNode = new(x, y, GetNeighbors(x, y).ToArray(), this);
                    uiNode.Click += UiNodeOnClick;
                    uiNode.MouseEnter += UiNodeOnMouseEnter;
                    Grid.SetColumn(uiNode, x);
                    Grid.SetRow(uiNode, y);
                    Children.Add(uiNode);
                    Nodes[y][x] = uiNode;
                }
            }

            SetStartPoint(Nodes[0][0]);
            SetEndPoint(Nodes[height - 1][width - 1]);
        }

        private void UiNodeOnMouseEnter(object sender, MouseEventArgs e) {

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            UiNode uiNode = (UiNode) sender;

            if (DrawMode is NodeState.Destination or NodeState.Start)
                return;

            uiNode.SetState(DrawMode);
        }

        private void UiNodeOnClick(object sender, RoutedEventArgs e) {
            UiNode uiNode = (UiNode) sender;

            if (DrawMode == NodeState.Destination)
                SetEndPoint(uiNode);
            else if (DrawMode == NodeState.Start)
                SetStartPoint(uiNode);
            else
                uiNode.SetState(DrawMode);
        }

        public void SetStartPoint(UiNode node) {
            StartNode?.SetState(NodeState.Normal);
            StartNode = node;
            StartNode.SetState(NodeState.Start);
        }

        public void SetEndPoint(UiNode node) {
            EndNode?.SetState(NodeState.Normal);
            EndNode = node;
            EndNode.SetState(NodeState.Destination);
        }

        public List<UiNode> GetNeighbors(UiNode uiNode) => GetNeighbors(uiNode.X, uiNode.Y);

        public UiNode[] GetNeighbors(UiNode uiNode, bool invertedState = false, params NodeState[] states) =>
            GetNeighbors(uiNode).Where(node => invertedState ? !states.Contains(node.State) : states.Contains(node.State)).ToArray();

        public List<UiNode> GetNeighbors(int x, int y) {
            List<UiNode> neighbors = new(4);

            if (x > 0)
                neighbors.Add(Nodes[y][x - 1]);
            if (x < Columns - 1)
                neighbors.Add(Nodes[y][x + 1]);
            if (y > 0)
                neighbors.Add(Nodes[y - 1][x]);
            if (y < Rows - 1)
                neighbors.Add(Nodes[y + 1][x]);

            return neighbors;
        }
    }
}
