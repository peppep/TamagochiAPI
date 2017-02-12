using System;

namespace TamagochiAPI.Client
{
	internal class CommandAttribute : Attribute
	{
		internal CommandAttribute(ConsoleKey key)
		{
			AssociatedKey = key;
		}

		internal ConsoleKey AssociatedKey { get; private set; }
	}
}