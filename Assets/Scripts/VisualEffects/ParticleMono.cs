using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ecosystem.ParticleSystems
{
    public class ParticleMono : MonoBehaviour
    {
        public GameObject birthSystem;
        public GameObject breedSystem;
        public GameObject deathSystem;
        public GameObject drinkSystem;
        public GameObject eatmeatSystem;
        public GameObject eatplantSystem;
        public GameObject killSystem;
        public static GameObject birth;
        public static GameObject breed;
        public static GameObject death;
        public static GameObject drink;
        public static GameObject eatmeat;
        public static GameObject eatplant;
        public static GameObject kill;

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

        public static void InstantiateParticles(GameObject ps, Vector3 pos, float time)
        {
            GameObject o = (GameObject)Instantiate(ps, pos, Quaternion.Euler(0, 0, 0));
            Destroy(o, time);
        }
    }
}
