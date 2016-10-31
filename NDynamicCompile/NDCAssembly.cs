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

		public NDCAssembly(Assembly assembly)
		{
			//
			this._Assembly = assembly;
		}

		public List<NDCClass> Types()
		{
			//
			List<NDCClass> result = new List<NDCClass>();

			//
			foreach (Type type in this._Assembly.GetTypes())
			{
				//
				result.Add(new NDCClass(type));
			}

			//
			return result;
		}
	}
}
