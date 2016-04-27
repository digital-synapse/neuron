using System;

namespace Neuron
{
    public class NeuralNet
    {

        public NeuralNet(params int[] layerSize)
        {
            if (layerSize == null || layerSize.Length == 0)
                throw new ArgumentException("Layer sizes must be defined");
            this.layerSize = layerSize;

            // calculate weights needed for the given layers dimensions
            int sum = 0;
            for (var i = 0; i < layerSize.Length - 1; i++)
            {
                sum += (layerSize[i] + 1)*layerSize[i + 1];
            }
            WeightsNeeded = sum;
            Bias = -1;
        }

        private readonly int[] layerSize;

        public int WeightsNeeded { get; private set; }
        private float[] weights;

        public float[] Weights
        {
            get { return weights; }
            set
            {
                if (value == null || value.Length != WeightsNeeded)
                    throw new ArgumentException("Invalid number of weights given");
                weights = value;
            }
        }

        public float Bias { get; set; }

        public float[] Solve(params float[] inputs)
        {

            // validate input        
            if (inputs == null || inputs.Length == 0 || inputs.Length > layerSize[0])
                throw new ArgumentException("Invalid number of inputs given");

            // solve  
            float[] outputs = null;
            int weightIndex = 0;
            for (var d = 1; d < layerSize.Length; d++)
            {
                outputs = new float[layerSize[d]];
                for (var n = 0; n < layerSize[d]; n++)
                {
                    float activation = 0;
                    for (var i = 0; i < inputs.Length; i++)
                    {
                        activation += inputs[i]*weights[i + weightIndex];
                    }
                    activation += Bias*weights[inputs.Length + weightIndex];
                    activation = (float)(1.7159* Math.Tanh(0.6666666*activation));
                    //activation =  (float)(1 / (1 + Math.Pow(Math.E, -activation)));
                    outputs[n] = activation;
                    weightIndex += inputs.Length + 1;
                }                
                inputs = outputs;
            }

            // clamp outputs (optional but helps NN training)
            
            for (var i = 0; i < outputs.Length; i++)
            {
                if (outputs[i] < 0.01) outputs[i] = 0;
                else if (outputs[i] > 0.99) outputs[i] = 1;
            }            
            return outputs;
        }
    }
}