namespace AdventOfCodeSolution.Models
{
    public class GridVisualization
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<string> GridLines { get; set; } = new();
        public string Visualization { get; set; } = string.Empty;
    }
}
