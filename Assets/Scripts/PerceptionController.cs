using System.Collections;
using UnityEngine;

public class PerceptionController : MonoBehaviour
{
    public delegate void PerceptionEnter(Collider2D other);

    public delegate void PerceptionExit(Collider2D other);

    public PerceptionEnter mPerceptionEnterCallback;
    public PerceptionExit mPerceptionExitCallback;

    public GameObject Parent { get { return transform.parent.gameObject; } }

    private GameObject mCurrentTarget;

    public GameObject CurrentTarget { get { return mCurrentTarget; } }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        mCurrentTarget = other.gameObject;
        if (mPerceptionEnterCallback != null)
        {
            mPerceptionEnterCallback(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == mCurrentTarget)
        {
            mCurrentTarget = null;
        }
        if (mPerceptionExitCallback != null)
        {
            mPerceptionExitCallback(other);
        }
    }
}