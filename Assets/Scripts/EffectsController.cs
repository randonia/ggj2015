using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    /// <summary>
    /// Make sure these map to the gameobject names.
    /// </summary>
    public enum EffectType
    {
        EffectMelee,
        EffectRanged,
        Portal
    }

    public GameObject[] GO_emitters;

    private Dictionary<string, ParticleSystem> effectSystems;

    // Use this for initialization
    private void Start()
    {
        effectSystems = new Dictionary<string, ParticleSystem>();
        foreach (GameObject go in GO_emitters)
        {
            effectSystems.Add(go.name, go.GetComponent<ParticleSystem>());
        }
    }

    public void EmitEffect(EffectType effect)
    {
        if (effectSystems.ContainsKey(effect.ToString()))
        {
            effectSystems[effect.ToString()].Play();
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}