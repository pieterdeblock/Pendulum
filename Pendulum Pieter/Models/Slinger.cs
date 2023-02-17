using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Pendulum_Pieter.Models
{
    public class Slinger
    {
        public Point3D AnchorPoint { get; set; }

        public Point3D EndPoint { get; set; }

        public double Length { get; set; }

        public double Angle { get; set; }

        public double SpeedChange { get; set; }
    }
}
