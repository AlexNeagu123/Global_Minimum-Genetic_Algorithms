using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2
{
	public static class Mutation
	{
		private static void Mutate(bool[] chromosome, double mutateProbability)
		{
			for(int i = 0; i < chromosome.Length; i++)
				if(RandomBits.Random.NextDouble() <= mutateProbability)
					chromosome[i] = !chromosome[i];
		}

		public static void MutatePopulation(List<bool[]> population, double mutateProbability)
		{
			for(var i = 0; i < population.Count; i++)
				Mutate(population[i], mutateProbability);
		}

		public static void MutatePopulation(List<bool[]> population, List<double> mutateProbability)
		{
			for (var i = 0; i < population.Count; i++)
				Mutate(population[i], mutateProbability[i]);
		}


		public static double CalculateMutationProb(double max, double average, double eval, double k1, double k2)
		{
			if (eval >= average)
				return  Math.Max(k1 * (max - eval) / (max - average), 0.0008);
			return k2;
		}
	}
}
