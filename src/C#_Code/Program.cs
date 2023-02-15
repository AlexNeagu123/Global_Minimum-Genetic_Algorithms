using Tema2;
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

class Program
{
	static double meanValue = 0;
	static List<double> values = new List<double>();
	static object sumlock = new object();

	static double meanTimestamp = 0;
	static List<long> Timestamps = new List<long>();

	static double mutateProbability = 0.0001;
	static double crossoverProbability = 0.65;
	static int dimensions = 5;
	static BaseFunction s_function = new RastriginFunction();
	static readonly int digitsOfprecision = 5;
	static double k1 = 0;
	static double k2 = 0.1;

	static SqliteConnection connection = new SqliteConnection("Data Source=C:\\Users\\crist\\OneDrive\\Desktop\\FII\\ANUL2\\Anul2, Sem1\\Alg genetici\\Teme\\Tema2\\Tema2\\bin\\Debug\\net6.0\\date.db");

	static List<BaseFunction> functions = new List<BaseFunction>(){
												 new MichalewicsFunction(),
												 new SphereFunction(),
												 new RastriginFunction(),
												 new SchwefelFunction()
												 };

	static void Main()
	{

		foreach (var function in functions)
		{
			for (int j = 0; j < 1; j++)
			{


				List<(string function_name, double crossoverProb, double k1)> best = new() { (function.ToString(), 1.0, 0.0058) };

				Main3(best);

			}
		}

		//List<(string function_name, double mutateProb, double crossoverProb)> best = GetFromDB();

		//foreach (var value in best)
		//	Console.WriteLine(value.function_name + " " + value.mutateProb + " " + value.crossoverProb);

		//Main4(best);

	}


	static List<(string function_name, double mutateProb, double crossoverProb)> GetFromDB()
	{
		var list = new List<(string function_name, double mutateProb, double crossoverProb)>();


		foreach (var function in functions)
		{
			using (connection)
			{
				var command = new SqliteCommand(
				  "SELECT function_name, mutate_prob, crossover_prob FROM Records2 where function_name = @function_name and nr_iterations = 1 and dimensions = 30 ORDER by mean_value limit 1;",
				  connection);
				connection.Open();

				command.Parameters.Add("@function_name", SqliteType.Text, 50).Value = function.ToString();

				var reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						list.Add((reader.GetString(0), reader.GetDouble(1), reader.GetDouble(2)));
					}
				}
				else
				{
					Console.WriteLine("No rows found.");
				}
				reader.Close();
			}
		}

		return list;
	}

	static void Main4(List<(string function_name, double mutateProb, double crossoverProb)> best)
	{
		var pars = new List<List<(string function_name, double mutateProb, double crossoverProb)>>();

		foreach (var function in functions)
			pars.Add(best.Where(x => x.function_name == function.ToString()).ToList());


		var precision = Math.Pow(10, -digitsOfprecision);

		//connection.Open();

		int f = 0;

		foreach (var function in functions)
		{
			int sizeOfArray = dimensions * (int)Math.Ceiling(Math.Log2((1 / precision) * (function.SearchDomain.max - function.SearchDomain.min)));

			foreach (var x in pars[f])
			{
				s_function = function;

				mutateProbability = x.mutateProb;
				crossoverProbability = x.crossoverProb;


				List<Task> tasks = new List<Task>();
				int iterations;

				meanValue = 0;
				values = new List<double>();

				meanTimestamp = 0;
				Timestamps = new List<long>();

				for (iterations = 0; iterations < 50; ++iterations)
				{
					tasks.Add(Task.Factory.StartNew(() => calc()));
					if ((iterations + 1) % 5 == 0)
					{
						Task.WaitAll(tasks.ToArray());
						tasks.Clear();
					}
				}

				Task.WaitAll(tasks.ToArray());

				meanValue /= iterations;
				double SDValues = 0;
				for (int i = 0; i < iterations; ++i)
				{
					SDValues += Math.Pow(values[i] - meanValue, 2);
				}

				SDValues = Math.Sqrt(SDValues / iterations);


				meanTimestamp /= iterations;
				double SDTimestamps = 0;
				for (int i = 0; i < iterations; ++i)
				{
					SDTimestamps += Math.Pow(Timestamps[i] - meanTimestamp, 2);
				}
				SDTimestamps = Math.Sqrt(SDTimestamps / iterations);


				var record = new Record();
				record.FunctionName = s_function.ToString();
				record.Dimensions = dimensions;
				record.DigitsOfPrecision = digitsOfprecision;
				record.NrIterations = iterations;
				record.MutateProb = mutateProbability;
				record.CrossoverProb = crossoverProbability;
				record.MeanValue = meanValue;
				record.SDValue = SDValues;
				record.MeanTime = meanTimestamp;
				record.SDTime = SDTimestamps;


				WriteToDB(record, "Records9");

				string toDisplay = "=====================================================================\n" +
					s_function.ToString() + ", Dimensions: " + dimensions + "; Precision: 10^(-" + digitsOfprecision + "); Iterations: " + iterations + ";\n" +
					"Mutate Probability: " + mutateProbability + "; Crossover Probability: " + crossoverProbability + ";\n" +
					"Mean value: " + string.Format("{0:0.0000000000}", meanValue) + "; SDValues: " + string.Format("{0:0.0000000000}", SDValues) + ";\n" +
					"Mean Timestamp: " + string.Format("{0:0.0000000000}", meanTimestamp) + " ms; SDTimestamps: " + string.Format("{0:0.0000000000}", SDTimestamps) + " ms;\n";


				Console.WriteLine(toDisplay);
				//WriteToFile(toDisplay, "file.txt");

			}

			++f;
			//WriteToFile("\n\n\n\nNew Function", "file.txt");
		}
		//connection.Close();
	}

	static void Main3(List<(string function_name, double crossoverProb, double k1)> best)
	{
		var pars = new List<List<(string function_name, double crossoverProb, double k1)>>();

		foreach (var function in functions)
			pars.Add(best.Where(x => x.function_name == function.ToString()).ToList());


		var precision = Math.Pow(10, -digitsOfprecision);

		//connection.Open();

		int f = 0;

		foreach (var function in functions)
		{
			int sizeOfArray = dimensions * (int)Math.Ceiling(Math.Log2((1 / precision) * (function.SearchDomain.max - function.SearchDomain.min)));

			foreach (var x in pars[f])
			{
				s_function = function;

				k1 = x.k1;
				crossoverProbability = x.crossoverProb;


				List<Task> tasks = new List<Task>();
				int iterations;

				meanValue = 0;
				values = new List<double>();

				meanTimestamp = 0;
				Timestamps = new List<long>();

				for (iterations = 0; iterations < 50; ++iterations)
				{
					tasks.Add(Task.Factory.StartNew(() => calc()));
					if ((iterations + 1) % 5 == 0)
					{
						Task.WaitAll(tasks.ToArray());
						tasks.Clear();
					}
				}

				Task.WaitAll(tasks.ToArray());

				meanValue /= iterations;
				double SDValues = 0;
				for (int i = 0; i < iterations; ++i)
				{
					SDValues += Math.Pow(values[i] - meanValue, 2);
				}

				SDValues = Math.Sqrt(SDValues / iterations);


				meanTimestamp /= iterations;
				double SDTimestamps = 0;
				for (int i = 0; i < iterations; ++i)
				{
					SDTimestamps += Math.Pow(Timestamps[i] - meanTimestamp, 2);
				}
				SDTimestamps = Math.Sqrt(SDTimestamps / iterations);


				var record = new Record();
				record.FunctionName = s_function.ToString();
				record.Dimensions = dimensions;
				record.DigitsOfPrecision = digitsOfprecision;
				record.NrIterations = iterations;
				record.MutateProb = k1;
				record.CrossoverProb = crossoverProbability;
				record.MeanValue = meanValue;
				record.SDValue = SDValues;
				record.MeanTime = meanTimestamp;
				record.SDTime = SDTimestamps;


				WriteToDB(record, "Records8_2");

				//string toDisplay = "=====================================================================\n" +
				//	s_function.ToString() + ", Dimensions: " + dimensions + "; Precision: 10^(-" + digitsOfprecision + "); Iterations: " + iterations + ";\n" +
				//	"Mutate Probability: " + mutateProbability + "; Crossover Probability: " + crossoverProbability + ";\n" +
				//	"Mean value: " + string.Format("{0:0.0000000000}", meanValue) + "; SDValues: " + string.Format("{0:0.0000000000}", SDValues) + ";\n" +
				//	"Mean Timestamp: " + string.Format("{0:0.0000000000}", meanTimestamp) + " ms; SDTimestamps: " + string.Format("{0:0.0000000000}", SDTimestamps) + " ms;\n";


				string toDisplay = "=====================================================================\n" +
					s_function.ToString() + ", Dimensions: " + dimensions + "; Precision: 10^(-" + digitsOfprecision + "); Iterations: " + iterations + ";\n" +
					"K1 Probability: " + k1 + "; Crossover Probability: " + crossoverProbability + ";\n" +
					"Mean value: " + string.Format("{0:0.0000000000}", meanValue) + "; SDValues: " + string.Format("{0:0.0000000000}", SDValues) + ";\n" +
					"Mean Timestamp: " + string.Format("{0:0.0000000000}", meanTimestamp) + " ms; SDTimestamps: " + string.Format("{0:0.0000000000}", SDTimestamps) + " ms;\n";


				Console.WriteLine(toDisplay);
				//WriteToFile(toDisplay, "file.txt");

			}

			++f;
			//WriteToFile("\n\n\n\nNew Function", "file.txt");
		}
		//connection.Close();
	}


	static void Main2()
	{
		var precision = Math.Pow(10, -digitsOfprecision);

		//connection.Open();

		foreach (var function in functions)
		{
			int sizeOfArray = dimensions * (int)Math.Ceiling(Math.Log2((1 / precision) * (function.SearchDomain.max - function.SearchDomain.min)));

			for (crossoverProbability = 0.5; crossoverProbability < 0.76; crossoverProbability += 0.01)
			{
				s_function = function;
				for (mutateProbability = 0.05 / sizeOfArray; mutateProbability < 2.1 / sizeOfArray; mutateProbability += 0.05 / sizeOfArray)
				{
					List<Task> tasks = new List<Task>();
					int iterations;

					meanValue = 0;
					values = new List<double>();

					meanTimestamp = 0;
					Timestamps = new List<long>();

					for (iterations = 0; iterations < 1; ++iterations)
					{
						tasks.Add(Task.Factory.StartNew(() => calc()));
						if (iterations == 4 || iterations == 9 || iterations == 14 || iterations == 19 || iterations == 24)
						{
							Task.WaitAll(tasks.ToArray());
							tasks.Clear();
						}
					}

					Task.WaitAll(tasks.ToArray());

					meanValue /= iterations;
					double SDValues = 0;
					for (int i = 0; i < iterations; ++i)
					{
						SDValues += Math.Pow(values[i] - meanValue, 2);
					}

					SDValues = Math.Sqrt(SDValues / iterations);


					meanTimestamp /= iterations;
					double SDTimestamps = 0;
					for (int i = 0; i < iterations; ++i)
					{
						SDTimestamps += Math.Pow(Timestamps[i] - meanTimestamp, 2);
					}
					SDTimestamps = Math.Sqrt(SDTimestamps / iterations);


					var record = new Record();
					record.FunctionName = s_function.ToString();
					record.Dimensions = dimensions;
					record.DigitsOfPrecision = digitsOfprecision;
					record.NrIterations = iterations;
					record.MutateProb = mutateProbability;
					record.CrossoverProb = crossoverProbability;
					record.MeanValue = meanValue;
					record.SDValue = SDValues;
					record.MeanTime = meanTimestamp;
					record.SDTime = SDTimestamps;


					WriteToDB(record, "Records5");

					string toDisplay = "=====================================================================\n" +
						s_function.ToString() + ", Dimensions: " + dimensions + "; Precision: 10^(-" + digitsOfprecision + "); Iterations: " + iterations + ";\n" +
						"Mutate Probability: " + mutateProbability + "; Crossover Probability: " + crossoverProbability + ";\n" +
						"Mean value: " + string.Format("{0:0.0000000000}", meanValue) + "; SDValues: " + string.Format("{0:0.0000000000}", SDValues) + ";\n" +
						"Mean Timestamp: " + string.Format("{0:0.0000000000}", meanTimestamp) + " ms; SDTimestamps: " + string.Format("{0:0.0000000000}", SDTimestamps) + " ms;\n";

					Console.WriteLine(toDisplay);
					WriteToFile(toDisplay, "file.txt");
				}

				WriteToFile("\n\n\n\n", "file.txt");
			}

			WriteToFile("\n\n\n\nNew Function", "file.txt");
		}
		//connection.Close();

	}

	static void calc()
	{
		var TimestampStart = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

		double dd = GeneticAlgorithm.GetMin(s_function, dimensions, digitsOfprecision, 2000, 200, crossoverProbability, k1, k2);

		//double dd = GeneticAlgorithm.GetMin(s_function, dimensions, digitsOfprecision, 2000, 200, mutateProbability, crossoverProbability);

		Console.WriteLine("Value: " + dd);

		var TimestampFinish = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

		lock (sumlock)
		{
			Timestamps.Add(TimestampFinish - TimestampStart);
			meanTimestamp += TimestampFinish - TimestampStart;

			meanValue += dd;
			values.Add(dd);
		}
	}


	private static void WriteToFile(string toDisplay, string file)
	{

		using (var stream = new FileStream(file, FileMode.Append))
		using (var streamBuffered = new BufferedStream(stream))
		using (var streamWriter = new StreamWriter(streamBuffered))
		{
			streamWriter.WriteLine(toDisplay);

			streamWriter.Flush();
		}

	}

	private static void WriteToDB(Record record, string table)
	{
		//var command = connection.CreateCommand();
		//command.CommandText = $"INSERT INTO Records VALUES ( {1}, {record.FunctionName}, {record.Dimensions}," +
		//	$"{record.DigitsOfPrecision}, {record.NrIterations}, {record.MutateProb}, {record.CrossoverProb}," +
		//	$"{record.MeanValue}, {record.SDValue}, {record.MeanTime}, {record.SDTime} );";

		string query = "INSERT INTO " + table + " (function_name, dimensions, digits_of_precision, nr_iterations, mutate_prob, crossover_prob, mean_value, sd_value, mean_time, sd_time) " +
				   "VALUES (@function_name, @dimensions, @digits_of_precision, @nr_iterations, @mutate_prob, @crossover_prob, @mean_value, @sd_value, @mean_time, @sd_time) ";

		// create connection and command
		using (var cmd = new SqliteCommand(query, connection))
		{
			// define parameters and their values
			cmd.Parameters.Add("@function_name", SqliteType.Text, 50).Value = record.FunctionName;
			cmd.Parameters.Add("@dimensions", SqliteType.Integer, 50).Value = record.Dimensions;
			cmd.Parameters.Add("@digits_of_precision", SqliteType.Integer, 50).Value = record.DigitsOfPrecision;
			cmd.Parameters.Add("@nr_iterations", SqliteType.Integer, 50).Value = record.NrIterations;
			cmd.Parameters.Add("@mutate_prob", SqliteType.Real).Value = record.MutateProb;
			cmd.Parameters.Add("@crossover_prob", SqliteType.Real, 50).Value = record.CrossoverProb;
			cmd.Parameters.Add("@mean_value", SqliteType.Real, 50).Value = record.MeanValue;
			cmd.Parameters.Add("@sd_value", SqliteType.Real, 50).Value = record.SDValue;
			cmd.Parameters.Add("@mean_time", SqliteType.Real, 50).Value = record.MeanTime;
			cmd.Parameters.Add("@sd_time", SqliteType.Real, 50).Value = record.SDTime;

			// open connection, execute INSERT, close connection
			connection.Open();
			cmd.ExecuteNonQuery();
			connection.Close();
		}
	}
}