using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Neuron
{
    public struct TrainingSet
    {
        public float[] inputs;
        public float[] expectedResults;
    }

    public class Trainer : GeneticAlgorithm
    {
        public Trainer(TrainingSet[] trainingSet, NeuralNet net)
        {
            this.trainingSet = trainingSet;
            this.net = net;
        }
        public void Train(int populationSize=3000, int maxGenerations=3000)
        {
            Init(populationSize, net.WeightsNeeded);

            Genome best = new Genome();            
            for (int x = 0; x < maxGenerations && best.fitness < 1; x++)
            {
                best = GenerationCycle();                
            }
            net.Weights = best.chromosome;

            if (best.fitness < 1) Console.ForegroundColor = ConsoleColor.DarkRed;
            else Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Fitness: " + best.fitness);
            Console.ResetColor();            
        }
        private readonly NeuralNet net;
        private readonly TrainingSet[] trainingSet;

        protected override float calculateFitness(float[] chromosome)
        {
            net.Weights = chromosome;
            float total = 0;
            for (var i = 0; i < trainingSet.Length; i++)
            {
                var training = trainingSet[i];
                var results = net.Solve(training.inputs);                                              
                total += dist(results, training.expectedResults);
            }
            var fitness = total / trainingSet.Length;
            return fitness;
        }
    }

    public class Training
    {
        public static Training Helper {
            get { return new Training(); }
        }

        private List<TrainingSet> trainingSets;
        private TrainingSet currentTrainingSet;
        private int inputCounter;
        private int outputCounter;

        public Training()
        {
            trainingSets = new List<TrainingSet>();
            inputCounter = 0;
            outputCounter = 0;
        }

        public Training When(params float[] inputs)
        {
            currentTrainingSet = new TrainingSet();
            currentTrainingSet.inputs = inputs;
            if (inputs.Length > inputCounter) inputCounter = inputs.Length;
            return this;
        }

        
        public Training Then(params float[] expect)
        {
            currentTrainingSet.expectedResults = expect;
            if (expect.Length > outputCounter) outputCounter = expect.Length;
            trainingSets.Add(currentTrainingSet);
            return this;
        }


        public NeuralNet Train(params int[] hiddenLayers)
        {            
            NeuralNet n;
            if (hiddenLayers == null)
                n = new NeuralNet(inputCounter, outputCounter);
            else
            {
                var layers = new List<int>();
                layers.Add(inputCounter);
                layers.AddRange(hiddenLayers);
                layers.Add(outputCounter);
                n = new NeuralNet(layers.ToArray());
            }
            var trainer = new Trainer(trainingSets.ToArray(), n);
            trainer.Train();
            return n;
        }
    }
}
