using System.Collections;
using UnityEngine;

public class PerceptionController : MonoBehaviour
{
    public delegate void PerceptionEnter(Collider2D other);

    public delegate void PerceptionExit(Collider2D other);

    public PerceptionEnter mPerceptionEnterCallback;
    public PerceptionExit mPerceptionExitCallback;

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
        mPerceptionEnterCallback(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        mPerceptionExitCallback(other);
    }
}