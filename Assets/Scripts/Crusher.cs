using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour
{
    public bool IsAlreadyActivated { get; private set; }
    private ParticleSystem _particlesystem;
    private AudioSource _audioSource;

    private void Start()
    {
        _particlesystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        StartCoroutine(StartMove());
        IsAlreadyActivated = true;
    }

    private IEnumerator StartMove()
    {
        for (int i = 0; i < 250; i++) 
        {
            transform.position += Vector3.forward * i * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Zombie")
        {
            if (!IsAlreadyActivated)
            {
                Activate();
                _particlesystem.Play();
                _audioSource.Play();
            }

            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            zombie.TakeDamage(999999);
        }
    }
}
