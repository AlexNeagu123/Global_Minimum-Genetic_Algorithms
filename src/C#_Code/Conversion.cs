using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2
{
	public static class Conversion
	{
		public static double[] FromBitsToDouble(bool[] X_bits, (double a, double b) functionDomain, int dimensions)
		{
			double[] result = new double[dimensions];
			var dimensionBitsCount = X_bits.Length / dimensions;

			for (int i = 0; i < dimensions; i++)
			{
				var X_int = 0ul;

				for (int j = 0; j < dimensionBitsCount; j++)
				{
					X_int *= 2;
					X_int += X_bits[i * dimensionBitsCount + j] ? 1ul : 0ul;
				}

				result[i] = (X_int / (Math.Pow(2, dimensionBitsCount) - 1)) * (functionDomain.b - functionDomain.a) + functionDomain.a;
			}

			return result;
		}

		public static bool[] FromGrayToBinary(bool[] gray)
		{

			var binary = new List<bool>(gray.Length);

			binary.Add(gray[0]);

			for (int i = 1; i < gray.Length; i++)
			{
				if (gray[i] == false)
					binary.Add(binary[i - 1]);
				else
					binary.Add(!binary[i - 1]);
			}

			return binary.ToArray();
		}

		public static double[] FromGrayToDouble(bool[] gray, (double a, double b) functionDomain, int dimensions)
		{
			var result = new List<double>(dimensions);

			for (int i = 0; i < dimensions; ++i)
			{
				int size = gray.Length / dimensions;
				var subArray = new List<bool>(size);

				for (int j = 0; j < size; ++j)
					subArray.Add(gray[i * size + j]);


				result.Add(FromBitsToDouble(FromGrayToBinary(subArray.ToArray()), functionDomain, 1)[0]);
			}
			
			return result.ToArray();
		}
	}
}
