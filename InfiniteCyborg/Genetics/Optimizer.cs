using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Genetics
{
    public class Optimizer
    {
        public float TargetFitness { get; set; }

        public int OutputLength { get; set; }

        public int NumGenerations { get; set; }

        public int ChildrenPerGeneration { get; set; }

        public Func<bool> Randomizer { get; set; }

        public Optimizer(Func<bool> randomizer, int outputLength)
        {
            this.Randomizer = randomizer;
            this.OutputLength = outputLength;
            this.TargetFitness = float.MaxValue;
            this.NumGenerations = 1;
            this.ChildrenPerGeneration = 2;
        }

        /// <summary>
        /// Generates a single randomly near-optimal bitfield for a fitness test.
        /// </summary>
        /// <param name="fitness">A fitness test rating the effectiveness of a bitfield</param>
        /// <returns>An optimised bitfield</returns>
        public BitField OptimiseOne(Func<BitField, float> fitness)
        {
            // Float helps us from losing bits when len % perGen != 0
            int BitsPerChild = OutputLength / ChildrenPerGeneration;

            // First parent is always random
            BitField output = new BitField(OutputLength).Randomize(this.Randomizer);
            float outputFitness = fitness(output);

            BitField parent = output;

            for (int gen = 0; gen < NumGenerations; ++gen)
            {
                float bestChildFitness = float.MinValue;
                BitField bestChild = null;
                for (int i = 0; i < ChildrenPerGeneration; ++i)
                {
                    BitField child = new BitField(OutputLength).Randomize(this.Randomizer);
                    child.CopyBits(parent, BitsPerChild * i, BitsPerChild);

                    // TODO: Mutations. Could be as simple as doing a RandomOR with the parent bits

                    float childFitness = fitness(child);
                    if (childFitness >= bestChildFitness)
                    {
                        bestChildFitness = childFitness;
                        bestChild = child;
                    }

                    if (childFitness >= outputFitness)
                    {
                        outputFitness = childFitness;
                        output = child;
                    }
                }

                parent = bestChild;
            }

            return output;
        }
    }
}
