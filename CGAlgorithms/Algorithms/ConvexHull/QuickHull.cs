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
            double v = a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y);
            if (v < 0) return -1; // cw
            if (v > 0) return +1; // ccw
            return 0;
        }
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
                if (ori(A, C , st[i]) > 0) st1.Add(st[i]);
                else if(ori(C , B , st[i]) > 0)st2.Add(st[i]);
            }
            rec(st1, A, C, ref outPoints);
            rec(st2, C, B, ref outPoints);
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            points = points.OrderBy(point => point.X).ThenBy(point => point.Y).ToList();

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
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
