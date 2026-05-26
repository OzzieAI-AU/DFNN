namespace DFNN
{
    using System;
    using System.Collections.Generic;
    using System.Linq;


    public interface IFractalManifold
    {
        double ComputeWeightAt(int row, int col, int layerIndex, double phaseModulation);
    }


    // 1. The Entropy-Driven Manifold
    public class GeometricManifold : IFractalManifold
    {
        public double ComputeWeightAt(int row, int col, int layerIndex, double structuralSignature)
        {
            // FREQUENCY WARPING: We use the signature as a multiplier, not a phase shift offset.
            // This physically stretches or shatters the mathematical space on the fly.
            double frequencyBase = 1.0 + Math.Abs(structuralSignature);

            // Map coordinates using irrational constants to prevent harmonic looping
            double angleX = (row * 1.61803) * frequencyBase; // Golden Ratio
            double angleY = (col * 2.71828) * frequencyBase; // Euler's Number
            double depthZ = (layerIndex * 3.14159) * frequencyBase; // Pi

            // 3D Phase-Coupled Coordinate
            double angle = angleX + angleY + depthZ;

            // Orthogonal Wave Channels
            double realComponent = Math.Sin(angle);
            double imagComponent = Math.Cos(angle * 1.5);

            // Wigner's Surmise Level Repulsion
            double distance = Math.Sqrt(realComponent * realComponent + imagComponent * imagComponent);
            double repulsion = distance * Math.Exp(-Math.PI * (distance * distance) / 4.0);

            // The structural signature violently alters the final output topology
            return Math.Sin(repulsion * frequencyBase * (row + col + 1));
        }
    }

    // 2. The Variance-Tracking Sequence Engine
    public class GeometricSequenceEngine
    {
        private readonly ContinuousGeometryNetwork _neuroManifold;
        private readonly int _embeddingDimension;

        public GeometricSequenceEngine(List<int> topology)
        {
            var manifold = new GeometricManifold();
            _neuroManifold = new ContinuousGeometryNetwork(topology, manifold);
            _embeddingDimension = topology.First();
        }

        public double[] ProcessSequence(List<double[]> tokenSequence)
        {
            double[] stateVector = new double[_embeddingDimension];

            for (int i = 0; i < tokenSequence.Count; i++)
            {
                double[] currentToken = tokenSequence[i];

                // BREAKTHROUGH: Calculate the mathematical "Entropy" (Variance) of the token
                double mean = currentToken.Average();
                double variance = currentToken.Select(v => Math.Pow(v - mean, 2)).Average();

                // Create a structural signature. 
                // Low variance (rhythmic) = small scalar. High variance (chaotic) = massive scalar.
                double tokenStructuralSignature = (variance * 15.0) + (i * 0.5);

                double[] mixedInput = new double[_embeddingDimension];
                for (int d = 0; d < _embeddingDimension; d++)
                {
                    // Non-linear memory blending to preserve structural momentum
                    mixedInput[d] = currentToken[d] + (stateVector[d] * Math.Cos(tokenStructuralSignature));
                }

                // Propagate through the warped manifold
                stateVector = _neuroManifold.Process(mixedInput, tokenStructuralSignature);
            }

            return stateVector;
        }
    }

    public class GeometricTensorLayer
    {
        private readonly int _inputs;
        private readonly int _outputs;
        private readonly int _layerIndex;
        private readonly IFractalManifold _manifold;

        public GeometricTensorLayer(int inputs, int outputs, int layerIndex, IFractalManifold manifold)
        {
            _inputs = inputs;
            _outputs = outputs;
            _layerIndex = layerIndex;
            _manifold = manifold;
        }

        public double[] Forward(double[] inputSignals, double phaseModulation)
        {
            double[] outputSignals = new double[_outputs];
            double scale = Math.Sqrt(2.0 / _inputs);

            for (int row = 0; row < _outputs; row++)
            {
                double accumulation = 0;
                for (int col = 0; col < _inputs; col++)
                {
                    // The tokens dynamically shift the manifold's generation phase on-the-fly
                    double dynamicWeight = _manifold.ComputeWeightAt(row, col, _layerIndex, phaseModulation) * scale;
                    accumulation += inputSignals[col] * dynamicWeight;
                }
                // Linear/Leaky response to protect the unique signature from being crushed by Tanh
                outputSignals[row] = accumulation > 0 ? accumulation : accumulation * 0.1;
            }

            return outputSignals;
        }
    }

    public class ContinuousGeometryNetwork
    {
        private readonly List<GeometricTensorLayer> _layers = new List<GeometricTensorLayer>();

        public ContinuousGeometryNetwork(List<int> topology, IFractalManifold manifold)
        {
            for (int i = 0; i < topology.Count - 1; i++)
            {
                _layers.Add(new GeometricTensorLayer(topology[i], topology[i + 1], i, manifold));
            }
        }

        public double[] Process(double[] inputs, double phaseModulation)
        {
            double[] signals = inputs;
            foreach (var layer in _layers)
            {
                signals = layer.Forward(signals, phaseModulation);
            }
            return signals;
        }
    }
}