using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms.LiveSolvers {
    public static class UiNearestNeighbor {
        
        public static async Task Solve(GridField field, int stepDelayMs) {

            UiNode current = field.StartNode!;

            while (true) {

                UiNode[] neighbors = field.GetNeighbors(current, true, NodeState.Wall, NodeState.Start, NodeState.Path);

                if (neighbors.Length == 0)
                    break;

                double min = double.PositiveInfinity;

                foreach (UiNode neighbor in neighbors) {
                    
                    double dist = neighbor.SquaredEuclideanDistance(field.EndNode!);
                    
                    if (dist >= min)
                        continue;

                    current = neighbor;
                    min = dist;
                }
                
                if (current.Position == field.EndNode!.Position)
                    break;
                
                current.SetState(NodeState.Path);

                await Task.Delay(stepDelayMs);
            }
        }
    }
}
