using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] public bool open;
    [SerializeField] bool reversed;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] NavMeshLink link;

    public string color;

    [SerializeField] float range;

    Dictionary<string, Color> colors;

    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();
        colors = new Dictionary<string, Color>();
        colors.Add("Red", new Color(255, 0, 0, 127));
        colors.Add("Blue", new Color(0, 0, 255, 127));
        colors.Add("Green", new Color(0, 255, 0, 127));
        colors.Add("Yellow", new Color(255, 255, 0, 127));
        colors.Add("Orange", new Color(255, 127, 0, 127));

        Color[] colorlist = new Color[colors.Count];
        colors.Values.CopyTo(colorlist, 0);

        sprite.color = colorlist[rand.Next() % colorlist.Length];
        color = sprite.color.ToString();
        player = GameObject.Find("MainCharacter").GetComponent<PlayerController>();
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
