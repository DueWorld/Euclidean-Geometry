namespace MathEuclideanPrimitives
{
    using System;

    /// <summary>
    /// Class built using Euclidean 2D geometry properties.
    /// Geometry line in linear form. General form and in slope form respectively:
    /// A(x Constant)x + B(y constant)y +(individual constant) C = 0;
    ///  Y= m(slope)x+b(y interception);
    /// <see href="http://en.wikipedia.org/wiki/Linear_equation"/>
    /// <see href="https://en.wikipedia.org/wiki/Euclidean_geometry"/>
    /// </summary>
    public class Line2D
    {

        private float slope;
        private float yIntercept;
        private float xConstant;
        private float yConstant;
        private float constant;
        private Point2D startPoint;
        private Point2D endPoint;



        public float Slope { get => slope; }
        public float YIntercept { get => yIntercept; }
        public float XConstant { get => xConstant; }
        public float YConstant { get => yConstant; }
        public float Constant { get => constant; }
        public Point2D StartPoint => startPoint;
        public Point2D EndPoint => endPoint;

        #region Constructors and Factory Methods

        /// <summary>
        /// A hidden constructor.
        /// </summary>
        /// <param name="slope"></param>
        /// <param name="yInter"></param>
        /// <param name="xConst"></param>
        /// <param name="yConst"></param>
        /// <param name="Const"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        protected Line2D(float slope, float yInter, float xConst, float yConst, float Const, Point2D start, Point2D end)
        {

            this.slope = slope;
            yIntercept = yInter;
            xConstant = xConst;
            yConstant = yConst;
            constant = Const;
            startPoint = start;
            endPoint = end;

        }

        /// <summary>
        /// A hidden constructor.
        /// </summary>
        /// <param name="slope"></param>
        /// <param name="yInter"></param>
        /// <param name="xConst"></param>
        /// <param name="yConst"></param>
        /// <param name="Const"></param>
        protected Line2D(float slope, float yInter, float xConst, float yConst, float Const)
        {
            this.slope = slope;
            yIntercept = yInter;
            xConstant = xConst;
            yConstant = yConst;
            constant = Const;
        }

        /// <summary>
        /// Creates a Line by the start and the end point if known, will calculate every other form.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Line2D CreateByPoints(Point2D start, Point2D end) => new Line2D(GetSlopeFromPoints(start, end), GetYInterceptFromPoints(start, end), 1, 1 / (-GetSlopeFromPoints(start, end)), -GetYInterceptFromPoints(start, end) / (-GetSlopeFromPoints(start, end)), start, end);

        /// <summary>
        /// Creates a line using the slope and the y intercept if known, will calculate every other form. 
        /// </summary>
        /// <param name="slope"></param>
        /// <param name="yIntercept"></param>
        /// <returns></returns>
        public static Line2D CreateBySlope(float slope, float yIntercept) => new Line2D(slope, yIntercept, -slope, 1, -yIntercept);

        /// <summary>
        /// Creates a line using the general form factors if known, will calculate every other form. 
        /// </summary>
        /// <param name="xConstant"></param>
        /// <param name="yConstant"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        public static Line2D CreateByStandardForm(float xConstant, float yConstant, float constant) => new Line2D((-xConstant / yConstant), (-constant / yConstant), xConstant, yConstant, constant);


        #endregion

        /// <summary>
        /// Intersecting 2 Lines together using linear Equations. 
        /// <seealso href="https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection"/>
        /// </summary>
        /// <param name="intersectedLine">Line to be intersected with.</param>
        /// <returns>Intersection point.</returns>
        public Point2D Intersect(Line2D intersectedLine)
        {
            if (this.startPoint.X == this.endPoint.X && intersectedLine.startPoint.Y == intersectedLine.endPoint.Y)
            {
                return new Point2D(startPoint.X, intersectedLine.startPoint.Y);
            }

            float x = ((intersectedLine.YIntercept - YIntercept) / (Slope - intersectedLine.Slope));
            float y = intersectedLine.Slope * x + intersectedLine.YIntercept;
            return new Point2D(x, y);

        }

        /// <summary>
        /// Intersecting 2 Lines together using linear Equations.
        /// <seealso href="https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection"/>
        /// </summary>
        /// <param name="intersectLine1">First Line.</param>
        /// <param name="intersectLine2">Second Line.</param>
        /// <returns>Intersection Point.</returns>
        public static Point2D Intersect(Line2D intersectLine1, Line2D intersectLine2)
        {
            float x = ((intersectLine1.YIntercept - intersectLine2.YIntercept) / (intersectLine2.Slope - intersectLine1.Slope));
            float y = intersectLine1.Slope * x + intersectLine1.YIntercept;
            return new Point2D(x, y);
        }

        /// <summary>
        /// Intersecting 2 Lines together using linear Equations. 
        /// The Line is being formed using the slope Form.
        /// <seealso href="https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection"/>
        /// <param name="slope">slope (m) </param>
        /// <param name="yIntercept">y interception coordinate (b)</param>
        /// <returns>Intersection Point.</returns>
        public Point2D Intersect(float slope, float yIntercept)
        {
            Line2D refLine = CreateBySlope(slope, yIntercept);
            return Intersect(refLine);
        }

        /// <summary>
        /// Intersecting 2 Lines together using linear Equations. 
        /// The Line is being formed using the General Form.
        /// <seealso href="https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection"/>
        /// <param name="xConstant">X constant.</param>
        /// <param name="yConstant">Y constant.</param>
        /// <param name="constant">Individual constant.</param>
        /// <returns>Intersection Point.</returns>
        public Point2D Intersect(float xConstant, float yConstant, float constant)
        {
            Line2D refLine = CreateByStandardForm(xConstant, yConstant, constant);
            return Intersect(refLine);
        }

        /// <summary>
        /// Gets the perpendicular distance between a point and a line.
        /// <seealso href="https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line"/>
        /// </summary>
        /// <param name="p1">Point in 2D space</param>
        /// <returns>The distance between the line and the point.</returns>
        public float GetDistance(Point2D p)
        {
            float sqrtForm = (float)Math.Sqrt(Math.Abs(Math.Pow(xConstant, 2) + (Math.Pow(yConstant, 2))));
            float nominator = xConstant * p.X + yConstant * p.Y + constant;
            return Math.Abs(nominator) / sqrtForm;
        }

        /// <summary>
        /// Gets the perpendicular distance between a point and a line.
        /// <seealso href="https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line"/>
        /// </summary>
        /// <param name="p1">Point in 2D space</param>
        /// <param name="l1">Line</param>
        /// <returns>The distance between the line and the point.</returns>
        public static float GetDistance(Point2D p1, Line2D l1) => l1.GetDistance(p1);


        /// <summary>
        /// Gets both the inner and the outer angles between 2 lines.
        /// <seealso href="https://www.mathstopia.net/coordinate-geometry/angle-two-lines"/>
        /// </summary>
        /// <param name="l1">first line.</param>
        /// <returns>Value Tuples of the Acute and Obtuse Angle between 2 lines</returns>
        public (float acuteAngle, float obtuseAngle) GetAngles(Line2D l)
        {
            float minusSlope = this.slope - l.slope;
            float productSlope = this.slope * l.slope;
            float radAngle1 = (float)Math.Atan(minusSlope / (1 + productSlope));
            float radAngle2 = -radAngle1;
            float angle1InDeg = (float)((radAngle1 * 180) / Math.PI);
            float angle2InDeg = (float)((radAngle2 * 180) / Math.PI);

            if (angle1InDeg > 0)
            {
                return (angle1InDeg, angle2InDeg + 180);
            }

            return (angle2InDeg, angle1InDeg + 180);
        }


        /// <summary>
        /// Gets both the inner and the outer angles between 2 lines.
        /// <seealso href="https://www.mathstopia.net/coordinate-geometry/angle-two-lines"/>
        /// </summary>
        /// <param name="l1">first line.</param>
        /// <param name="l2">second line.</param>
        /// <returns>Value Tuples of the Acute and Obtuse Angle between 2 lines</returns>
        public static (float acuteAngle, float obtuseAngle) GetAngles(Line2D l1, Line2D l2) => l1.GetAngles(l2);



        /// <summary>
        /// Using Algebraic calculations getting the slope from start and end points.
        /// y2-y1/x2-x1;
        /// </summary>
        /// <param name="p1">Start Point.</param>
        /// <param name="p2">End Point.</param>
        /// <returns>Slope</returns>
        protected static float GetSlopeFromPoints(Point2D p1, Point2D p2)
        {
            return ((p2.Y - p1.Y) / (p2.X - p1.X));
        }


        /// <summary>
        /// Using Algebraic calculations getting the y intercept from start and end points.
        /// </summary>
        /// <param name="p1">Start Point.</param>
        /// <param name="p2">End Point.</param>
        /// <returns>Y Intercepting.</returns>
        protected static float GetYInterceptFromPoints(Point2D p1, Point2D p2)
        {
            return ((p1.Y) - (GetSlopeFromPoints(p1, p2) * p1.X));
        }
    }
}
