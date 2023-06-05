using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TiledMaterial : MonoBehaviour
{
    private Material material;
    [SerializeField] private float scale;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        material.mainTextureScale = transform.lossyScale / scale;
    }
}
