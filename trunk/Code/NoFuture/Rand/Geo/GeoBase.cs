using System;

namespace NoFuture.Rand.Geo
{
    /// <summary>
    /// Base type to wrap access to the underlying address data
    /// </summary>
    [Serializable]
    public abstract class GeoBase
    {
        private readonly AddressData _data;

        protected GeoBase(AddressData d)
        {
            _data = d;
        }

        public AddressData GetData()
        {
            return _data ?? new AddressData();
        }
    }
}
