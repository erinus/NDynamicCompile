using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;

using Microsoft.CSharp.RuntimeBinder;

namespace NDynamicCompile
{
	public class NDCAssembly
	{
		//
		private Assembly _Assembly;

		//
		public List<NDCClass> Classes;

		public NDCAssembly(Assembly assembly)
		{
			//
			this._Assembly = assembly;

			//
			this.Classes = new List<NDCClass>();

			//
			foreach (Type _type in this._Assembly.GetTypes())
			{
				//
				dynamic _class = new NDCClass(_type);

				//
				foreach (FieldInfo info in _type.GetFields())
				{
					//Console.WriteLine(info.Name);

					//
					if (info.IsPublic && info.IsStatic)
					{
						//
						_class[info.Name] = info.GetValue(null);
					}
				}

				//
				foreach (MethodInfo info in _type.GetMethods())
				{
					//Console.WriteLine(info.Name);

					//
					if (info.IsPublic && info.IsStatic)
					{
						//
						if (info.ReturnType == typeof(void))
						{
							//
							_class[info.Name] = new NDCDelegate.ActionDelegate<object>(args =>
							{
								//
								BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static;
								//
								MemberInfo[] infos = _type.GetMember(info.Name, flags);
								//
								if (infos != null && infos.Length > 0)
								{
									//
									MethodInfo _info = infos[0] as MethodInfo;
									//
									_info.Invoke(null, args);
								}
							});
						}
						else
						{
							//
							_class[info.Name] = new NDCDelegate.FuncDelegate<dynamic, object>(args =>
							{
								//
								MemberInfo[] infos = _type.GetMember(info.Name, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static);
								//
								if (infos != null && infos.Length > 0)
								{
									//
									MethodInfo _info = infos[0] as MethodInfo;
									//
									return _info.Invoke(null, args);
								}
								//
								return null;
							});
						}
					}
				}

				//
				this.Classes.Add(_class);
			}
		}

		public NDCClass Class(string name)
		{
			//
			NDCClass result = null;

			//
			foreach (NDCClass _class in this.Classes)
			{
				//
				if (name.Equals(_class.Name))
				{
					//
					result = _class;
					//
					break;
				}
			}

			//
			return result;
		}
	}
}
