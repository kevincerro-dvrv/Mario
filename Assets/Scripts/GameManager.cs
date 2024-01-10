using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public GameObject marioPrefab;
    private Vector3 marioSpawnPoint;
    

    public GameObject turtlePrefab;
    public Transform leftTurtleSpawnPoint;
    public Transform rightTurtleSpawnPoint;
    private int turtleCount;
    private int lifesLeft;

    public SpawnData[] spawnPlan = new SpawnData[] {
        new SpawnData(SpawnData.SpawnPoint.Right, 1.5f),
        new SpawnData(SpawnData.SpawnPoint.Left, 2f),
        new SpawnData(SpawnData.SpawnPoint.Right, 2.5f),
        new SpawnData(SpawnData.SpawnPoint.Left, 2f)
    };

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        lifesLeft = 4;
        StartCoroutine(SpawnCoroutine());
        marioSpawnPoint = new Vector3(0f, 5f, 0f);
    }

    // Update is called once per frame
    void Update() {
        
    }

    private IEnumerator SpawnCoroutine() {
        foreach(SpawnData spawnData in spawnPlan) {
            yield return new WaitForSeconds(spawnData.spawnInterval);
            if(spawnData.spawnPoint == SpawnData.SpawnPoint.Left) {
                SpawnTurtle(leftTurtleSpawnPoint);
            } else  {
                SpawnTurtle(rightTurtleSpawnPoint);
            }

        }
    }

    private void SpawnTurtle(Transform spawnPoint) {
        GameObject turtleGO = Instantiate(turtlePrefab, spawnPoint.position, Quaternion.identity);
        turtleGO.name = "Turtle_" + turtleCount;
        turtleCount++;
    }


    public void MarioDied() {
        lifesLeft--;
        Debug.Log("[GameManager] lifesLeft " + lifesLeft);

        if(lifesLeft >= 0) {
            Invoke("SpawnMario", 1.5f);
        }
    }

    private void SpawnMario() {
        GameObject marioGO = Instantiate(marioPrefab, marioSpawnPoint, Quaternion.identity);
        marioGO.GetComponent<Mario>().SpawnInicialization();
        Camera.main.GetComponent<CameraMovement>().mario = marioGO.transform;
    }
}
