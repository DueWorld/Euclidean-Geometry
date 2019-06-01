using MathEuclideanPrimitives.Geometric_Objects;
using MathEuclideanPrimitives.Utilities;

namespace MathEuclideanPrimitives
{
    static class TestProgram
    {
        public static void Main()
        {

            //Circle circle = new Circle(6250, new Point2D(3354.1019662495246, -4974.9371855332083));
            //var point = circle.GetMidArcPoint(new Point2D(-139.75424859375335, 207.28904939722588), new Point2D(2126.3065604231151, 1153.2777618583232));
            //var point2 = circle.GetMidArcPoint( new Point2D(2126.307, 1153.278), new Point2D(4581.9,1153.28));

            //bool test1 = circle.IsPointOn(new Point2D(-139.75424859375335, 207.28904939722588));
            //bool test2 = circle.IsPointOn(new Point2D(2126.3065604231151, 1153.2777618583232));
            //bool test3 = circle.IsPointOn(new Point2D(4581.8, 1153.28));

            Circle circle = new Circle(6500.0000,new Point2D(16.7729, 13.8043));
            var point = new Point2D(-3694.2199, 5350.3324);
            var point2 = circle.OffsetPoint(point, 300);
            var point3 = circle.OffsetPoint(point, -300);

           
        }
    }
}
