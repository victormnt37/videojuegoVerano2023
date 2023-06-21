using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrySystem : MonoBehaviour
{
    public float parryDuration = 0.2f;
    public float parryCooldown = 1f;

    public bool isParrying = false;
    public float parryTimer;
    public float parryTimerCooldown;
    
    // Update is called once per frame
    void Update()
    {
        if (isParrying) {
            parryTimer += Time.deltaTime;

            if (parryTimer >= parryDuration) {
                isParrying = false;
                parryTimer = 0f;
                parryTimerCooldown = parryCooldown;
            }
        }

        else if (parryTimerCooldown > 0) {
            parryTimerCooldown -= Time.deltaTime;
        }    

        if (Input.GetKeyDown(KeyCode.Q) && !isParrying && parryTimerCooldown <= 0) {
            isParrying = true;
        }

        // Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (!gameObject.GetComponent<Player>().playersTarget) {
            return;
        }
        if (gameObject.GetComponent<Player>().playersTarget.GetComponent<Enemy>().isDealingDamage && isParrying) {
            Debug.Log("Parry");
        }
    }

}
