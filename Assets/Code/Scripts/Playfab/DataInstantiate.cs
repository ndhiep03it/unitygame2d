using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInstantiate : MonoBehaviour
{
    public GameObject OBJ;

    public void InstantiateUser()
    {
        Instantiate(OBJ, transform, false);
    }
}
