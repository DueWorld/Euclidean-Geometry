namespace MathEuclideanPrimitives.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Containing various static and extended functions
    /// that serve as a boolean and other operations on 
    /// primitive elements such as a point or a line or a polygon.
    /// NOTE: All of the operations related to the line requires
    /// definite start and end points defined for this line.
    /// </summary>
    [Obsolete("Use Utilities class instead.")]
    public static class PrimitiveUtils
    {
        /// <summary>
        /// A fudge leftover of equating a double point space.
        /// </summary>
        private const double fudge = 0.001d;

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
        public static bool ArePointsInPolygon(Polygon polygon, params Point2D [] points)
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
        /// Inquires if any of the points is in a polygon.
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsAnyPointInPolygon(Polygon polygon, params Point2D [] points)
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
        /// Given a planar element with thickness,
        /// this function will determine its boundary extreme lines from the centerline.
        /// </summary>
        /// <param name="startPoint">Starting point of the center line.</param>
        /// <param name="endPoint">Ending point of the center line.</param>
        /// <param name="width">Width of the planar element.</param>
        /// <returns>A value tuple include the 2 extreme sides of the planar element.</returns>
        public static (Line2D firstLine, Line2D secondLine) GetLineOffsetFromCenterLine
            (Point2D startPoint, Point2D endPoint, double width)
        {
            // Get the center-line vector by subtracting start point from the end point.
            Vector2D lineDirectionVector = new Vector2D(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
           
            // Get the perpendicular vector by equating the dot produce by zero. it will be [-y,x]
            Vector2D perpendicularVector = new Vector2D(lineDirectionVector.Y * -1, lineDirectionVector.X);
           
            // Normalize the perpendicular vector.
            var unitVector = perpendicularVector.Normalize();

            // Add the unit vector multiplied by half the width to each terminal vector of the center line.
            // This gets the upper extreme side.
            var upperStartVec = unitVector * (width / 2) + new Vector2D(startPoint.X, startPoint.Y);
            var upperEndVec = unitVector * (width / 2) + new Vector2D(endPoint.X, endPoint.Y);

            // Subtract the unit vector multiplied by half the width to each terminal vector of the center line.
            // This gets the lower extreme side.

            var lowerStartVec = unitVector * -1 * (width / 2) + new Vector2D(startPoint.X, startPoint.Y);
            var lowerEndVec = unitVector * -1 * (width / 2) + new Vector2D(endPoint.X, endPoint.Y);

            // Create a line reference holding the 2 extreme sides.
            Line2D firstLine = Line2D.CreateByPoints(upperStartVec.ToPoint(), upperEndVec.ToPoint());
            Line2D secondLine = Line2D.CreateByPoints(lowerStartVec.ToPoint(), lowerEndVec.ToPoint());
            return (firstLine, secondLine);
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
                if (IsPointOnLine(side, point))
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
                if (IsLineHorizontal(side))
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
                if (IsLineVertical(side))
                {
                    flag = true;
                }
            }

            return flag;
        }
        #endregion


        #region Line applications.

        /// <summary>
        /// Inquires if the line is vertical.
        /// Note: Line must have a definite start and end point.
        /// </summary>
        /// <param 0name="line"></param>
        /// <returns></returns>
        public static bool IsLineVertical(Line2D line)
        {
            if (!IsLineHavingPoints(line))
                throw new Exception("This line has no defined points");
            if (Math.Abs(line.StartPoint.X - line.EndPoint.X) <= fudge)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Inquires if the line is horizontal.
        /// Note: Line must have a definite start and end point.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsLineHorizontal(Line2D line)
        {
            if (!IsLineHavingPoints(line))
                throw new Exception("This line has no defined points");
            if (Math.Abs(line.StartPoint.Y - line.EndPoint.Y) <= fudge)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Inquires if the line is inclined.
        /// Note: Line must have a definite start and end point.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsLineInclined(Line2D line)
        {
            return (!IsLineHorizontal(line) && !IsLineVertical(line));
        }

        /// <summary>
        /// Inquires if the incline lines are collinear,
        /// meaning that they share the same slope and intercept value of the Y-axis.
        /// Note: Lines must have a definite start and end point.
        /// </summary>
        /// <see href="https://www.easycalculation.com/maths-dictionary/collinear_line.html"/>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static bool InclinedLinesCollinear(Line2D line1, Line2D line2)
        {
            if (!IsLineInclined(line1) || !IsLineInclined(line2))
                throw new Exception("Lines are not inclined");

            if (Math.Abs(line1.Slope - line2.Slope) <= fudge &&
                Math.Abs(line1.YIntercept - line2.YIntercept) <= fudge)
                return true;
            return false;
        }

        /// <summary>
        /// Inquires if one of the lines is totally inside the other line.
        /// Note: Lines must have a definite start and end point.
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>           
        public static bool IsLineTotallyOverLapping(Line2D l1, Line2D l2)
        {
            if (IsLineHorizontal(l1) && IsLineHorizontal(l2))
            {
                var difference = Math.Abs(l1.StartPoint.Y - l2.StartPoint.Y);

                var coords = new List<double>{l1.StartPoint.X, l1.EndPoint.X,
                                             l2.StartPoint.X,l2.EndPoint.X};
                coords.Sort();
                if (difference <= fudge && IsXLineCoordsInRange(l1, l2))
                {
                    return true;
                }
                return false;
            }
            else if (IsLineVertical(l1) && IsLineVertical(l2))
            {
                var difference = Math.Abs(l1.StartPoint.X - l2.StartPoint.X);

                var coords = new List<double>{l1.StartPoint.Y, l1.EndPoint.Y,
                                             l2.StartPoint.Y,l2.EndPoint.Y};
                coords.Sort();
                if (difference <= fudge && IsYLineCoordsInRange(l1, l2))
                {
                    return true;
                }
                return false;
            }
            else if (IsLineInclined(l1) && IsLineInclined(l2))
            {
                if (InclinedLinesCollinear(l1, l2))
                {
                    if (IsPointOnLine(l1, l2.StartPoint) && IsPointOnLine(l1, l2.EndPoint) ||
                        IsPointOnLine(l2, l1.StartPoint) && IsPointOnLine(l2, l1.EndPoint))
                        return true;
                }
                return false;
            }

            else return false;
        }

        /// <summary>
        /// Inquires if one of the lines is partially inside the other line.
        /// NOTE: If the start or end of the first line is the same as the 
        /// start or end of the second line, this function will return false.
        /// Note: Lines must have a definite start and end point.
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public static bool IsLinePartiallyOverLapping(Line2D l1, Line2D l2)
        {
            if (l1.StartPoint.Equals(l2.StartPoint) || l1.StartPoint.Equals(l2.EndPoint)
               || l1.EndPoint.Equals(l2.StartPoint) || l1.EndPoint.Equals(l2.EndPoint))
            {
                return false;
            }
            else if (IsLineHorizontal(l1) && IsLineHorizontal(l2))
            {
                var difference = Math.Abs(l1.StartPoint.Y - l2.StartPoint.Y);
                if (difference <= fudge &&
                   (IsPointOnLine(l1, l2.StartPoint) || IsPointOnLine(l1, l2.EndPoint) ||
                    IsPointOnLine(l2, l1.StartPoint) || IsPointOnLine(l2, l1.EndPoint)))
                {
                    return true;
                }
                return false;
            }
            else if (IsLineVertical(l1) && IsLineVertical(l2))
            {
                var difference = Math.Abs(l1.StartPoint.X - l2.StartPoint.X);
                if (difference <= fudge &&
                   (IsPointOnLine(l1, l2.StartPoint) || IsPointOnLine(l1, l2.EndPoint) ||
                    IsPointOnLine(l2, l1.StartPoint) || IsPointOnLine(l2, l1.EndPoint)))
                {
                    return true;
                }
                return false;
            }
            else if (IsLineInclined(l1) && IsLineInclined(l2))
            {
                if (InclinedLinesCollinear(l1, l2))
                {
                    if ((IsPointOnLine(l1, l2.StartPoint) || IsPointOnLine(l1, l2.EndPoint)
                      || IsPointOnLine(l2, l1.StartPoint) || IsPointOnLine(l2, l1.EndPoint)))
                    {
                        return true;
                    }
                }
                return false;
            }

            else return false;
        }

        /// <summary>
        /// Inquire if one of the lines is partially or totally inside the other line.
        /// Note: Lines must have a definite start and end point.
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public static bool IsLineGenerallyOverLapping(Line2D l1, Line2D l2)
        {
            return (IsLinePartiallyOverLapping(l1, l2) || IsLineTotallyOverLapping(l1, l2));
        }

        /// <summary>
        /// Get the mid point of an Euclidean 2D Line.
        /// </summary>
        /// <param name="line">2 dimensional line.</param>
        /// <returns>Mid point.</returns>
        public static Point2D GetMidPoint(Line2D line)
        {
            return GetMidPoint(line.StartPoint, line.EndPoint);
        }


        #endregion


        #region Point application.

        /// <summary>
        /// Inquires if the point is on a line.
        /// NOTE: This function only acknowledges the boundaries of the start and end
        /// of the line, meaning if the point is satisfying the equation of a line but
        /// lies outside the range of the line's start or end point this function will
        /// return false.
        /// Note: Line must have a definite start and end point.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsPointOnLine(Line2D line, Point2D point)
        {
            if (point.Equals(line.StartPoint) || point.Equals(line.EndPoint))
            {
                return true;
            }
            else if (IsLineHorizontal(line))
            {
                var difference = Math.Abs(line.StartPoint.Y - point.Y);
                var flag = AreNumbersInRange(line.StartPoint.X, line.EndPoint.X, point.X);
                if (difference <= fudge && flag)
                    return true;
                return false;

            }
            else if (IsLineVertical(line))
            {
                var difference = Math.Abs(line.StartPoint.X - point.X);
                var flag = AreNumbersInRange(line.StartPoint.Y, line.EndPoint.Y, point.Y);
                if (difference <= fudge && flag)
                    return true;
                return false;
            }
            else if (IsLineInclined(line))
            {
                var difference = Math.Abs(point.Y - (line.Slope * point.X + line.YIntercept));
                var flagX = AreNumbersInRange(line.StartPoint.X, line.EndPoint.X, point.X);
                var flagY = AreNumbersInRange(line.StartPoint.Y, line.EndPoint.Y, point.Y);
                if (difference <= fudge && flagX && flagY)
                    return true;
                return false;
            }

            return false;
        }


        /// <summary>
        /// Gets the midpoint between 2 points.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Point2D GetMidPoint(Point2D start, Point2D end)
        {
            return new Point2D((start.X + end.X) / 2, (start.Y + end.Y) / 2);
        }

        /// <summary>
        /// Mirrors a point around an aligned axis.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Point2D MirrorPoint(Point2D point, Line2D line)
        {
            if (IsLineHorizontal(line))
            {
                Point2D intersectionPoint = new Point2D(point.X, line.StartPoint.Y);
                return new Point2D(2 * intersectionPoint.X - point.X, 2 * intersectionPoint.Y - point.Y);

            }

            else if (IsLineVertical(line))
            {
                Point2D intersectionPoint = new Point2D(line.StartPoint.X, point.Y);
                return new Point2D(2 * intersectionPoint.X - point.X, 2 * intersectionPoint.Y - point.Y);
            }
            else
            {
                double perpendicularSlope = -1 / line.Slope;
                Line2D linePerp = Line2D.CreateByPointAndSlope(point, perpendicularSlope);
                Point2D intersectionPoint = linePerp.Intersect(line);
                return new Point2D(2 * intersectionPoint.X - point.X, 2 * intersectionPoint.Y - point.Y);
            }
        }

        #endregion


        #region Helper functions.

        /// <summary>
        /// The Line2D class can be instantiated only by its Euclidean space
        /// slope or general formula, if this line is instantiated by
        /// the end and start points this function will return true.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static bool IsLineHavingPoints(Line2D line)
        {
            if (line.StartPoint == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// A helper function to check if the Y coordinates of the start or end point
        /// of a line is totally inside the Y coordinates range of the start or end point
        /// of the other line.
        /// Note: Lines must have a definite start and end point.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        private static bool IsYLineCoordsInRange(Line2D line1, Line2D line2)
        {
            var coords = new List<double>{line1.StartPoint.Y, line1.EndPoint.Y,
                                         line2.StartPoint.Y,line2.EndPoint.Y};
            coords.Sort();
            if (line1.StartPoint.Y == coords.First() && line1.EndPoint.Y == coords.Last() ||
                line1.EndPoint.Y == coords.First() && line1.StartPoint.Y == coords.Last() ||
                line2.StartPoint.Y == coords.First() && line2.EndPoint.Y == coords.Last() ||
                line2.EndPoint.Y == coords.First() && line2.StartPoint.Y == coords.Last())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// A helper function to check if the X coordinates of the start or end point
        /// of a line is totally inside the X coordinates range of the start or end point
        /// of the other line.
        /// Note: Lines must have a definite start and end point.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        private static bool IsXLineCoordsInRange(Line2D line1, Line2D line2)
        {
            var coords = new List<double>{line1.StartPoint.X, line1.EndPoint.X,
                                         line2.StartPoint.X,line2.EndPoint.X};
            coords.Sort();
            if (line1.StartPoint.X == coords.First() && line1.EndPoint.X == coords.Last() ||
                line1.EndPoint.X == coords.First() && line1.StartPoint.X == coords.Last() ||
                line2.StartPoint.X == coords.First() && line2.EndPoint.X == coords.Last() ||
                line2.EndPoint.X == coords.First() && line2.StartPoint.X == coords.Last())
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// A helper function to determine if some numbers are in the range of 2 extremes numbers.
        /// </summary>
        /// <param name="extreme1"></param>
        /// <param name="extreme2"></param>
        /// <param name="numbers"></param>
        /// <returns></returns>
        private static bool AreNumbersInRange(double extreme1, double extreme2, params double[] numbers)
        {
            var array = new List<double> { extreme1, extreme2 };
            array.AddRange(numbers);
            array.Sort();
            if (array[0] == extreme1 && array[array.Count - 1] == extreme2 ||
                array[0] == extreme2 && array[array.Count - 1] == extreme1)
            {
                return true;
            }
            return false;
        }

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
