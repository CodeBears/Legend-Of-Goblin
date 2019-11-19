using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelToLoad : MonoBehaviour {
    public int sceneIndex;

    void OnTriggerEnter2D(Collider2D col)
    {
       GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SaveGame();
        if (col.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(1);
        }
    }
}
