using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    //
    public PowerUp powerUp;
    PowerUpManager _powerUpManager;
    public Transform player;
    public GameObject hatPrefab;  // Prefab del sombrero
    public GameObject heart;  // Prefab del sombrero


    public int health = 100;
    public int damage = 10;
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        _powerUpManager = FindObjectOfType<PowerUpManager>();
        if (_powerUpManager == null)
        {
            Debug.LogError("No PowerUpManager found in the scene!");
        }
    }


    void Update()
    {

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));  // Crear un vector de movimiento

        transform.Translate(movement * speed * Time.deltaTime);  // Mover el objeto según el vector de movimiento
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hammer"))
        {
            Debug.Log(powerUp.damageModifier);
            damage = _powerUpManager.ModifyDamage(powerUp.damageModifier);

        }
        else if (other.CompareTag("beam"))
        {
            speed = _powerUpManager.ModifySpeed(powerUp.speedModifier);
        }
        else if (other.CompareTag("heart"))
        {
            health = _powerUpManager.ModifyHealth(powerUp.healthModifier);


        }

    }






}
