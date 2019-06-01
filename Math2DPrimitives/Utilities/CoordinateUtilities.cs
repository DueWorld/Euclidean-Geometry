namespace MathEuclideanPrimitives.Utilities
{
    using System;

    public static partial class GeometricUtilities
    {
        #region Point application
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
        public static bool LiesOnBoundedLine(this Point2D point, Line2D line)
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
        /// Inquires if the point is on a line.
        /// Note: Line must have a definite start and end point.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool LiesOnUnBoundedLine(this Point2D point, Line2D line)
        {
            if (point.Equals(line.StartPoint) || point.Equals(line.EndPoint))
            {
                return true;
            }
            else if (line.IsHorizontal())
            {
                var difference = Math.Abs(line.StartPoint.Y - point.Y);
                if (difference <= fudge)
                    return true;
                return false;

            }

            else if (line.IsVertical())
            {
                var difference = Math.Abs(line.StartPoint.X - point.X);
                if (difference <= fudge)
                    return true;
                return false;
            }

            else if (line.IsInclined())
            {
                var difference = Math.Abs(point.Y - (line.Slope * point.X + line.YIntercept));
                if (difference <= fudge)
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
        public static Point2D GetMidPoint(this Point2D start, Point2D end)
        {
            return new Point2D((start.X + end.X) / 2, (start.Y + end.Y) / 2);
        }

        /// <summary>
        /// Mirrors a point around an aligned axis.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Point2D MirrorPoint(this Point2D point, Line2D line)
        {
            if (line.IsHorizontal())
            {
                Point2D intersectionPoint = new Point2D(point.X, line.StartPoint.Y);
                return new Point2D(2 * intersectionPoint.X - point.X, 2 * intersectionPoint.Y - point.Y);

            }

            else if (line.IsVertical())
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

        /// <summary>
        /// Rotates a point by a given angle in 2D space around the origin point of the vector.
        /// Positive direction is the anti-clock wise direction.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="angleInDegree"></param>
        /// <returns></returns>
        public static Point2D RotateDeg(this Point2D vector, double angleInDegree)
        {
            double rotatedX = vector.X * Math.Cos(ToRad(angleInDegree)) - vector.Y * Math.Sin(ToRad(angleInDegree));
            double rotatedY = vector.X * Math.Sin(ToRad(angleInDegree)) + vector.Y * Math.Cos(ToRad(angleInDegree));
            return new Point2D(rotatedX, rotatedY);
        }

        /// <summary>
        /// Rotates a point by a given angle in 2D space around the origin point of the vector.
        /// Positive direction is the anti-clock wise direction. 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="angleInRad"></param>
        /// <returns></returns>
        public static Point2D RotateRad(this Point2D vector, double angleInRad)
        {
            double rotatedX = vector.X * Math.Cos(angleInRad) - vector.Y * Math.Sin(angleInRad);
            double rotatedY = vector.X * Math.Sin(angleInRad) + vector.Y * Math.Cos(angleInRad);
            return new Point2D(rotatedX, rotatedY);
        }

        #endregion

        #region Vector applications

        /// <summary>
        /// Rotates a vector by a given angle in 2D space around the origin point of the vector.
        /// Positive direction is the anti-clock wise direction.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="angleInDegree"></param>
        /// <returns></returns>
        public static Vector2D RotateDeg(this Vector2D vector,double angleInDegree)
        {
            double rotatedX = vector.X * Math.Cos(ToRad(angleInDegree)) - vector.Y * Math.Sin(ToRad(angleInDegree));
            double rotatedY = vector.X * Math.Sin(ToRad(angleInDegree)) + vector.Y * Math.Cos(ToRad(angleInDegree));
            return new Vector2D(rotatedX, rotatedY);
        }
        
        /// <summary>
        /// Rotates a vector by a given angle in 2D space around the origin point of the vector.
        /// Positive direction is the anti-clock wise direction.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="angleInRad"></param>
        /// <returns></returns>
        public static Vector2D RotateRad(this Vector2D vector, double angleInRad)
        {
            double rotatedX = vector.X * Math.Cos(angleInRad) - vector.Y * Math.Sin(angleInRad);
            double rotatedY = vector.X * Math.Sin(angleInRad) + vector.Y * Math.Cos(angleInRad);
            return new Vector2D(rotatedX, rotatedY);
        }
        #endregion

        #region Conversions 

        /// <summary>
        /// Converts from a vector into a point.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Point2D ToPoint(this Vector2D vector)
        {
            return new Point2D(vector.X, vector.Y);
        }

        /// <summary>
        /// Converts from a point into a vector.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2D ToVector(this Point2D point)
        {
            return new Vector2D(point.X, point.Y);
        }
        #endregion
    }
}
