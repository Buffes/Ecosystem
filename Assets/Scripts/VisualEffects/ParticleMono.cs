using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ecosystem.ParticleSystems
{
    public class ParticleMono : MonoBehaviour
    {
        public ParticleSystem birthSystem;
        public ParticleSystem breedSystem;
        public ParticleSystem deathSystem;
        public ParticleSystem drinkSystem;
        public ParticleSystem eatmeatSystem;
        public ParticleSystem eatplantSystem;
        public ParticleSystem killSystem;
        public static ParticleSystem birth;
        public static ParticleSystem breed;
        public static ParticleSystem death;
        public static ParticleSystem drink;
        public static ParticleSystem eatmeat;
        public static ParticleSystem eatplant;
        public static ParticleSystem kill;

        void Start()
        {
            birth = birthSystem;
            breed = breedSystem;
            death = deathSystem;
            drink = drinkSystem;
            eatmeat = eatmeatSystem;
            eatplant = eatplantSystem;
            kill = killSystem;
            
        }

        public static void InstantiateParticles(ParticleSystem ps, Vector3 pos, float time)
        {
           var o = Instantiate(ps, pos, Quaternion.Euler(0, 0, 0));
            Destroy(o, time);
        }
    }
}
