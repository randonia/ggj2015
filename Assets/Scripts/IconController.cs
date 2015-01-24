using System.Collections;
using UnityEngine;

public class IconController : MonoBehaviour
{
    public Sprite[] Sprites;

    public Sprite CurrentSprite { get { return Sprites[SpriteIndex]; } }

    public int SpriteIndex;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}