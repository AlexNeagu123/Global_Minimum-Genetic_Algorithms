namespace Tema2
{
	public static class MetaGeneticAlgorithm
	{
		public static (double eval, double crossoverProb, double k1)[] EvaluateFunctionPopulation(List<bool[]> population, int mutationCount, int crossoverCount, (BaseFunction function, int dimensions, int digitsOfprecision, int maxT, int populationSize, double k2) geneticAlg)
		{
			var result = new (double eval, double crossoverProb, double k1)[population.Count];
			var k1s = new List<double>();
			var crossovers = new List<double>();

			for (int i = 0; i < population.Count; i++)
			{
				var t = new bool[mutationCount];
				Array.Copy(population[i], 0, t, 0, mutationCount);

				k1s.Add(Conversion.FromBitsToDouble(t, (0.001, 0.01), 1)[0]);

				t = new bool[crossoverCount];
				Array.Copy(population[i], mutationCount, t, 0, crossoverCount);
				crossovers.Add(Conversion.FromBitsToDouble(t, (0.5, 1.0), 1)[0]);
			}

			List<Task> tasks = new List<Task>();

			for (int j = 0; j < result.Length; j++)
			{
				int m = j;
				tasks.Add(Task.Factory.StartNew(() => result[m] = (Eval(geneticAlg.function, geneticAlg.dimensions, geneticAlg.digitsOfprecision, geneticAlg.maxT, geneticAlg.populationSize, crossovers[m], k1s[m], geneticAlg.k2), crossovers[m], k1s[m])));
				//tasks.Add(Task.Factory.StartNew(() => result[m] = (GeneticAlgorithm.GetMin(geneticAlg.function, geneticAlg.dimensions, geneticAlg.digitsOfprecision, geneticAlg.maxT, geneticAlg.populationSize, crossovers[m], k1s[m], geneticAlg.k2), crossovers[m], k1s[m])));
				if ((m + 1) % 6 == 0)
				{
					Task.WaitAll(tasks.ToArray());
					tasks.Clear();
				}
			}

			Task.WaitAll(tasks.ToArray());

			return result;
		}

		public static (double eval, double crossoverProb, double k1) GetBestParameters((int digitsOfprecision, int maxT, int populationSize, double mutateProbability, double crossoverProbability) metaGeneticAlg, (BaseFunction function, int dimensions, int digitsOfprecision, int maxT, int populationSize, double k2) geneticAlg)
		{
			var precision = Math.Pow(10, -metaGeneticAlg.digitsOfprecision);
			int t = 0;

			var k1Population = RandomBits.GetRandomPopulation((0.001, 0.01), 1, 0.001, metaGeneticAlg.populationSize);
			var crossoverPopulation = RandomBits.GetRandomPopulation((0.5, 1.0), 1, 0.01, metaGeneticAlg.populationSize);

			var population = new List<bool[]>();

			for (int i = 0; i < metaGeneticAlg.populationSize; ++i)
			{
				bool[] mc = new bool[k1Population[0].Length + crossoverPopulation[0].Length];
				k1Population[i].CopyTo(mc, 0);
				crossoverPopulation[i].CopyTo(mc, k1Population[0].Length);

				population.Add(mc);
			}

			var eval = EvaluateFunctionPopulation(population, k1Population[0].Length, crossoverPopulation[0].Length, geneticAlg);

			while (t < metaGeneticAlg.maxT)
			{
				population = Select(population, metaGeneticAlg.populationSize, eval);
				Mutation.MutatePopulation(population, metaGeneticAlg.mutateProbability);
				Crossover.CrossoverPopulation(population, metaGeneticAlg.crossoverProbability);
				eval = EvaluateFunctionPopulation(population, k1Population[0].Length, crossoverPopulation[0].Length, geneticAlg);
				++t;
			}

			var min = eval.Min(x => x.eval);

			return eval.Where(x => x.eval == min).ToArray()[0];

		}

		private static List<bool[]> Select(List<bool[]> population, int populationSize, (double eval, double mutationProb, double crossoverProb)[] r)
		{
			double[] evalNorm = new double[population.Count];
			List<bool[]> result = new List<bool[]>(populationSize);
			double[] p = new double[population.Count];
			double[] q = new double[population.Count + 1];

			double[] eval = r.Select(x => x.eval).ToArray();


			var l = new List<(bool[] individual, double eval)>(population.Count);
			for (int i = 0; i < population.Count; i++)
				l.Add((population[i], eval[i]));

			l = l.OrderByDescending(x => x.eval).ToList();

			int ii = 0;
			for (ii = 0; ii < (populationSize * 5) / 100; ii++)
				result.Add(l[ii].individual);


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

			for (; ii < populationSize; ii++)
			{
				double rand = RandomBits.Random.NextDouble();

				for (int j = 0; j < population.Count; j++)
					if (q[j] < rand && rand <= q[j + 1])
						result.Add(population[j]);
			}

			return result;
		}


		static double Eval(BaseFunction function, int dimensions, int digitsOfprecision, int maxT, int populationSize, double crossoverProb, double k1, double k2)
		{
			List<Task> tasks = new List<Task>();
			var values = new List<double>();

			for (int iterations = 0; iterations < 1; ++iterations)
				values.Add(GeneticAlgorithm.GetMin(function, dimensions, digitsOfprecision, maxT, populationSize, crossoverProb, k1, k2));

			Task.WaitAll(tasks.ToArray());
			return values.Average();
		}
	}
}
