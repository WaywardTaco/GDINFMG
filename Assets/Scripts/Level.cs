using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private GameObject spike;

    [SerializeField]
    private float speed = 0.01f;

    private Vector3 initPos;

    List<GameObject> activeSpike = new List<GameObject>();

    private float spawnTimer;
    private float maxSpawnTimer;

    private const float spikeDestroy = -5.4f;
    // Start is called before the first frame update
    private void Awake()
    {
        maxSpawnTimer = 2f;
    }
    void Start()
    {
        initPos = spike.transform.position;
        create();
    }

    private void spawn()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer < 0)
        {
            spawnTimer += Random.Range(0f,maxSpawnTimer);
            create();
        }
    }
    private void create()
    {
        GameObject spikes = Instantiate(spike, initPos, Quaternion.identity);
        activeSpike.Add(spikes);

    }
    private void move()
    {
        for(int i = 0; i < activeSpike.Count; i++) {

            activeSpike[i].transform.position += Vector3.left * speed / 30;
        }
    }

    private void destroySpike()
    {
        for (int i = 0; i < activeSpike.Count; i++)
        {
            if (activeSpike[i].transform.position.x < spikeDestroy)
            {
                Destroy(activeSpike[i].gameObject);
                activeSpike.RemoveAt(i);
            }
        }
    }
    private void FixedUpdate()
    {
        move();
        
    }
    // Update is called once per frame
    void Update()
    {
        spawn();
        destroySpike();
        
    }
}
