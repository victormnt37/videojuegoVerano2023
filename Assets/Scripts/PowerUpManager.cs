using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    
    public PowerUp[] powerUps;  // Array de Scriptable Objects con los diferentes objetos de power-up

    PlayerPowerUp player;

    public void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPowerUp>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto atravesado es el jugador
        if (other.CompareTag("Player"))
        {
            // Obtener un índice aleatorio para seleccionar un power-up del array
            int randomIndex = Random.Range(0, powerUps.Length);

            // Obtener el power-up seleccionado aleatoriamente
            PowerUp selectedPowerUp = powerUps[randomIndex];
            Instantiate(selectedPowerUp.prefab, new Vector3(transform.position.x + 2, 2f, transform.position.z + 2), Quaternion.identity);
            // Instanciar el prefab en la posición del activador
            // Instantiate(selectedPowerUp, transform.position, Quaternion.identity);
            // Aplicar las características del power-up al jugador (asumiendo que el jugador tiene un script Player)
            // Player player = other.GetComponent<Player>();
            // if (player != null)
            // {
            //     player.ModifyHealth(selectedPowerUp.healthModifier);
            //     player.ModifyDamage(selectedPowerUp.damageModifier);
            //     player.ModifySpeed(selectedPowerUp.speedModifier);
            // }

            // Eliminar el objeto de power-up
            Destroy(gameObject);
        }

    }
    public int ModifyHealth(int healthModifier)
    {
        player.health += healthModifier;
        Debug.Log("Health modified by: " + healthModifier + ". Current health: " + player.health);
        return player.health;
    }

    public int ModifyDamage(int damageModifier)
    {
        player.damage += damageModifier;
        Debug.Log("Damage modified by: " + damageModifier + ". Current damage: " + player.damage);
        return player.damage;
    }

    public float ModifySpeed(float speedModifier)
    {
        player.speed += speedModifier;
        Debug.Log("Speed modified by: " + speedModifier + ". Current speed: " + player.speed);
        return player.speed;
    }

}
