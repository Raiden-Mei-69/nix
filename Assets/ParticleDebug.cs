using UnityEngine;

public class ParticleDebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Part Collision");
    }

    private void OnParticleTrigger()
    {
        Debug.Log("Part Trigger");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
    }
}
