using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    private Button titleButton;
    public int sceneToLoad = 0;

    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();
		button.onClick.AddListener(onClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onClick() {
        SceneManager.LoadScene(sceneToLoad);
    }
}
