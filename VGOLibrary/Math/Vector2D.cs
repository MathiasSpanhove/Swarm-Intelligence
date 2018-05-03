using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathematics
{
    public class Vector2D
    {
        public Vector2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; }

        public double Y { get; }

        public static Vector2D operator +(Vector2D u, Vector2D v)
        {
            double x = u.X + v.X;
            double y = u.Y + v.Y;

            return new Vector2D(x, y);
        }

        public static Vector2D operator -(Vector2D v)
        {
            double x = -v.X;
            double y = -v.Y;

            return new Vector2D(x, y);
        }

        public static Vector2D operator -(Vector2D u, Vector2D v)
        {
            return u + -v;
        }

        public static Vector2D operator *(Vector2D v, double constant)
        {
            double x = v.X * constant;
            double y = v.Y * constant;

            return new Vector2D(x, y);
        }

        public static Vector2D operator *(double constant, Vector2D v)
        {
            return v * constant;
        }

        public static Vector2D operator /(Vector2D v, double constant)
        {
            return v * (1.0 / constant);
        }

        public double SquaredNorm => X * X + Y * Y;

        public double Norm => Math.Sqrt(SquaredNorm);

        public Vector2D Normalized => this / Norm;

        public double DistanceTo(Vector2D v)
        {
            return (v - this).Norm;
        }

        public double Dot(Vector2D v)
        {
            return this.X * v.X + this.Y * v.Y;
        }

        public Vector2D ClampNorm(double max)
        {
            if ( Norm > max )
            {
                return Normalized * max;
            }
            else
            {
                return this;
            }
        }

        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }
    }
}
