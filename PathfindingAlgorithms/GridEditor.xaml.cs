using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using PathfindingAlgorithms.Algorithms.LiveSolvers;

namespace PathfindingAlgorithms {
    public partial class GridEditor {

        public Dictionary<string, NodeState> DrawingModesCaptions { get; } = new() {
            { "Empty", NodeState.Normal },
            { "Wall", NodeState.Wall },
            { "Destination", NodeState.Destination },
            { "Start", NodeState.Start }
        };
        
        public int StepDelayMs => int.Parse(StepDelayBox.Text);

        private GridField? _field;

        public GridEditor() {
            InitializeComponent();
            NewField(50, 50, 10);
            DataContext = this;
        }

        private void NewField(int width, int height, int nodeSize) {
            if (_field != null)
                Root.Children.Remove(_field);
            
            GridField newField = new(width, height, nodeSize) {
                VerticalAlignment = VerticalAlignment.Center,
            };

            if (_field != null && newField.Rows == _field.Rows && newField.Columns == _field.Columns) {
                foreach (UiNode[] row in _field.Nodes) {
                    foreach (UiNode node in row) {
                        
                        if (node.State == NodeState.Start)
                            newField.SetStartPoint(node);
                        else if (node.State == NodeState.Destination)
                            newField.SetEndPoint(node);
                        
                        if (DrawingModesCaptions.ContainsValue(node.State))
                            newField.Nodes[node.Y][node.X].SetState(node.State);
                    }
                }
            }

            _field = newField;
            Root.Children.Add(_field);
        }

        private void DrawModeBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (_field != null)
                _field.DrawMode = ((KeyValuePair<string, NodeState>) DrawModeBox.SelectedItem).Value;
        }

        private async void Solve_Click(object sender, RoutedEventArgs e) {
            NewField(50, 50, 10);

            UiAlgorithm algorithm = AlgorithmBox.SelectedIndex switch {
                0 => new UiAStar(_field!, StepDelayMs),
                1 => new UiBreathFirstSearch(_field!, StepDelayMs),
                2 => new UiDepthFirstSearch(_field!, StepDelayMs),
                _ => throw new InvalidEnumArgumentException()
            };

            Stopwatch sw = Stopwatch.StartNew();
            await algorithm.Solve();
            sw.Stop();
            
            SolvingTimeLabel.Content = $"Time: {sw.ElapsedMilliseconds}ms";
        }

        
    }
}
