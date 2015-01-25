using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public UnitController.UnitTeam Team;

    private int mDamage;

    public int Damage { get { return mDamage; } set { mDamage = value; } }

    public float mSpeed = 1.0f;
    private Vector2 mDir;

    public Vector2 Direction { set { mDir = value; } }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(mDir.x * Time.deltaTime * mSpeed, mDir.y * Time.deltaTime * mSpeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        UnitController uc = other.gameObject.GetComponent<UnitController>();
        if (uc != null && !uc.Team.Equals(Team))
        {
            uc.TakeDamage(Damage);
            GameObject.Destroy(gameObject);
        }
    }
}