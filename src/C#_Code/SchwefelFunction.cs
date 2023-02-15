using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2
{
	public class SchwefelFunction : BaseFunction
	{
		public SchwefelFunction() : base((-500, 500))
		{
		}

		protected override double EvaluateFunction(double[] x)
		{
			/*
				d = length(xx);
				sum = 0;
				for ii = 1:d
					xi = xx(ii);
					sum = sum + xi*sin(sqrt(abs(xi)));
				end

				y = 418.9829*d - sum;

				end
			 */

			double sum = 0;

			for(int i = 0; i < x.Length; i++)
				sum += x[i] * Math.Sin(Math.Sqrt(Math.Abs(x[i])));

			return 418.9829 * x.Length - sum;
		}

		public override string ToString()
		{
			return "Schwefel Function";
		}
	}
}
