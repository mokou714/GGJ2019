using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrinkParticle : MonoBehaviour {


    private ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
	// Use this for initialization
	void Start () {
        //InitializeIfNeeded();
        ////m_System = GetComponent<ParticleSystem>();
        //int numParticlesAlive = m_System.GetParticles(m_Particles);

        //// Change only the particles that are alive
        //for (int i = 0; i < numParticlesAlive; i++)
        //{
        //    m_Particles[i].startSize = 20;
        //}
        //m_System.SetParticles(m_Particles, numParticlesAlive);
	}
	

	// Update is called once per frame
	void Update () {
        
	}

    void InitializeIfNeeded()
    {
        if (m_System == null)
            m_System = GetComponent<ParticleSystem>();

        if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
            m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
    }
}
