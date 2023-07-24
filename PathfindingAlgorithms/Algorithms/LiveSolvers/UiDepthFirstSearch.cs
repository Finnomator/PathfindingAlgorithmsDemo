using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms.LiveSolvers {
    public static class UiDepthFirstSearch {

        static private int _stepDelayMs;
        static private readonly Dictionary<UiNode, UiNode> Predecessor = new();
        static private bool _foundEnd;
        
        public static async Task<bool> Solve(GridField field, int stepDelayMs) {
            _stepDelayMs = stepDelayMs;
            _foundEnd = false;
            Predecessor.Clear();
            
            await Solve(field, field.StartNode!);
            
            if (!Predecessor.ContainsKey(field.EndNode!))
                return false;
            
            UiNode current = Predecessor[field.EndNode!];

            while (current.Position != field.StartNode!.Position) {
                current.SetState(NodeState.Path);
                current = Predecessor[current];
                await Task.Delay(stepDelayMs);
            }

            return true;
        }

        static private async Task Solve(GridField field, UiNode startNode) {

            if (startNode.Position == field.EndNode!.Position) {
                Predecessor[startNode] = field.EndNode;
                _foundEnd = true;
                return;
            }
            
            if (startNode != field.StartNode) {
                startNode.SetState(NodeState.SearchHead);
                await Task.Delay(_stepDelayMs);
                startNode.SetState(NodeState.Visited);
            }

            foreach (UiNode node in field.GetNeighbors(startNode, false, NodeState.Destination, NodeState.Normal)) {
                if (!_foundEnd)
                    await Solve(field, node);
                Predecessor[node] = startNode;
            }
        }
    }
}
