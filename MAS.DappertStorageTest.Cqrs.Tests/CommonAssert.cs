namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommonAssert
    {
        public static void Collections<T>(IEnumerable<T> expectedCollection, IEnumerable<T> actualCollection, Action<T, T> assertFunction)
        {
            if (expectedCollection == null)
            {
                throw new ArgumentNullException(nameof(expectedCollection));
            }
            if (actualCollection == null)
            {
                throw new ArgumentNullException(nameof(actualCollection));
            }
            if (assertFunction == null)
            {
                throw new ArgumentNullException(nameof(assertFunction));
            }

            var expectedCollectionAsArray = expectedCollection.ToArray();
            var actualCollectionAsArray = actualCollection.ToArray();

            if (expectedCollectionAsArray.Length != actualCollectionAsArray.Length)
            {
                throw new ArgumentException("Collections must be same size");
            }

            for (var i = 0; i < expectedCollectionAsArray.Length; i++)
            {
                assertFunction(expectedCollectionAsArray[i], actualCollectionAsArray[i]);
            }
        }
    }
}
