namespace MathEuclideanPrimitives
{
    /// <summary>
    /// A fairly horizontal line made, this class can be helpful implementing many 
    /// polygon algorithms.
    /// </summary>
    public sealed class PointThreshold : Line2D
    {
        /// <summary>
        /// Create the threshold by the start and the end points.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static new PointThreshold CreateByPoints(Point2D start, Point2D end) => new PointThreshold(GetSlopeFromPoints(start, end), GetYInterceptFromPoints(start, end), 1, 1 / (-GetSlopeFromPoints(start, end)), -GetYInterceptFromPoints(start, end) / (-GetSlopeFromPoints(start, end)), start, end);

        /// <summary>
        /// Creates the threshold by the yIntercept
        /// Slope is zero in case of a horizontal threshold -the default case-)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="slope"></param>
        /// <param name="yIntercept"></param>
        /// <returns></returns>
        public static new PointThreshold CreateBySlope(double slope, double yIntercept) => new PointThreshold(0, yIntercept, 0, 1, -yIntercept);


        //Filling up constructors of the Line.

        private PointThreshold(double slope, double yInter, double xConst, double yConst, double Const, Point2D start, Point2D end) : base(slope, yInter, xConst, yConst, Const, start, end)
        { }

        private PointThreshold(double slope, double yInter, double xConst, double yConst, double Const) : base(slope, yInter, xConst, yConst, Const)
        { }

    }
}
