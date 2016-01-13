using NUnitLite;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NGettext.Tests
{
	public class Program
	{
		// http://www.alteridem.net/2015/11/04/testing-net-core-using-nunit-3/
		public static int Main(string[] args)
		{
#if DNXCORE50

			return new AutoRun().Execute(typeof(Program).GetTypeInfo().Assembly, Console.Out, Console.In, args);
#else
			return new AutoRun().Execute(args);
#endif
		}
	}
}
