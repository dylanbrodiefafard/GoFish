using System;

namespace GoFishCommon
{
    /// <summary>
    /// Class used to extend EventArgs and store any kind of object we need to.
    /// </summary>
    /// <typeparam name="T">The type of object to store</typeparam>
    public class GenericEventArgs<T> : EventArgs
    {
        private readonly T data;
        public GenericEventArgs(T dataIn)
        {
            this.data = dataIn;
        }

        public T GetInfo()
        {
            return data;
        }
    }
}
