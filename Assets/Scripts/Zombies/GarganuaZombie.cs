using UnityEngine;

public class GarganuaZombie : Zombie
{
    [SerializeField] private GameObject ImpToSpawn;
    [SerializeField] private GameObject ImpInParent;

    private bool HasImp = true;

    private void Start()
    {
        transform.position += Vector3.up * 6;
    }

    private void Update()
    {
        if (Health < 0 ) return;

        _animator.speed = Speed;

        if (!IsAttack)
        {
            transform.position += -Vector3.forward * Speed * Time.deltaTime;
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.tag == "Plant")
        {
            collision.gameObject.GetComponent<Plant>().Death();
            IsAttack = true;
            _animator.Play("Attack");
        }
    }

    public void DestroyPlant()
    {
        IsAttack = false;
    }

    public override void TakeDamage(int Damage)
    {



        Health -= Damage;
        if (Health <= 0)
        {
            _animator.Play("Death");
            return;
        }

        if (Health < 1000 && HasImp)
        {
            HasImp = false;
            _animator.Play("ThrowImp");
        }
    }

    public void SummonImp()
    {
        Instantiate(ImpToSpawn, transform.position, Quaternion.Euler(0f, -90f, 0f));
        ImpInParent.SetActive(false);
    }
}
