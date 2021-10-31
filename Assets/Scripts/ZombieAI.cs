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

    Vector3 home;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        home = transform.position;

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
                Vector3 dest = new Vector3(Mathf.Cos(Random.Range(0, 360)), Mathf.Sin(Random.Range(0, 360))) * wanderRange;

                agent.SetDestination(dest + transform.position);
            }
            wander = true;
        }

        animator.SetBool("Walk", agent.hasPath);
    }
}
