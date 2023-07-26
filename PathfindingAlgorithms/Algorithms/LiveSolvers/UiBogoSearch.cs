using System;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms.LiveSolvers {
    public class UiBogoSearch : UiAlgorithm  {

        static private readonly Random Rnd = new();
        
        public override async Task Solve() {
            UiNode current = Field.StartNode!;

            while (true) {
                UiNode[] neighbors = Field.GetNeighbors(current, true, NodeState.Path, NodeState.Wall, NodeState.Start);

                if (neighbors.Length == 0)
                    break;

                current = neighbors[Rnd.Next(neighbors.Length)];
                
                if (current.Position == Field.EndNode!.Position)
                    break;
                
                current.SetState(NodeState.Path);

                await Task.Delay(StepDelayMs);
            }
        }

        public UiBogoSearch(GridField field, int stepDelayMs) : base(field, stepDelayMs) {
        }
    }
}
