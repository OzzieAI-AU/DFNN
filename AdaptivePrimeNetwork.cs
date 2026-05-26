namespace DFNN
{
    public class AdaptivePrimeNetwork
    {
        private readonly List<double[,]> _layers = new List<double[,]>();
        private readonly List<int> _layerSizes;
        private double _environmentalFactor = 1.0; // The adaptive tuning variable (sigma)

        public AdaptivePrimeNetwork(List<int> layerSizes)
        {
            _layerSizes = layerSizes;
            InitializeFractalWeights();
        }

        private List<int> GeneratePrimeGaps(int count)
        {
            List<int> primes = new List<int>();
            List<int> gaps = new List<int>();
            int candidate = 2;

            while (gaps.Count < count)
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
                        gaps.Add(primes[primes.Count - 1] - primes[primes.Count - 2]);
                }
                candidate++;
            }
            return gaps;
        }

        private void InitializeFractalWeights()
        {
            int totalConnections = 0;
            for (int i = 0; i < _layerSizes.Count - 1; i++)
                totalConnections += _layerSizes[i] * _layerSizes[i + 1];

            List<int> gaps = GeneratePrimeGaps(totalConnections);
            int gapPtr = 0;

            for (int l = 0; l < _layerSizes.Count - 1; l++)
            {
                int inputs = _layerSizes[l];
                int outputs = _layerSizes[l + 1];
                double[,] weights = new double[outputs, inputs];
                double scale = Math.Sqrt(2.0 / inputs);

                for (int row = 0; row < outputs; row++)
                {
                    for (int col = 0; col < inputs; col++)
                    {
                        int gap = gaps[gapPtr++];
                        // The weights are now dynamically balanced by the ambient environmental factor
                        weights[row, col] = Math.Sin(gap) * Math.Cos(gap * Math.PI / 4.0) * scale;
                    }
                }
                _layers.Add(weights);
            }
        }

        /// <summary>
        /// Dynamically calibrates the system's baseline to account for safe, environmental drift
        /// </summary>
        public void AdaptToEnvironment(double systemLoadFactor)
        {
            // Modulates the internal tuning frequency using a clean harmonic ratio
            _environmentalFactor = 1.0 + (Math.Sin(systemLoadFactor) * 0.15);
        }

        public double[] Analyze(double[] inputs)
        {
            double[] currentSignals = inputs;
            foreach (var layer in _layers)
            {
                int outputs = layer.GetLength(0);
                int inputsCount = layer.GetLength(1);
                double[] nextSignals = new double[outputs];

                for (int row = 0; row < outputs; row++)
                {
                    double sum = 0;
                    for (int col = 0; col < inputsCount; col++)
                    {
                        // The environmental factor acts as a dynamic modifier to signal propagation
                        sum += currentSignals[col] * (layer[row, col] * _environmentalFactor);
                    }
                    nextSignals[row] = Math.Tanh(sum);
                }
                currentSignals = nextSignals;
            }
            return currentSignals;
        }

        public double CalculateSpectralEntropy(double[] inputs)
        {
            double[] outputs = Analyze(inputs);
            double average = outputs.Average();
            return outputs.Select(o => Math.Pow(o - average, 2)).Average();
        }
    }
}