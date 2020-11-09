

using System.Collections.Generic;
using UnityEngine;

namespace Stdlib
{
    static class Stdlib
    {

        public static Context GetStandardLibrary()
        {
            Context dict = new Context();
            dict.Set("append", new StdLibFunc(Append));
            dict.Set("print", new StdLibFunc(Print)); 
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

        /// <summary>
        /// Prints into `Stdout`
        /// </summary>
        /// <use>
        /// print(elements...)
        /// </use>
        /// <accepted>
        /// ANY
        /// </accepted>
        /// <name>
        ///  print
        /// </name>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static Object Print(Object[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                Debug.Log(parameters[i]);
            }
            return Null.NULL;
        }
    }

}
 
