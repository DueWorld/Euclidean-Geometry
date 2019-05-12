namespace MathEuclideanPrimitives.Geometric_Objects
{
    using System;
    using Utilities;

    /// <summary>
    /// 
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


        public double XFactor
        {
            get => xFactor;
            set
            {
                xFactor = value;
                AssignFromGeneralEquation();
            }
        }

        public double YFactor
        {
            get => yFactor;
            set
            {
                yFactor = value;
                AssignFromGeneralEquation();
            }
        }

        public double Constant
        {
            get => constant;
            set
            {
                constant = value;
                AssignFromGeneralEquation();
            }
        }

        public double Radius
        {
            get => radius;
            set
            {
                radius = value;
                AssignFromRadialEquation();
            }
        }

        public Point2D CenterPoint
        {
            get => centerPoint;
            set
            {
                centerPoint = value;
                AssignFromRadialEquation();
            }
        }

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
        /// 
        /// </summary>
        /// <returns></returns>
        private Point2D GetCenterPoint()
        {
            return new Point2D(-1 * xFactor / 2, -1 * yFactor / 2);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double GetXFactor()
        {
            return -1 * centerPoint.X * 2;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double GetYFactor()
        {
            return -1 * centerPoint.Y * 2;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double GetConstant()
        {
            return (Math.Pow((centerPoint.X), 2)) + (Math.Pow((centerPoint.Y), 2)) - (radius * radius);

        }


        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="centerPoint"></param>
        public Circle(double radius, Point2D centerPoint)
        {
            this.radius = radius;
            this.centerPoint = centerPoint;
            AssignFromRadialEquation();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        public Circle(Point2D point1, Point2D point2, Point2D point3)
            : this(GetCenterPoint3Pts(point1, point2, point3), point3)
        { }


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

        private void AssignFromGeneralEquation()
        {
            radius = GetRadius();
            centerPoint = GetCenterPoint();
            area = GetArea();
            circumference = GetCircumference();
        }

        private void AssignFromRadialEquation()
        {
            this.xFactor = GetXFactor();
            this.yFactor = GetYFactor();
            this.constant = GetConstant();
            this.area = GetArea();
            this.circumference = GetCircumference();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="circumference"></param>
        public Circle(Point2D center, double circumference)
            : this((circumference / 2 * Math.PI), center)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="pointOnCircle"></param>
        public Circle(Point2D center, Point2D pointOnCircle)
            : this(center.GetDistance(pointOnCircle), center)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double GetRadius()
        {
            double xBalance = Math.Pow((xFactor / 2), 2);
            double yBalance = Math.Pow((yFactor / 2), 2);
            return Math.Sqrt(xBalance + yBalance - constant);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double GetArea()
        {
            return Math.Pow(Math.PI * radius, 2);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double GetCircumference()
        {
            return 2 * Math.PI * radius;

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        public bool Equals(Circle circle)
        {
            return ((this.radius == circle.radius) && (this.centerPoint.Equals(circle.centerPoint)));
        }


    }
}
