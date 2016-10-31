using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NDynamicCompile;

namespace NDynamicCompile.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
			NDCCompiler _compiler = new NDCCompiler();

			_compiler.ReferencedAssemblies.Add(@"System.dll");
			_compiler.ReferencedAssemblies.Add(@"System.Core.dll");
			_compiler.ReferencedAssemblies.Add(@"System.Collections.dll");

			NDCAssembly _assembly = _compiler.Compile(
				"using System;" +
				"namespace Tester" +
				"{" +
				"	class Test" +
				"	{" +
				"		public string Version = \"Field: 1.0.0\";" +
				"		public Test()" +
				"		{" +
				"			Console.WriteLine(\"Constructor: Test\");" +
				"		}" +
				"		public void Test1()" +
				"		{" +
				"			Console.WriteLine(\"Method: Test1\");" +
				"		}" +
				"		public void Test2(string t)" +
				"		{" +
				"			Console.WriteLine(string.Format(\"Method: Test2({0})\", t));" +
				"		}" +
				"		public void Test3(string t1, string t2)" +
				"		{" +
				"			Console.WriteLine(string.Format(\"Method: Test3({0}, {1})\", t1, t2));" +
				"		}" +
				"		public string Test4()" +
				"		{" +
				"			return \"Method: Test4() -> Return String\";" +
				"		}" +
				"		public string Test5(string t)" +
				"		{" +
				"			return string.Format(\"Method: Test5({0}) -> Return String\", t);" +
				"		}" +
				"		public string Test6(string t1, string t2)" +
				"		{" +
				"			return string.Format(\"Method: Test6({0}, {1}) -> Return String\", t1, t2);" +
				"		}" +
				"	}" +
				"}"
			);

			foreach (NDCClass _class in _assembly.Types())
			{
				Console.WriteLine(_class.FullName);

				if ("Test".Equals(_class.Name))
				{
					dynamic test = _class.New();

					test.Test1();

					test.Test2("p");

					test.Test3("p1", "p2");

					Console.WriteLine(test.Test4());

					Console.WriteLine(test.Test5("p"));

					Console.WriteLine(test.Test6("p1", "p2"));

					Console.WriteLine(test.Version);
				}
			}
		}
	}
}
