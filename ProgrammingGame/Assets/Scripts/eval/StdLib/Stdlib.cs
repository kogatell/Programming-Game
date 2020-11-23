

using System.Collections.Generic;
using interactor;
using UnityEngine;

namespace Stdlib
{
    static class Stdlib
    {

        public static Context GetStandardLibrary()
        {
            Lib[] libs = ProblemManager.Instance.CurrentProblem.Libs;
            Context ctx = new Context();
            ctx.Set("append", new StdLibFunc(Append));
            ctx.Set("print", new StdLibFunc(Print)); 
            ctx.Set("move", new StdLibFunc(Move));
            if (libs == null) return ctx;
            for (int i = 0; i < libs.Length; i++)
            {
                ctx = libs[i].InjectLibrary(ctx);
            }
            return ctx;
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

        /// <summary>
        /// Test action
        /// </summary>
        /// <use>
        /// move({1, 2})
        /// </use>
        /// <accepted>
        /// Array of 2 elements
        /// </accepted>
        /// <name>
        ///  move
        /// </name>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static Object Move(Object[] parameters)
        {
            Interactor.InteractorTask<Object> interactor = Interactor.Do(ActionType.Move, parameters);
            Object response = interactor.WaitInteraction();
            return response;
        }
    }

}
 
