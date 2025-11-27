namespace AdventOfCodeSolution.Models
{
    public class SimulationResult
    {
        // عدد الروبوتات في الربع العلوي الأيسر
        public int TopLeft { get; set; }
        // عدد الروبوتات في الربع العلوي الأيمن
        public int TopRight { get; set; }
        // عدد الروبوتات في الربع السفلي الأيسر
        public int BottomLeft { get; set; }
        // عدد الروبوتات في الربع السفلي الأيمن
        public int BottomRight { get; set; }
        // عامل الأمان = حاصل ضرب الأعداد في الأرباع الأربعة
        public int SafetyFactor { get; set; }
        // قائمة بالروبوتات بعد المحاكاة
        public List<Robot> FinalPositions { get; set; } = new();
        public GridVisualization Visualization { get; set; } = new();
        public string Analysis { get; set; } = string.Empty;
    }
}
