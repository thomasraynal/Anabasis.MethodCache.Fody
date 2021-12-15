using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MemoryCache.Test
{
	public static class TestHelpers
	{
		public static dynamic CreateInstance<T>(Assembly assembly, object parameter)
		{
			if (null == parameter)
				return Activator.CreateInstance(CreateType<T>(assembly));

			return Activator.CreateInstance(CreateType<T>(assembly), parameter);
		}

		private static Type CreateType<T>(Assembly assembly)
		{
			return assembly.GetType(typeof(T).FullName ?? string.Empty);
		}
	}
}
