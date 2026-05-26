namespace DFNN
{
    public class FunctionalPrimeNetwork
    {
        private readonly List<double[,]> _layers = new List<double[,]>();
        private readonly List<int> _layerSizes;

        public FunctionalPrimeNetwork(List<int> layerSizes)
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
                        // Embedding the level repulsion signature
                        weights[row, col] = Math.Sin(gap) * Math.Cos(gap * Math.PI / 4.0) * scale;
                    }
                }
                _layers.Add(weights);
            }
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
                        sum += currentSignals[col] * layer[row, col];

                    // Tanh activation to map signals strictly between -1 and 1 for clean wave variance
                    nextSignals[row] = Math.Tanh(sum);
                }
                currentSignals = nextSignals;
            }
            return currentSignals;
        }

        // Process a stream of data and calculate its unique "Mathematical Fingerprint"
        public double CalculateSpectralEntropy(double[] inputs)
        {
            double[] outputs = Analyze(inputs);
            double average = outputs.Average();
            return outputs.Select(o => Math.Pow(o - average, 2)).Average();
        }
    }
}