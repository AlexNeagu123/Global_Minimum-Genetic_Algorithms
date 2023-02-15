using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2
{
	public class Record
	{
		public int Id { get; set; }
		public string FunctionName { get; set; }
		public int Dimensions { get; set; }
		public int DigitsOfPrecision { get; set; }
		public int NrIterations { get; set; }
		public double MutateProb { get; set; }
		public double CrossoverProb { get; set; }
		public double MeanValue { get; set; }
		public double SDValue { get; set; }
		public double MeanTime { get; set; }
		public double SDTime { get; set; }
	}
}
