using System.Threading;
using UnityEngine;

public delegate void InteractionAction(Action action, SetResponse setResponse);

public delegate void SetResponse(Object response);

namespace interactor
{
    /// <summary>
    /// Interactor provides the interface to the internals of the interpreter to call Actions of the game in a
    /// thread safe way, waiting for it to answer.
    /// </summary>
    public static class Interactor
    {
        /// <summary>
        /// InteractorTask is a useful class that we use to wait on the Unity3D thread to respond to the Lua thread with the
        /// action response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class InteractorTask<T>
        {
            protected T data;
            private bool finished;
            private Semaphore waitHandler = new Semaphore(0, 1);

            public bool Finished => finished;

            public T Data => data;

            /// <summary>
            /// Wait until the interaction finishes
            /// </summary>
            /// <returns></returns>
            public T WaitInteraction()
            {
                waitHandler.WaitOne();
                return data;
            }

            /// <summary>
            /// This shouldn't be called by the waiter
            /// </summary>
            /// <param name="data"></param>
            internal void Give(T data)
            {
                this.data = data;
                finished = true;
                waitHandler.Release();
            }
        }

        private static Action _receivedData;

        /// <summary>
        /// Does the desired action with the passed parameters
        /// It will return a task which you have to wait on to receive de response data.
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static InteractorTask<Object> Do(ActionType actionType, Object[] parameters)
        {
            InteractorTask<Object> interactor = new InteractorTask<Object>();
            _receivedData = new Action(actionType, parameters, interactor, SetResponseCb);
            return interactor;
        }

        public static Action GetAction()
        {
            return _receivedData;
        }

        private static void SetResponseCb(Object response)
        {
            _receivedData.Interactor.Give(response);
            _receivedData = null;
        }

    }
}
