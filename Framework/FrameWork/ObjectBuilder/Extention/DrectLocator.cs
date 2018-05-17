using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;

namespace DrectSoft.FrameWork.ObjectBuilder
{
    /// <summary>
    /// 定位器
    /// 具体请参考微软企业库
    /// </summary>
    public class DrectLocator : ReadWriteLocator
    {
        private Dictionary<object, object> references = new Dictionary<object, object>();

        /// <summary>
        /// Constructor. Creates an root locator.
        /// </summary>
        public DrectLocator()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor. Creates a child locator.
        /// </summary>
        /// <param name="parentLocator">The parent locator.</param>
        public DrectLocator(IReadableLocator parentLocator)
        {
            SetParentLocator(parentLocator);
        }

        /// <summary>
        /// See <see cref="IReadableLocator.Count"/> for more information.
        /// </summary>
        public override int Count
        {
            get { return references.Count; }
        }

        /// <summary>
        /// See <see cref="IReadWriteLocator.Add(object, object)"/> for more information.
        /// </summary>
        public override void Add(object key, object value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            references.Add(key, value);
        }

        /// <summary>
        /// See <see cref="IReadableLocator.Contains(object, SearchMode)"/> for more information.
        /// </summary>
        public override bool Contains(object key, SearchMode options)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (!Enum.IsDefined(typeof(SearchMode), options))
                throw new ArgumentException();

            if (references.ContainsKey(key))
                return true;

            if (options == SearchMode.Up && ParentLocator != null)
                return ParentLocator.Contains(key, options);

            return false;
        }

        /// <summary>
        /// See <see cref="IReadableLocator.Get(object, SearchMode)"/> for more information.
        /// </summary>
        public override object Get(object key, SearchMode options)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (!Enum.IsDefined(typeof(SearchMode), options))
                throw new ArgumentException();

            if (references.ContainsKey(key))
                return references[key];

            if (options == SearchMode.Up && ParentLocator != null)
                return ParentLocator.Get(key, options);

            return null;
        }

        /// <summary>
        /// See <see cref="IReadWriteLocator.Remove(object)"/> for more information.
        /// </summary>
        public override bool Remove(object key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            return references.Remove(key);
        }

        /// <summary>
        /// See <see cref="IEnumerable{T}.GetEnumerator()"/> for more information.
        /// </summary>
        public override IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            return references.GetEnumerator();
        }
    }
}
