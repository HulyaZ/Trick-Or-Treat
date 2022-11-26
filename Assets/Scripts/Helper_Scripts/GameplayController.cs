using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayController : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] private PlayerController playerController;

    int scoreCount;

    public static GameplayController instance;

    public GameObject[] fruit_pickUp;
    public GameObject bomb_PickUp;


    private float minX = -3.55f, maxX = 3.55f, minY = -1.8f, maxY = 1.5f; //Pickable spawn boundaries
    private float posZ = -0.2f;

    private void Awake()
    {
        MakeInstance();

    }
    void Start()
    {
        Invoke("StartSpawning", 0.5f);
    }

    void MakeInstance()
    {
       if(instance == null) 
            instance = this;
    }

    void StartSpawning()
    {
        StartCoroutine(SpawnPickUps());
    }

    void StopSpawning()
    {
        CancelInvoke("StartSpawning");
    }

    IEnumerator SpawnPickUps()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));

      
        if (Random.Range(0,10) >= 2)
        {  
            int randomFruit = Random.Range(0, fruit_pickUp.Length);
            GameObject spawnFruit = fruit_pickUp[randomFruit];
            Instantiate(spawnFruit, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), posZ), Quaternion.identity);
        }
        else
        {
            Instantiate(bomb_PickUp, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), posZ), Quaternion.identity);
        }

        Invoke("StartSpawning", 0f);
    }

    int counter = 15;
    public void IncreaseScore()
    {        
        counter++;
        
        if(counter > 5)
        {
             playerController.movement_Frequency -= 0.005f;
            counter = 0;
        }
        scoreCount++;
        scoreText.text = scoreCount + " Yummies!" ; 
    }
}
