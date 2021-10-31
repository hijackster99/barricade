using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] bool open;
    [SerializeField] bool reversed;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] NavMeshLink link;

    public string color;

    NavMeshSurface2d surface;

    [SerializeField] float range;

    SortedDictionary<string, Color> colors;

    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        colors = new SortedDictionary<string, Color>();
        colors.Add("Red", new Color(255, 0, 0, 127));
        colors.Add("Blue", new Color(0, 0, 255, 127));
        colors.Add("Green", new Color(0, 255, 0, 127));
        colors.Add("Purple", new Color(127, 0, 255, 127));
        colors.Add("Yellow", new Color(255, 255, 0, 127));
        colors.Add("Orange", new Color(255, 127, 0, 127));
        sprite.color = colors.GetValueOrDefault(color);

        player = GameObject.Find("Grid").GetComponentInChildren<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.transform.position - transform.position).magnitude <= range)
        {
            player.nearDoor = this;
        }
        else if (player.nearDoor == this)
        {
            player.nearDoor = null;
        }

        Debug.Log(link.area);
    }

    public void Toggle()
    {
        if (open)
        {
            if (reversed)
            {
                transform.Rotate(Vector3.forward, -90);
            }
            else
            {
                transform.Rotate(Vector3.forward, 90);
            }
        }
        else
        {
            if (reversed)
            {
                transform.Rotate(Vector3.forward, 90);
            }
            else
            {
                transform.Rotate(Vector3.forward, -90);
            }
        }
        open = !open;
        if (open) link.area = 0;
        else link.area = 1;

    }
}
