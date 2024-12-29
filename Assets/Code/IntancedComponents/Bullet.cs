using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody bulletrb;

    int damage;
    [SerializeField] private float velocity = 20;
    [SerializeField] private GameObject bulletVFX;
    private TankPawn _owner;

    public float lifeTime = 5f;

    public void InitBullet(TankPawn firingowner, Vector3 direction)
    {
        _owner = firingowner;
        bulletrb.velocity = direction * velocity;
        damage = firingowner.Parameters.ShotDamage;
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Health health))
        {
            health.SetLastDamager(_owner.GetComponent<TankPawn>());
            health.OnDamageTaken(damage);
        }

        if(bulletVFX)
            Instantiate(bulletVFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
