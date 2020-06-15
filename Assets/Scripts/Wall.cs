using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    int initX = -5;
    int initY = 0;
    [SerializeField]
    Transform cube;
    // Start is called before the first frame update
    void Start()
    {
        Transform instance = Instantiate(cube);
        instance.localPosition = new Vector3(0,5,5);
        instance.localScale = new Vector3(10,10,1);

    }


}
