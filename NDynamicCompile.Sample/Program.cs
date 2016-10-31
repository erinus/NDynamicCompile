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
				"		public static string StaticVersion = \"Static Field: 0.0.0\";" +
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
				"		public static void Test7()" +
				"		{" +
				"			Console.WriteLine(\"Static Method: Test7\");" +
				"		}" +
				"		public static void Test8(string t)" +
				"		{" +
				"			Console.WriteLine(string.Format(\"Static Method: Test8({0})\", t));" +
				"		}" +
				"		public static void Test9(string t1, string t2)" +
				"		{" +
				"			Console.WriteLine(string.Format(\"Static Method: Test9({0}, {1})\", t1, t2));" +
				"		}" +
				"		public static string Test10()" +
				"		{" +
				"			return \"Static Method: Test10() -> Return String\";" +
				"		}" +
				"		public static string Test11(string t)" +
				"		{" +
				"			return string.Format(\"Static Method: Test11({0}) -> Return String\", t);" +
				"		}" +
				"		public static string Test12(string t1, string t2)" +
				"		{" +
				"			return string.Format(\"Static Method: Test12({0}, {1}) -> Return String\", t1, t2);" +
				"		}" +
				"	}" +
				"}"
			);

			foreach (dynamic _class in _assembly.Types())
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

					_class.Test7();

					_class.Test8("p");

					_class.Test9("p1", "p2");

					Console.WriteLine(_class.Test10());

					Console.WriteLine(_class.Test11("p"));

					Console.WriteLine(_class.Test12("p1", "p2"));

					Console.WriteLine(_class.StaticVersion);
				}
			}
		}
	}
}
