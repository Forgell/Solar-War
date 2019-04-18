using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Threading_Example
{
	class Program
	{
		static int num1 = 0;
		static int num2 = 0;
		static void Main(string[] args)
		{
			Thread thread_one = new Thread(methoud_one);
			Thread thread_two = new Thread(methoud_two);
			thread_one.Start();
			thread_two.Start();
			Console.Read();
			thread_two.Suspend();
			thread_one.Suspend();
			//Console.Read();
			//	thread_one.Start();
			//	thread_two.Start();

		}

		private static void methoud_one()
		{
			Console.WriteLine("Methud One: "+ (++num1));
			Thread.Sleep(600);
			methoud_one();
		}

		private static void methoud_two()
		{
			Console.WriteLine("Methud Two: " + (++num1));
			Thread.Sleep(600);
			methoud_two();
		}
	}
}
