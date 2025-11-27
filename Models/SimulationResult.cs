namespace AdventOfCodeSolution.Models
{
    public class SimulationResult
    {
        
        public int TopLeft { get; set; }
        
        public int TopRight { get; set; }
        
        public int BottomLeft { get; set; }
      
        public int BottomRight { get; set; }
        
        public int SafetyFactor { get; set; }
       
        public List<Robot> FinalPositions { get; set; } = new();
        public GridVisualization Visualization { get; set; } = new();
        public string Analysis { get; set; } = string.Empty;
    }
}
