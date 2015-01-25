using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject GO_Player;

    public float CameraSpeed = 1.0f;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 delta = (GO_Player.transform.position - transform.position).normalized;
        delta.z = 0.0f;
        transform.Translate(delta * Time.deltaTime * CameraSpeed);
    }
}