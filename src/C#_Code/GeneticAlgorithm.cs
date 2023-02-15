using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2
{
	public static class GeneticAlgorithm
	{
		public static double GetMin(BaseFunction function, int dimensions, int digitsOfprecision, int maxT, int populationSize, double crossoverProbability, double k1, double k2)
		{
			var precision = Math.Pow(10, -digitsOfprecision);
			int t = 0;

			List<double> mutationProb;
			var population = RandomBits.GetRandomPopulation(function.SearchDomain, dimensions, precision, populationSize);

			var eval = function.EvaluateFunctionPopulation(population, dimensions);

			while(t < maxT)
			{
				(population, mutationProb) = Selection.Select(population, populationSize, eval, k1, k2);
				Mutation.MutatePopulation(population, mutationProb);
				Crossover.CrossoverPopulation(population, crossoverProbability);
				eval = function.EvaluateFunctionPopulation(population, dimensions);
				++t;
			}

			return eval.Min();

		}

		public static double GetMin(BaseFunction function, int dimensions, int digitsOfprecision, int maxT, int populationSize, double mutationProbability, double crossoverProbability)
		{
			var precision = Math.Pow(10, -digitsOfprecision);
			int t = 0;

			var population = RandomBits.GetRandomPopulation(function.SearchDomain, dimensions, precision, populationSize);

			var eval = function.EvaluateFunctionPopulation(population, dimensions);
			
			while (t < maxT)
			{
				population = Selection.Select(population, populationSize, eval);
				Mutation.MutatePopulation(population, mutationProbability);
				Crossover.CrossoverPopulation(population, crossoverProbability);
				eval = function.EvaluateFunctionPopulation(population, dimensions);
				++t;
			}

			return eval.Min();

		}
	}
}
