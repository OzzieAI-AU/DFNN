namespace DFNN
{

    using System;
    using System.Collections.Generic;
    using System.Linq;


    public class Program
    {
        private static double CalculateCosineSimilarity(double[] vecA, double[] vecB)
        {
            double dotProduct = 0.0;
            double normA = 0.0;
            double normB = 0.0;

            for (int i = 0; i < vecA.Length; i++)
            {
                dotProduct += vecA[i] * vecB[i];
                normA += vecA[i] * vecA[i];
                normB += vecB[i] * vecB[i];
            }

            if (normA == 0.0 || normB == 0.0) return 0.0;
            return dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }

        public static void Main()
        {

            BenchmarkSuite benchmarkSuite = new BenchmarkSuite();
            BenchmarkSuite.RunBenchmark();
            BenchmarkSuite.GenerateVisualizationData();
            BenchmarkSuite.GenerateNoiseStabilityGraph();
            BenchmarkSuite.GeneratePhaseSensitivityGraph();

            StructuralDriftBenchmark StructuralDriftBenchmark = new StructuralDriftBenchmark();
            StructuralDriftBenchmark.Run();
            StructuralDriftBenchmark.GenerateDriftVisualizationData();

            LinearReadout linearReadout = new LinearReadout(8);
            //linearReadout.



            Console.WriteLine("=== ZERO-FOOTPRINT FRACTAL CONTEXT CLUSTERING ENGINE ===");
            Console.WriteLine("System Architecture: Phase-Coupled Orthogonal Manifold.\n");

            List<int> networkTopology = new List<int> { 8, 32, 16, 8 };
            GeometricSequenceEngine framework = new GeometricSequenceEngine(networkTopology);

            // --- ORDERED / RHYTHMIC CONCEPT SEQUENCE ---
            List<double[]> rhythmicSequence = new List<double[]>
        {
            new double[] { 0.10, 0.20, 0.30, 0.40, 0.50, 0.60, 0.70, 0.80 },
            new double[] { 0.15, 0.25, 0.35, 0.45, 0.55, 0.65, 0.75, 0.85 },
            new double[] { 0.20, 0.30, 0.40, 0.50, 0.60, 0.70, 0.80, 0.90 }
        };

            // --- TURBULENT / CHAOTIC CONCEPT SEQUENCE ---
            List<double[]> turbulentSequence = new List<double[]>
        {
            new double[] { -0.80, 0.12, -0.95, 0.44, -0.73, 0.02, -0.66, 0.19 },
            new double[] { 0.91, -0.55, 0.88, -0.12, 0.94, -0.73, 0.02, -0.66 },
            new double[] { -0.45, 0.77, -0.33, 0.81, -0.11, 0.99, -0.52, 0.23 }
        };

            Console.WriteLine("Routing Sequences through Phase-Coupled channels...");

            double[] rhythmicTrajectory = framework.ProcessSequence(rhythmicSequence);
            double[] turbulentTrajectory = framework.ProcessSequence(turbulentSequence);

            Console.WriteLine("\n=== COMPRESSED RECURSIVE FRACTAL STATES ===");
            Console.WriteLine($"Rhythmic Path Vector:  [ {string.Join(", ", rhythmicTrajectory.Take(4).Select(v => v.ToString("F3")))}... ]");
            Console.WriteLine($"Turbulent Path Vector: [ {string.Join(", ", turbulentTrajectory.Take(4).Select(v => v.ToString("F3")))}... ]");
            Console.WriteLine("-----------------------------------------------------------------");

            double spatialCongruence = CalculateCosineSimilarity(rhythmicTrajectory, turbulentTrajectory);

            Console.WriteLine("\n=== TOPOLOGICAL CLUSTERING REPORT ===");
            Console.WriteLine($"Spatial Trajectory Cosine Similarity: {spatialCongruence:F5}");

            if (spatialCongruence < 0.4)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nSUCCESS: Unsupervised Segregation Confirmed.");
                Console.WriteLine("The phase-coupled manifold has shattered the dimensional overlap!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\n[Status]: Overlapping values present. Phase angle requires structural isolation.");
            }
        }
    }


    // A simple trainable linear head to interpret the Manifold's state
    public class LinearReadout
    {
        private double[] _weights;
        private double _bias;

        public LinearReadout(int inputSize)
        {
            _weights = new double[inputSize];
            Random rand = new Random();
            for (int i = 0; i < inputSize; i++) _weights[i] = rand.NextDouble() * 0.1;
        }

        public double Predict(double[] manifoldOutput)
        {
            return manifoldOutput.Zip(_weights, (val, weight) => val * weight).Sum() + _bias;
        }

        // Extremely fast 1-shot update (Delta Rule)
        public void Train(double[] manifoldOutput, double target, double learningRate)
        {
            double prediction = Predict(manifoldOutput);
            double error = target - prediction;
            for (int i = 0; i < _weights.Length; i++)
                _weights[i] += learningRate * error * manifoldOutput[i];
            _bias += learningRate * error;
        }
    }

    public class ProofOfUtility
    {
        public static void Run()
        {
            // 1. Setup Manifold
            var topology = new List<int> { 8, 32, 16, 8 };
            var framework = new GeometricSequenceEngine(topology);
            var classifier = new LinearReadout(8);

            // --- DATASET 1: NETWORK PACKET LOGS ---
            // Simulate: 0 = Normal, 1 = Malicious/Chaotic
            var packets = new List<(double[] data, double label)> {
                (new double[]{ 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, 0),
                (new double[]{ -0.9, 0.5, -0.8, 0.4, -0.7, 0.2, -0.6, 0.3 }, 1)
            };

            // --- DATASET 2: TEXT CLASSIFICATION ---
            // Simulate: 0 = Tech (Ordered), 1 = Creative (Turbulent)
            var texts = new List<(double[] data, double label)> {
                (new double[]{ 0.8, 0.9, 0.7, 0.8, 0.9, 0.7, 0.8, 0.9 }, 0),
                (new double[]{ -0.2, 0.8, -0.5, 0.3, -0.9, 0.1, 0.2, -0.4 }, 1)
            };

            Console.WriteLine("=== TRAINING READOUT LAYER (1-SHOT DELTA) ===");
            foreach (var item in packets.Concat(texts))
            {
                var manifoldState = framework.ProcessSequence(new List<double[]> { item.data });
                classifier.Train(manifoldState, item.label, 0.05);
            }

            Console.WriteLine("Training Complete. Testing Inference...");

            // Test Inference
            double[] testPacket = new double[] { -0.85, 0.45, -0.75, 0.35, -0.65, 0.25, -0.55, 0.25 };
            var output = framework.ProcessSequence(new List<double[]> { testPacket });
            Console.WriteLine($"Packet Inference: {(classifier.Predict(output) > 0.5 ? "MALICIOUS" : "NORMAL")}");
        }
    }
}