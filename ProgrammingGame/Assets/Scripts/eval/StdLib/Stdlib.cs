

using System.Collections.Generic;
using System.Linq;
using interactor;
using UnityEngine;

namespace Stdlib
{
    static class Stdlib
    {

        public static Context GetStandardLibrary()
        {
            
            Context ctx = new Context();
            
            // ctx.Set("append", new StdLibFunc(Append));

            // Prints to the current Stdout (Unity Terminal and Game's terminal)
            ctx.Set("print", new StdLibFunc(Print));
            // Cancel from LUA code
            ctx.Set("cancel", new StdLibFunc(Cancel));
            // Tries to import a library, throws an error when the library doesn't exist
            ctx.Set("import", new StdLibFunc(p => Import(p, ctx)));
            // Tries to convert to number, if fail, returns null
            ctx.Set("number", new StdLibFunc(ToNumber));
            // Tries to convert to string, always succeeds
            ctx.Set("string", new StdLibFunc(ToString));
            ctx.Set("table", new StdLibFunc((_) => new Table()));

            // Declare null constant
            ctx.Set("null", Null.NULL);
          
            
            // Import special libraries (From a problem)
            if (ProblemManager.Instance == null) return ctx;
            Lib[] libs = ProblemManager.Instance.CurrentProblem.Libs;
            if (libs == null) return ctx;
            for (int i = 0; i < libs.Length; i++)
            {
                ctx = libs[i].InjectLibrary(ctx);
            }
            return ctx;
        }
        
       

        private static Object ToString(Object[] parameters)
        {
            if (parameters.Length != 1) return new Error("expected only 1 parameter on string call");
            switch (parameters[0])
            {
                case String s: 
                    return s;
                case Number n:
                    return new String($"{n.value}");
            }
            return new String(parameters[0].ToString());
        }

        private static Object ToNumber(Object[] parameters)
        {
            if (parameters.Length != 1)
            {
                return new Error($"expected only 1 parameter on number conversion");
            }

            if (parameters[0] is String str)
            {
                try
                {
                    return new Number(double.Parse(str.Value));
                }
                catch
                {
                    return Null.NULL;
                }
            }

            if (parameters[0] is Null)
            {
                return new Number(0);
            }
            
            if (parameters[0] is Boolean b)
            {
                return b == Boolean.True ? new Number(1) : new Number(0);
            }

            if (parameters[0] is Number n)
            {
                return n;
            }
            
            return Null.NULL;
        }

        private static Object Import(Object[] parameters, Context context)
        {
            if (parameters.Length < 1) return new Error($"expected 1 or more elements, got {parameters.Length}");
            if (parameters[0] is String arr)
            {
                Lib l = Libraries.Get(arr.Value);
                if (l == null)
                {
                    return new Error($"library {arr.Value} doesn't exist");
                }
                l.InjectLibrary(context);
                return Boolean.True;
            }
            return new Error($"expected string, got: {parameters[0].Type()} on import");
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
            Interactor.Do(ActionType.Print, parameters).WaitInteraction();
            for (int i = 0; i < parameters.Length; i++)
            {
                Debug.Log(parameters[i]);
            }
            return Null.NULL;
        }
        
        /// <summary>
        /// Stops the program from Lua
        /// </summary>
        /// <param name="parameters"></param>
        /// <name>
        /// cancel
        /// </name>
        /// <returns></returns>
        private static Object Cancel(Object[] _)
        {
            Eval.State = Eval.EvalState.CancelledByUser;
            return Null.NULL;
        }
    }

}
 
