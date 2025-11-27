using AdventOfCodeSolution.Models;

namespace AdventOfCodeSolution.Services
{
    public class RobotService : IRobotService

    {
        // دالة لتحليل النص المدخل
        public List<Robot> ParseInput(string input)
        {
            // إنشاء قائمة فارغة لتخزين الروبوتات
            var robots = new List<Robot>();

            // تقسيم النص إلى أسطر وإزالة الأسطر الفارغة
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            // تكرار على كل سطر في النص المدخل
            foreach (var line in lines)
            {
                // تقسيم السطر إلى أجزاء باستخدام المسافة كفاصل
                var parts = line.Split(' ');
                // إذا كان السطر لا يحتوي على جزءين على الأقل، تخطيه
                if (parts.Length < 2) continue;

                try
                {
                    // معالجة جزء الموضع: p=0,4
                    var posPart = parts[0].Substring(2); // إزالة "p="
                    var posCoords = posPart.Split(','); // تقسيم إلى x,y
                    var x = int.Parse(posCoords[0]); // تحويل x إلى رقم
                    var y = int.Parse(posCoords[1]); // تحويل y إلى رقم

                    // معالجة جزء السرعة: v=3,-3
                    var velPart = parts[1].Substring(2); // إزالة "v="
                    var velCoords = velPart.Split(','); // تقسيم إلى vx,vy
                    var vx = int.Parse(velCoords[0]); // تحويل vx إلى رقم
                    var vy = int.Parse(velCoords[1]); // تحويل vy إلى رقم
                    // إضافة روبوت جديد إلى القائمة
                    robots.Add(new Robot { X = x, Y = y, VelocityX = vx, VelocityY = vy });
                }
                catch (Exception ex)
                {
                    // في حالة خطأ في التحويل، طباعة رسالة والمتابعة
                    Console.WriteLine($"Error parsing line: {line}, Error: {ex.Message}");
                }
            }
            // إرجاع القائمة النهائية للروبوتات
            return robots;
        }

        // دالة لمحاكاة حركة الروبوتات
        public SimulationResult SimulateRobots(List<Robot> robots, int width, int height, int seconds)
        {
            // إنشاء كائن لتخزين النتائج
            var result = new SimulationResult();
            // قائمة لتخزين المواضع النهائية
            var finalPositions = new List<Robot>();
            // تكرار على كل روبوت في القائمة
            foreach (var robot in robots)
            {
                // حساب الموضع النهائي باستخدام الدالة الخاصة بنا Modulo  
                var finalX = Modulo(robot.X + seconds * robot.VelocityX, width);
                var finalY = Modulo(robot.Y + seconds * robot.VelocityY, height);
                // إضافة الموضع النهائي إلى القائمة
                finalPositions.Add(new Robot
                {
                    X = finalX,
                    Y = finalY,
                    VelocityX = robot.VelocityX,
                    VelocityY = robot.VelocityY
                });
            }
            // تخزين المواضع النهائية في النتيجة
            result.FinalPositions = finalPositions;

            // إضافة التصور والتحليل
            result.Visualization = GenerateGridVisualization(finalPositions, width, height);
            result.Analysis = GenerateDetailedAnalysis(finalPositions, width, height);

            // حساب الخط الأوسط الأفقي والرأسي
            int midX = width / 2;
            int midY = height / 2;
            // عد الروبوتات في كل ربع
            foreach (var robot in finalPositions)
            {
                // إذا كان الروبوت على الخط الأوسط، تخطيه
                if (robot.X == midX || robot.Y == midY)
                    continue;
                // الربع العلوي الأيسر: x أقل من المنتصف و y أقل من المنتصف
                if (robot.X < midX && robot.Y < midY)
                    result.TopLeft++;
                // الربع العلوي الأيمن: x أكبر من المنتصف و y أقل من المنتصف
                else if (robot.X > midX && robot.Y < midY)
                    result.TopRight++;
                // الربع السفلي الأيسر: x أقل من المنتصف و y أكبر من المنتصف
                else if (robot.X < midX && robot.Y > midY)
                    result.BottomLeft++;
                // الربع السفلي الأيمن: x أكبر من المنتصف و y أكبر من المنتصف
                else if (robot.X > midX && robot.Y > midY)
                    result.BottomRight++;
            }
            // حساب عامل الأمان = حاصل ضرب الأعداد في الأرباع الأربعة
            result.SafetyFactor = result.TopLeft * result.TopRight * result.BottomLeft * result.BottomRight;
            // إرجاع النتيجة النهائية
            return result;
        }
        
        // دالة modulo مخصصة للتعامل مع الأعداد السالبة
        private int Modulo(int a, int b)
        {
            // حساب الباقي باستخدام مشغل %
            int result = a % b;
            // إذا كان الباقي سالباً، إضافة b ليجعل الناتج بين 0 و b-1
            return result < 0 ? result + b : result;
        }



        public GridVisualization GenerateGridVisualization(List<Robot> robots, int width, int height)
        {
            var visualization = new GridVisualization
            {
                Width = width,
                Height = height
            };

            // إنشاء شبكة فارغة
            char[,] grid = new char[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[y, x] = '.';
                }
            }

            // عدّ الروبوتات في كل موضع
            var positionCounts = new Dictionary<(int, int), int>();
            foreach (var robot in robots)
            {
                var pos = (robot.X, robot.Y);
                if (positionCounts.ContainsKey(pos))
                    positionCounts[pos]++;
                else
                    positionCounts[pos] = 1;
            }

            // وضع الروبوتات على الشبكة
            foreach (var ((x, y), count) in positionCounts)
            {
                if (count == 1)
                    grid[y, x] = 'R';
                else
                    grid[y, x] = (char)('0' + count); // '2', '3', إلخ
            }

            // بناء النص المرئي
            var gridLines = new List<string>();

            // العنوان
            gridLines.Add($"الشبكة بعد المحاكاة ({width} × {height}):");
            gridLines.Add("");

            // الشبكة مع الإحداثيات
            for (int y = height - 1; y >= 0; y--)
            {
                var line = $"{y,2} | ";
                for (int x = 0; x < width; x++)
                {
                    line += $"{grid[y, x]}  ";
                }
                gridLines.Add(line);
            }

            // الخط السفلي للإحداثيات الأفقية
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

            // عدّ الروبوتات في كل منطقة
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
