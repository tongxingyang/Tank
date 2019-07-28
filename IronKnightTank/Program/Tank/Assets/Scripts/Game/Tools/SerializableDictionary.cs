namespace Game.Tools
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> _keys = new List<TKey>();
        [SerializeField]
        private List<TValue> _values = new List<TValue>();

        public void OnBeforeSerialize()
        {
            this._keys.Clear();
            this._values.Clear();
            this._keys.Capacity = this.Count;
            this._values.Capacity = this.Count;
            foreach (var kvp in this)
            {
                this._keys.Add(kvp.Key);
                this._values.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            // Debug.Log("OnAfterDeserialize");
            this.Clear();
            int count = Mathf.Min(this._keys.Count, this._values.Count);
            for (int i = 0; i < count; ++i)
            {
                this.Add(this._keys[i], this._values[i]);
            }
        }
    }
}