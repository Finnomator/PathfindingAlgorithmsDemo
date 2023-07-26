using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms.LiveSolvers {
    public class UiBreathFirstSearch: UiAlgorithm  {


        public override async Task Solve() {

            Dictionary<(int, int), (int, int)> predecessor = new();

            Queue<UiNode> queue = new();
            queue.Enqueue(Field.StartNode!);

            while (queue.Count > 0) {
                UiNode current = queue.Dequeue();

                if (current.Position == Field.EndNode!.Position)
                    break;

                foreach (UiNode neighbor in Field.GetNeighbors(current)
                             .Where(neighbor => neighbor.State is not (NodeState.Visited or NodeState.Start or NodeState.Wall))) {

                    predecessor[neighbor.Position] = current.Position;
                    queue.Enqueue(neighbor);
                    
                    if (neighbor.Position != Field.EndNode.Position) {
                        neighbor.SetState(NodeState.SearchHead);
                        await Task.Delay(StepDelayMs);
                        neighbor.SetState(NodeState.Visited);
                    }
                }
            }

            if (!predecessor.ContainsKey(Field.EndNode!.Position))
                return;

            (int x, int y) walkBackNode = predecessor[Field.EndNode!.Position];

            while (walkBackNode != Field.StartNode!.Position) {
                Field.Nodes[walkBackNode.y][walkBackNode.x].SetState(NodeState.Path);
                walkBackNode = predecessor[walkBackNode];
                await Task.Delay(StepDelayMs);
            }
        }

        public UiBreathFirstSearch(GridField field, int stepDelayMs) : base(field, stepDelayMs) {
        }
    }
}
