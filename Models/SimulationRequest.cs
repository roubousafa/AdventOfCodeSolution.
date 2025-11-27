namespace AdventOfCodeSolution.Models
{
    public class SimulationRequest
    {
        // النص المدخل للروبوتات
        public string InputData { get; set; } = string.Empty;
        // عرض الشبكة (الافتراضي 101)
        public int Width { get; set; } = 101;
        // ارتفاع الشبكة (الافتراضي 103)
        public int Height { get; set; } = 103;
        // عدد الثواني للمحاكاة (الافتراضي 100)
        public int Seconds { get; set; } = 100;
    }
}
