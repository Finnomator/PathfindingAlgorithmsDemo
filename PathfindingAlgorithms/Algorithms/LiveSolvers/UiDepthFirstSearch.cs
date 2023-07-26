using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms.LiveSolvers {
    public class UiDepthFirstSearch: UiAlgorithm  {

        static private readonly Dictionary<(int, int), (int, int)> Predecessor = new();
        static private bool _foundEnd;
        
        public override async Task<bool> Solve() {
            _foundEnd = false;
            Predecessor.Clear();
            
            await Solve(Field.StartNode!);
            
            if (!Predecessor.ContainsKey(Field.EndNode!.Position))
                return false;
            
            (int x, int y) current = Predecessor[Field.EndNode!.Position];

            while (current != Field.StartNode!.Position) {
                Field.Nodes[current.y][current.x].SetState(NodeState.Path);
                current = Predecessor[current];
                await Task.Delay(StepDelayMs);
            }

            return true;
        }

        private async Task Solve(UiNode startNode) {

            if (startNode.Position == Field.EndNode!.Position) {
                Predecessor[startNode.Position] = Field.EndNode.Position;
                _foundEnd = true;
                return;
            }
            
            if (startNode != Field.StartNode) {
                startNode.SetState(NodeState.SearchHead);
                await Task.Delay(StepDelayMs);
                startNode.SetState(NodeState.Visited);
            }

            foreach (UiNode node in Field.GetNeighbors(startNode, false, NodeState.Destination, NodeState.Normal)) {
                if (!_foundEnd)
                    await Solve(node);
                Predecessor[node.Position] = startNode.Position;
            }
        }

        public UiDepthFirstSearch(GridField field, int stepDelayMs) : base(field, stepDelayMs) {
        }
    }
}
