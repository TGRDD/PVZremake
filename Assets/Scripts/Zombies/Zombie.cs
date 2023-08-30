using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Zombie : MonoBehaviour
{
    public static Action OnDied;
    public static Action OnReachHouse;

    [SerializeField] private int DangerLevel;

    [SerializeField] protected int Health;
    [SerializeField] protected float Speed;
    private float ChachedSpeed;

    [SerializeField] protected float MyDamage;

    [SerializeField] private LayerMask PlantLayerMask;

    protected Animator _animator;
    protected Collider _collider;

    protected bool IsAttack;

    private Plant CollisionPlant;

    protected AudioSource _audiosource;
    [SerializeField] protected AudioClip[] Sounds;
    [SerializeField] private AudioClip[] ChompSounds;

    private void Awake()
    {
        ChachedSpeed = Speed;
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _audiosource = GetComponent<AudioSource>();

        Speed = 4;
    }

    public void MoveForward()
    {
        if (Health <= 0) return;

        _animator.speed = Speed;

        Ray ray = new Ray(transform.position - Vector3.up, -Vector3.forward);
        RaycastHit hit;


        _animator.SetBool("IsAttack", IsAttack);
        Debug.DrawRay(transform.position - Vector3.up, -Vector3.forward, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, 2f, PlantLayerMask))
        {
            IsAttack = true;
            if (CollisionPlant == null)
            {
                CollisionPlant = hit.collider.GetComponent<Plant>();
            }

            CollisionPlant.TakeDamage(MyDamage * Time.deltaTime);
        }
        else
        {
            IsAttack = false;
            CollisionPlant = null;
        }


        if (!IsAttack)
        {
            transform.position += -Vector3.forward * Speed * Time.deltaTime;
        }
    }

    public virtual void TakeDamage(int Damage)
    {



        Health -= Damage;
        Debug.Log("Take damage " + Damage);
        if (Health <= 0)
        {
            Debug.Log("Die");
            _animator.Play("Death");
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
        OnDied.Invoke();
    }

    public void disablecollider()
    {
        _collider.enabled = false;
    }

    public int GetDangerLevel()
    {
        return DangerLevel;
    }

    public void ActivateColdEffect()
    {

        if (Health < 0) return;

        StopCoroutine(ColdEffect());
        SetSpeedToChached();
        StartCoroutine(ColdEffect());
    }

    public void SetSpeedToChached()
    {
        Speed = ChachedSpeed;
    }

    private IEnumerator ColdEffect()
    {
        float tmp = Speed;
        Speed /= 2;
        yield return new WaitForSeconds(3);
        Speed = tmp;
    }

    public void PlayFirstChompSound()
    {
        _audiosource.PlayOneShot(ChompSounds[0]);
    }

    public void PlaySecondChompSound()
    {
        _audiosource.PlayOneShot(ChompSounds[1]);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SpedDown")
        {
            Speed = ChachedSpeed;
        }

        if (collision.gameObject.tag == "EndGame")
        {
            SceneManager.LoadScene("GameOverLost");
        }
    }
}
