using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class IA : MonoBehaviour
{
    public bool hasTarget;
    public TMP_Text stateText; 
    public Transform playerPos;
    public GameObject target;
    public float speed;
    public float currentHealth;
    public float maxHealth;
    public int healthFrasks = 3;
    public enum State {
        Follow,
        Attack,
        Hold,
        Flee
    }

    [SerializeField] State currentState; 
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentState = State.Follow;
        //Es un gameObject que esta detrás del jugador para que cuando no haya enemigos se vaya ahi
        playerPos = GameObject.FindGameObjectWithTag("AllyPosition").transform; 
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }

    void Follow() {
        transform.position = Vector3.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
        if (hasTarget) {
            currentState = State.Attack;
        }
    }

    void Attack() {
        if (!hasTarget) {
            currentState = State.Follow;
        }
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > 1.5f) {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(target.transform);
        }

        if (currentHealth <= (maxHealth * 30/100) && healthFrasks >= 1) {
            currentState = State.Flee;
        }
    }

    void Hold() {
        //Para que se quede quieto
        //Podria utilizarse en cualquier momento
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

            if (distance < closestDistance) {
                closestDistance = distance;
                currentTarget = targetGO;
            }

        }
        Debug.Log(currentTarget);
        return currentTarget;
    }
}
