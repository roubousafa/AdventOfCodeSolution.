using AdventOfCodeSolution.Models;
using AdventOfCodeSolution.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdventOfCodeSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RobotController : ControllerBase
    {
        private readonly IRobotService _robotService;

        public RobotController(IRobotService robotService)
        {
            _robotService = robotService;
        }

        [HttpPost("simulate")]
        public IActionResult Simulate([FromBody] SimulationRequest request)
        {
            try
            {
                var robots = _robotService.ParseInput(request.InputData);
                var result = _robotService.SimulateRobots(robots, request.Width, request.Height, request.Seconds);

                return Ok(new { Success = true, Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            var testInput = @"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3";

            var robots = _robotService.ParseInput(testInput);
            var result = _robotService.SimulateRobots(robots, 11, 7, 100);

            return Ok(new
            {
                Success = true,
                TestResult = result,
                ExpectedSafetyFactor = 12,
                TestPassed = result.SafetyFactor == 12
            });
        }

        [HttpPost("simulate-with-visualization")]
        public IActionResult SimulateWithVisualization([FromBody] SimulationRequest request)
        {
            try
            {
                var robots = _robotService.ParseInput(request.InputData);
                var result = _robotService.SimulateRobots(robots, request.Width, request.Height, request.Seconds);

                var response = new
                {
                    Success = true,
                    Data = result,
                    Visualization = result.Visualization,
                    Analysis = result.Analysis,
                    GridText = result.Visualization.Visualization
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }

        [HttpGet("test-visual")]
        public IActionResult TestVisual()
        {
            var testInput = @"p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3";

            var robots = _robotService.ParseInput(testInput);
            var result = _robotService.SimulateRobots(robots, 11, 7, 100);

            return Ok(new
            {
                Success = true,
                TestResult = result,
                Visualization = result.Visualization,
                Analysis = result.Analysis,
                GridText = result.Visualization.Visualization,
                ExpectedSafetyFactor = 12,
                TestPassed = result.SafetyFactor == 12
            });
        }

    }
}

