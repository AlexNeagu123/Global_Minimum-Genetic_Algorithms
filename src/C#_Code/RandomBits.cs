using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2
{
	public static class RandomBits
	{
		public static Random Random = new Random();

		private static bool[] GetRandomBits( (double a, double b) functionDomain, int dimensions, double precision)
		{
			int sizeOfArray = dimensions * (int)Math.Ceiling(Math.Log2((1 / precision) * (functionDomain.b - functionDomain.a)));
			bool[] result = new bool[sizeOfArray];


			for (int i = 0; i < sizeOfArray; i++)
				result[i] = Random.NextDouble() < 0.5;

			return result;
		}

		public static List<bool[]> GetRandomPopulation((double a, double b) functionDomain, int dimensions, double precision, int populationSize)
		{
			List<bool[]> result = new List<bool[]>();

			for (int i = 0; i < populationSize; ++i)
				result.Add(GetRandomBits(functionDomain, dimensions, precision));


			return result;
		}
	}
}
