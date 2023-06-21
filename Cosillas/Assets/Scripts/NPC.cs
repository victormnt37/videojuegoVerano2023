using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    [SerializeField] protected float currentResistance;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float maxResistance; 
    [SerializeField] protected float maxHealth;
    [SerializeField] protected GameObject target;
    [SerializeField] protected List<GameObject> nearTargets = new List<GameObject>();
    [SerializeField] protected TMP_Text stateText; 
    [SerializeField] protected Animator anim;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;

    protected virtual void Start() {
        //Al inicializarse, se establecera la resistencia y la vida a todos los NPCS;
        currentResistance = maxResistance;
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

    }

    protected virtual void Update()
    {
        //Lo mismo con la resistencia si no tiene pues que haga otra cosa
        if (currentResistance <= 0) {
            return;
        }
    }
    public void RotateTowardsTarget(Vector3 targetPosition) {
        //Para que gire siempre hacia el objetivo
        Vector3 directionToEnemy = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

        //La velocidad con la que gira de manera suave
        float rotationSpeed = 5f;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void ConsumeResistance(float Value)
    {
        //Al atacar o hacer cualquier otra cosa que gaste energia que llame a este método
        currentResistance -= Value;
    }

    public float GetCurrentHealth() {
        //Cosas bien hechas para que no se llame a una variable si no que llame al metodo para evitar errores
        return currentHealth;
    }

    public void TakeDamage(float Value) {
        //Para cuando le hagan daño que sea en función al daño del enemigo + el arma + lo que se quiera añadir
        currentHealth -= Value;
    }

    public bool IsAnimActive(string AnimName) {
        //Lo mismo que en el Get
        return anim.GetBool(AnimName);
    }

    public void StopAnim(string AnimName) {
        //Me da pereza escribir anim.SetBool()
        anim.SetBool(AnimName, false);
    }

    public void RecoverEnergy(float Value = 10) {
        //Que vaya recuperando energia
        if (currentResistance >= maxResistance) {
            currentResistance = maxResistance;
        }
        currentResistance += Value * Time.deltaTime;
    }

    public GameObject GetClosestTargetWithTag(string Tag) {
        //Un filtro con el tag que se quiera coger
        GameObject currentTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject targetGO in GameObject.FindGameObjectsWithTag(Tag)) {
            float distance = Vector3.Distance(transform.position, targetGO.transform.position);

            if (distance < closestDistance) {
                closestDistance = distance;
                currentTarget = targetGO;
            }

        }

        return currentTarget;
    }
}
