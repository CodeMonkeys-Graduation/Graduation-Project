using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevCanvas : MonoBehaviour
{
    [SerializeField] public Animator devPanelAnimator;

    public static DevCanvas Instance { get; protected set; }


    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void OpenDevPanel()
    {
        bool isOpened = devPanelAnimator.GetBool("Open");
        devPanelAnimator.SetBool("Open", !isOpened);
        devPanelAnimator.SetTrigger("OpenCloseTrigger");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
