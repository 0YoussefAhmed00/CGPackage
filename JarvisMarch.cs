using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
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

            outPoints.Add(points[0]);

            while (outPoints.Count <= 1 || !outPoints[0].Equals(outPoints[outPoints.Count - 1]))
            {
                Point mi = new Point(-1, -1);
                Point cur = outPoints[outPoints.Count - 1];
                for (int i = 0; i < points.Count; i++)
                {
                    if (!points[i].Equals(cur))
                    {
                        mi = points[i];
                        break;
                    }
                }

                for (int i = 0; i < points.Count; i++)
                {
                    if (!cur.Equals(points[i]) && (ori(cur, mi, points[i]) < 0 || (ori(cur, mi, points[i]) == 0 && dist(cur, mi) < dist(cur, points[i]))))
                    {
                        mi = points[i];
                    }
                }
                outPoints.Add(mi);
            }

            outPoints.RemoveAt(outPoints.Count - 1);



            for (int i = 0; i < outPoints.Count; i++)
            {
                outLines.Add(new Line(outPoints[i], outPoints[(i + 1) % outPoints.Count]));
            }

            outPolygons.Add(new Polygon(outLines));
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
