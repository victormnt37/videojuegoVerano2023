using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health;
    public int level = 1;
    public float expToLevelUp = 25;
    public float damage;
    public float exp;
    [SerializeField] int healthFrasks = 3;
    public CharacterController player;
    public Animator anim;
    public bool isSprinting = false;
    float sprintDistance = 500f;  // Distancia del sprint en unidades
    public float durationTime = 2f;  // Duración del sprint en segundos
    public float startTime = 0f;  // Temporizador para controlar el tiempo de sprint


    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        // Se mueve el jugador
        if (playerInput.magnitude > 0.01f)
        {
            anim.SetBool("IsMoving", true);
            // Caso 1: Se está moviendo y se presiona la tecla 'E'
            if (Input.GetKeyDown(KeyCode.E))
            {
                startTime = Time.time;
                Debug.Log(startTime);
                // Realizar el sprint en la dirección en la que está mirando el personaje
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                durationTime = Time.time - startTime;
                if (durationTime > 0 && durationTime > 0.1)
                {
                    isSprinting = true;

                    float movementSpeed = isSprinting ? sprintDistance : 1f;  // Define la velocidad de movimiento según el estado de sprint

                    Vector3 sprintMovement = playerInput.normalized * movementSpeed;  // Calcula el desplazamiento del sprint

                    player.Move(sprintMovement * Time.deltaTime);

                }
                startTime = 0f;

            }

        }

        // No se mueve el jugador
        if (playerInput == Vector3.zero)
        {
            StopAnim("IsMoving");
            // Caso 3: Está quieto y se presiona la tecla 'E'
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Realizar el sprint en la dirección en la que está mirando el personaje
                Vector3 sprintDirection = transform.forward;  // Dirección en la que está mirando el personaje
                player.Move(sprintDirection * sprintDistance * Time.deltaTime);  // Aplicar el desplazamiento del sprint en esa dirección
            }
        }

        player.Move(playerInput * 5f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetBool("IsAttacking", true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Heal();
        }

        if (exp >= expToLevelUp)
        {
            LevelUp();
        }
    }

    public void DealDamage()
    {
        //Coge los collider del radio de una esfera
        //Solo golpea si esta en un rango menor o igual que 90º 
        //Se podria cambiar el rango en funcion del arma?¿?¿?¿??¿?¿?¿?¿?¿?¿?¿¿¿?¿?¿?¿¿?¿¿??¿?¿¿¿?¿?¿?¿??¿?¿¿?¿¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿¿?¿?¿?¿?
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 3f);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.gameObject.GetComponent<Enemy>())
            {
                Enemy target = enemy.gameObject.GetComponent<Enemy>();
                Vector3 enemyDirection = (target.transform.position - transform.position).normalized;
                float angleToEnemy = Vector3.Angle(transform.forward, enemyDirection);
                float maxAngle = 90f;
                Debug.Log(angleToEnemy);
                if (angleToEnemy <= maxAngle)
                {
                    target.health -= damage;
                }
            }
        }
    }

    void Heal()
    {
        if (healthFrasks <= 0)
        {
            return;
        }
        healthFrasks--;
        health += 30;
    }
    public void StopAnim(string name)
    {
        //Para poner en false una animación que me daba pereza escribir esto todo el rato
        anim.SetBool(name, false);
    }
    void LevelUp()
    {
        exp -= expToLevelUp;
        expToLevelUp += (expToLevelUp * (float)level * 0.25f);
        level++;
    }
}
