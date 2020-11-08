

using System.Collections.Generic;

namespace Stdlib
{
    static class Stdlib
    {

        public static Context GetStandardLibrary()
        {
            Context dict = new Context();
            dict.Set("append", new StdLibFunc(Append)); 
            return dict;
        }


        /// <summary>
        /// Append a new element to the collection
        /// </summary>
        /// <use>
        /// `append(collection, elements...)`
        /// </use>
        /// <accepted>
        /// First element = ArrayObject | Table
        /// Second And Beyond = Any
        /// </accepted>
        /// <name>
        ///  append
        /// </name>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static Object Append(Object[] parameters)
        {
            if (parameters.Length < 2) return new Error($"expected 2 or more elements, got {parameters.Length}");
            if (parameters[0] is ArrayObject arr)
            {
                for (int i = 1; i < parameters.Length; i++)
                {
                    arr.Append(parameters[i]);
                }
            }
            return Null.NULL;
        }
    }

}
 
