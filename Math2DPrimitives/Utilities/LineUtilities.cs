namespace MathEuclideanPrimitives.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class GeometricUtilities
    {
        #region Line applications

        /// <summary>
        /// Inquires if the line is vertical.
        /// Note: Line must have a definite start and end point.
        /// </summary>
        /// <param 0name="line"></param>
        /// <returns></returns>
        public static bool IsVertical(this Line2D line)
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
        public static bool IsHorizontal(this Line2D line)
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
        public static bool IsInclined(this Line2D line)
        {
            return (!IsHorizontal(line) && !IsVertical(line));
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
            if (!IsInclined(line1) || !IsInclined(line2))
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
        public static bool IsTotallyOverLapping(this Line2D l1, Line2D l2)
        {
            if (IsHorizontal(l1) && IsHorizontal(l2))
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
            else if (IsVertical(l1) && IsVertical(l2))
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
            else if (IsInclined(l1) && IsInclined(l2))
            {
                if (InclinedLinesCollinear(l1, l2))
                {
                    if (l2.StartPoint.LiesOnBoundedLine(l1) && l2.EndPoint.LiesOnBoundedLine(l1) ||
                        l1.StartPoint.LiesOnBoundedLine(l2) && l1.EndPoint.LiesOnBoundedLine(l2))
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
        public static bool IsPartiallyOverLapping(this Line2D l1, Line2D l2)
        {
            if (l1.StartPoint.Equals(l2.StartPoint) || l1.StartPoint.Equals(l2.EndPoint)
               || l1.EndPoint.Equals(l2.StartPoint) || l1.EndPoint.Equals(l2.EndPoint))
            {
                return false;
            }
            else if (IsHorizontal(l1) && IsHorizontal(l2))
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
            else if (IsVertical(l1) && IsVertical(l2))
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
            else if (IsInclined(l1) && IsInclined(l2))
            {
                if (InclinedLinesCollinear(l1, l2))
                {
                    if ((IsPointOnLine(l1, l2.StartPoint) || IsPointOnLine(l1, l2.EndPoint)
        ||
        IsPointOnLine(l2, l1.StartPoint) || IsPointOnLine(l2, l1.EndPoint)))
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
        public static bool IsGenerallyOverLapping(this Line2D l1, Line2D l2)
        {
            return (IsPartiallyOverLapping(l1, l2) || IsTotallyOverLapping(l1, l2));
        }

        /// <summary>
        /// Get the mid point of an Euclidean 2D Line.
        /// </summary>
        /// <param name="line">2 dimensional line.</param>
        /// <returns>Mid point.</returns>
        public static Point2D MidPoint(this Line2D line)
        {
            return line.StartPoint.GetMidPoint(line.EndPoint);
        }
        #endregion
        
        #region HelperFunctions

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
        [Obsolete("Use the LiesOn function instead")]
        public static bool IsPointOnLine(Line2D line, Point2D point)
        {
            if (point.Equals(line.StartPoint) || point.Equals(line.EndPoint))
            {
                return true;
            }
            else if (line.IsHorizontal())
            {
                var difference = Math.Abs(line.StartPoint.Y - point.Y);
                var flag = AreNumbersInRange(line.StartPoint.X, line.EndPoint.X, point.X);
                if (difference <= fudge && flag)
                    return true;
                return false;

            }
            else if (line.IsVertical())
            {
                var difference = Math.Abs(line.StartPoint.X - point.X);
                var flag = AreNumbersInRange(line.StartPoint.Y, line.EndPoint.Y, point.Y);
                if (difference <= fudge && flag)
                    return true;
                return false;
            }
            else if (line.IsInclined())
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
        /// The Line2D class can be instantiated only by its Euclidean space
        /// slope or general formula, if this line is instantiated by
        /// the end and start points this function will return true.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsLineHavingPoints(this Line2D line)
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
        #endregion
    }
}
