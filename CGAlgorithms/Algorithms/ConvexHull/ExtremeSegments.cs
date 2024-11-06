using CGUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> points_Msh_Mtkrra = new List<Point>();

            // handel el mtkrr zy el extrem points
            foreach (var pt in points)
            {
                if (!points_Msh_Mtkrra.Contains(pt))
                {
                    points_Msh_Mtkrra.Add(pt);
                }
            }

            if (points_Msh_Mtkrra.Count < 3)
            {
                outPoints = new List<Point>(points_Msh_Mtkrra);
                return;
            }

            List<Line> hullLines = new List<Line>();

           
            for (int i = 0; i < points_Msh_Mtkrra.Count; i++)
            {
                for (int j = i + 1; j < points_Msh_Mtkrra.Count; j++)
                {
                    bool isEx_Segment = true;
                    Point p1 = points_Msh_Mtkrra[i];
                    Point p2 = points_Msh_Mtkrra[j];
                    Enums.TurnType init_tu = Enums.TurnType.Colinear;

                    foreach (var point in points_Msh_Mtkrra)
                    {
                        if (point == p1 || point == p2)
                            continue;

                        Enums.TurnType turn = HelperMethods.CheckTurn(new Line(p1, p2), point);

                        if (init_tu == Enums.TurnType.Colinear)
                            init_tu = turn;

                        if (turn == Enums.TurnType.Colinear && !HelperMethods.PointOnSegment(point, p1, p2))
                        {
                            isEx_Segment = false;
                            break;
                        }

                        if (turn != init_tu && turn != Enums.TurnType.Colinear)
                        {
                            isEx_Segment = false;
                            break;
                        }

                        if ((turn == Enums.TurnType.Left && init_tu == Enums.TurnType.Right) ||
                            (turn == Enums.TurnType.Right && init_tu == Enums.TurnType.Left))
                        {
                            isEx_Segment = false;
                            break;
                        }
                    }

                    if (isEx_Segment)
                    {
                        hullLines.Add(new Line(p1, p2));
                    }
                }
            }

            // 7wl el lines to points 3ashan tshof el points
            HashSet<Point> hullPoints = new HashSet<Point>();
            foreach (var line in hullLines)
            {
                hullPoints.Add(line.Start);
                hullPoints.Add(line.End);
            }

            
            outPoints = new List<Point>(hullPoints);
            foreach (Point x in hullPoints)
            {
                Debug.WriteLine(x.X + " " + x.Y + " ");
            }
            outLines = hullLines;
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
