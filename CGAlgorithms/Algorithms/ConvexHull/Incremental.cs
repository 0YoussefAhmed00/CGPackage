using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public int ori(Point a, Point b, Point c)
        {
            long v = (long)a.X * ((long)b.Y - (long)c.Y) + (long)b.X * ((long)c.Y - (long)a.Y) + (long)c.X * ((long)a.Y - (long)b.Y);
            if (v < 0) return -1; // cw
            if (v > 0) return +1; // ccw
            return 0;
        }
        public double dist(Point a, Point b)
        {
            return (b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y);
        }
        public bool cw(Point a, Point b, Point c)
        {
            if (ori(a, b, c) < 0) return true;
            else if (ori(a, b, c) == 0) return dist(a, c) > dist(a, b);
            return false;
        }
        public bool ccw(Point a, Point b, Point c)
        {
            if (ori(a, b, c) > 0) return true;
            else if (ori(a, b, c) == 0) return dist(a, c) > dist(a, b);
            return false;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1)
            {
                outPoints = points;
                return;
            }

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

            var pL = points[0];
            var pR = points[points.Count - 1];
            var stk1 = new List<Point>();
            var stk2 = new List<Point>();
            stk1.Add(pL);
            stk2.Add(pL);

            for (int i = 1; i < points.Count; i++)
            {
                if (i == points.Count - 1 || cw(pL, points[i], pR))
                {
                    while (stk1.Count > 1 && cw(points[i], stk1[stk1.Count - 1], stk1[stk1.Count - 2]))
                        stk1.RemoveAt(stk1.Count - 1);
                    stk1.Add(points[i]);
                }
                if (i == points.Count - 1 || ccw(pL, points[i], pR))
                {
                    while (stk2.Count > 1 && ccw(points[i], stk2[stk2.Count - 1], stk2[stk2.Count - 2]))
                        stk2.RemoveAt(stk2.Count - 1);
                    stk2.Add(points[i]);
                }
            }

            for (int i = 0; i < stk1.Count; i++)
            {
                outPoints.Add(stk1[i]);
            }

            for (int i = stk2.Count - 2; i > 0; i--)
            {
                outPoints.Add(stk2[i]);
            }

            for (int i = 0; i < outPoints.Count; i++)
            {
                outLines.Add(new Line(outPoints[i], outPoints[(i + 1) % outPoints.Count]));
            }

            outPolygons.Add(new Polygon(outLines));



        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}