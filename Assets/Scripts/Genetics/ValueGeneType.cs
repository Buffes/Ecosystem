using UnityEngine;

namespace Ecosystem.Genetics
{
    public class ValueGeneType : IGeneType
    {
        private float maxMutationAmount; // Max amount this gene can mutate per generation

        public ValueGeneType(float maxMutationAmount)
        {
            this.maxMutationAmount = maxMutationAmount;
        }

        public void Mutate(ref float value)
        {
            float amount = Random.value * maxMutationAmount;
            value *= DNA.CoinFlip(1 * (1 + amount), 1 / (1 + amount));
        }
    }
}
