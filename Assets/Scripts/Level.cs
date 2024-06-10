using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    
public void Level1()
    {
        
        SceneManager.LoadScene(2);
    }

    public void Level2()
    {
        
        SceneManager.LoadScene(3);
    }
 public void Back()
    {
        
        SceneManager.LoadScene(0);
    }

}
