using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Collections.Immutable;

namespace Pendulum_Pieter.Models
{
    internal class World : IWorld
    {
        
        public int WorldSize { get; set; }
        private readonly Random _rnd = new();

        public Point3D Origin => new();
        public (Point3D p1, Point3D p2) Bounds { get; private set; }

        public Beam Beam { get; private set; }
        public Slinger Slinger { get; private set; }

        public ImmutableList<Point3D> SpherePositions { get; private set; }
        public World()
        {
            WorldSize = 1000;
            Bounds = (new Point3D(-WorldSize / 2, -WorldSize / 2, -WorldSize / 2),
                      new Point3D(WorldSize / 2, WorldSize / 2, WorldSize / 2));
            SpherePositions = ImmutableList<Point3D>.Empty;
            InitBeam();
        }
        private void InitBeam()
        {
            Beam = new Beam
            { 
                AnchorPoint = GetRandomPosition(),
                Angle = 0,
                Length = WorldSize * 0.3 * (1 + _rnd.NextDouble()),
                RotationalDelta = 18 * (1 + _rnd.NextDouble())
            };
        }
        private Point3D GetRandomPosition()
        {
            return new Point3D
            {
                X = Bounds.p1.X + ((Bounds.p2.X - Bounds.p1.X) * _rnd.NextDouble()),
                Y = Bounds.p1.Y + ((Bounds.p2.Y - Bounds.p1.Y) * _rnd.NextDouble()),
                Z = Bounds.p1.Z + ((Bounds.p2.Z - Bounds.p1.Z) * _rnd.NextDouble())
            };
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void AddSphere()
        {
            var position = GetRandomPosition();
            SpherePositions = SpherePositions.Add(position);
        }

        public void MoveObjects()
        {
            Beam.Angle += Beam.RotationalDelta;

            // just move spheres each by a small random distance
            var newPositions = ImmutableList<Point3D>.Empty;
            foreach (var position in SpherePositions)
            {
                double magnitude = WorldSize / 5;
                var vector = new Vector3D(magnitude * (_rnd.NextDouble() - 0.5), magnitude * (_rnd.NextDouble() - 0.5), magnitude * (_rnd.NextDouble() - 0.5));
                newPositions = newPositions.Add(position + vector);
            }
            SpherePositions = newPositions;
        }
    }
}
