using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2
{
	public static class Selection
	{
		public static (List<bool[]> population, List<double> mutationProb) Select(List<bool[]> population, int populationSize, double[] eval, double k1, double k2)
		{
			double[] evalNorm = new double[population.Count];
			List<bool[]> newPopulation = new List<bool[]>(populationSize);
			List<double> mutationProb = new List<double>(populationSize);
			double[] p = new double[population.Count];
			double[] q = new double[population.Count + 1];


			double max = eval.Max();
			double min = eval.Min();

			for(int i = 0; i < population.Count; i++)
				evalNorm[i] = (max - eval[i]) / (max - min + 0.000001) + 0.01;


			double T = 0;
			for (int i = 0; i < population.Count; i++)
				T += evalNorm[i];


			for (int i = 0; i < population.Count; i++)
				p[i] = evalNorm[i] / T;

			q[0] = 0;
			for (int i = 0; i < population.Count; i++)
				q[i + 1] = q[i] + p[i];

			q[^1] = 1;


			double maxNorm = evalNorm.Max();
			double avgNorm = evalNorm.Average();


			var l = new List<(bool[] individual, double evalNorm)>(population.Count);
			for (int i = 0; i < population.Count; i++)
				l.Add((population[i], evalNorm[i]));

			l = l.OrderByDescending(x => x.evalNorm).ToList();

			int ii = 0;
			for (ii = 0; ii < (populationSize * 5) / 100; ii++)
			{
				newPopulation.Add(l[ii].individual);
				mutationProb.Add(Mutation.CalculateMutationProb(maxNorm, avgNorm, l[ii].evalNorm, k1, k2));
			}

			for (; ii < populationSize; ii++)
			{
				double rand = RandomBits.Random.NextDouble();

				for (int j = 0; j < population.Count; j++)
					if (q[j] < rand && rand <= q[j + 1])
					{
						newPopulation.Add(population[j]);
						mutationProb.Add(Mutation.CalculateMutationProb(maxNorm, avgNorm, evalNorm[j], k1, k2));
					}
			}

			return (newPopulation, mutationProb);
		}



		public static List<bool[]> Select(List<bool[]> population, int populationSize, double[] eval)
		{
			double[] evalNorm = new double[population.Count];
			List<bool[]> newPopulation = new List<bool[]>(populationSize);
			double[] p = new double[population.Count];
			double[] q = new double[population.Count + 1];


			double max = eval.Max();
			double min = eval.Min();

			for (int i = 0; i < population.Count; i++)
				evalNorm[i] = (max - eval[i]) / (max - min + 0.000001) + 0.01;


			double T = 0;
			for (int i = 0; i < population.Count; i++)
				T += evalNorm[i];


			for (int i = 0; i < population.Count; i++)
				p[i] = evalNorm[i] / T;

			q[0] = 0;
			for (int i = 0; i < population.Count; i++)
				q[i + 1] = q[i] + p[i];

			q[^1] = 1;


			//double maxNorm = evalNorm.Max();
			//double avgNorm = evalNorm.Average();


			//var l = new List<(bool[] individual, double evalNorm)>(population.Count);
			//for (int i = 0; i < population.Count; i++)
			//	l.Add((population[i], evalNorm[i]));

			//l = l.OrderByDescending(x => x.evalNorm).ToList();

			int ii = 0;
			//for (ii = 0; ii < (populationSize * 5) / 100; ii++)
			//	newPopulation.Add(l[ii].individual);


			for (; ii < populationSize; ii++)
			{
				double rand = RandomBits.Random.NextDouble();

				for (int j = 0; j < population.Count; j++)
					if (q[j] < rand && rand <= q[j + 1])
						newPopulation.Add(population[j]);

			}

			return newPopulation;
		}
	}
}
