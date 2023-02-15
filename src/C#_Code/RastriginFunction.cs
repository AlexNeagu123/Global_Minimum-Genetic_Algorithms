using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2
{
	public class RastriginFunction : BaseFunction
	{
		public RastriginFunction() : base((-5.12, 5.12))
		{
		}

		protected override double EvaluateFunction(double[] x)
		{
			double sum = 0;
			for (int i = 0; i < x.Length; i++)
			{
				if (x[i] < SearchDomain.min && x[i] > SearchDomain.max)
					throw new ArgumentOutOfRangeException();

				sum += (Math.Pow(x[i], 2) - 10 * Math.Cos(2 * Math.PI * x[i]));
			}

			return 10 * x.Length + sum;
		}

		public override string ToString()
		{
			return "Rastrigin Function";
		}
	}
}
