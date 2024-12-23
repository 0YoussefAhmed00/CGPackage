using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class SubtractingEars : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // MUST DRAW CCW 
            List<Point> vertices = new List<Point>();
            foreach(Line x in lines)
            {
                Console.WriteLine(x.Start);
                vertices.Add(x.Start);
            }


            Dictionary<int, bool> mp = new Dictionary<int, bool>();

            for (int i = 0; i < vertices.Count; i++)
            {
                if (IsEar(vertices, i))
                    mp[i] = true;
            }

            while (vertices.Count > 3)
            {
                int earIndex = mp.Keys.First();
                int prevIndex = (earIndex - 1 + vertices.Count) % vertices.Count;
                int nextIndex = (earIndex + 1) % vertices.Count;

                var prev = vertices[prevIndex];
                var current = vertices[earIndex];
                var next = vertices[nextIndex];

                outLines.AddRange(new List<Line> { new Line(prev, current), new Line(current, next), new Line(next, prev) });

                vertices.RemoveAt(earIndex);

                mp.Remove(earIndex);

                int newPrevIndex = (earIndex - 1 + vertices.Count) % vertices.Count;
                if (IsEar(vertices, newPrevIndex))
                    mp[newPrevIndex] = true;

                int newNextIndex = earIndex % vertices.Count;
                if (IsEar(vertices, newNextIndex))
                    mp[newNextIndex] = true;
            }

            var finalEdges = new List<Line> { new Line(vertices[0], vertices[1]), new Line(vertices[1], vertices[2]), new Line(vertices[2], vertices[0]) };
            outLines.AddRange(finalEdges);
        }

        private bool IsEar(List<Point> vertices, int i)
        {
            int prevIndex = (i - 1 + vertices.Count) % vertices.Count;
            int nextIndex = (i + 1) % vertices.Count;

            var prev = vertices[prevIndex];
            var current = vertices[i];
            var next = vertices[nextIndex];

            if (!IsConvex(prev, current, next))
                return false;

            for (int j = 0; j < vertices.Count; j++)
            {
                if (j == prevIndex || j == i || j == nextIndex)
                    continue;

                if (HelperMethods.PointInTriangle(vertices[j], prev, current, next) != Enums.PointInPolygon.Outside)
                    return false;
            }

            return true;
        }

        private bool IsConvex(Point a, Point b, Point c)
        {
            double crossProduct = (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
            return crossProduct > 0;
        }

        public override string ToString()
        {
            return "Subtracting Ears";
        }
    }
}
