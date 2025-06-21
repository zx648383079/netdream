namespace NetDream.Modules.Counter.Models
{
    public class TimeResult
    {

        public int Pv { get; set; }
        public int Uv { get; set; }
        public int IpCount { get; set; }
        public int JumpCount { get; set; }
        public double StayTime { get; set; }
        public int NextTime { get; set; }

        public static TimeResult operator *(TimeResult v1, double v2)
        {
            return new()
            {
                Pv = (int)(v1.Pv * v2),
                Uv = (int)(v1.Uv * v2),
                IpCount = (int)(v1.IpCount * v2),
                JumpCount = (int)(v1.JumpCount * v2),
                StayTime = v1.StayTime * v2,
                NextTime = (int)(v1.NextTime * v2),
            };
        }
    }
}
