using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "ScriptableObjects/Data")]
public class PowerUp : ScriptableObject
{
    //
    public GameObject prefab;  // Prefab del power-up
    public int healthModifier;  // Modificador de salud
    public int damageModifier;  // Modificador de daño
    public float speedModifier;  // Modificador de velocidad
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
