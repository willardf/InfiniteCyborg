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
        public Func<bool> Randomizer { get; set; }

        public Optimizer(Func<bool> randomizer)
        {
            this.Randomizer = randomizer;
        }

        /// <summary>
        /// Generates a single randomly near-optimal bitfield for a fitness test.
        /// </summary>
        /// <param name="fitness">A fitness test with returns bounded [0, 1]</param>
        /// <param name="outputLen">The length of the bitfield to return</param>
        /// <param name="generations">The number of generations to test</param>
        /// <param name="perGen">The number of children to produce per generation</param>
        /// <returns>A</returns>
        public BitField OptimiseOne(Func<BitField, float> fitness, int outputLen, int generations = 1, int perGen = 2)
        {
            // Float helps us from losing bits when len % perGen != 0
            float BitsPerChild = outputLen / (float)perGen;

            // First parent is always random
            BitField output = new BitField(outputLen).Randomize(this.Randomizer);
            float outputFitness = fitness(output);

            BitField parent = output;

            for (int gen = 0; gen < generations; ++gen)
            {
                SortedList<float, BitField> children = new SortedList<float, BitField>();
                for (int i = 0; i < perGen; ++i)
                {
                    BitField child = new BitField(outputLen).Randomize(this.Randomizer);
                    child.CopyBits(parent, (int)(i * BitsPerChild), (int)BitsPerChild);

                    // TODO: Mutations. Could be as simple as doing a RandomOR with the parent bits

                    children.Add(fitness(child), child);
                }

                var bestChild = children.First();
                parent = bestChild.Value;
                if (bestChild.Key >= outputFitness)
                {
                    outputFitness = bestChild.Key;
                    output = bestChild.Value;
                }
            }

            return output;
        }
    }
}
