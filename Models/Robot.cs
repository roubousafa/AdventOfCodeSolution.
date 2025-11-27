namespace AdventOfCodeSolution.Models
{
    public class Robot
    {
        // الخاصية: الموضع الأفقي (المسافة من الحائط الأيسر)
        public int X { get; set; }
        // الخاصية: الموضع الرأسي (المسافة من الحائط الأعلى)
        public int Y { get; set; }
        // الخاصية: السرعة الأفقية (مربعات/ثانية)
        public int VelocityX { get; set; }
        // الخاصية: السرعة الرأسية (مربعات/ثانية)
        public int VelocityY { get; set; }
    }
}
