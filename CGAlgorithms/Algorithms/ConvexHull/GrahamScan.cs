using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{

    public class GrahamScan : Algorithm
    {
        public int ori(Point a, Point b, Point c)
        {
            long v = (long)a.X * ((long)b.Y - (long)c.Y) + (long)b.X * ((long)c.Y - (long)a.Y) + (long)c.X * ((long)a.Y - (long)b.Y);
            if (v < 0) return -1; // cw
            if (v > 0) return +1; // ccw
            return 0;
        }

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> st, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Point p0 = new Point(double.MaxValue, double.MaxValue);
            for (int i = 0; i < points.Count; i++)
            {
                if (p0.Y > points[i].Y || (p0.Y == points[i].Y && p0.X > points[i].X))
                {
                    p0 = points[i];
                }
            }

            points.Sort((a, b) =>
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


            for (int i = 0; i < points.Count; i++)
            {
                while (st.Count > 1 && ori(st[st.Count - 2], st[st.Count - 1], points[i]) >= 0)
                {
                    st.RemoveAt(st.Count - 1);
                }
                st.Add(points[i]);
            }

            if (st.Count == 2 && st[0].Equals(st[1])) st.RemoveAt(st.Count - 1);

            for (int i = 0; i < st.Count; i++)
            {
                outLines.Add(new Line(st[i], st[(i + 1) % st.Count]));
            }
            outPolygons.Add(new Polygon(outLines));
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}