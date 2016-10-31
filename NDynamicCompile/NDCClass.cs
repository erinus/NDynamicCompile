using System;
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
	public class NDCClass
	{
		//
		private Type _Type;

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

		//
		public delegate void ActionDelegate<P>(params P[] args);

		//
		public delegate R FuncDelegate<P, R>(params P[] args);

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
				//
				dict[info.Name] = info.GetValue(inst);
			}
		}

		private void _BindMethods(dynamic inst, IDictionary<string, object> dict)
		{
			//
			foreach (MethodInfo info in this._Type.GetMethods())
			{
				//Console.WriteLine(string.Format("{0} {1}", info.Name, info.ReturnType));

				// 
				if (info.DeclaringType == typeof(object))
				{
					//
					continue;
				}

				//
				if (info.ReturnType == typeof(void))
				{
					//
					dict[info.Name] = new ActionDelegate<object>(args =>
					{
						//
						MemberInfo[] infos = this._Type.GetMember(info.Name, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance);
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
					dict[info.Name] = new FuncDelegate<dynamic, object>(args =>
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
