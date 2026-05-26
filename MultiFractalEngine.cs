namespace DFNN
{
    public class MultiFractalEngine
    {
        private readonly int _rows;
        private readonly int _cols;
        private readonly Random _rand = new Random();

        public MultiFractalEngine(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
        }

        public double[,] GenerateFractalWeights(FractalType type)
        {
            double[,] weights = new double[_rows, _cols];
            double scale = Math.Sqrt(2.0 / _cols); // Analytical scaling parameter

            int index = 0;
            List<double> pool = new List<double>();

            switch (type)
            {
                case FractalType.PrimeGapSignature:
                    pool = GetPrimeGapPool(_rows * _cols);
                    break;
                case FractalType.MandelbrotBifurcation:
                    pool = GetMandelbrotPool(_rows * _cols);
                    break;
                case FractalType.CantorDustSieve:
                    pool = GetCantorDustPool(_rows * _cols);
                    break;
            }

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    weights[i, j] = pool[index++] * scale;
                }
            }
            return weights;
        }

        // 1. Prime Gap Generator
        private List<double> GetPrimeGapPool(int size)
        {
            List<int> primes = new List<int>();
            List<double> pool = new List<double>();
            int num = 2;

            while (pool.Count < size)
            {
                bool isPrime = true;
                for (int i = 2; i * i <= num; i++)
                    if (num % i == 0) { isPrime = false; break; }

                if (isPrime)
                {
                    primes.Add(num);
                    if (primes.Count > 1)
                    {
                        int gap = primes[primes.Count - 1] - primes[primes.Count - 2];
                        pool.Add(Math.Sin(gap) * Math.Cos(gap * Math.PI / 4.0));
                    }
                }
                num++;
            }
            return pool;
        }

        // 2. Mandelbrot Set Boundary Generator (Z = Z^2 + C)
        private List<double> GetMandelbrotPool(int size)
        {
            List<double> pool = new List<double>();
            int side = (int)Math.Ceiling(Math.Sqrt(size));

            for (int x = 0; x < side; x++)
            {
                for (int y = 0; y < side; y++)
                {
                    // Map network matrix index to complex coordinate plane
                    double cr = -2.0 + (x * 3.0 / side);
                    double ci = -1.5 + (y * 3.0 / side);

                    double zr = 0, zi = 0;
                    int iter = 0;
                    int maxIter = 100;

                    while (zr * zr + zi * zi <= 4.0 && iter < maxIter)
                    {
                        double temp = zr * zr - zi * zi + cr;
                        zi = 2.0 * zr * zi + ci;
                        zr = temp;
                        iter++;
                    }

                    // Normalize the escape velocity velocity into an analogue weight signal
                    double weightValue = (double)iter / maxIter;
                    pool.Add(Math.Tanh(weightValue * 2.0 - 1.0));

                    if (pool.Count == size) return pool;
                }
            }
            return pool;
        }

        // 3. Cantor Dust Sieve Generator (Iterative 1D Fragmentation)
        private List<double> GetCantorDustPool(int size)
        {
            List<double> pool = new List<double>();
            for (int i = 0; i < size; i++)
            {
                double val = (double)i / size;
                bool inDust = true;
                double currentVal = val;

                // Iterate through 6 architectural levels of the Cantor deletion sieve
                for (int level = 0; level < 6; level++)
                {
                    double ternaryDigit = Math.Floor(currentVal * 3.0);
                    if (ternaryDigit == 1) // If it falls into the middle third, drop it
                    {
                        inDust = false;
                        break;
                    }
                    currentVal = (currentVal * 3.0) - ternaryDigit;
                }

                // High sparse activation signature characteristic of sparse networks
                pool.Add(inDust ? 0.85 : -0.85);
            }
            return pool;
        }
    }
}