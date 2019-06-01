namespace MathEuclideanPrimitives.Utilities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// General Euclidean Geometry utilities for coordinate geometry.
    /// </summary>
    public static partial class GeometricUtilities
    {
        /// <summary>
        /// A fudge leftover of equating a double point space.
        /// </summary>
        private const double fudge = 0.001d;

        /// <summary>
        /// A helper function to determine if some numbers are in the range of 2 extremes numbers.
        /// </summary>
        /// <param name="extreme1"></param>
        /// <param name="extreme2"></param>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static bool AreNumbersInRange(double extreme1, double extreme2, params double[] numbers)
        {
            var array = new List<double> { extreme1, extreme2 };
            array.AddRange(numbers);
            array.Sort();
            if (array[0] == extreme1 && array[array.Count - 1] == extreme2 ||
                array[0] == extreme2 && array[array.Count - 1] == extreme1)
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Converts an angle from radians to degree.
        /// </summary>
        /// <param name="angleInRad"></param>
        /// <returns></returns>
        public static double ToDegree(double angleInRad)
        {
            return (angleInRad * 180 / Math.PI);
        }

        /// <summary>
        /// Converts an angle to radians from degree.
        /// </summary>
        /// <param name="angleInDeg"></param>
        /// <returns></returns>
        public static double ToRad(double angleInDeg)
        {
            return (angleInDeg * Math.PI / 180);

        }
    }
}
