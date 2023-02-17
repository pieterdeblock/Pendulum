using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pendulum_Pieter.Models
{
    public class Logic : ILogic
    {
        public (double angle, double speed) BerekenHoek(double angle, double length, double time, double speed)
        {
            return (((angle / 180 * Math.PI) + (speed + -9.81 / -length * Math.Sin(angle / 180 * Math.PI) * time) * time) * 180 / Math.PI, speed + -9.81 / -length * Math.Sin(angle / 180 * Math.PI) * time);

        }
    }
}
