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
            float mutation = NextGaussianFloat() / 3;
            // E.g., 0.7 becomes 1 / 1.3 = 0.77
            if (mutation < 1) mutation = 1 / (1 + (1 - mutation));

            value += mutation * (value * maxMutationAmount);
            if (value < 0) value = 0;
        }

        /// <summary>
        /// Returns a random float with the standard normal distribution.
        /// </summary>
        private float NextGaussianFloat()
        {
            float u, v, S;
            do
            {
                u = 2.0f * Random.value - 1.0f;
                v = 2.0f * Random.value - 1.0f;
                S = u * u + v * v;
            }
            while (S >= 1.0);
            float fac = Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
            return u * fac;
        }
    }
}
