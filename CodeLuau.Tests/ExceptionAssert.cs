using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CodeLuau.Tests
{
	public static class ExceptionAssert
	{
		public static T Throws<T>(Action action) where T : Exception
		{
			try
			{
				action();
			}
			catch (T ex)
			{
				return ex;
			}

			Assert.Fail("Expected exception of type {0}.", typeof(T));

			return null;
		}
	}
}
