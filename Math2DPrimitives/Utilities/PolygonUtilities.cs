namespace MathEuclideanPrimitives.Utilities
{
    using System.Collections.Generic;

    public static partial class GeometricUtilities
    {
        #region Polygon Applications

        /// <summary>
        /// A simple point in polygon algorithm implementation.
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Point_in_polygon"/>
        /// <param name="polygon">Primtivie polygon.</param>
        /// <param name="point">Primtive point.</param>
        /// <returns>bool flag indicating whether a point is in or out a polygon
        /// NOTE:points on the edge of a polygon are considered inside the polygon.</returns>
        public static bool IsPointInPolygon(Polygon polygon, Point2D point)
        {
            Point2D p;
            bool flag = false;
            PointThreshold threshold = PointThreshold.CreateBySlope(0, point.Y);
            List<Point2D> intersectionPointList = new List<Point2D>();

            ModifyPolygonSides(polygon.PolygonSides, threshold);

            foreach (PolygonSide side in polygon.PolygonSides)
            {
                if (side.IsCrossSide)
                {
                    p = side.Intersect(threshold);
                    if (p.X > point.X)
                    {
                        intersectionPointList.Add(side.Intersect(threshold));
                    }
                }
            }

            if (intersectionPointList.Count % 2 != 0 || IsPointOnEdge(polygon, point))
            { flag = true; }
            ResetPolygonSides(polygon.PolygonSides);

            return flag;

        }

        /// <summary>
        /// Inquires if all the points are in a polygon.
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool ArePointsInPolygon(Polygon polygon, params Point2D[] points)
        {
            bool flag = true;
            foreach (var point in points)
            {
                if (!IsPointInPolygon(polygon, point))
                    flag = false;
            }

            return flag;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool IsAnyPointInPolygon(Polygon polygon, params Point2D[] points)
        {
            bool flag = false;
            foreach (var point in points)
            {
                if (IsPointInPolygon(polygon, point))
                    flag = true;
            }

            return flag;
        }

        /// <summary>
        /// Inquires if the given point lies on the edge of a given polygon.
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="point"></param>
        /// <returns>A flag if true the point is on the edge.</returns>
        public static bool IsPointOnEdge(Polygon polygon, Point2D point)
        {
            foreach (PolygonSide side in polygon.PolygonSides)
            {
                if (point.LiesOnBoundedLine(side))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Inquires if all the points are on the edge of a polygon.
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="points">Any number of points to be predicated.</param>
        /// <returns>A flag if true all the points must be on the edge.</returns>
        public static bool AreAllPointsOnEdge(Polygon polygon, params Point2D[] points)
        {
            bool flag = true;
            foreach (var point in points)
            {
                if (!IsPointOnEdge(polygon, point))
                    flag = false;
            }
            return flag;
        }

        /// <summary>
        /// Inquires if any of the points are on the edge of a polygon.
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="points"></param>
        /// <returns>A flag if true one or more points must be on the edge.</returns>
        public static bool IsAnyPointOnEdge(Polygon polygon, params Point2D[] points)
        {
            bool flag = true;
            foreach (var point in points)
            {
                if (IsPointOnEdge(polygon, point))
                    flag = true;
            }
            return flag;
        }

        /// <summary>
        /// Inquires if the polygon has any horizontal sides.
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool IsPolygonHavingHorizontalSides(Polygon polygon)
        {
            bool flag = false;
            foreach (PolygonSide side in polygon.PolygonSides)
            {
                if (side.IsHorizontal())
                {
                    flag = true;
                }
            }

            return flag;
        }

        /// <summary>
        /// Inquires if the polygon has any vertical sides.
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static bool IsPolygonHavingVerticalSides(Polygon polygon)
        {
            bool flag = false;
            foreach (PolygonSide side in polygon.PolygonSides)
            {
                if (side.IsVertical())
                {
                    flag = true;
                }
            }

            return flag;
        }
        #endregion

        #region HelperFunctions
        /// <summary>
        /// Resets all the cross sides of a polygon, to ensure lose connections between various classes.
        /// </summary>
        /// <param name="sides">List of polygon sides.</param>
        private static void ResetPolygonSides(List<PolygonSide> sides)
        {
            foreach (PolygonSide side in sides)
            {

                side.IsCrossSide = false;

            }
        }

        /// <summary>
        /// Decides whether a polygon side is a cross side or not,
        /// this concept exits in various polygon algorithms like
        /// an efficient fill-in algorithm or a point in polygon algorithm
        /// this concept is used to handle degenerate cases,
        /// when a threshold of a point intersects the start/end of a polygon side.
        /// </summary>
        /// <param name="sides">List of a polygo's sides.</param>
        /// <param name="threshold">Point threshold to decide which side is a cross side.</param>
        private static void ModifyPolygonSides(List<PolygonSide> sides, PointThreshold threshold)
        {
            foreach (PolygonSide side in sides)
            {
                if ((side.StartPoint.Y > threshold.YIntercept && side.EndPoint.Y < threshold.YIntercept)
                    || (side.EndPoint.Y > threshold.YIntercept && side.StartPoint.Y < threshold.YIntercept)
                    || (side.StartPoint.Y == threshold.YIntercept && side.EndPoint.Y < threshold.YIntercept)
                    || (side.StartPoint.Y < threshold.YIntercept && side.EndPoint.Y == threshold.YIntercept))
                {
                    side.IsCrossSide = true;
                }
                else
                {
                    side.IsCrossSide = false;
                }
            }
        }
        #endregion
    }
}
