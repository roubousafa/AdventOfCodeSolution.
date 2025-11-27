using AdventOfCodeSolution.Models;

namespace AdventOfCodeSolution.Services
{
    public interface IRobotService
    {
        // دالة لمحاكاة حركة الروبوتات
        SimulationResult SimulateRobots(List<Robot> robots, int width, int height, int seconds);
        // دالة لتحليل النص المدخل إلى كائنات روبوت
        List<Robot> ParseInput(string input);
    }
}
