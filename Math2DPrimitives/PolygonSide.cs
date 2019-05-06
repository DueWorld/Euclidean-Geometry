namespace MathEuclideanPrimitives
{
    /// <summary>
    /// A primitive element of the Polygon representing a side of the polygon.
    /// </summary>
    public sealed class PolygonSide : Line2D
    {
        public bool IsCrossSide { get; set; }


        private PolygonSide(float slope, float yInter, float xConst, float yConst, float Const, Point2D start, Point2D end)
            : base(slope, yInter, xConst, yConst, Const, start, end)
        { }

        private PolygonSide(float slope, float yInter, float xConst, float yConst, float Const)
            : base(slope, yInter, xConst, yConst, Const)
        { }

        /// <summary>
        /// Creates a side by the start and the end point if known, will calculate every other form.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static new PolygonSide CreateByPoints(Point2D start, Point2D end) => new PolygonSide(GetSlopeFromPoints(start, end), GetYInterceptFromPoints(start, end), 1, 1 / (-GetSlopeFromPoints(start, end)), -GetYInterceptFromPoints(start, end) / (-GetSlopeFromPoints(start, end)), start, end);

        /// <summary>
        /// Creates a side using the slope and the y intercept if known, will calculate every other form. 
        /// </summary>
        /// <param name="slope"></param>
        /// <param name="yIntercept"></param>
        /// <returns></returns>
        public static new PolygonSide CreateBySlope(float slope, float yIntercept) => new PolygonSide(slope, yIntercept, -slope, 1, -yIntercept);


    }
}
