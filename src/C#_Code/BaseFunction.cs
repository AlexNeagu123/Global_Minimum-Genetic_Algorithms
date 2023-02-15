using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2
{
	public abstract class BaseFunction
	{
		public (double min, double max) SearchDomain { get; init; }

		public BaseFunction((double, double) searchDomain)
		{
			SearchDomain = searchDomain;
		}

		protected abstract double EvaluateFunction(double[] x);

		public double EvaluateFunctionBits(bool[] x, int dimensions)
		{
			//var point = Conversion.FromGrayToDouble(x, SearchDomain, dimensions);//////
			var point = Conversion.FromBitsToDouble(x, SearchDomain, dimensions);
			return EvaluateFunction(point);
		}

		public double[] EvaluateFunctionPopulation(List<bool[]> population, int dimensions)
		{
			double[] result = new double[population.Count];

			for(int i = 0; i < population.Count; i++)
				result[i] = EvaluateFunctionBits(population[i], dimensions);

			return result;
		}

	}
}
