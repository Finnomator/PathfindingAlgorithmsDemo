using System.Collections.Generic;
using System.Windows.Controls;

namespace PathfindingAlgorithms {
    public partial class GridEditor {

        public Dictionary<string, NodeState> DrawingModesCaptions { get; } = new() {
            { "Empty", NodeState.Normal },
            { "Wall", NodeState.Wall },
            { "Destination", NodeState.Destination },
            { "Start", NodeState.Start }
        };

        public int StepDelayMs => int.Parse(StepDelayBox.Text);

        public GridEditor() {
            InitializeComponent();
            DataContext = this;
        }

        private void DrawModeBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            Field.DrawMode = ((KeyValuePair<string, NodeState>) DrawModeBox.SelectedItem).Value;
        }
    }
}
