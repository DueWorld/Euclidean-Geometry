namespace MathEuclideanPrimitives
{
    using System.Collections.Generic;

    /// <summary>
    /// A primitive polygon class,
    /// this class will construct its sides in a listed fashion,
    /// preferably to make the points list in an Anti-Clock-Wise format,
    /// this class will have its points in a listed fashion.
    /// </summary>
    public sealed class Polygon
    {
        private List<Point2D> polygonPoints;
        private List<PolygonSide> polygonSides;


        public List<Point2D> PolygonPoints => polygonPoints;
        public List<PolygonSide> PolygonSides => polygonSides;

        /// <summary>
        /// Instantiates a polygon by its respectively added vertices.
        /// </summary>
        /// <param name="polygonList">List of vertices.</param>
        public Polygon(List<Point2D> polygonList)
        {
            polygonPoints = polygonList;
            polygonSides = ConstructSides(polygonPoints);
        }

        /// <summary>
        /// Instantiates a polygon by its respectively added vertices.
        /// </summary>
        /// <param name="points"> Will be added to the list of Vertices of the polygon.</param>
        public Polygon(params Point2D[] points)
            : this(new List<Point2D>(points))
        { }


        /// <summary>
        /// Construct every side of the polygon using the same order of the vertices.
        /// </summary>
        /// <param name="points">List of vertices.</param>
        /// <returns>List of side.</returns>
        private List<PolygonSide> ConstructSides(List<Point2D> points)
        {
            var sides = new List<PolygonSide>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                sides.Add(PolygonSide.CreateByPoints(points[i], points[i + 1]));
            }
            sides.Add(PolygonSide.CreateByPoints(points[points.Count - 1], points[0]));

            return sides;
        }



    }
}
