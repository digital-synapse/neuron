using System;
using System.Linq;
using System.Threading.Tasks;

namespace Neuron
{
    public struct Genome
    {
        public float[] chromosome;
        public float fitness;
    }

    public abstract class GeneticAlgorithm
    {

        protected void Init(int populationSize, int chromosomeSize, double mutationRate = 0.3, double crossoverRate = 0.7, double learningRate = 0.1)
        {
            // start with random population
            population = new float[populationSize][];
            for (var i = 0; i < populationSize; i++)
            {
                population[i] = randomArray(chromosomeSize);
            }        
            fitness = new float[populationSize];

            // get the fitness score for the entire population
            getFitnessScoresAndSum();

            // save parameters
            this.mutationRate = mutationRate;
            this.crossoverRate = crossoverRate;
            this.learningRate = learningRate;
            this.populationSize = populationSize;
            this.chromosomeSize = chromosomeSize;
            this.chromosomeBytes = chromosomeSize*sizeof (float);
        }

        // performs another generation and returns the most fit genome
        public Genome GenerationCycle()
        {            
            int i;
            double r;
            float f;            

            // get random genomes (roulette selection)            
            for (r = rng.NextDouble() * fitnessSum, i=0; r > 0;) r -= fitness[i++];
            int a = i - 1;
            var chromosomeA = population[a];
            for (r = rng.NextDouble() * fitnessSum, i = 0; r > 0;) r -= fitness[i++];
            int b = i - 1;
            var chromosomeB = population[b];            

            // perform crossover
            if (rng.NextDouble() < crossoverRate)
            {
                var cut = (int)Math.Floor(rng.NextDouble()*chromosomeSize);
                for (i = cut; i < chromosomeSize; i++)
                {
                    f = chromosomeA[i];
                    chromosomeA[i]= chromosomeB[i];
                    chromosomeB[i] = f;
                }
            }

            // perform mutations
            if (rng.NextDouble() < mutationRate)
            {
                i = (int)Math.Floor(rng.NextDouble() * chromosomeSize);
                f = (float)(learningRate*(rng.NextDouble() - 0.5)*(1 - fitness[a]));
                chromosomeA[i] += f;
            }
            if (rng.NextDouble() < mutationRate)
            {
                i = (int)Math.Floor(rng.NextDouble() * chromosomeSize);
                f = (float)(learningRate * (rng.NextDouble() - 0.5) * (1 - fitness[b]));
                chromosomeB[i] += f;
            }

            // get the fitness score for the entire population ( for next generation)
            getFitnessScoresAndSum();

            // find the best and worst genome
            int bi = 0;
            int wi = 0;
            for (i = 1; i < population.Length; i++)
            {
                if (fitness[i] > fitness[bi]) bi = i;
                if (fitness[i] < fitness[wi]) wi = i;
            }

            // copy the best over the worst (best genome gets to become self replicating)
            if (bi != wi) Buffer.BlockCopy(population[bi], 0, population[wi], 0, chromosomeBytes);            

            // return the best genome
            return new Genome() {chromosome = population[bi], fitness = fitness[bi]};
        }

        /// <summary>
        /// This method calculates the fitness score for the given chromosome
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        protected abstract float calculateFitness(float[] chromosome);

        /// <summary>
        /// Helper method to return the avg distance between 2 numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected float dist(float a, float b)
        {
            return 1/ (((a > b) ? a - b : b - a)+1);
        }

        /// <summary>
        /// Helper method to return the avg distance between 2 numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected float dist(float[] a, float[] b)
        {
            float s = 0;
            for (var i = 0; i < a.Length; i++)
            {
                s+= dist(a[i], b[i]);
            }
            return s;// / a.Length;
        }
        protected float[][] population;
        protected float[] fitness;
        protected float fitnessSum;
        private Random rng = new Random();
        private float[] randomArray(int length, float min = -2, float max = 2)
        {
            
            float[] arr = new float[length];
            for (var i = 0; i < length; i++)
            {
                double range = max - min;
                double sample = rng.NextDouble();
                double scaled = (sample * range) + min;
                arr[i] = (float)scaled;
            }
            return arr;
        }

        private double mutationRate;
        private double crossoverRate;
        private double learningRate;
        private int chromosomeSize;
        private int populationSize;
        private int chromosomeBytes;

        private void getFitnessScoresAndSum()
        {
            // get the fitness score for the entire population
            for (var i = 0; i < population.Length; i++)
            {
                fitness[i] = this.calculateFitness(this.population[i]);
            }
            fitnessSum = fitness.Sum();
        }
    }
}