using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class BombFactory : ScriptableObject
{
    [SerializeField]
    Bomb Bomb1;
    List<Bomb> pool;

    public Bomb Get()
    {
        Bomb instance = Instantiate(Bomb1);
        return instance;
    }
    void CreatePool()
    {
        pool = new List<Bomb>();
    }

}
