using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public GameObject[] pressAnyCanvases;
    public AudioSource music;
    public VideoPlayer videoPlayer;

    private GameObject mainCanvas;
    private GameObject currentCanvas;
    private bool isLoading;
    private string levelName = "Final";
    private Vector3 cursorInitPos;
    private bool mouseUser = false;
    private float timer = 56.5f;
    private float currentTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {

        mainCanvas = pressAnyCanvases[0];
        currentCanvas = mainCanvas;

        Cursor.visible = false;
        cursorInitPos = Input.mousePosition;
        videoPlayer.loopPointReached += EndVideo;

    }

    // Update is called once per frame
    void Update()
    {

        ManageAudio();

        if (!mouseUser && Input.mousePosition != cursorInitPos)
            mouseUser = true;

        if (Input.anyKeyDown)
        {

            ManageInput();

        }

        CursorVisibility();

    }
    private void ManageAudio()
    {

        currentTimer += Time.deltaTime;

        if (currentTimer >= timer)
        {

            music.Play();
            currentTimer = 0f;

        }

    }
    private void ManageInput()
    {
        if (currentCanvas.name == "Introduction")
        {
            isLoading = true;
            if (currentCanvas.gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
                StartCoroutine(LoadGameScene());
                
        } else
                LoadPage(mainCanvas);
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

    void EndVideo(VideoPlayer vp)
    {

        SceneManager.LoadSceneAsync(levelName);

    }

    private IEnumerator LoadGameScene()
    {
        // Carga la siguiente escena mientras el resto del código carga. La carga es prácticamente instantánea.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);

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
