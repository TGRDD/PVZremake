using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int Damage = 20;
    [SerializeField] private LayerMask ZombieLayer;
    [SerializeField] private float Speed;

    private float LifeTime = 10f;

    [SerializeField] GameObject particleprefab;
    [SerializeField] ProjectileModificator _projectileModificator;
    [SerializeField] Trajectory _trajectory;

    [HideInInspector] public Vector3 StartPosition;
    [HideInInspector] public Vector3 Spot;
    [HideInInspector] public float G;
    private float DistanceToSpot;

    private void Start()
    {
        if (_trajectory == Trajectory.Mounted)
        {
            DistanceToSpot = Vector3.Distance(StartPosition, Spot);
        }
    }

    void FixedUpdate()
    {

        if (_trajectory == Trajectory.Forward)
        {
            transform.position += Vector3.forward * Speed * Time.deltaTime;
        }
        
        if (_trajectory == Trajectory.Mounted)
        {
            if (G < 0) G = 1;

            if (Vector3.Distance(new Vector3(transform.position.x, StartPosition.y, transform.position.z), Spot) > DistanceToSpot/2)
            {
                G -= Time.deltaTime * 12;
                transform.position += ((Vector3.forward * Speed) + (Vector3.up * G)) * Time.deltaTime;
            }
            else
            {
                G += Time.deltaTime * 12;
                transform.position += ((Vector3.forward * Speed) + (-Vector3.up * G)) * Time.deltaTime;
            }
        }

        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.tag == "Zombie")
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            zombie.TakeDamage(Damage);

            switch (_projectileModificator)
            {
                case ProjectileModificator.Ice:
                    zombie.ActivateColdEffect();
                    break;

                default: break;
            }


            Instantiate(particleprefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private enum ProjectileModificator
    {
        None,
        Ice
    }

    private enum Trajectory
    {
        Forward,
        Mounted
    }
}
