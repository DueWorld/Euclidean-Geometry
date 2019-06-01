namespace MathEuclideanPrimitives.Utilities
{
    using MathEuclideanPrimitives.Geometric_Objects;
    using System;

    public static partial class GeometricUtilities
    {
        #region Circle Utilities

        #region Booleans

        /// <summary>
        /// Inquires if the two circle are touching externally at a point.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="inquiredCircle"></param>
        /// <returns></returns>
        public static bool IsTouchingExternally(this Circle circle, Circle inquiredCircle)
        {
            if (circle.Equals(inquiredCircle))
                return false;
            double distanceBetRadii = circle.CenterPoint.GetDistance(inquiredCircle.CenterPoint);
            return Math.Abs(distanceBetRadii - (circle.Radius + inquiredCircle.Radius)) <= fudge;
        }

        /// <summary>
        /// Inquires if the two circle intersect each other.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="inquiredCircle"></param>
        /// <returns></returns>
        public static bool IsTouchingInternally(this Circle circle, Circle inquiredCircle)
        {
            if (circle.Equals(inquiredCircle))
                return false;
            double distanceBetRadii = circle.CenterPoint.GetDistance(inquiredCircle.CenterPoint);
            return Math.Abs(distanceBetRadii - (circle.Radius - inquiredCircle.Radius)) <= fudge;
        }

        /// <summary>
        /// Inquires if the two circle do not intersect at all.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="inquiredCircle"></param>
        /// <returns></returns>
        public static bool IsDistant(this Circle circle, Circle inquiredCircle)
        {
            if (circle.Equals(inquiredCircle))
                return false;
            double distanceBetRadii = circle.CenterPoint.GetDistance(inquiredCircle.CenterPoint);
            return distanceBetRadii > (circle.Radius + inquiredCircle.Radius);
        }

        /// <summary>
        /// Inquires if a circle is inscribed inside each other.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="inquiredCircle"></param>
        /// <returns></returns>
        public static bool IsInscribedInEachOther(this Circle circle, Circle inquiredCircle)
        {
            if (circle.Equals(inquiredCircle))
                return false;
            double distanceBetRadii = circle.CenterPoint.GetDistance(inquiredCircle.CenterPoint);
            return distanceBetRadii <= Math.Abs(inquiredCircle.Radius - circle.Radius);
        }

        /// <summary>
        /// Inquires if the two circle are intersecting by any means.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="inquiredCircle"></param>
        /// <returns></returns>
        public static bool IsIntersecting(this Circle circle, Circle inquiredCircle)
        {
            if (circle.Equals(inquiredCircle))
                return false;
            return !circle.IsInscribedInEachOther(inquiredCircle) && !circle.IsDistant(inquiredCircle);
        }

        /// <summary>
        /// Inquires if the given circle doesn't intersect with the given line.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsDistant(this Circle circle, Line2D line)
        {
            double distanceFromCenter = line.GetDistance(circle.CenterPoint);
            return distanceFromCenter > circle.Radius;
        }

        /// <summary>
        /// Inquires if the line is intersecting the circle.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsIntersecting(this Circle circle, Line2D line)
        {
            double distanceFromCenter = line.GetDistance(circle.CenterPoint);
            return distanceFromCenter < circle.Radius;
        }

        /// <summary>
        /// Inquires if the circle and the line touches at a point.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsTangent(this Circle circle, Line2D line)
        {
            double distanceFromCenter = line.GetDistance(circle.CenterPoint);
            return Math.Abs(distanceFromCenter - circle.Radius) <= fudge;
        }

        /// <summary>
        /// Inquires if the given point is on the circle.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsPointOn(this Circle circle, Point2D point)
        {
            double distance = circle.CenterPoint.GetDistance(point);
            return Math.Abs(distance - circle.Radius) <= fudge;
        }

        /// <summary>
        /// Inquires if the given point is in the circle.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsPointIn(this Circle circle, Point2D point)
        {
            double distance = circle.CenterPoint.GetDistance(point);
            return distance < circle.Radius;

        }

        /// <summary>
        /// Inquires if the given point is out the circle.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsPointOut(this Circle circle, Point2D point)
        {
            double distance = circle.CenterPoint.GetDistance(point);
            return distance > circle.Radius;

        }
        #endregion

        #region Creation

        /// <summary>
        /// Creates 2 circle each two given points will pass right on them.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static int CreateCirclesFrom2Pts(Point2D point1, Point2D point2, double radius, out (Circle upperCircle, Circle lowerCircle) result)
        {
            double distance = point1.GetDistance(point2);

            if (2 * radius < distance)
            {
                result = (null, null);
                return 0;
            }

            else if (Math.Abs((2 * radius) - distance) <= fudge)
            {
                var centerPoint = Line2D.CreateByPoints(point1, point2).MidPoint();
                Circle circle = new Circle(radius, centerPoint);
                result = (circle, circle);
                return 1;
            }

            else
            {
                double height = Math.Sqrt((radius * radius) - ((distance / 2) * (distance / 2)));
                Vector2D vec = (point2 - point1).Normalize();
                Point2D intersectionPt = point1 + (vec * (distance / 2));
                Vector2D vec1 = new Vector2D(-vec.Y, vec.X);
                Vector2D vec2 = new Vector2D(vec.Y, -vec.X);
                Point2D centerPoint1 = intersectionPt + vec1 * height;
                Point2D centerPoint2 = intersectionPt + vec2 * height;
                result = (new Circle(radius, centerPoint1), new Circle(radius, centerPoint2));
                return 2;
            }
        }


        /// <summary>
        /// Gets the mid point of an arc which boundaries are given.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Point2D GetMidArcPoint(this Circle circle, Point2D point1, Point2D point2)
        {
            Vector2D v = point2 - point1;

            if (Math.Abs((v.Magintude) - (circle.Radius * 2)) <= fudge)
            {
                v = v.Normalize();
                v *= circle.Radius;
                Vector2D perpVector = new Vector2D(-v.Y, v.X);
                return circle.CenterPoint + perpVector;
            }

            Line2D line = Line2D.CreateByPoints(point1, point2);
            Point2D midPoint = line.MidPoint();
            Line2D perpLine = Line2D.CreateByPoints(circle.CenterPoint, midPoint);
            circle.Intersect(perpLine, out Point2D pIntersect1, out Point2D pIntersect2);

            var chosenPoint = line.GetDistance(pIntersect1) > line.GetDistance(pIntersect2)
                ? pIntersect2 : pIntersect1;

            return chosenPoint;
        }


        /// <summary>
        /// Offsets a point on a circle with a given arc length.
        /// The positive arc length indicates an offset taking place int eh anti clock wise direction.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point"></param>
        /// <param name="arcLength"></param>
        /// <returns></returns>
        public static Point2D OffsetPoint(this Circle circle, Point2D point, double arcLength)
        {
            if (!circle.IsPointOn(point))
            {
                return null;
            }
            if (Math.Abs(arcLength - 0) <= fudge)
            {
                return point;
            }
            double thetaRad = arcLength / circle.Radius;
            Vector2D vec = point - circle.CenterPoint;
            return circle.CenterPoint + vec.RotateRad(thetaRad);
        }
        #endregion



        #region Intersections



        /// <summary>
        /// Intersects 2 circles with each other.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="intersectedCircle"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns>The number of points produced after intersection 0 or 1 or 2 points.</returns>
        public static int Intersect(this Circle circle, Circle intersectedCircle, out Point2D point1, Point2D point2)
        {
            if (circle.IsDistant(intersectedCircle))
            {
                point1 = point2 = null;
                return 0;
            }
            else if (circle.IsTouchingExternally(intersectedCircle))
            {
                Vector2D vector = intersectedCircle.CenterPoint - circle.CenterPoint;
                vector = vector.Normalize();
                Point2D point = circle.CenterPoint + (vector * circle.Radius);
                point1 = point;
                point2 = point;
                return 1;
            }
            else if (intersectedCircle.IsTouchingInternally(intersectedCircle))
            {
                var biggerCircle = circle.Radius > intersectedCircle.Radius ? circle : intersectedCircle;
                var smallerCircle = circle.Radius < intersectedCircle.Radius ? circle : intersectedCircle;

                Vector2D vector = smallerCircle.CenterPoint - biggerCircle.CenterPoint;
                vector = vector.Normalize();
                Point2D point = biggerCircle.CenterPoint + (vector * biggerCircle.Radius);
                point1 = point;
                point2 = point;
                return 1;

            }
            else
            {
                double dist = circle.CenterPoint.GetDistance(intersectedCircle.CenterPoint);


                double a = (circle.Radius * circle.Radius -
                    intersectedCircle.Radius * intersectedCircle.Radius + dist * dist) / (2 * dist);


                double h = Math.Sqrt(circle.Radius * circle.Radius - a * a);


                double cx2 = circle.CenterPoint.X + a * (intersectedCircle.CenterPoint.X - circle.CenterPoint.X) / dist;
                double cy2 = circle.CenterPoint.Y + a * (intersectedCircle.CenterPoint.Y - circle.CenterPoint.Y) / dist;


                point1 = new Point2D(
                   (cx2 + h * (intersectedCircle.CenterPoint.Y - circle.CenterPoint.Y) / dist),
                   (cy2 - h * (intersectedCircle.CenterPoint.X - circle.CenterPoint.X) / dist));


                point2 = new Point2D(
                    (cx2 - h * (intersectedCircle.CenterPoint.Y - circle.CenterPoint.Y) / dist),
                    (cy2 + h * (intersectedCircle.CenterPoint.X - circle.CenterPoint.X) / dist));


                return 2;
            }
        }






        /// <summary>
        /// Intersects a line with a circle.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="line"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns>Number of points produced after intersection 0 or 1 or 2 points.</returns>
        public static int Intersect(this Circle circle, Line2D line, out Point2D point1, out Point2D point2)
        {
            if (circle.IsDistant(line))
            {
                point1 = point2 = null;
                return 0;
            }
            else if (circle.IsTangent(line))
            {
                Point2D point = line.Project(circle.CenterPoint);
                point1 = point2 = point;
                return 1;
            }
            else
            {
                //HANDLE IF THE LINE IS ON THE CENTER POINT.

                double distance = line.GetDistance(circle.CenterPoint);
                if (Math.Abs(distance - 0) <= fudge)
                {
                    if (line.IsLineHavingPoints())
                    {
                        if (line.IsVertical() || line.IsVertical())
                        {
                            Vector2D vector = (line.StartPoint - line.EndPoint).Normalize();

                            point1 = circle.CenterPoint + vector * circle.Radius;
                            point2 = circle.CenterPoint - vector * circle.Radius;
                            return 2;
                        }
                    }
                    Point2D arbitraryPoint = new Point2D();
                    arbitraryPoint.X = 6;
                    arbitraryPoint.Y = line.Slope * arbitraryPoint.X + line.YIntercept;
                    Vector2D arbitraryVector = (arbitraryPoint - circle.CenterPoint).Normalize();

                    point1 = circle.CenterPoint + arbitraryVector * circle.Radius;
                    point2 = circle.CenterPoint - arbitraryVector * circle.Radius;
                    return 2;

                }
                double a = Math.Sqrt(circle.Radius * circle.Radius - distance * distance);

                Point2D projectionPoint = line.Project(circle.CenterPoint);
                Vector2D vec = (projectionPoint - circle.CenterPoint).Normalize();

                Vector2D vec1 = new Vector2D(-1 * vec.Y, vec.X);
                Vector2D vec2 = new Vector2D(vec.Y, -vec.X);

                point1 = projectionPoint + vec1 * a;
                point2 = projectionPoint + vec2 * a;
                return 2;
            }
        }
        #endregion



        #region General Calculation

        /// <summary>
        /// Calculates an arc length inscribed between a given angle.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="angleInRad"></param>
        /// <returns></returns>
        public static double CalculateArcLength(this Circle circle, double angleInRad)
        {
            return circle.Radius * angleInRad;
        }


        /// <summary>
        /// Calculates an arc length which boundary points are given.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double CalculateArcLength(this Circle circle, Point2D point1, Point2D point2)
        {
            Vector2D vector1 = point1 - circle.CenterPoint;
            Vector2D vector2 = point2 - circle.CenterPoint;
            double angle = vector1.ComputeAngle(vector2);
            return CalculateArcLength(circle, ToRad(angle));
        }

        #endregion





        #endregion
    }
}
