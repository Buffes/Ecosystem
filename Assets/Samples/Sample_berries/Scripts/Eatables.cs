using UnityEngine;

public class Eatables : MonoBehaviour
{

    [SerializeField]
    private float lifeSpan = 10f;

    private float lifeTime = 0f;

    void Update()
    {
        lifeTime += Time.deltaTime;
        if(lifeTime>lifeSpan) { Destroy(gameObject); }
    }
}
