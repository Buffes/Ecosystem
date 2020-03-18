using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Ecosystem.Genetics
{
    /// <summary>
    /// Contains gene information differentiating entities of the same species from each other.
    /// <para/>
    /// A gene is represented by a float value starting at default = 1. It then has a chance to be
    /// mutated by an amount in each new generation. For example, if it is mutated by 30% it becomes
    /// either 1 * 1.3 = 1.3 or 1 / 1.3 = 0.77.
    /// </summary>
    public class DNA : IComponentData
    {
        private const float MUTATION_CHANCE = 0.2f; // Chance of mutation per gene
        private const float MAX_MUTATION_AMOUNT = 0.3f; // Max amount a gene can mutate per generation

        private const float DEFAULT_GENE = 1f; // Default value of a new gene

        public bool IsMale { get; private set; }
        private float[] genes;

        private int lastGeneIndex = -1;

        public DNA() : this(false) { }
        private DNA(bool isMale, float[] genes = null)
        {
            IsMale = isMale;
            this.genes = genes ?? new float[0];
        }

        /// <summary>
        /// Returns the next gene, or a new default gene if past the length of the genes array.
        /// <para/>
        /// Use it to iterate through all genes that you are interested in and then call
        /// <see cref="UpdateGenes"/> to resize the genes array.
        /// </summary>
        public float NextGene()
        {
            return ++lastGeneIndex < genes.Length ? genes[lastGeneIndex] : DEFAULT_GENE;
        }

        /// <param name="appliedTo">A variable to apply the gene to (multiplied with)</param>
        public float NextGene(ref float appliedTo)
        {
            float gene = NextGene();
            appliedTo *= gene;
            return gene;
        }

        /// <summary>
        /// Updates the genes array to match the amount of genes iterated by <see cref="NextGene"/>.
        /// </summary>
        public void UpdateGenes()
        {
            int diff = lastGeneIndex + 1 - genes.Length;

            // Append a new default gene value for every new gene
            if (diff > 0) genes = genes.Concat(Enumerable.Repeat(DEFAULT_GENE, diff)).ToArray();
            // Truncate to the new amount of genes
            else if (diff < 0) System.Array.Resize(ref genes, genes.Length + diff);
        }

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
            if (parent1.genes.Length != parent2.genes.Length)
            {
                throw new System.ArgumentException("The parents' genes do not have the same format");
            }

            float[] genes = new float[parent1.genes.Length];

            for (int i = 0; i < genes.Length; i++)
            {
                genes[i] = InheritedGene(parent1.genes[i], parent2.genes[i]);
            }

            return new DNA(CoinFlip(), genes);
        }

        private static float InheritedGene(float gene1, float gene2)
            => Mutation(CoinFlip(gene1, gene2), MAX_MUTATION_AMOUNT, MUTATION_CHANCE);

        private static float Mutation(float gene, float maxAmount, float chance)
            => Roll(chance) ? Mutation(gene, Random.value * maxAmount, CoinFlip()) : gene;

        private static float Mutation(float gene, float amount, bool up)
        => gene * (up ? 1 * (1 + amount) : 1 / (1 + amount));

        private static T CoinFlip<T>(T heads, T tails) => CoinFlip() ? heads : tails;

        private static bool CoinFlip() => Roll(0.5f);

        private static bool Roll(float chance) => Random.value < chance;
    }
}
