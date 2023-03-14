using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    [SerializeField] GameObject Light;

    Transform Parent;
    List<object> destination = new List<object>();
    void Start()
    {
        Parent = transform.parent;
        var light = Instantiate(Light, Parent.transform.position, Parent.transform.rotation);
        destination.Add(light);

        foreach(var item in particleSystems)
        {
            var system = Instantiate(item, Parent.transform.position, Parent.transform.rotation);
            destination.Add(system.gameObject);
        }
    }

    public void DestroyParticle()
    {
        foreach (var item in destination)
        {
            switch (item)
            {
                case ParticleSystem particle:
                    particle.Stop();
                    break;
                case GameObject light:
                    Destroy(light);
                    break;
            }
        }
    }
}
