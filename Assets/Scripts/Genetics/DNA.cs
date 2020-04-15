using System;
using System.Collections.Generic;
using Unity.Entities;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Ecosystem.Genetics
{
    /// <summary>
    /// Contains gene information differentiating entities of the same species from each other.
    /// <para/>
    /// A gene is represented by a float value which has a chance to be mutated by an amount in
    /// each new generation. For example, if a gene with the value 1 is mutated by 30%, it becomes
    /// either 1 * 1.3 = 1.3 or 1 / 1.3 = 0.77.
    /// </summary>
    public class DNA : IComponentData
    {
        private class Gene
        {
            public float Value;
            public IGeneType Type;

            public Gene(float value, IGeneType type)
            {
                Value = value;
                Type = type;
            }
        }

        private float mutationRate; // Chance of mutation per gene per generation
        private float maxMutationAmount; // Max amount a gene can mutate per generation

        public bool IsMale { get; private set; }
        private List<Gene> genes = new List<Gene>();

        private int lastGeneIndex = -1;

        public DNA() : this(false) { }
        private DNA(bool isMale, List<Gene> genes = null,
            float mutationRate = 0.2f, float maxMutationAmount = 0.3f)
        {
            IsMale = isMale;
            this.genes = genes ?? new List<Gene>();
            this.mutationRate = mutationRate;
            this.maxMutationAmount = maxMutationAmount;
        }

        /// <summary>
        /// Applies the next gene to the value. Adds a new default gene with the given value if past the
        /// current total genes count.
        /// </summary>
        public void NextGene(ref float value) => value = NextGene(value);

        /// <summary>
        /// Returns the next gene value. Adds a new default gene with the default value if past the
        /// current total genes count.
        /// </summary>
        public float NextGene(float defaultValue)
            => NextGene(defaultValue, DefaultGeneType);

        /// <summary>
        /// Returns the next gene value. Adds a new gene with the default value if past the current
        /// total genes count.
        /// </summary>
        public float NextGene(float defaultValue, IGeneType geneType)
        {
            if (++lastGeneIndex < genes.Count) return genes[lastGeneIndex].Value;
            genes.Add(new Gene(defaultValue, geneType));
            return defaultValue;
        }

        public IGeneType DefaultGeneType => new ValueGeneType(maxMutationAmount);

        /// <summary>
        /// Returns new DNA with default genes and a random sex.
        /// </summary>
        public static DNA DefaultGenes() => DefaultGenes(CoinFlip());

        /// <summary>
        /// Returns new DNA with default genes.
        /// </summary>
        public static DNA DefaultGenes(bool isMale) => new DNA(isMale);

        /// <summary>
        /// Returns new DNA with a random sex and with genes based on the parents' DNA.
        /// </summary>
        public static DNA InheritedDNA(DNA parent1, DNA parent2)
        {
            if (parent1.genes.Count != parent2.genes.Count) throw ParentGeneMismatchException;

            int amount = parent1.genes.Count;
            List<Gene> genes = new List<Gene>(amount);
            for (int i = 0; i < amount; i++)
            {
                if (parent1.genes[i].Type.GetType() != parent2.genes[i].Type.GetType()) throw ParentGeneMismatchException;
                genes.Add(InheritedGene(parent1.genes[i], parent2.genes[i], parent1.mutationRate));
            }

            return new DNA(CoinFlip(), genes);
        }
        
        private static Gene InheritedGene(Gene gene1, Gene gene2, float mutationRate)
        {
            Gene selectedGene = CoinFlip(gene1, gene2);
            if (Roll(mutationRate)) selectedGene.Type.Mutate(ref selectedGene.Value);
            return selectedGene;
        }

        public static T CoinFlip<T>(T heads, T tails) => CoinFlip() ? heads : tails;

        public static bool CoinFlip() => Roll(0.5f);

        public static bool Roll(float chance) => Random.value < chance;

        private static Exception ParentGeneMismatchException => new ArgumentException("The parents' genes do not have the same format");
    }
}
