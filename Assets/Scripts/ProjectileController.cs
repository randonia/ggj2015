using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public UnitController.UnitTeam Team;
    public GameObject spriteRenderer;
    private int mDamage;

    private const float kLifespan = 2.0f;
    private float mBirthTime;

    public int Damage { get { return mDamage; } set { mDamage = value; } }

    public float mSpeed = 1.0f;
    private Vector2 mDir;

    public Vector2 Direction { set { mDir = value; } }

    // Use this for initialization
    private void Start()
    {
        float rotAngle = Vector2.Angle(Vector2.right, mDir);
        mBirthTime = Time.time;
        spriteRenderer.transform.Rotate(Vector3.forward, rotAngle * ((mDir.y != 0) ? mDir.y : 1));
    }

    // Update is called once per frame
    private void Update()
    {
        if (mBirthTime + kLifespan < Time.time)
        {
            GameObject.Destroy(gameObject);
        }
        transform.Translate(mDir.x * mSpeed * Time.deltaTime, mDir.y * mSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Terrain"))
        {
            GameObject.Destroy(gameObject);
        }
        UnitController uc = other.gameObject.GetComponent<UnitController>();
        if (uc != null && !uc.Team.Equals(Team))
        {
            uc.TakeDamage(Damage, EffectsController.EffectType.EffectRanged);
            GameObject.Destroy(gameObject);
        }
    }
}