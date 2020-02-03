using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //range the enemy can be within to attack the hero
    [SerializeField] private float range = 3f;
    //time between each attack animation
    [SerializeField] private float timeBetweenAttacks = 1f;

    //anim for enemy attack animation
    private Animator anim;
    //instance of hero
    private GameObject player;
    //keep track if enemy is close enough to hero to attack
    private bool playerInRange = false;
    //keep track of all the enemy box colliders
    private BoxCollider[] weaponColliders;
    private EnemyHealth enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        weaponColliders = GetComponentsInChildren<BoxCollider>();
        player = GameManager.instance.Player;
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        StartCoroutine(attack());
    }

    // Update is called once per frame
    void Update()
    {
        //if the enemy position is in range of the hero's position then change the bool value so the enemy can attack
        if (Vector3.Distance(transform.position, player.transform.position) < range && enemyHealth.IsAlive)
        {
            playerInRange = true;
        }
        else {
            playerInRange = false;
        }
    }

    //helps keep animations from overlapping by waiting a set amount of time before executing code again.
    IEnumerator attack()
    {
        if (playerInRange && !GameManager.instance.GameOver) {
            anim.Play("Attack");
            //wait for the user set amount of time between attacks
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
        yield return null;
        //recursively call attack to always check to see if we can attack
        StartCoroutine(attack());
    }

    public void EnemyBeginAttack() {
        foreach (var weapon in weaponColliders) {
            weapon.enabled = true;
        }
    }

    public void EnemyEndAttack() {
        foreach (var weapon in weaponColliders) {
            weapon.enabled = false;
        }
    }
}
