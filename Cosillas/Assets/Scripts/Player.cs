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

    //Que tenga un target para el parry
    public GameObject playersTarget;
    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (playerInput.magnitude > 0.01f) {
            anim.SetBool("IsMoving", true);
        }
        
        if (playerInput == Vector3.zero) {
            StopAnim("IsMoving");
        }
        
        player.Move(playerInput * 5f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.T)) {
            anim.SetBool("IsAttacking", true);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Heal();
        }

        if (exp >= expToLevelUp) {
            LevelUp();
        }
    }

    public void DealDamage() {
        //Coge los collider del radio de una esfera
        //Solo golpea si esta en un rango menor o igual que 90º 
        //Se podria cambiar el rango en funcion del arma?¿?¿?¿??¿?¿?¿?¿?¿?¿?¿¿¿?¿?¿?¿¿?¿¿??¿?¿¿¿?¿?¿?¿??¿?¿¿?¿¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿?¿¿?¿?¿?¿?
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 3f);

        foreach (Collider enemy in hitEnemies){
            if (enemy.gameObject.GetComponent<Enemy>()) {
                Enemy target = enemy.gameObject.GetComponent<Enemy>();
                Vector3 enemyDirection = (target.transform.position - transform.position).normalized;
                float angleToEnemy = Vector3.Angle(transform.forward, enemyDirection);
                float maxAngle = 90f;
                Debug.Log(angleToEnemy);
                playersTarget = enemy.gameObject;
                if (angleToEnemy <= maxAngle) {
                    target.health -= damage;
                }
            }
        }
    }

    void Heal() {
        if (healthFrasks <= 0) {
            return;
        }
        healthFrasks--;
        health += 30;
    }
    public void StopAnim(string name) {
        //Para poner en false una animación que me daba pereza escribir esto todo el rato
        anim.SetBool(name, false);
    }
    void LevelUp() {
        exp -= expToLevelUp;
        expToLevelUp += (expToLevelUp * (float)level * 0.25f);
        level++;
    }
}
