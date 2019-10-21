using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class EasingFunctions
    {
        public static Func<double, double> AnimateScalar(double initialValue, double endValue, Func<double, double> easingFunction)
        {
            return (t) => initialValue + (easingFunction(t) * (endValue - initialValue));
        }

        public static Func<double, Color> AnimateColor(Color initialValue, Color endValue, Func<double, double> easingFunction)
        {
            return (t) => new Color(
                (byte) (initialValue.R + (easingFunction(t) * (endValue.R - initialValue.R))),
                (byte) (initialValue.G + (easingFunction(t) * (endValue.G - initialValue.G))),
                (byte) (initialValue.B + (easingFunction(t) * (endValue.B - initialValue.B))),
                (byte) (initialValue.A + (easingFunction(t) * (endValue.A - initialValue.A)))
            );
        }

        public static readonly Func<double, double> Linear = (t) => t;
        // accelerating from zero velocity
        public static readonly Func<double, double> EaseInQuad = (t) => t * t;
        // decelerating to zero velocity
        public static readonly Func<double, double> EaseOutQuad = (t) => t * (2 - t);
        // acceleration until halfway, then deceleration
        public static readonly Func<double, double> EaseInOutQuad = (t) => t < .5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
        // accelerating from zero velocity 
        public static readonly Func<double, double> EaseInCubic = (t) => t * t * t;
        // decelerating to zero velocity 
        public static readonly Func<double, double> EaseOutCubic = (t) => (--t) * t * t + 1;
        // acceleration until halfway, then deceleration 
        public static readonly Func<double, double> EaseInOutCubic = (t) => t < .5 ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
        // accelerating from zero velocity 
        public static readonly Func<double, double> EaseInQuart = (t) => t * t * t * t;
        // decelerating to zero velocity 
        public static readonly Func<double, double> EaseOutQuart = (t) => 1 - (--t) * t * t * t;
        // acceleration until halfway, then deceleration
        public static readonly Func<double, double> EaseInOutQuart = (t) => t < .5 ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;
        // accelerating from zero velocity
        public static readonly Func<double, double> EaseInQuint = (t) => t * t * t * t * t;
        // decelerating to zero velocity
        public static readonly Func<double, double> EaseOutQuint = (t) => 1 + (--t) * t * t * t * t;
        // acceleration until halfway, then deceleration 
        public static readonly Func<double, double> EaseInOutQuint = (t) => t < .5 ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;
    }
}
