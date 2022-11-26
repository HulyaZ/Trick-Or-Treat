using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField] public GameObject gameOverText;


    [HideInInspector]
    public PlayerDirection direction;

   // [HideInInspector]
    public float step_Length = 0.2f; // snake move steps

    //[HideInInspector]
    public float movement_Frequency = 0.1f; // instead of moving snake every frame

    private float counter;
    private bool move;

    [SerializeField]
    private GameObject[] tailPrefab;

    private List<Vector3> delta_Position;
    private List<Rigidbody> nodes;

    private Rigidbody main_Body;
    private Rigidbody head_Body;
    private Transform tr;

    private bool createNodeAtTail_b;

    private void Awake()
    {
        tr = transform;
        main_Body = GetComponent<Rigidbody>();

        InitSnakeModes();
        InitPlayer();

        delta_Position = new List<Vector3>() // Which direction we are going.
        {
            new Vector3(-step_Length, 0f),  //  direction -x... LEFT
            new Vector3(0f, step_Length),   //  direction  y... UP
            new Vector3(step_Length, 0f),   //  direction  x... RIGHT
            new Vector3(0f, -step_Length),  //  direction -y... DOWN
        };
    }

    void SetDirectionRandom()
    {
        int dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRandom; // Converts direction from tags script according to the random value
    }

    void InitSnakeModes()
    {
        nodes = new List<Rigidbody>();

        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());

        head_Body = nodes[0];
    }

    void InitPlayer()
    {
        SetDirectionRandom();

        switch (direction)
        {
            case PlayerDirection.RIGHT:

                nodes[1].position = nodes[0].position - new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(Metrics.NODE *2f, 0f, 0f);

                Vector3 dirRight = new Vector3(0, 180, 0);

                nodes[0].gameObject.transform.eulerAngles = dirRight;
                nodes[1].gameObject.transform.eulerAngles = dirRight;
                nodes[2].gameObject.transform.eulerAngles = dirRight;

                break;

            case PlayerDirection.LEFT:

                nodes[1].position = nodes[0].position + new Vector3(Metrics.NODE, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(Metrics.NODE * 2f, 0f, 0f);

                Vector3 dirLeft = new Vector3(0, 0, 0);

                nodes[0].gameObject.transform.eulerAngles = dirLeft;
                nodes[1].gameObject.transform.eulerAngles = dirLeft;
                nodes[2].gameObject.transform.eulerAngles = dirLeft;
                break;

            case PlayerDirection.UP:
                nodes[1].position = nodes[0].position - new Vector3(0f,Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, Metrics.NODE * 2f, 0f);

                break;

            case PlayerDirection.DOWN:
                nodes[1].position = nodes[0].position + new Vector3(0f, Metrics.NODE, 0f);
                nodes[2].position = nodes[0].position + new Vector3(0f, Metrics.NODE * 2f, 0f);
                break;
        }
    }
   
    private float minX = -4f, maxX = 4f, minY = -2.5f, maxY = 2.5f; //Seamless boundaries   
    Vector3 SeamlessScreenMovement(Vector3 parentPos)
    {
        Vector3 leftStartPos = new Vector3(minX, head_Body.position.y, main_Body.position.z);
        Vector3 rightStartPos = new Vector3(maxX, head_Body.position.y, main_Body.position.z);
        Vector3 upStartPos = new Vector3(head_Body.position.x, minY, main_Body.position.z);
        Vector3 downStartPos = new Vector3(head_Body.position.x, maxY, main_Body.position.z);


        if (direction == PlayerDirection.RIGHT && parentPos.x > maxX)
        {
            head_Body.position = leftStartPos;
            main_Body.position = leftStartPos;    
        }
        
        if (direction == PlayerDirection.LEFT && parentPos.x < minX)
        {
            head_Body.position = rightStartPos;
            main_Body.position = rightStartPos; 
        }

        if (direction == PlayerDirection.UP && parentPos.y > maxY)
               {
            head_Body.position = upStartPos;
            main_Body.position = upStartPos;
        }

        if (direction == PlayerDirection.DOWN && parentPos.y < minY)
        {
            head_Body.position = downStartPos;
            main_Body.position = downStartPos;
        }
       
        return head_Body.position;
   
     }
    void Move()
    {
        Vector3 dPosition = delta_Position[(int)direction];// direction position

        Vector3 parentPos = head_Body.position;
        Vector3 prevPosition;

       
        parentPos = SeamlessScreenMovement(head_Body.position);
    
        main_Body.position = main_Body.position + dPosition;
        head_Body.position = head_Body.position + dPosition;

       

        for (int i = 1; i < nodes.Count; i++)
        {
            prevPosition = nodes[i].position;

            nodes[i].position = parentPos;
            parentPos = prevPosition;
        }
    

        if(createNodeAtTail_b)
        {
            createNodeAtTail_b = false;

            int randomTail = Random.Range(0,tailPrefab.Length);
            GameObject tailObj = tailPrefab[randomTail];

            GameObject newNode = Instantiate(tailObj, nodes[nodes.Count -1].position, tailObj.transform.rotation);

            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());

        }
    }
 

           void CheckMovementFrequency()
           {
               counter += Time.deltaTime;

               if(counter >= movement_Frequency)
               {
                   counter = 0f;
                   move = true;
               }
           }



           void Update()
           {
               CheckMovementFrequency();
           }

           private void FixedUpdate()
           {
               if(move)
               {
                   move = false;
                   Move();
               }
           }


           public void SetInputDirection(PlayerDirection dir)
           {
               if (Mathf.Abs(((int)direction - (int)dir)) == 2) //Up and Down - Left and Right difference equals 2 
               {
                   return;
               }    

               direction = dir;

               if(direction == PlayerDirection.RIGHT)
               {
                   SetRotation(new Vector3(0, 180, 0));
               }

               if (direction == PlayerDirection.LEFT)
               {
                   SetRotation(new Vector3(0, 0, 0));
               }
        /* 
               if (direction == PlayerDirection.UP)
               {
                   SetRotation(new Vector3(0, 0, -90));
               }

               if (direction == PlayerDirection.DOWN)
               {
                   SetRotation(new Vector3(0, 0, 90));
               }
             */
       
        ForceMove(); // not to wait for time frequency if player puts an input
    }

    void SetRotation(Vector3 rotationDir)
    {
        Quaternion angleLerp = Quaternion.Euler((int)rotationDir.x, (int)rotationDir.y, (int)rotationDir.z);
        nodes[0].transform.rotation = Quaternion.Lerp(transform.rotation, angleLerp, 2f); // Changes rotation of each note according to movement frequency
    }

    void SetRotationArr(Vector3 rotationDir) 
    {
          StartCoroutine(Waitor(rotationDir)); // Changes rotation of each note according to movement frequency
    }

    IEnumerator Waitor(Vector3 rotationDir)
    {
        Quaternion angleLerp = Quaternion.Euler((int)rotationDir.x, (int)rotationDir.y, (int)rotationDir.z);

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].transform.rotation = Quaternion.Lerp(transform.rotation, angleLerp, 2f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void ForceMove()
    {
        counter = 0;
        move = false;
        Move();
    }

     void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tags.FRUIT)
        {
            target.gameObject.SetActive(false);
            createNodeAtTail_b = true;

            GameplayController.instance.IncreaseScore();
            AudioManager.instance.PlayPickUpSound();
        }

        if(target.tag == Tags.WALL || target.tag == Tags.BOMB || target.tag == Tags.TAIL)
        {
            Time.timeScale = 0;
            gameOverText.SetActive(true);
            AudioManager.instance.PlayDeadSound();
        }
        
    }
}
