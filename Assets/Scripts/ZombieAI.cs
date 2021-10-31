using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float range;
    [SerializeField] float wanderRange;

    [SerializeField] Animator animator;

    [SerializeField] bool wander;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        player = GameObject.Find("MainCharacter");
    }

    // Update is called once per frame
    void Update()
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

        transform.rotation = Quaternion.LookRotation(transform.forward, agent.desiredVelocity * -1);

        animator.SetBool("Walk", agent.hasPath);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") player.GetComponent<PlayerController>().damaage();
    }
}
