
using System;

class Program
{
	static void Main(string[] args)
	{
		// Delegar la ejecución a ProgramLogic para mantener Main delgada.
		ProgramLogic.Run(args);
	}
}
