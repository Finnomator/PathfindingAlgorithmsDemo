using System.Threading.Tasks;

namespace PathfindingAlgorithms.Algorithms.LiveSolvers {
    public abstract class UiAlgorithm {
        protected readonly GridField Field;
        protected readonly int StepDelayMs;

        protected UiAlgorithm(GridField field, int stepDelayMs) {
            Field = field;
            StepDelayMs = stepDelayMs;
        }
        
        public abstract Task Solve();
    }
}
