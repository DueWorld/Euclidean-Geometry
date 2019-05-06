namespace MathEuclideanPrimitives
{
    using System;

    /// <summary>
    /// A Primitive representation of 2D coordinate tuples in space.
    /// </summary>
    public struct Point2D:IEquatable<Point2D>
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point2D Origin { get => new Point2D(0, 0); }
        public Point2D BasisX { get => new Point2D(1, 0); }
        public Point2D BasisY { get => new Point2D(0, 1); }
        

        /// <summary>
        /// Instantiates a point by its X and Y coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point2D(float x, float y)
        {
            X = x;
            Y = y;
        }


        /// <summary>
        /// Gets the distance between 2 points.
        /// </summary>
        /// <param name="p">The other Point.</param>
        /// <returns>Distance between the 2 points.</returns>
        public float GetDistance (Point2D p)
        {
            float coordDifY = Y - p.Y;
            float coordDifX = X - p.X;
            return (float)Math.Sqrt((Math.Pow(coordDifY, 2)) + (Math.Pow(coordDifX, 2)));
        }


        /// <summary>
        /// Gets the distance between 2 points.
        /// </summary>
        /// <param name="p1">First Point.</param>
        /// <param name="p2">Second Point.</param>
        /// <returns>Distance between the 2 points.</returns>
        public static float GetDistance(Point2D p1, Point2D p2) => p1.GetDistance(p2);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector2D ToVector()
        {
            return new Vector2D(this.X, this.Y);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector2D ToVector(Point2D point) => point.ToVector();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Point2D other)
        {
            return (X == other.X && Y == other.Y);
        }
    }
}
