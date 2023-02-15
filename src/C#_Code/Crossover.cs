using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2
{
	public static class Crossover
	{
		private static (bool[], bool[]) CrossoverParents((bool[] p1, bool[] p2) parents)
		{
			(bool[] d1, bool[] d2) descendants = (new bool[parents.p1.Length], new bool[parents.p2.Length]);
			parents.p1.CopyTo(descendants.d1, 0);
			parents.p2.CopyTo(descendants.d2, 0);

			int position = 1 + RandomBits.Random.Next(0, parents.p1.Length - 3);

			for(int i = position; i < parents.p1.Length; ++i)
			{
				descendants.d1[i] = parents.p2[i];
				descendants.d2[i] = parents.p1[i];
			}

			return descendants;
		}

		public static void CrossoverPopulation(List<bool[]> population, double crossoverProb)
		{
			var list = new List<(bool[] chromosome, double prob)>(population.Count);

			for (int i = 0; i < population.Count; ++i)
				list.Add((population[i], RandomBits.Random.NextDouble()));

			list.Sort((a, b) => a.prob.CompareTo(b.prob));


			for (int i = 0; i < (list.Count - 1); i += 2)
			{
				if (list[i].prob >= crossoverProb)
					break;

				if (list[i + 1].prob >= crossoverProb)
					if (RandomBits.Random.NextDouble() >= 0.5)
						break;


				var descendants = CrossoverParents((list[i].chromosome, list[i+1].chromosome));
				population.Add(descendants.Item1);
				population.Add(descendants.Item2);
			}
		}
	}
}
