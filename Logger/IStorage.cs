namespace Logger
{
    internal interface IStorage
    {
        void Write(string key, object value);

        /// <summary>
        /// Read an item from storage
        /// </summary>
        /// <exception cref="StorageKeyNotFoundException">Item not found in Storage for provided <paramref name="key"/></exception>
        /// <param name="key">Key for looked up item</param>
        /// <returns>Value of found item</returns>
        object Read(string key);

        void Clear();
    }
}