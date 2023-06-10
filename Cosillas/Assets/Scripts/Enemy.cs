using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float health;
    public float damage;
    public float expToGive;
    public float timer;
    public GameObject target;
    public GameObject player;
    public float distance;
    public Animator anim;
    public enum State {
        Idle,
        Patroll,
        Walking,
        Attacking,
        Casting,
        Die
    }

    public TMP_Text stateText;

    public State currentState;

    //Por si se hacen enemigos a distancia
    [Header("Distance Enemy")]
    public float castingTimer;
    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Idle;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) {
            currentState = State.Die;
        }
        timer += 1 * Time.deltaTime;

        switch(currentState) {
            case State.Idle:
                Idle();
                break;
            case State.Patroll:
                Patroll();
                break;
            case State.Walking:
                Walking();
                break;
            case State.Casting:
                // Casting();
                break;
            case State.Attacking:
                Attacking();
                break;
            case State.Die:
                Die();
                break;
        }

        if (timer >= 2f) {
            timer = 0;
            target = GetClosestTarget();
        }
        if (target != null) {
            distance = Vector3.Distance(transform.position, target.transform.position);
        }

        // castingTimer += 1 * Time.deltaTime;
        stateText.text = currentState.ToString();
    }

    public void Idle() {
        if (target != null) {
            currentState = State.Walking;
        }

        if (distance > 10) {
            currentState = State.Patroll;
        }
        //Cosas para evitar que vaya a por el target

    }

    public void Walking() {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 3f * Time.deltaTime);
        anim.SetBool("IsMoving", true);
        

        if (distance < 1.5f) {
            currentState = State.Attacking;
        }
    }

    public void Patroll() {
        //De momento no hace na pero estaria bien que vaya rondando por el mapa y haciendo cositas
        if (distance <= 5) {
            currentState = State.Walking;
        }
    }
    public void Attacking() {
        anim.SetBool("IsAttacking", true);
        StopAnim("IsMoving");
        
        if (distance >= 3.5) {
            currentState = State.Walking;
        }
    }

    public void Die() {
        if (!anim.GetBool("IsDead")) {
            player.GetComponent<Player>().exp += expToGive;
        }
        anim.SetBool("IsDead", true);
        Destroy(gameObject, 10f);
    }

    public void DealDamage() {
        //Lo mismo que en el player la esfera y tal tal tal y evitar que lo haga en 360º
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, 3f);
        // transform.LookAt(target.transform);
        

        foreach (Collider targetCollider in hitTargets){
            if (targetCollider.gameObject.GetComponent<Player>()) {
                Player targetHit = targetCollider.gameObject.GetComponent<Player>();
                if (GetAngleVector(targetHit.transform) <= 90) {
                    targetHit.health -= damage; 
                }
            }
            if (targetCollider.gameObject.GetComponent<IA>()) {
                IA targetHit = targetCollider.gameObject.GetComponent<IA>();
                if (GetAngleVector(targetHit.transform) <= 90) {
                    targetHit.currentHealth -= damage;
                }
            }
        }
    }

    public void StopAnim(string name) {
        anim.SetBool(name, false);
    }

    public float GetAngleVector(Transform target) {
        Vector3 enemyDirection = (target.position - transform.position).normalized;
        float angleToEnemy = Vector3.Angle(transform.forward, enemyDirection);

        return angleToEnemy;
    }

    public void RotateTowardsTarget(Transform targetPosition) {
        Vector3 directionToEnemy = (target.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

        float rotationSpeed = 5f;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public GameObject GetClosestTarget() {
        GameObject currentTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject targetGO in GameObject.FindGameObjectsWithTag("Target")) {
            float distance = Vector3.Distance(transform.position, targetGO.transform.position);

            if (distance < closestDistance) {
                closestDistance = distance;
                currentTarget = targetGO;
            }

        }

        return currentTarget;
    }
}
