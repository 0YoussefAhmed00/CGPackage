using CGUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public int ori(Point a, Point b, Point c)
        {
            double v = a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y);
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
        public Tuple<int, int> getLowerTanget(List<Point> L, List<Point> R)
        {
            int a = 0, b = 0;
            for (int i = 0; i < L.Count; i++)
            {
                if (L[a].X < L[i].X || (L[a].X == L[i].X && L[a].Y > L[i].Y)) a = i;
            }
            for (int i = 0; i < R.Count; i++)
            {
                if (R[b].X > R[i].X || (R[b].X == R[i].X && R[b].Y > R[i].Y)) b = i;
            }

            bool f = true;

            while (f)
            {
                f = false;
                while (ccw(R[b], L[a], L[(a - 1 + L.Count) % L.Count]))
                {
                    a = (a - 1 + L.Count) % L.Count;
                    f = true;
                }
                while (cw(L[a], R[b], R[(b + 1 + R.Count) % R.Count]))
                {
                    b = (b + 1 + R.Count) % R.Count;
                    f = true;
                }
            }

            return new Tuple<int, int>(a, b);

        }
        public Tuple<int, int> getUpperTanget(List<Point> L, List<Point> R)
        {
            int a = 0, b = 0;
            for (int i = 0; i < L.Count; i++)
            {
                if (L[a].X < L[i].X || (L[a].X == L[i].X && L[a].Y < L[i].Y)) a = i;
            }
            for (int i = 0; i < R.Count; i++)
            {
                if (R[b].X > R[i].X || (R[b].X == R[i].X && R[b].Y < R[i].Y)) b = i;
            }

            bool f = true;

            while (f)
            {
                f = false;
                while (cw(R[b], L[a], L[(a + 1 + L.Count) % L.Count]))
                {
                    a = (a + 1 + L.Count) % L.Count;
                    f = true;
                }
                while (ccw(L[a], R[b], R[(b- 1 + R.Count) % R.Count]))
                {
                    b = (b - 1 + R.Count) % R.Count;
                    f = true;
                }
            }

            return new Tuple<int, int>(a, b);
        }

        public List<Point> hull(List<Point> points)
        {
            if (points.Count < 2)
            {
                return points;
            }

            List<Point> ret = new List<Point>(), L = new List<Point>(), R = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count / 2) L.Add(points[i]);
                else R.Add(points[i]);
            }

            var lHull = hull(L);
            var rHull = hull(R);
            var U = getUpperTanget(lHull, rHull);
            var D = getLowerTanget(lHull, rHull);

           
            for (int i = D.Item2; ; i = (i + 1) % rHull.Count)
            {
                ret.Add(rHull[i]);
                if (i == U.Item2) break;
            }

            for (int i = U.Item1; ; i = (i + 1) % lHull.Count)
            {
                ret.Add(lHull[i]);
                if (i == D.Item1) break;
            }
           
            return new List<Point>(ret);
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

            outPoints = hull(points);

        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}
