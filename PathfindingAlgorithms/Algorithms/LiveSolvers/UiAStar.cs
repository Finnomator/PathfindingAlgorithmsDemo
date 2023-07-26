using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms.LiveSolvers {
    public class UiAStar : UiAlgorithm {
        
        public UiAStar(GridField field, int stepDelayMs) : base(field, stepDelayMs) {
        }

        public override async Task Solve() {
            PriorityQueue<UiNode, double> openQueue = new();
            HashSet<(int x, int y)> openSet = new() {
                Field.StartNode!.Position
            };
            openQueue.Enqueue(Field.StartNode!, Field.StartNode.EuclideanDistance(Field.EndNode!));
            Dictionary<(int, int), UiNode> predecessors = new();
            Dictionary<(int, int), double> gScore = new();

            foreach (UiNode[] row in Field.Nodes)
            foreach (UiNode node in row)
                gScore[node.Position] = double.PositiveInfinity;

            gScore[Field.StartNode.Position] = 0;

            while (openQueue.Count > 0) {
                
                UiNode current = openQueue.Dequeue();
                openSet.Remove(current.Position);

                if (current.Position == Field.EndNode!.Position) {
                    await ReconstructPath(predecessors, current, StepDelayMs);
                    return;
                }

                foreach (UiNode neighbor in Field.GetNeighbors(current, true, NodeState.Wall, NodeState.Start)) {
                    // tentativeGScore is the distance from start to the neighbor through current
                    double tentativeGScore = gScore[current.Position] + 1;

                    if (tentativeGScore >= gScore[neighbor.Position])
                        continue;

                    predecessors[neighbor.Position] = current;
                    gScore[neighbor.Position] = tentativeGScore;

                    if (openSet.Contains(neighbor.Position))
                        continue;
                    
                    //openQueue.Enqueue(neighbor,tentativeGScore + neighbor.EuclideanDistance(Field.EndNode));
                    openQueue.Enqueue(neighbor,neighbor.EuclideanDistance(Field.EndNode));
                    //openQueue.Enqueue(neighbor,neighbor.EuclideanDistance(Field.StartNode) + neighbor.EuclideanDistance(Field.EndNode));
                    openSet.Add(neighbor.Position);
                    
                    neighbor.SetState(NodeState.SearchHead);
                    await Task.Delay(StepDelayMs);
                    neighbor.SetState(NodeState.Visited);
                }
            }
        }

        static private async Task ReconstructPath(IReadOnlyDictionary<(int, int), UiNode> predecessors, UiNode current,
            int stepDelayMs) {
            while (predecessors.ContainsKey(current.Position)) {
                current = predecessors[current.Position];
                current.SetState(NodeState.Path);
                await Task.Delay(stepDelayMs);
            }
        }

        
    }
}
