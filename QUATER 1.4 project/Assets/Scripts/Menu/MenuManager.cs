using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    

    private bool comingFromOptions = false;
    private bool clicked = false;
    private bool loadScene = false;
    private bool changedValues = false;

    int scene;
    public Text levelName;

    public GameObject menu;
    public GameObject playPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject exitPanel;
    public GameObject applyPanel;
    public GameObject newConfirmPanel;
    public GameObject titleText;
    public GameObject startText;

    public GameObject playMenu;
    public GameObject missionList;
    //public GameObject missionInfo;
    public GameObject background;
    public GameObject loadingPopup;

    Image spriteRenderer;
    public Sprite missionSprite;
    public Sprite playSprite;
    public Sprite newSprite;
    public Sprite optionsSprite;
    public Sprite creditsSprite;
    Sprite defaultSprite;

    public GameObject optionsText;
    public GameObject playText;
    public GameObject creditsText;
    public GameObject continueText;
    public GameObject selectText;
    public GameObject newText;
    public GameObject exitText;

   

    Animator playMenuAnimator;
    Animator missionListAnimator;
    //Animator missionInfoAnimator;
    Animator loadingAnimator;
   // Animator backgroundAnimator;
    Animator optionsAnimator;

    // Use this for initialization
    void Start () {

        spriteRenderer = background.GetComponent<Image>();
        defaultSprite = spriteRenderer.sprite;

        playMenuAnimator = playPanel.GetComponent< Animator > ();
        loadingAnimator = loadingPopup.GetComponent<Animator>();
        //backgroundAnimator = background.GetComponent<Animator>();

        optionsAnimator = optionsPanel.GetComponent<Animator>();

        playPanel.SetActive(!true);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        exitPanel.SetActive(!true);
        applyPanel.SetActive(false);
        newConfirmPanel.SetActive(false);
        titleText.SetActive(true);
        menu.SetActive(!true);

        playText.SetActive(!true);
        optionsText.SetActive(!true);
        creditsText.SetActive(!true);
        exitText.SetActive(!true);
        continueText.SetActive(!true);
        selectText.SetActive(!true);
        newText.SetActive(!true);

       
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)) {
            menu.SetActive(true);
            startText.SetActive(false);
        }
        if (clicked == true && loadScene == false)
        {

            // ...set the loadScene boolean to true to prevent loading a new scene more than once...
            loadScene = true;

            // ...and start a coroutine that will load the desired scene.
            StartCoroutine(LoadNewScene());

        }
    }

    public void ChangedValues()
    {
        changedValues = true;
    }

    public void Play()
    {
        if (comingFromOptions && changedValues) { Apply(); comingFromOptions = !comingFromOptions;  changedValues = false; }
        else
        {
            playPanel.SetActive(true);
            optionsPanel.SetActive(false);
            creditsPanel.SetActive(false);
            titleText.SetActive(!true);

            playMenuAnimator.SetBool("Play", true);
            playMenuAnimator.SetBool("MissionListRight", !true);
            //playMenuAnimator.SetBool("MissionInfoRight", false);

            playMenuAnimator.SetBool("Disappear", !true);

            //optionsAnimator.SetBool("OptionsDisappear", true);
            //optionsAnimator.SetBool("Options", !true);
            //optionsAnimator.SetBool("Audio", !true);
        }
    }
    public void PlayInactive()
    {
        playMenuAnimator.SetBool("Play", true);
        playMenuAnimator.SetBool("MissionListRight", !true);
        //playMenuAnimator.SetBool("MissionInfoRight", false);
        
    }

    //background change
    public void BGMission()
    {
        spriteRenderer.sprite = missionSprite;
        selectText.SetActive(true);
    }
    public void BGPlay()
    {
        spriteRenderer.sprite = playSprite;
        //Debug.Log("FUCK THAT SHIT");
        playText.SetActive(true);  
    }
    public void BGContinue()
    {
        spriteRenderer.sprite = playSprite;
        //Debug.Log("FUCK THAT SHIT");
        continueText.SetActive(true);
    }
    public void BGExit()
    {
        //spriteRenderer.sprite = playSprite;
        //Debug.Log("FUCK THAT SHIT");
        exitText.SetActive(true);
    }
    public void BGNew()
    {
        spriteRenderer.sprite = newSprite;
        newText.SetActive(true);
    }
    public void BGOptions()
    {
        //spriteRenderer.sprite = optionsSprite;
        optionsText.SetActive(true);
    }
    public void BGCredits()
    {
        spriteRenderer.sprite = creditsSprite;
        creditsText.SetActive(true);
    }
    public void OptionsGFX()
    {
        
    }
    public void OptionsCTRL()
    {
        
    }
    public void OptionsDISPLAY()
    {
        
    }
    public void OptionsAUDIO()
    {
        
    }
    public void BGDefault()
    {
        spriteRenderer.sprite = defaultSprite;
        playText.SetActive(!true);
        optionsText.SetActive(!true);
        creditsText.SetActive(!true);
        exitText.SetActive(!true);
        continueText.SetActive(!true);
        selectText.SetActive(!true);
        newText.SetActive(!true);

       
    }
    //

    public void SelectMission()
    {
        playMenuAnimator.SetBool("MissionListRight", true);
        playMenuAnimator.SetBool("Play", !true);
        //playMenuAnimator.SetBool("MissionInfoRight", !true);
       
    }
    public void SelectMissionInactive()
    {
       playMenuAnimator.SetBool("MissionListRight", true);
       //playMenuAnimator.SetBool("MissionInfoRight", false);
       playMenuAnimator.SetBool("Play", !true);
      
    }

    //public void Mission1()
    //{
    //    //playMenuAnimator.SetBool("MissionInfoRight", true);
    //    playMenuAnimator.SetBool("Play", !true);
    //    playMenuAnimator.SetBool("MissionListRight", !true);
        
    //}

    public void Launch1()
    {
        Debug.Log("WTF");
        loadingAnimator.SetBool("Launch", true);
        playMenuAnimator.SetBool("Disappear", true);
        menu.SetActive(false);
        titleText.SetActive(!true);
        clicked = true;
        scene = 2;
        levelName.text = "Prologue";
    }
    public void Launch2()
    {
        loadingAnimator.SetBool("Launch", true);
        playMenuAnimator.SetBool("Disappear", true);
        menu.SetActive(false);
        titleText.SetActive(!true);
        clicked = true;
        scene = 1;
    }
    public void Launch3()
    {
        loadingAnimator.SetBool("Launch", true);
        playMenuAnimator.SetBool("Disappear", true);
        menu.SetActive(false);
        titleText.SetActive(!true);
        clicked = true;
        scene = 1;
    }
    public void Launch4()
    {
        loadingAnimator.SetBool("Launch", true);
        playMenuAnimator.SetBool("Disappear", true);
        menu.SetActive(false);
        titleText.SetActive(!true);
        clicked = true;
        scene = 1;
    }
    public void Launch5()
    {
        loadingAnimator.SetBool("Launch", true);
        playMenuAnimator.SetBool("Disappear", true);
        menu.SetActive(false);
        titleText.SetActive(!true);
        clicked = true;
        scene = 2;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        if (comingFromOptions && changedValues) { Apply(); comingFromOptions = false; changedValues = false; }

        else
        {
            //playPanel.SetActive(!true);
            optionsPanel.SetActive(!false);
            creditsPanel.SetActive(false);
            titleText.SetActive(!true);

            playMenuAnimator.SetBool("Play", !true);
            playMenuAnimator.SetBool("MissionListRight", !true);
            //playMenuAnimator.SetBool("MissionInfoRight", false);


            playMenuAnimator.SetBool("Disappear", true);


            //optionsAnimator.SetBool("Options", true);
            //optionsAnimator.SetBool("Audio", !true);
            //optionsAnimator.SetBool("OptionsDisappear", !true);

            comingFromOptions = true;
        }
    }

    //public void Audio()
    //{
    //    comingFromOptions = true;
    //    optionsAnimator.SetBool("Audio", true);
    //    optionsAnimator.SetBool("Options", !true);
    //}

    public void Credits()
    {
        if (comingFromOptions && changedValues) { Apply(); comingFromOptions = false; changedValues = false; }
        else
        {

            //playPanel.SetActive(!true);
            optionsPanel.SetActive(false);
            creditsPanel.SetActive(!false);
            titleText.SetActive(!true);

            playMenuAnimator.SetBool("Play", !true);
            playMenuAnimator.SetBool("MissionListRight", !true);
            //playMenuAnimator.SetBool("MissionInfoRight", false);

            playMenuAnimator.SetBool("Disappear", true);

            //optionsAnimator.SetBool("OptionsDisappear", true);
            //optionsAnimator.SetBool("Options", !true);
            //optionsAnimator.SetBool("Audio", !true);

            comingFromOptions = false;

        }
    }
    public void Exit()
    {
        if (comingFromOptions && changedValues) { Apply(); comingFromOptions = false; changedValues = false; }
        else
        {
            //playPanel.SetActive(!true);
            optionsPanel.SetActive(false);
            creditsPanel.SetActive(false);
            exitPanel.SetActive(true);
            titleText.SetActive(!true);

            playMenuAnimator.SetBool("Play", !true);
            playMenuAnimator.SetBool("MissionListRight", !true);
            //playMenuAnimator.SetBool("MissionInfoRight", false);

            playMenuAnimator.SetBool("Disappear", true);

            //optionsAnimator.SetBool("OptionsDisappear", true);
            //optionsAnimator.SetBool("Options", !true);
            //optionsAnimator.SetBool("Audio", !true);

            comingFromOptions = false;
        }
    }
    public void Default()
    {
        changedValues = false;
        //playPanel.SetActive(!true);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        exitPanel.SetActive(!true);
        titleText.SetActive(true);
        applyPanel.SetActive(false);
        newConfirmPanel.SetActive(false);

        playMenuAnimator.SetBool("Play", !true);
        playMenuAnimator.SetBool("MissionListRight", !true);
        //playMenuAnimator.SetBool("MissionInfoRight", false);

        playMenuAnimator.SetBool("Disappear", !true);

        //optionsAnimator.SetBool("OptionsDisappear", !true);
        //optionsAnimator.SetBool("Options", !true);
        //optionsAnimator.SetBool("Audio", !true);

        comingFromOptions = false;
        
    }

    public void ResetSliders(GameObject button)
    {
        Debug.Log("resetsliders");
        button.GetComponent<SliderApply>().ResetSliders();
        Default();
        
    }

    public void ResetOptions(GameObject button)
    {
        button.GetComponent<SliderApply>().ResetValues();

    }

    public void ApplyDefault(GameObject button)
    {
        Debug.Log("applydefault");
        button.GetComponent<SliderApply>().ApplySliders();
        Default();
    }

    public void Apply()
    {
        Debug.Log("APLLY CHANGES?");

        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        exitPanel.SetActive(!true);
        playMenuAnimator.SetBool("Play", !true);
        if(changedValues)
            applyPanel.SetActive(!false);
    }

    public void NewGameConfirm()
    {
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        exitPanel.SetActive(!true);
        playMenuAnimator.SetBool("Play", !true);
        playMenuAnimator.SetBool("Disappear", true);
        applyPanel.SetActive(false);
        newConfirmPanel.SetActive(!false);
    }


    public void NewGame()
    {
        Utils.ResetLastLevel();
        Default();
    }

    IEnumerator LoadNewScene()
    {


        yield return new WaitForSeconds(5);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = Application.LoadLevelAsync(scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

    }
}
