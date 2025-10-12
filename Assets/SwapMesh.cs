using System;
using UnityEngine;

public class SwapMesh : MonoBehaviour
{
    public GameObject Deer, Lizard;

    private void Start()
    {
        print(UnityEngine.Random.Range(0, 2));
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            Deer.SetActive(false);
            Lizard.SetActive(true);
        }
        else
        {
            Lizard.SetActive(false);
            Deer.SetActive(true);
        }
    }
}
