using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

using Microsoft.CSharp;

namespace NDynamicCompile
{
	public class NDCCompiler
	{
		//
		private CSharpCodeProvider _Provider;

		//
		private CompilerParameters _Parameters;

		//
		public List<string> ReferencedAssemblies;

		//
		public List<string> CompiledResults;

		//
		public NDCCompiler()
		{
			//
			this._Provider = new CSharpCodeProvider();
			//
			this._Parameters = new CompilerParameters();
			//
			this.ReferencedAssemblies = new List<string>();
			//
			this.CompiledResults = new List<string>();
		}

		public NDCAssembly Compile(string code)
		{
			//
			this._Parameters.GenerateInMemory = true;
			//
			this._Parameters.GenerateExecutable = false;
			//
			foreach (string reference in this.ReferencedAssemblies)
			{
				//
				this._Parameters.ReferencedAssemblies.Add(reference);
			}

			//
			NDCAssembly assembly = null;

			//
			CompilerResults results = this._Provider.CompileAssemblyFromSource(this._Parameters, code);

			//
			foreach (CompilerError error in results.Errors)
			{
				//
				Console.WriteLine(error.ErrorText);
			}

			//
			if (results.Errors.Count == 0)
			{
				//
				assembly = new NDCAssembly(results.CompiledAssembly);
			}
			else
			{
				//
				foreach (CompilerError error in results.Errors)
				{
					//
					this.CompiledResults.Add(error.ErrorText);
				}
			}

			//
			return assembly;
		}
	}
}
