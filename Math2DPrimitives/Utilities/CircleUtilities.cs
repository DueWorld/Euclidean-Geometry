namespace MathEuclideanPrimitives.Utilities
{
    using MathEuclideanPrimitives.Geometric_Objects;
    using System;

    public static partial class GeometricUtilities
    {
        #region Circle Utilities

        #region Booleans

        /// <summary>
        /// 
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
        /// 
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
        /// 
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
        /// 
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
        /// 
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
        /// 
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
        /// 
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
        /// 
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
        /// 
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
        /// 
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
        /// 
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
        /// 
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
        /// 
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
            if (arcLength == 0)
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
        /// 
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="intersectedCircle"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int Intersect(this Circle circle, Circle intersectedCircle, out (Point2D point1, Point2D point2) result)
        {
            if (circle.IsDistant(intersectedCircle))
            {
                result = (null, null);
                return 0;
            }
            else if (circle.IsTouchingExternally(intersectedCircle))
            {
                Vector2D vector = intersectedCircle.CenterPoint - circle.CenterPoint;
                vector = vector.Normalize();
                Point2D point = circle.CenterPoint + (vector * circle.Radius);
                result = (point, point);
                return 1;
            }
            else if (intersectedCircle.IsTouchingInternally(intersectedCircle))
            {
                var biggerCircle = circle.Radius > intersectedCircle.Radius ? circle : intersectedCircle;
                var smallerCircle = circle.Radius < intersectedCircle.Radius ? circle : intersectedCircle;

                Vector2D vector = smallerCircle.CenterPoint - biggerCircle.CenterPoint;
                vector = vector.Normalize();
                Point2D point = biggerCircle.CenterPoint + (vector * biggerCircle.Radius);
                result = (point, point);
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


                Point2D point1 = new Point2D(
                    (cx2 + h * (intersectedCircle.CenterPoint.Y - circle.CenterPoint.Y) / dist),
                    (cy2 - h * (intersectedCircle.CenterPoint.X - circle.CenterPoint.X) / dist));


                Point2D point2 = new Point2D(
                    (cx2 - h * (intersectedCircle.CenterPoint.Y - circle.CenterPoint.Y) / dist),
                    (cy2 + h * (intersectedCircle.CenterPoint.X - circle.CenterPoint.X) / dist));


                result = (point1, point2);
                return 2;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="line"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int Intersect(this Circle circle, Line2D line, out (Point2D point1, Point2D point2) result)
        {
            if (circle.IsDistant(line))
            {
                result = (null, null);
                return 0;
            }
            else if (circle.IsTangent(line))
            {
                Point2D point = line.Project(circle.CenterPoint);
                result = (point, point);
                return 1;
            }
            else
            {
                double distance = line.GetDistance(circle.CenterPoint);
                double a = Math.Sqrt(circle.Radius * circle.Radius - distance * distance);

                Point2D projectionPoint = line.Project(circle.CenterPoint);
                Vector2D vec = (projectionPoint - circle.CenterPoint).Normalize();

                Vector2D vec1 = new Vector2D(-1 * vec.Y, vec.X);
                Vector2D vec2 = new Vector2D(vec.Y, -vec.X);

                result = (projectionPoint + vec1 * a, projectionPoint + vec2 * a);
                return 2;
            }
        }
        #endregion

        #endregion
    }
}
