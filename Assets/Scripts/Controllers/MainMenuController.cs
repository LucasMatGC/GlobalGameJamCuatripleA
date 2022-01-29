using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public GameObject[] pressAnyCanvases;
    private GameObject mainCanvas;

    private GameObject currentCanvas;
    private bool isLoading;
    private Vector3 cursorInitPos;
    private bool mouseUser = false;

    // Start is called before the first frame update
    void Start()
    {

        mainCanvas = pressAnyCanvases[0];
        currentCanvas = mainCanvas;

        Cursor.visible = false;
        cursorInitPos = Input.mousePosition;

    }

    // Update is called once per frame
    void Update()
    {

        if (!mouseUser && Input.mousePosition != cursorInitPos)
        {
            mouseUser = true;
        }
        if (Array.Exists(pressAnyCanvases, el => el == currentCanvas) && Input.anyKeyDown)
        {
            if (currentCanvas.name == "Introduction")
            {
                isLoading = true;
                if (currentCanvas.gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
                    StartCoroutine(LoadGameScene());
                
            } else
                    LoadPage(mainCanvas);
        }

        CursorVisibility();

    }

    private void CursorVisibility()
    {

        if (isLoading)
            Cursor.visible = false;
        else if (mouseUser)
            Cursor.visible = true;

    }

    private void LoadPage(GameObject nextCanvas)
    {
        currentCanvas.SetActive(false);
        nextCanvas.SetActive(true);
        currentCanvas = nextCanvas;

    }

    public void ChangeCanvasBtn(GameObject nextCanvas)
    {
        LoadPage(nextCanvas);

    }

    private IEnumerator LoadGameScene()
    {
        // Carga la siguiente escena mientras el resto del código carga. La carga es prácticamente instantánea.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Prueba1");

        // Espera hasta que la escena esté cargada
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void ExitBtn()
    {
        Application.Quit();
    }
}
