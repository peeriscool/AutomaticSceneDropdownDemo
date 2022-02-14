using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleOwner : MonoBehaviour
{
    ParticleSystem controlledSystem;
    ParticleSystem.Particle[] _particles;
   
    // Start is called before the first frame update
    void Start()
    {
        controlledSystem = this.GetComponent<ParticleSystem>();

        if (_particles == null || _particles.Length < controlledSystem.maxParticles)
        {
            _particles = new ParticleSystem.Particle[controlledSystem.maxParticles];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            //stop particle system
            controlledSystem.enableEmission =! controlledSystem.enableEmission;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            controlledSystem.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            controlledSystem.Play(true);
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            controlledSystem.playbackSpeed = controlledSystem.playbackSpeed / 2;
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            controlledSystem.playbackSpeed = controlledSystem.playbackSpeed * 2;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("V is pressed");
            int numparticlesAlive = controlledSystem.GetParticles(_particles);
            for (int i = 0; i < numparticlesAlive; i++)
            {
               
                _particles[i].size = Mathf.InverseLerp(_particles[i].size * _particles[i].lifetime, 0, _particles[i].size/2);
                
            }
            controlledSystem.SetParticles(_particles);
        }

    }
    private void LateUpdate()
    {
       
    }

  
}
