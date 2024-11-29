using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public int ori(Point a, Point b, Point c)
        {
            long v = (long)a.X * ((long)b.Y - (long)c.Y) + (long)b.X * ((long)c.Y - (long)a.Y) + (long)c.X * ((long)a.Y - (long)b.Y);
            if (v < 0) return -1; // cw
            if (v > 0) return +1; // ccw
            return 0;
        }

        // dist point x to line AB
        public double get_d(Point x, Point A, Point B)
        {
            double numerator = Math.Abs((B.Y - A.Y) * x.X - (B.X - A.X) * x.Y + B.X * A.Y - B.Y * A.X);
            double denominator = Math.Sqrt((B.Y - A.Y) * (B.Y - A.Y) + (B.X - A.X) * (B.X - A.X));
            return numerator / denominator;
        }
        public void rec(List<Point> st, Point A, Point B, ref List<Point> outPoints)
        {
            if (st.Count == 0) return;
            Point C = st[0];
            for (int i = 0; i < st.Count; i++)
            {
                if (get_d(st[i], A, B) > get_d(C, A, B))
                    C = st[i];
            }
            List<Point> st1 = new List<Point>();
            List<Point> st2 = new List<Point>();
            outPoints.Add(C);
            for (int i = 0; i < st.Count; i++)
            {
                if (ori(A, C, st[i]) > 0) st1.Add(st[i]);
                else if (ori(C, B, st[i]) > 0) st2.Add(st[i]);
            }
            rec(st1, A, C, ref outPoints);
            rec(st2, C, B, ref outPoints);
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            points.Sort((a, b) =>
            {
                if (a.X > b.X || (a.X == b.X && a.Y > b.Y)) return -1;
                return 1;
            });

            List<Point> temp = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                if (temp.Count == 0 || !temp[temp.Count - 1].Equals(points[i]))
                    temp.Add(points[i]);
            }

            points = temp;

            if (points.Count <= 2)
            {
                outPoints = points;
                return;
            }

            var L = points[0];
            var R = points[points.Count - 1];
            List<Point> st1 = new List<Point>();
            List<Point> st2 = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                if (ori(L, R, points[i]) > 0)
                {
                    st1.Add(points[i]);
                }
                else if (ori(L, R, points[i]) < 0)
                {
                    st2.Add(points[i]);
                }
            }
            outPoints.Add(L);
            outPoints.Add(R);
            rec(st1, L, R, ref outPoints);
            rec(st2, R, L, ref outPoints);

            Point p0 = new Point(double.MaxValue, double.MaxValue);
            for (int i = 0; i < outPoints.Count; i++)
            {
                if (p0.Y > outPoints[i].Y || (p0.Y == outPoints[i].Y && p0.X > outPoints[i].X))
                {
                    p0 = outPoints[i];
                }
            }

            outPoints.Sort((a, b) =>
            {
                int o = ori(p0, a, b);
                if (o == 0)
                {
                    double distA = (p0.X - a.X) * (p0.X - a.X) + (p0.Y - a.Y) * (p0.Y - a.Y);
                    double distB = (p0.X - b.X) * (p0.X - b.X) + (p0.Y - b.Y) * (p0.Y - b.Y);
                    return distA.CompareTo(distB);
                }
                return o < 0 ? -1 : 1;
            });

            for (int i = 0; i < outPoints.Count; i++)
            {
                outLines.Add(new Line(outPoints[i], outPoints[(i + 1) % outPoints.Count]));
            }

            outPolygons.Add(new Polygon(outLines));
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}