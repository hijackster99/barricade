using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EndDoor : MonoBehaviour
{
    [SerializeField] NavMeshLink link;
    [SerializeField] Animator animator;

    [SerializeField] float range;
    [SerializeField] int stages;
    [SerializeField] int stageTime;

    int currentStage;
    int currentStageTime;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("MainCharacter");
        currentStage = 0;
        currentStageTime = -1;
    }

    // Update is called once per frame
    void Update()
    {
        player.GetComponent<PlayerController>().nearEnd = false;

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D c in cols)
        {
            if (c.tag == "Player")
            {
                player.GetComponent<PlayerController>().nearEnd = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentStageTime == 0)
        {
            currentStage++;
            currentStageTime = -1;
        }else if(currentStageTime > 0)
        {
            currentStageTime--;
        }
        if (currentStage == stages)
        {
            Open();
        }
    }

    public void Interact()
    {
        currentStageTime = stageTime;
    }

    private void Open()
    {
        animator.SetBool("Open", true);
        link.area = 0;
    }
}
