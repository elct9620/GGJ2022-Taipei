using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Passboard : MonoBehaviour
{
    public static Passboard PB;
    // Start is called before the first frame update
    void Awake()
    {
        if (PB == null)
        {
            DontDestroyOnLoad(gameObject);
            PB = this;
        }
        else if (PB != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    public void ShowLayout()
    {
        Debug.Log("PD's Pass");
        SceneManager.LoadScene(2);
        Debug.Log("ShowLayout End");
    }
}
