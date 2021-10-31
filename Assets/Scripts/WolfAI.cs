using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float range;
    [SerializeField] float wanderRange;

    [SerializeField] Animator animator;
    [SerializeField] bool wander;

    [SerializeField] float huntSpeed;
    [SerializeField] int huntCooldown;

    int currentHuntCooldown;

    float baseSpeed;

    int hunt;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        baseSpeed = agent.speed;
        hunt = 0;

        player = GameObject.Find("MainCharacter");
    }

    // Update is called once per frame
    void Update()
    {
        if (hunt == 1)
        {
            agent.speed = huntSpeed;
            agent.SetDestination(player.transform.position);
            hunt = 2;
        }
        else if (hunt == 2)
        {
            if (!agent.hasPath)
            {
                hunt = 0;
                agent.speed = baseSpeed;
                currentHuntCooldown = huntCooldown;
            }
        }
        else
        {
            if ((player.transform.position - transform.position).magnitude <= range)
            {
                agent.SetDestination(player.transform.position);
                wander = false;
            }
            else
            {
                if (!agent.hasPath || !wander)
                {
                    float angle = Random.Range(0, 360);
                    Vector3 dest = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * wanderRange;

                    agent.SetDestination(dest + transform.position);
                }
                wander = true;
            }
        }

        transform.rotation = Quaternion.LookRotation(transform.forward, agent.desiredVelocity * -1);

        animator.SetBool("Walk", agent.hasPath);
        animator.SetBool("Hunt", hunt > 0);
    }

    private void FixedUpdate()
    {
        if (currentHuntCooldown > 0)
        {
            currentHuntCooldown--;
        }
        else if(currentHuntCooldown == 0)
        {
            hunt = 1;
            currentHuntCooldown--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") player.GetComponent<PlayerController>().damage();
    }
}
