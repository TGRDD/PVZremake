using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SunFlower : Plant
{
    private EconomicSystem _ecosystem;
    private int SunToGive = 25;
    private Animator _animator;
    private ParticleSystem _particleSystem;

    new protected void Start()
    {
        base.Start();
        _particleSystem = GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
        _ecosystem = GameObject.FindObjectOfType<EconomicSystem>();
        StartCoroutine(SpawnASun());
    }


    private IEnumerator SpawnASun()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(10);
            
            _animator.Play("GiveSun");
        }
    }

    public void PlayParticle()
    {
        _source.PlayOneShot(sounds[0]);
        _ecosystem.AddSun(SunToGive);
        _particleSystem.Play();
    }

}
