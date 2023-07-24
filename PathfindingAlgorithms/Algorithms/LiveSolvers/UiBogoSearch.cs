using System;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms.LiveSolvers {
    public static class UiBogoSearch {

        static private readonly Random Rnd = new();
        
        public static async Task Solve(GridField field, int stepDelayMs) {
            UiNode current = field.StartNode!;

            while (true) {
                UiNode[] neighbors = field.GetNeighbors(current, true, NodeState.Path, NodeState.Wall, NodeState.Start);

                if (neighbors.Length == 0)
                    break;

                current = neighbors[Rnd.Next(neighbors.Length)];
                
                if (current.Position == field.EndNode!.Position)
                    break;
                
                current.SetState(NodeState.Path);

                await Task.Delay(stepDelayMs);
            }
        }
    }
}
