using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody bulletrb;

    int damage;
    [SerializeField] private float velocity = 20;
    [SerializeField] private GameObject bulletVFX;
    private TankPawn _owner;

    public float lifeTime = 5f;
    float shotForce = 1f;
    Vector3 shotOrigin;

    public void InitBullet(TankPawn firingowner, Vector3 direction)
    {
        shotOrigin = transform.position;
        _owner = firingowner;
        bulletrb.velocity = direction * velocity;
        shotForce = firingowner.Parameters.ShotForce;
        damage = firingowner.Parameters.ShotDamage;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Health health))
        {
            health.SetLastDamager(_owner.GetComponent<TankPawn>());
            health.OnDamageTaken(damage);
        }

        if (bulletVFX)
            Instantiate(bulletVFX, transform.position, Quaternion.identity);

        AddForce(other);

        Destroy(gameObject);
    }

    private void AddForce(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        
        if (rb == null) return;

        var dir = (other.transform.position - shotOrigin).normalized;
        rb.AddForce(dir * shotForce);
    }
}
