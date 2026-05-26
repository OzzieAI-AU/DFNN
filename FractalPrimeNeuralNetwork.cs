namespace DFNN
{
    public class FractalPrimeNeuralNetwork
    {
        private readonly List<double[,]> _layers = new List<double[,]>();
        private readonly List<int> _layerSizes;
        private readonly List<int> _primeGaps;

        public FractalPrimeNeuralNetwork(List<int> layerSizes)
        {
            _layerSizes = layerSizes;

            // Calculate total structural points needed to seed the layers
            int totalConnections = 0;
            for (int i = 0; i < layerSizes.Count - 1; i++)
            {
                totalConnections += layerSizes[i] * layerSizes[i + 1];
            }

            // Generate the underlying deterministic Prime Gap template
            _primeGaps = GeneratePrimeGaps(totalConnections);

            // Build and initialize the network weights using the fractal signature
            InitializeFractalWeights();
        }

        // 1. Core Mathematical Sieve to extract the Prime Gaps
        private List<int> GeneratePrimeGaps(int requiredCount)
        {
            List<int> primes = new List<int>();
            List<int> gaps = new List<int>();
            int candidate = 2;

            // Generate enough primes to calculate the requested number of gaps
            while (gaps.Count < requiredCount)
            {
                bool isPrime = true;
                for (int i = 2; i * i <= candidate; i++)
                {
                    if (candidate % i == 0) { isPrime = false; break; }
                }
                if (isPrime)
                {
                    primes.Add(candidate);
                    if (primes.Count > 1)
                    {
                        gaps.Add(primes[primes.Count - 1] - primes[primes.Count - 2]);
                    }
                }
                candidate++;
            }
            return gaps;
        }

        // 2. Initializing weights to match the Level Repulsion property of the gaps
        private void InitializeFractalWeights()
        {
            int gapPtr = 0;

            for (int l = 0; l < _layerSizes.Count - 1; l++)
            {
                int inputs = _layerSizes[l];
                int outputs = _layerSizes[l + 1];
                double[,] weightMatrix = new double[outputs, inputs];

                // Standard normalization scaling parameter derived from network depth
                double scaleFactor = Math.Sqrt(2.0 / inputs);

                for (int row = 0; row < outputs; row++)
                {
                    for (int col = 0; col < inputs; col++)
                    {
                        int gapValue = _primeGaps[gapPtr++];

                        // Transforming the discrete prime gap using a continuous wave function
                        // This embeds a self-similar fractal distribution into the matrix array
                        double fractalWeight = Math.Sin(gapValue) * Math.Cos(gapValue * Math.PI / 4.0);

                        weightMatrix[row, col] = fractalWeight * scaleFactor;
                    }
                }
                _layers.Add(weightMatrix);
            }
        }

        // 3. Activation function mapping (using LeakyReLU to preserve negative fractal spacing data)
        private double Activate(double x)
        {
            return x > 0 ? x : x * 0.01;
        }

        // 4. Forward Propagation of inputs through the network
        public double[] ForwardPropagate(double[] inputs)
        {
            double[] currentSignals = inputs;

            for (int l = 0; l < _layers.Count; l++)
            {
                double[,] weights = _layers[l];
                int outputs = weights.GetLength(0);
                int inputsCount = weights.GetLength(1);
                double[] nextSignals = new double[outputs];

                for (int row = 0; row < outputs; row++)
                {
                    double sum = 0;
                    for (int col = 0; col < inputsCount; col++)
                    {
                        sum += currentSignals[col] * weights[row, col];
                    }
                    nextSignals[row] = Activate(sum);
                }
                currentSignals = nextSignals;
            }

            return currentSignals;
        }
    }
}