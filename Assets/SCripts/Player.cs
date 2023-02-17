using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color currentColor;
    public int index;
    [SerializeField] private MeshRenderer mesh;

    private void Awake()
    {
        currentColor = mesh.material.color;
    }



}
