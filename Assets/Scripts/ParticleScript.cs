using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ParticleScript : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        switch (other.name)
        {
            case "Bomber":
                other.GetComponent<PlayerScript>().GotHit();
                break;
            case "Enemy(Clone)":
                other.GetComponent<EnemyScript>().GotHit();
                break;
        }
    }

    void OnParticleTrigger()
    {

    }
}