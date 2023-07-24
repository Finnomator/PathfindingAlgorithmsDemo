using System.Collections.Generic;
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
            NewField();
            DataContext = this;
        }

        private void NewField() {
            if (_field != null)
                Root.Children.Remove(_field);
            
            GridField newField = new(20, 20, 10) {
                VerticalAlignment = VerticalAlignment.Center,
            };

            if (_field != null && newField.Rows == _field.Rows && newField.Columns == _field.Columns) {
                foreach (UiNode[] row in _field.Nodes) {
                    foreach (UiNode node in row) {
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
            NewField();
            Stopwatch sw = Stopwatch.StartNew();
            await UiDepthFirstSearch.Solve(_field!, StepDelayMs);
            sw.Stop();
            SolvingTimeLabel.Content = $"Time: {sw.ElapsedMilliseconds}ms";
        }
    }
}
