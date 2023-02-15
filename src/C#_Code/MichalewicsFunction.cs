using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2
{
	public class MichalewicsFunction : BaseFunction
	{
		public MichalewicsFunction() : base((0, Math.PI))
		{
		}

		protected override double EvaluateFunction(double[] x)
		{
			double sum = 0;
			int m = 10;

			for (int i = 0; i < x.Length; i++)
			{
				if (x[i] < SearchDomain.min && x[i] > SearchDomain.max)
					throw new ArgumentOutOfRangeException();

				sum += Math.Sin(x[i]) * Math.Pow((Math.Sin((i + 1) * Math.Pow(x[i], 2) / Math.PI)), (2 * m));
			}

			return -sum;
		}

		public override string ToString()
		{
			return "Michalewics Function";
		}
	}
}
