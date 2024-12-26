using CGUtilities.DataStructures;
using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGAlgorithms.Algorithms.SegmentIntersection.SweepLine;
using System.Data;
using System.Runtime.InteropServices;

namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    class SweepLine : Algorithm
    {
        OrderedSet<(Point, int)> Q;
        OrderedSet<Line> T;
        double curX;
        public Point inter(Line s1, Line s2)
        {
            if (s1 == null || s2 == null) return null;

            double x1 = s1.Start.X, x2 = s1.End.X, x3 = s2.Start.X, x4 = s2.End.X;
            double y1 = s1.Start.Y, y2 = s1.End.Y, y3 = s2.Start.Y, y4 = s2.End.Y;

            double d = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (Math.Abs(d) < 1e-8) return null;

            double pre = (x1 * y2 - y1 * x2), post = (x3 * y4 - y3 * x4);
            double x = (pre * (x3 - x4) - (x1 - x2) * post) / d;
            double y = (pre * (y3 - y4) - (y1 - y2) * post) / d;

            if (IsBetween(x, y, s1.Start, s1.End) && IsBetween(x, y, s2.Start, s2.End))
            {
                return new Point(x, y);
            }

            return null;
        }
        private bool IsBetween(double px, double py, Point start, Point end)
        {
            return px >= Math.Min(start.X, end.X) && px <= Math.Max(start.X, end.X) &&
                   py >= Math.Min(start.Y, end.Y) && py <= Math.Max(start.Y, end.Y);
        }
        public List<Tuple<int, int>> brute(List<Line> lines)
        {
            List<Tuple<int, int>> ret = new List<Tuple<int, int>>();
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = i + 1; j < lines.Count; j++)
                {
                    Point p = inter(lines[i], lines[j]);
                    if (p != null)
                    {
                        ret.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            return ret;
        }

        public void findNewEvent(Line s1, Line s2, Point p, ref List<Point> outPoints)
        {
            var x = inter(s1, s2);
            if (x == null) return;
            outPoints.Add(x);
            if (x.X > p.X || (x.X == p.X && x.Y < p.Y - Constants.Epsilon))
            {
                if (!Q.Contains((x, 0)))
                    Q.Add((x, 0));
            }
        }


        public double get_y(Line s, double x)
        {
            Point p = s.Start, q = s.End;
            if (Math.Abs(p.X - q.X) < Constants.Epsilon)
                return p.Y;
            return p.Y + (q.Y - p.Y) * (x - p.X) / (q.X - p.X);

        }
        public int comparerEventY(Line s1, Line s2)
        {
            if (s1.Equals(s2)) return 0;
            if (get_y(s1, curX) > get_y(s2, curX))
                return -1;
            return 1;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Q = new OrderedSet<(Point, int)>();

            for (int i = 0; i < lines.Count; i++)
            {
                var s = lines[i];

                if (s.Start.X > s.End.X || (s.Start.X == s.End.X && s.Start.Y < s.End.Y))
                {
                    Point temp = s.Start;
                    s.Start = s.End;
                    s.End = temp;
                }

                lines[i] = s;

                Q.Add((s.Start, +(i + 1)));
                Q.Add((s.End, -(i + 1)));
            }

            T = new OrderedSet<Line>(comparerEventY);

            while (Q.Count > 0)
            {
                var (p, type) = Q.First();
                Console.WriteLine(p + " " + type);
                if (type > 0)
                {
                    curX = p.X;
                    var get = T.DirectUpperAndLower(lines[type - 1]);

                    if (get.Value != default)
                        findNewEvent(get.Value, lines[type - 1], p, ref outPoints);
                    if (get.Key != default)
                        findNewEvent(get.Key, lines[type - 1], p, ref outPoints);

                    if (T.Contains(lines[type - 1]))
                        throw new Exception("no wayyyyyyyy");

                    T.Add(lines[type - 1]);
                }
                else if (type < 0)
                {
                    curX = p.X;
                    type = -type;

                    if (!T.Contains(lines[type - 1]))
                        throw new Exception("no wayyyyyyyy");

                    var get = T.DirectUpperAndLower(lines[type - 1]);

                    if (get.Value != default && get.Key != default)
                        findNewEvent(get.Key, get.Value, p, ref outPoints);

                    T.Remove(lines[type - 1]);
                    type = -type;
                }
                else
                {
                    curX = p.X - 0.01;
                    int l = 0, r = T.Count - 1, idx = -1;
                    while (l <= r)
                    {
                        int mid = l + r >> 1;
                        if (Math.Abs(get_y(T[mid], p.X) - p.Y) < Constants.Epsilon || (get_y(T[mid], p.X) < p.Y))
                        {
                            idx = mid;
                            r = mid - 1;
                        }
                        else
                        {
                            l = mid + 1;
                        }
                    }
                    if (T.Count < 2 || idx == -1)
                        throw new Exception("no wayyyyyyyy");
                    var temp = T[idx];
                    T.Remove(T[idx]);
                    curX = p.X + 0.01;
                    T.Add(temp);
                    if (idx > 0)
                        findNewEvent(T[idx - 1], T[idx], p, ref outPoints);
                    if (idx + 2 < T.Count)
                        findNewEvent(T[idx + 1], T[idx + 2], p, ref outPoints);
                }

                Q.Remove((p, type));
            }
        }



        public override string ToString()
        {
            return "Sweep Line Algorithm";
        }
    }
}
