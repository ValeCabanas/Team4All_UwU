using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coches : MonoBehaviour
{
    public GameObject PrefabCoche;
    public int NumeroCoches = 10;
    List<GameObject> losCoches;

    // Start is called before the first frame update
    void Start()
    {
        losCoches = new List<GameObject>();
        for(int i = 0; i < NumeroCoches; i++)
        {
            float x = Random.Range(-10, 10);
            float y = 0;
            float z = Random.Range(-10, 10);
            losCoches.Add(Instantiate(PrefabCoche, new Vector3(x, y, z), Quaternion.Euler(90, x*3, 0)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
