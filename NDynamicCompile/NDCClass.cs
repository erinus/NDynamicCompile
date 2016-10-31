using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using Microsoft.CSharp.RuntimeBinder;

namespace NDynamicCompile
{
	public class NDCClass : DynamicObject, IDictionary<string, object>
	{
		//
		private Dictionary<string, object> _Dict = new Dictionary<string, object>();

		//
		private Type _Type;

		//
		public ICollection<string> Keys
		{
			get
			{
				//
				return ((IDictionary<string, object>)_Dict).Keys;
			}
		}

		//
		public ICollection<object> Values
		{
			get
			{
				//
				return ((IDictionary<string, object>)_Dict).Values;
			}
		}

		//
		public int Count
		{
			get
			{
				//
				return ((IDictionary<string, object>)_Dict).Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				//
				return ((IDictionary<string, object>)_Dict).IsReadOnly;
			}
		}

		//
		public object this[string key]
		{
			get
			{
				//
				return ((IDictionary<string, object>)_Dict)[key];
			}

			set
			{
				//
				((IDictionary<string, object>)_Dict)[key] = value;
			}
		}

		//
		public string Name
		{
			get
			{
				//
				return this._Type.Name;
			}
		}

		//
		public string FullName
		{
			get
			{
				//
				return this._Type.FullName;
			}
		}

		//
		public string Namespace
		{
			get
			{
				//
				return this._Type.Namespace;
			}
		}

		public NDCClass(Type type)
		{
			//
			this._Type = type;
		}

		public bool ContainsKey(string key)
		{
			//
			return ((IDictionary<string, object>)_Dict).ContainsKey(key);
		}

		public void Add(string key, object value)
		{
			//
			((IDictionary<string, object>)_Dict).Add(key, value);
		}

		public bool Remove(string key)
		{
			//
			return ((IDictionary<string, object>)_Dict).Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			//
			return ((IDictionary<string, object>)_Dict).TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<string, object> item)
		{
			//
			((IDictionary<string, object>)_Dict).Add(item);
		}

		public void Clear()
		{
			//
			((IDictionary<string, object>)_Dict).Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			//
			return ((IDictionary<string, object>)_Dict).Contains(item);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			//
			((IDictionary<string, object>)_Dict).CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			//
			return ((IDictionary<string, object>)_Dict).Remove(item);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			//
			return ((IDictionary<string, object>)_Dict).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//
			return ((IDictionary<string, object>)_Dict).GetEnumerator();
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			//
			return this._Dict.TryGetValue(binder.Name, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			//
			this._Dict[binder.Name] = value;
			//
			return true;
		}

		public dynamic New(params object[] args)
		{
			//
			dynamic result = new ExpandoObject();

			//
			IDictionary<string, object> dict = result as IDictionary<string, object>;

			//
			dynamic inst = Activator.CreateInstance(this._Type, args);

			//
			this._BindFields(inst, dict);

			//
			this._BindMethods(inst, dict);

			//
			return result;
		}

		private void _BindFields(dynamic inst, IDictionary<string, object> dict)
		{
			//
			foreach (FieldInfo info in this._Type.GetFields())
			{
				if (info.IsPublic && !info.IsStatic)
				{
					//
					dict[info.Name] = info.GetValue(inst);
				}
			}
		}

		private void _BindMethods(dynamic inst, IDictionary<string, object> dict)
		{
			//
			foreach (MethodInfo info in this._Type.GetMethods())
			{
				//Console.WriteLine(string.Format("{0} {1}", info.Name, info.ReturnType));

				// inherited from System.Object
				if (info.DeclaringType == typeof(object))
				{
					//
					continue;
				}

				//
				if (info.IsPublic && !info.IsStatic)
				{
					//
					if (info.ReturnType == typeof(void))
					{
						//
						dict[info.Name] = new NDCDelegate.ActionDelegate<object>(args =>
						{
							//
							BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance;
							//
							MemberInfo[] infos = this._Type.GetMember(info.Name, flags);
							//
							if (infos != null && infos.Length > 0)
							{
								//
								MethodInfo _info = infos[0] as MethodInfo;
								//
								_info.Invoke(inst, args);
							}
						});
					}
					else
					{
						//
						dict[info.Name] = new NDCDelegate.FuncDelegate<dynamic, object>(args =>
						{
							//
							MemberInfo[] infos = this._Type.GetMember(info.Name, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance);
							//
							if (infos != null && infos.Length > 0)
							{
								//
								MethodInfo _info = infos[0] as MethodInfo;
								//
								return _info.Invoke(inst, args);
							}
							//
							return null;
						});
					}
				}
			}
		}
	}
}
