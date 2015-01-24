using UnityEngine;
using System.Collections;

public class PerceptionController : MonoBehaviour {

    public delegate void PerceptionEnter(Collider2D other);
    public delegate void PerceptionExit(Collider2D other);

    public PerceptionEnter mPerceptionEnterCallback;
    public PerceptionExit mPerceptionExitCallback;

	// Use this for initialization
	void Start () { }

	// Update is called once per frame
	void Update () { }

    void OnTriggerEnter2D(Collider2D other)
    {
        mPerceptionEnterCallback(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        mPerceptionExitCallback(other);
    }
}
