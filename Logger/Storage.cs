namespace Logger
{
    using System;
    using System.Collections.Generic;

    internal class Storage : IStorage
    {
        private Dictionary<string, object> items;

        public Storage()
        {
            this.items = new Dictionary<string, object>();
        }

        private Dictionary<string, object> Items
        {
            get
            {
                this.items = this.items ?? new Dictionary<string, object>();
                return this.items;
            }
        }

        public void Write(string key, object value)
        {
            if (this.Items.ContainsKey(key))
            {
                this.Items[key] = value;
            }
            else
            {
                this.Items.Add(key, value);
            }
        }

        public object Read(string key)
        {
            try
            {
                return this.Items[key];
            }
            catch (Exception ex)
            {
                throw new StorageKeyNotFoundException($"Storage item for key '{key}' not found", ex);
            }
        }

        public void Clear()
        {
            this.Items.Clear();
        }
    }
}