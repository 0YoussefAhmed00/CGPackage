using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public int ori(Point a, Point b, Point c)
        {
            long v = (long)a.X * ((long)b.Y - (long)c.Y) + (long)b.X * ((long)c.Y - (long)a.Y) + (long)c.X * ((long)a.Y - (long)b.Y);
            if (v < 0) return -1; // cw
            if (v > 0) return +1; // ccw
            return 0;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // shel el mtkrr
            List<Point> points_Msh_Mtkrra = new List<Point>();
            foreach (var pt in points)
            {
                if (!points_Msh_Mtkrra.Contains(pt))
                {
                    points_Msh_Mtkrra.Add(pt);
                }
            }
            // will have the filered from elymsh mtkrr
            List<Point> filtered_points = new List<Point>(points_Msh_Mtkrra);

            for (int i = 0; i < points_Msh_Mtkrra.Count; i++)
            {
                Point p1 = points_Msh_Mtkrra[i];
                bool isContained = false;

                for (int j = 0; j < points_Msh_Mtkrra.Count && !isContained; j++)
                {
                    if (j == i) continue;
                    for (int k = j + 1; k < points_Msh_Mtkrra.Count && !isContained; k++)
                    {
                        if (k == i || k == j) continue;
                        for (int l = k + 1; l < points_Msh_Mtkrra.Count && !isContained; l++)
                        {
                            if (l == i || l == j || l == k) continue;

                            Point p2 = points_Msh_Mtkrra[j];
                            Point p3 = points_Msh_Mtkrra[k];
                            Point p4 = points_Msh_Mtkrra[l];

                            // Check if p1 is inside or on an edge
                            if (HelperMethods.PointInTriangle(p1, p2, p3, p4) == Enums.PointInPolygon.Inside ||
                                HelperMethods.PointOnSegment(p1, p2, p3) || HelperMethods.PointOnSegment(p1, p3, p4) || HelperMethods.PointOnSegment(p1, p2, p4))
                            {
                                isContained = true;
                                break;
                            }
                        }
                    }
                }

                if (isContained)
                {
                    filtered_points.Remove(p1);
                }
            }


            outPoints = new List<Point>(filtered_points);

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
            return "Convex Hull - Extreme Points";
        }
    }
}