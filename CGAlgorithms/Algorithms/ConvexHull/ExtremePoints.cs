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


        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
