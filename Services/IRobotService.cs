using AdventOfCodeSolution.Models;

namespace AdventOfCodeSolution.Services
{
    public interface IRobotService
    {
        
        SimulationResult SimulateRobots(List<Robot> robots, int width, int height, int seconds);
        
        List<Robot> ParseInput(string input);
    }
}
