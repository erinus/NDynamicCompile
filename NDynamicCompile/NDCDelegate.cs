using System;

namespace NDynamicCompile
{
	public class NDCDelegate
	{
		//
		public delegate void ActionDelegate<P>(params P[] args);

		//
		public delegate R FuncDelegate<P, R>(params P[] args);
	}
}
