using AdventOfCodeSolution.Models;

namespace AdventOfCodeSolution.Services
{
    public class RobotService : IRobotService

    {
        
        public List<Robot> ParseInput(string input)
        {
            
            var robots = new List<Robot>();

            
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var line in lines)
            {
                
                var parts = line.Split(' ');
               
                if (parts.Length < 2) continue;

                try
                {
                    
                    var posPart = parts[0].Substring(2); 
                    var posCoords = posPart.Split(','); 
                    var x = int.Parse(posCoords[0]); 
                    var y = int.Parse(posCoords[1]); 

                    
                    var velPart = parts[1].Substring(2); 
                    var velCoords = velPart.Split(','); 
                    var vx = int.Parse(velCoords[0]); 
                    var vy = int.Parse(velCoords[1]); 
                   
                    robots.Add(new Robot { X = x, Y = y, VelocityX = vx, VelocityY = vy });
                }
                catch (Exception ex)
                {
                  
                    Console.WriteLine($"Error parsing line: {line}, Error: {ex.Message}");
                }
            }
         
            return robots;
        }

     
        public SimulationResult SimulateRobots(List<Robot> robots, int width, int height, int seconds)
        {
           
            var result = new SimulationResult();
          
            var finalPositions = new List<Robot>();
            
            foreach (var robot in robots)
            {
                
                var finalX = Modulo(robot.X + seconds * robot.VelocityX, width);
                var finalY = Modulo(robot.Y + seconds * robot.VelocityY, height);
               
                finalPositions.Add(new Robot
                {
                    X = finalX,
                    Y = finalY,
                    VelocityX = robot.VelocityX,
                    VelocityY = robot.VelocityY
                });
            }
           
            result.FinalPositions = finalPositions;

            
            result.Visualization = GenerateGridVisualization(finalPositions, width, height);
            result.Analysis = GenerateDetailedAnalysis(finalPositions, width, height);

            
            int midX = width / 2;
            int midY = height / 2;
            
            foreach (var robot in finalPositions)
            {
                
                if (robot.X == midX || robot.Y == midY)
                    continue;
                
                if (robot.X < midX && robot.Y < midY)
                    result.TopLeft++;
                
                else if (robot.X > midX && robot.Y < midY)
                    result.TopRight++;
                
                else if (robot.X < midX && robot.Y > midY)
                    result.BottomLeft++;
               
                else if (robot.X > midX && robot.Y > midY)
                    result.BottomRight++;
            }
            
            result.SafetyFactor = result.TopLeft * result.TopRight * result.BottomLeft * result.BottomRight;
            
            return result;
        }

       
        private int Modulo(int a, int b)
        {
            
            int result = a % b;
           
            return result < 0 ? result + b : result;
        }

        public GridVisualization GenerateGridVisualization(List<Robot> robots, int width, int height)
        {
            var visualization = new GridVisualization
            {
                Width = width,
                Height = height
            };

            
            char[,] grid = new char[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[y, x] = '.';
                }
            }

            
            var positionCounts = new Dictionary<(int, int), int>();
            foreach (var robot in robots)
            {
                var pos = (robot.X, robot.Y);
                if (positionCounts.ContainsKey(pos))
                    positionCounts[pos]++;
                else
                    positionCounts[pos] = 1;
            }

          
            foreach (var ((x, y), count) in positionCounts)
            {
                if (count == 1)
                    grid[y, x] = 'R';
                else
                    grid[y, x] = (char)('0' + count); 
            }

        
            var gridLines = new List<string>();

            gridLines.Add($"الشبكة بعد المحاكاة ({width} × {height}):");
            gridLines.Add("");

            
            for (int y = 0; y < height; y++)
            {
                var line = $"{y,2} | ";
                for (int x = 0; x < width; x++)
                {
                    line += $"{grid[y, x]}  ";
                }
                gridLines.Add(line);
            }

            
            var xAxis = "    ";
            for (int x = 0; x < width; x++)
            {
                xAxis += $"{x,-3}";
            }
            gridLines.Add(xAxis);

            visualization.GridLines = gridLines;
            visualization.Visualization = string.Join("\n", gridLines);

            return visualization;
        }

        public string GenerateDetailedAnalysis(List<Robot> robots, int width, int height)
        {
            var analysis = new System.Text.StringBuilder();

            int midX = width / 2;
            int midY = height / 2;

            analysis.AppendLine("📊 التحليل التفصيلي:");
            analysis.AppendLine($"الأبعاد: {width} × {height}");
            analysis.AppendLine($"الخط الأوسط: x={midX}, y={midY}");
            analysis.AppendLine();

           
            int topLeft = 0, topRight = 0, bottomLeft = 0, bottomRight = 0, onAxis = 0;

            foreach (var robot in robots)
            {
                if (robot.X == midX || robot.Y == midY)
                    onAxis++;
                else if (robot.X < midX && robot.Y < midY)
                    topLeft++;      
                else if (robot.X > midX && robot.Y < midY)
                    topRight++;     
                else if (robot.X < midX && robot.Y > midY)
                    bottomLeft++;   
                else if (robot.X > midX && robot.Y > midY)
                    bottomRight++;  
            }

            analysis.AppendLine("🧮 توزيع الروبوتات:");
            analysis.AppendLine($"الربع العلوي الأيسر: {topLeft} روبوت");
            analysis.AppendLine($"الربع العلوي الأيمن: {topRight} روبوت");
            analysis.AppendLine($"الربع السفلي الأيسر: {bottomLeft} روبوت");
            analysis.AppendLine($"الربع السفلي الأيمن: {bottomRight} روبوت");
            analysis.AppendLine($"على المحور: {onAxis} روبوت");
            analysis.AppendLine($"المجموع: {robots.Count} روبوت");
            analysis.AppendLine();

            int safetyFactor = topLeft * topRight * bottomLeft * bottomRight;
            analysis.AppendLine($"🎯 عامل الأمان: {topLeft} × {topRight} × {bottomLeft} × {bottomRight} = {safetyFactor}");

            return analysis.ToString();
        }

       
    }
}
