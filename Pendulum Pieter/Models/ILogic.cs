using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pendulum_Pieter.Models
{
    public interface ILogic
    {
        public (double angle, double speed) BerekenHoek(double angle, double length, double time, double speed);
    }
}
