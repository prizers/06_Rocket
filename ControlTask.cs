using System;

namespace func_rocket
{
    public class ControlTask
    {
        public static Turn ControlRocket(Rocket rocket, Vector target)
        { // FIXME!
            Vector dt = rocket.Location - target;
            Vector f = dt - rocket.Velocity;
            double desired = f.Angle;
            double rotor = (desired - rocket.Direction) % (Math.PI * 2) - Math.PI;
            return (rotor < 0) ? Turn.Left
                : (0 < rotor) ? Turn.Right
                : Turn.None;
        }
    }
}
