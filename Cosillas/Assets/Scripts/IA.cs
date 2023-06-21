using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IA : NPC
{
    public bool hasTarget;
    public Transform playerPos;
    public int healthFrasks = 3;
    public enum State {
        Follow,
        Attack,
        Hold,
        Flee
    }

    /*
    Que tenga un medidor de "cansancio"
    Que de ese cansancio no pueda caminar o correr si no tiene la energia suficiente
    Que de vueltas al rededor del enemigo si no tiene la energia suficiente o se eche hacia atrás
    Obviamente que vaya cambiando de estado según cada cosa
    */ 
    [SerializeField] State currentState; 
    // Start is called before the first frame update
    protected override void Start()
    {
        //base.Start() llama al start de la clase NPC es decir el padre
        base.Start();
        currentState = State.Follow;
        //Es un gameObject que esta detrás del jugador para que cuando no haya enemigos se vaya ahi
        playerPos = GameObject.FindGameObjectWithTag("AllyPosition").transform; 
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        switch(currentState) {
            case State.Follow:
                Follow();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Hold:
                Hold();
                break;
            case State.Flee:
                Flee();
                break;
        }
        stateText.text = currentState.ToString();
        if (target == null) {
            hasTarget = false;
            currentState = State.Follow;
        }

        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }

    void Follow() {
        transform.position = Vector3.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
        RotateTowardsTarget(playerPos.transform.position);
        if (hasTarget) {
            currentState = State.Attack;
        }
        StopAnim("IsAttacking");
        RecoverEnergy();
    }

    void Attack() {
        if (!hasTarget) {
            currentState = State.Follow;
        }
        float distance = Vector3.Distance(transform.position, target.transform.position);
        RotateTowardsTarget(target.transform.position);
        if (distance > 1.5f) {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(target.transform);
        } else {
            anim.SetBool("IsAttacking", true);
        }

        if (currentHealth <= (maxHealth * 30/100) && healthFrasks >= 1) {
            currentState = State.Flee;
        }

        if (target.GetComponent<Enemy>().IsAnimActive("IsDead")) {
            target = GetClosestTarget();
        }
    }

    void Hold() {
        //Para que se quede quieto
        //Podria utilizarse en cualquier momento
        StopAnim("IsAttacking");
        return;
    }

    void Flee() {
        //Para que huya y se cure si es posible
        Vector3 fleeDirection = (transform.position - target.transform.position).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * 5f;

        transform.position = Vector3.MoveTowards(transform.position, fleePosition, speed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > 10f) {
            Heal();
        }
        StopAnim("IsAttacking");
        RecoverEnergy();

    }

    void Heal() {
        if (healthFrasks <= 0) {
            return;
        }
        healthFrasks--;
        currentHealth += 30;
        if ((currentHealth >= (maxHealth * 30 /100) && hasTarget) || healthFrasks <= 0) {
            currentState = State.Attack;
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            target = GetClosestTarget();
            if (target != null) {
                hasTarget = true;
            }
        }
    }

    public GameObject GetClosestTarget() {
        GameObject currentTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject targetGO in GameObject.FindGameObjectsWithTag("Enemy")) {
            float distance = Vector3.Distance(transform.position, targetGO.transform.position);

            if (distance < closestDistance && !targetGO.GetComponent<Enemy>().IsAnimActive("IsDead")) {
                closestDistance = distance;
                currentTarget = targetGO;
            }

        }
        Debug.Log(currentTarget);
        return currentTarget;
    }
    public void DealDamage() {
        //Que haga mas daño según la resistencia o que la animación sea más corta? VOTEN!!!
        float variableDamage = damage * (currentResistance / 100);
        Debug.Log(variableDamage);

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 3f);

        foreach (Collider enemy in hitEnemies){
            if (enemy.gameObject.GetComponent<Enemy>()) {
                Enemy target = enemy.gameObject.GetComponent<Enemy>();
                Vector3 enemyDirection = (target.transform.position - transform.position).normalized;
                float angleToEnemy = Vector3.Angle(transform.forward, enemyDirection);

                float maxAngle = 90f;
                
                if (angleToEnemy <= maxAngle) {
                    target.health -= damage;
                }
            }
        }
    }
}
