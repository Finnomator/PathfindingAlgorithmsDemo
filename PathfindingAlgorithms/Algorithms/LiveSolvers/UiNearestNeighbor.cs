using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms.LiveSolvers {
    public class UiNearestNeighbor: UiAlgorithm  {
        
        public override async Task Solve() {

            UiNode current = Field.StartNode!;

            while (true) {

                UiNode[] neighbors = Field.GetNeighbors(current, true, NodeState.Wall, NodeState.Start, NodeState.Path);

                if (neighbors.Length == 0)
                    break;

                double min = double.PositiveInfinity;

                foreach (UiNode neighbor in neighbors) {
                    
                    double dist = neighbor.SquaredEuclideanDistance(Field.EndNode!);
                    
                    if (dist >= min)
                        continue;

                    current = neighbor;
                    min = dist;
                }
                
                if (current.Position == Field.EndNode!.Position)
                    break;
                
                current.SetState(NodeState.Path);

                await Task.Delay(StepDelayMs);
            }
        }

        public UiNearestNeighbor(GridField field, int stepDelayMs) : base(field, stepDelayMs) {
        }
    }
}
