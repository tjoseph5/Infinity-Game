using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Screen_Button : MonoBehaviour
{
    public void Load()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
