using System;
using System.Collections;

namespace Cubed.Collections
{
	/// <summary>
	/// See GenericHashlist.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Hashlist<T> : IEnumerable<T> where T : class
	{
		private GenericHashlist hashlist = new GenericHashlist();

		public int Count { get { return hashlist.Count; } }

		public void Add(T oKey) => hashlist.Add(oKey, null);

		//

		public void Remove(T oKey) => hashlist.Remove(oKey);
		public void Remove(int oKey) => hashlist.Remove(oKey);
		public void Clear() => hashlist.Clear();
		
		// 

		public bool Contains(T oKey) => hashlist.Contains(oKey);
		public bool ContainsKey(T oKey) => hashlist.ContainsKey(oKey);

		//

		IEnumerator<T> IEnumerable<T>.GetEnumerator() { return (IEnumerator<T>) hashlist.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return hashlist.GetEnumerator(); }

		//

		public T this[int Index] { get { return (T) hashlist[Index]; } }
	}


	public class GenericHashlist : ICollection, IDictionary, IEnumerable
	{
		// ArrayList that contains all the keys as they are inserted, the index is
		// associated with a key so when pulling out values by index we can get the
		// key for the index, pull from the hashtable the proper value with the
		// corresponding key
 
		// This is basically the same as a sorted list but does not sort the items,
		// rather it leaves them in the order they were inserted
		
		protected ArrayList m_oKeys = new ArrayList();
		protected Hashtable m_oValues = new Hashtable();

		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		//ICollection implementation

		public int Count { get { return m_oValues.Count; } }
		public bool IsSynchronized { get { return m_oValues.IsSynchronized; } }
		public object SyncRoot { get { return m_oValues.SyncRoot; } }

		public void CopyTo(System.Array oArray, int iArrayIndex)
		{
			m_oValues.CopyTo(oArray, iArrayIndex);
		}


		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// IDictionary implementation

		public bool IsFixedSize { get { return m_oKeys.IsFixedSize; } }
		public bool IsReadOnly { get { return m_oKeys.IsReadOnly; } }
		public ICollection Keys { get { return m_oValues.Keys; } }
		public ICollection Values { get { return m_oValues.Values; } }

		public void Add(object oKey, object oValue)
		{
			m_oKeys.Add(oKey);
			m_oValues.Add(oKey, oValue);
		}

		public void Clear()
		{
			m_oValues.Clear();
			m_oKeys.Clear();
		}

		public bool Contains(object oKey)
		{
			return m_oValues.Contains(oKey);
		}

		public bool ContainsKey(object oKey)
		{
			return m_oValues.ContainsKey(oKey);
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return m_oValues.GetEnumerator();
		}

		public void Remove(object oKey)
		{
			m_oValues.Remove(oKey);
			m_oKeys.Remove(oKey);
		}


		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// IEnumerable implementation
		IEnumerator IEnumerable.GetEnumerator() { return m_oValues.GetEnumerator(); }




		// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		// Specialized indexer routines

		public object this[object oKey]
		{
			get { return m_oValues[oKey]; }
			set { m_oValues[oKey] = value; }
		}

		public object this[string Key] { get { return m_oValues[Key]; } }
		public object this[int Index] { get { return m_oValues[m_oKeys[Index]]; } }

	}
}
