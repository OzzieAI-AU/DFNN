namespace DFNN
{
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public class BenchmarkSuite
    {
        public static void RunBenchmark()
        {
            Console.WriteLine("=== INITIALIZING BENCHMARK: FRACTAL VS. RANDOM ===");
            var topology = new List<int> { 8, 32, 16, 8 };

            // Engines
            var fractalEngine = new GeometricSequenceEngine(topology);
            var randomEngine = new RandomSequenceEngine(topology);

            // Dataset: Rhythmic (Low Variance) vs Turbulent (High Variance)
            double[] rhythmic = { 0.1, 0.2, 0.3, 0.4, 0.1, 0.2, 0.3, 0.4 };
            double[] turbulent = { -0.8, 0.9, -0.7, 0.6, -0.9, 0.8, -0.7, 0.6 };

            Console.WriteLine("\n[1] Measuring Fractal Manifold Separation...");
            double fractalScore = MeasureSeparation(fractalEngine, rhythmic, turbulent);

            Console.WriteLine("[2] Measuring Random Manifold Separation...");
            double randomScore = MeasureSeparation(randomEngine, rhythmic, turbulent);

            Console.WriteLine("\n=== FINAL RESULTS ===");
            Console.WriteLine($"Fractal Manifold Cosine Similarity: {fractalScore:F5}");
            Console.WriteLine($"Random Manifold Cosine Similarity:  {randomScore:F5}");

            double improvement = (randomScore - fractalScore) / randomScore * 100;
            Console.WriteLine($"\nFractal-Seeded Improvement in Separation: {improvement:F2}%");
        }

        public static void GenerateNoiseStabilityGraph()
        {
            var engine = new GeometricSequenceEngine(new List<int> { 8, 32, 16, 8 });
            double[] rhythmicInput = { 0.1, 0.2, 0.3, 0.4, 0.1, 0.2, 0.3, 0.4 };
            var baseline = engine.ProcessSequence(new List<double[]> { rhythmicInput });

            Console.WriteLine("NoiseLevel,StabilityIndex");
            Random rand = new Random();

            for (int i = 0; i <= 20; i++)
            {
                double noiseLevel = i * 0.05; // 0% to 100% noise
                double[] noisyInput = rhythmicInput.Select(x => x + (rand.NextDouble() * 2 - 1) * noiseLevel).ToArray();

                var noisyOutput = engine.ProcessSequence(new List<double[]> { noisyInput });

                // Stability Index: 1 - CosineDistance(Baseline, NoisyOutput)
                double stability = 1.0 - CalculateCosineDistance(baseline, noisyOutput);
                Console.WriteLine($"{noiseLevel:F2},{stability:F4}");
            }
        }

        public static void GeneratePhaseSensitivityGraph()
        {
            var net = new AdaptivePrimeNetwork(new List<int> { 8, 16, 8 });
            double[] input = { 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5 };

            Console.WriteLine("SystemLoad,SpectralEntropy");

            for (int i = 0; i <= 20; i++)
            {
                double load = i * 0.1; // Simulated system load
                net.AdaptToEnvironment(load);

                double entropy = net.CalculateSpectralEntropy(input);
                Console.WriteLine($"{load:F1},{entropy:F6}");
            }
        }

        private static double MeasureSeparation(dynamic engine, double[] a, double[] b)
        {
            var vecA = engine.ProcessSequence(new List<double[]> { a });
            var vecB = engine.ProcessSequence(new List<double[]> { b });
            return CalculateCosineSimilarity(vecA, vecB);
        }

        private static double CalculateCosineSimilarity(double[] vecA, double[] vecB)
        {
            double dot = vecA.Zip(vecB, (a, b) => a * b).Sum();
            double normA = Math.Sqrt(vecA.Select(x => x * x).Sum());
            double normB = Math.Sqrt(vecB.Select(x => x * x).Sum());
            return dot / (normA * normB);
        }

        private static double CalculateCosineDistance(double[] vectorA, double[] vectorB)
        {
            // 1. Calculate Dot Product
            double dotProduct = 0.0;
            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
            }

            // 2. Calculate Magnitudes
            double magnitudeA = Math.Sqrt(vectorA.Sum(x => x * x));
            double magnitudeB = Math.Sqrt(vectorB.Sum(x => x * x));

            // 3. Handle edge case for zero vectors
            if (magnitudeA == 0 || magnitudeB == 0) return 1.0;

            // 4. Calculate Cosine Similarity
            double cosineSimilarity = dotProduct / (magnitudeA * magnitudeB);

            // 5. Calculate Cosine Distance (1 - Similarity)
            // We clamp the result between 0 and 1 to prevent precision errors
            return Math.Max(0, Math.Min(1, 1.0 - cosineSimilarity));
        }

        public static void GenerateVisualizationData()
        {

            var engine = new GeometricSequenceEngine(new List<int> { 8, 32, 16, 8 });
            Console.WriteLine("Label,X,Y"); // CSV Header

            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                // Generate Rhythmic (Low Entropy)
                double[] r = Enumerable.Range(0, 8).Select(_ => rand.NextDouble() * 0.1).ToArray();
                var outR = engine.ProcessSequence(new List<double[]> { r });
                Console.WriteLine($"Rhythmic,{outR[0]},{outR[1]}");

                // Generate Turbulent (High Entropy)
                double[] t = Enumerable.Range(0, 8).Select(_ => (rand.NextDouble() * 2) - 1).ToArray();
                var outT = engine.ProcessSequence(new List<double[]> { t });
                Console.WriteLine($"Turbulent,{outT[0]},{outT[1]}");
            }
        }
    }

    public class StructuralDriftBenchmark
    {
        public static void Run()
        {
            var engine = new GeometricSequenceEngine(new List<int> { 8, 32, 16, 8 });

            // Define baseline rhythm
            double[] normalOp = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };
            var baseline = engine.ProcessSequence(new List<double[]> { normalOp });

            Console.WriteLine("\n=== STRUCTURAL DRIFT DETECTION ===");

            for (int i = 1; i <= 5; i++)
            {
                // Simulate machine wear: increase noise (0.1 -> 0.5)
                double[] drifted = normalOp.Select(x => x + (i * 0.1)).ToArray();
                var result = engine.ProcessSequence(new List<double[]> { drifted });

                // Calculate Euclidean Distance from baseline
                double drift = Math.Sqrt(result.Zip(baseline, (a, b) => Math.Pow(a - b, 2)).Sum());

                Console.WriteLine($"Wear Level {i}: Detected Drift = {drift:F4}");
            }
        }

        public static void GenerateDriftVisualizationData()
        {
            // Initialize the engine with the same topology used in the benchmark
            var engine = new GeometricSequenceEngine(new List<int> { 8, 32, 16, 8 });

            // Define the baseline operation (Normal State)
            double[] normalOp = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };
            var baseline = engine.ProcessSequence(new List<double[]> { normalOp });

            Console.WriteLine("WearLevel,DetectedDrift"); // CSV Header

            // Simulate a progression of wear from 0.0 to 1.0
            for (int i = 0; i <= 20; i++)
            {
                double wearFactor = i * 0.05;

                // Create a drifted signal based on the wear factor
                double[] drifted = normalOp.Select(x => x + wearFactor).ToArray();
                var result = engine.ProcessSequence(new List<double[]> { drifted });

                // Calculate Euclidean Distance (Drift Magnitude)
                double drift = Math.Sqrt(result.Zip(baseline, (a, b) => Math.Pow(a - b, 2)).Sum());

                // Output CSV row
                Console.WriteLine($"{i},{drift:F4}");
            }
        }
    }

    // A Standard Random Initialization Engine for Comparison
    public class RandomSequenceEngine
    {
        private readonly Random _rand = new Random();
        private readonly List<int> _topology;

        public RandomSequenceEngine(List<int> topology) => _topology = topology;

        public double[] ProcessSequence(List<double[]> inputs)
        {
            double[] state = inputs[0];
            for (int i = 0; i < _topology.Count - 1; i++)
            {
                double[] next = new double[_topology[i + 1]];
                for (int r = 0; r < next.Length; r++)
                {
                    double sum = 0;
                    for (int c = 0; c < state.Length; c++)
                        sum += state[c] * (_rand.NextDouble() * 2 - 1);
                    next[r] = Math.Tanh(sum);
                }
                state = next;
            }
            return state;
        }
    }
}