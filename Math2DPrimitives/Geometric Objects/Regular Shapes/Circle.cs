namespace MathEuclideanPrimitives.Geometric_Objects
{
    using System;
    using Utilities;

    /// <summary>
    /// A class representing a circle in euclidean space.
    /// </summary>
    public class Circle : IEquatable<Circle>
    {
        private double xFactor;
        private double yFactor;
        private double constant;

        private double radius;
        private Point2D centerPoint;
        private double circumference;
        private double area;

        /// <summary>
        /// The factor of the X of the equation of a circle before completing the square..
        /// <seealso href="https://www.purplemath.com/modules/sqrcircle.htm"/>
        /// </summary>
        public double XFactor
        {
            get => xFactor;
            set
            {
                xFactor = value;
                AssignFromGeneralEquation();
            }
        }

        /// <summary>
        /// The factor of the Y of the equation of a circle before completing the square..
        /// <seealso href="https://www.purplemath.com/modules/sqrcircle.htm"/>
        /// </summary>
        public double YFactor
        {
            get => yFactor;
            set
            {
                yFactor = value;
                AssignFromGeneralEquation();
            }
        }

        /// <summary>
        /// The constant value of the equation of a circle before completing the square.
        /// <seealso href="https://www.purplemath.com/modules/sqrcircle.htm"/>
        /// </summary>
        public double Constant
        {
            get => constant;
            set
            {
                constant = value;
                AssignFromGeneralEquation();
            }
        }

        /// <summary>
        ///The radius of the circle.
        /// </summary>
        public double Radius
        {
            get => radius;
            set
            {
                radius = value;
                AssignFromRadialEquation();
            }
        }

        /// <summary>
        /// The center point of the circle.
        /// </summary>
        public Point2D CenterPoint
        {
            get => centerPoint;
            set
            {
                centerPoint = value;
                AssignFromRadialEquation();
            }
        }

        /// <summary>
        /// The area of the circle.
        /// </summary>
        public double Area
        {
            get => area;
            set
            {
                radius = value;
                AssignFromRadialEquation();
            }
        }

        /// <summary>
        /// Calculates the center point of the circle.
        /// </summary>
        /// <returns></returns>
        private Point2D GetCenterPoint()
        {
            return new Point2D(-1 * xFactor / 2, -1 * yFactor / 2);
        }

        /// <summary>
        /// Calculates the x factor of the circle.
        /// </summary>
        /// <returns></returns>
        private double GetXFactor()
        {
            return -1 * centerPoint.X * 2;
        }

        /// <summary>
        /// Calculates the Y factor of the circle.
        /// </summary>
        /// <returns></returns>
        private double GetYFactor()
        {
            return -1 * centerPoint.Y * 2;

        }

        /// <summary>
        /// Calculates the constant of the circle.
        /// </summary>
        /// <returns></returns>
        private double GetConstant()
        {
            return (Math.Pow((centerPoint.X), 2)) + (Math.Pow((centerPoint.Y), 2)) - (radius * radius);

        }

        /// <summary>
        /// Instantiates a circle by the algebraic equation before completing the square.
        /// <seealso href="https://www.purplemath.com/modules/sqrcircle.htm"/>
        /// </summary>
        /// <param name="xFactor"></param>
        /// <param name="yFactor"></param>
        /// <param name="constant"></param>
        public Circle(double xFactor, double yFactor, double constant)
        {
            this.xFactor = xFactor;
            this.yFactor = yFactor;
            this.constant = constant;
            AssignFromGeneralEquation();
        }

        /// <summary>
        /// Instantiates a circle by a radius and center point to complete the square of the circle function.
        /// <seealso href="https://www.purplemath.com/modules/sqrcircle.htm"/>
        /// <param name="radius"></param>
        /// <param name="centerPoint"></param>
        public Circle(double radius, Point2D centerPoint)
        {
            this.radius = radius;
            this.centerPoint = centerPoint;
            AssignFromRadialEquation();

        }

        /// <summary>
        /// Instantiates a circle by 3 points.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        public Circle(Point2D point1, Point2D point2, Point2D point3)
            : this(GetCenterPoint3Pts(point1, point2, point3), point3)
        { }


        /// <summary>
        /// Calculates the center point given 3 points on the circle.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <returns></returns>
        private static Point2D GetCenterPoint3Pts(Point2D point1, Point2D point2, Point2D point3)
        {

            Line2D line = Line2D.CreateByPoints(point1, point2);
            Line2D line2 = Line2D.CreateByPoints(point1, point3);
            Point2D p1 = line.MidPoint();
            Point2D p2 = line2.MidPoint();
            Line2D perpLine1 = Line2D.CreateByPointAndSlope(p1, -1 / line.Slope);
            Line2D perpLine2 = Line2D.CreateByPointAndSlope(p2, -1 / line2.Slope);
            return perpLine1.Intersect(perpLine2);
        }


        /// <summary>
        /// Calculates the center point, radius, area and circumference of the circle from the general equation.
        /// </summary>
        private void AssignFromGeneralEquation()
        {
            radius = GetRadius();
            centerPoint = GetCenterPoint();
            area = GetArea();
            circumference = GetCircumference();
        }


        /// <summary>
        /// Calculates the X,Y and constant of the general equation from the complete square equation.
        /// </summary>
        private void AssignFromRadialEquation()
        {
            this.xFactor = GetXFactor();
            this.yFactor = GetYFactor();
            this.constant = GetConstant();
            this.area = GetArea();
            this.circumference = GetCircumference();
        }


        /// <summary>
        /// Instantiates a circle from a circumference and a center point.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="circumference"></param>
        public Circle(Point2D center, double circumference)
            : this((circumference / 2 * Math.PI), center)
        { }

        /// <summary>
        /// Instantiates a circle from a point on circle and a center point.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="pointOnCircle"></param>
        public Circle(Point2D center, Point2D pointOnCircle)
            : this(center.GetDistance(pointOnCircle), center)
        { }

        /// <summary>
        /// Calculates the radius.
        /// </summary>
        /// <returns></returns>
        private double GetRadius()
        {
            double xBalance = Math.Pow((xFactor / 2), 2);
            double yBalance = Math.Pow((yFactor / 2), 2);
            return Math.Sqrt(xBalance + yBalance - constant);
        }

        /// <summary>
        /// Calculates the area.
        /// </summary>
        /// <returns></returns>
        private double GetArea()
        {
            return Math.Pow(Math.PI * radius, 2);

        }

        /// <summary>
        /// Calculates the circumference.
        /// </summary>
        /// <returns></returns>
        private double GetCircumference()
        {
            return 2 * Math.PI * radius;

        }

        /// <summary>
        /// Checks the equality of two circles.
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public bool Equals(Circle circle)
        {
            return (Math.Abs(this.radius - circle.radius) <= 0.01d && (this.centerPoint.Equals(circle.centerPoint)));
        }


    }
}
