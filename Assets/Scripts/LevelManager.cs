using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public float currentGoal;
    public float goalProgress;

    public int currentPriority;

    public LevelProgressUI levelProgressBar;
    public TMP_Text levelProgressText;

    //TEMPORARIO PARA MOSTRAR MOCKUP NO EVENTO
    public List<LevelData> allLevels;
    //public List<GameObject> allLevels;

    public LevelData currentLevel;
    //public int currentLevelNumber = 1;


    ClickAnimation clickAnimation;

    void Awake()
    {
        clickAnimation = this.GetComponent<ClickAnimation>();
    }

    void Start()
    {
        currentLevel = allLevels[0];
        clickAnimation.currentInDisplay = currentLevel.GetComponent<LevelAnimation>().levelOnClickAnimations;
    }

    private void Update()
    {
        //NAO PRECISARA SER NO UPDATE, DEPOIS DO EVENTO MUDAR PARA ACONTECIMENTOS DE ACOES. ESTUDAR SE EVENTOS PODEM SER UMA BOA IDEIA
        //(POIS CONECTAR TODAS AS ENTRADAS DE PONTOS EM UMA ACTION SO TALVEZ, SEM NECESSITAR FAZER A OPERACAO TODO O FRAME)
        levelProgressBar.image.fillAmount = Mathf.Lerp(0,1, currentLevel.currentCumulative/currentLevel.cumulativeGoal);
        levelProgressText.text = $"{currentLevel.currentCumulative} / {currentLevel.cumulativeGoal}";
    }


    void SwitchCameraByPriority(CinemachineVirtualCamera fromCamera, CinemachineVirtualCamera toCamera)
    {
        currentPriority = fromCamera.Priority;
        
        toCamera.Priority = currentPriority;
        fromCamera.Priority = 0;
    }


    public void UnlockNextLevel()
    {
        var currentLevelIndex = allLevels.IndexOf(currentLevel);

        currentLevel.isConcluded = true;
        //FORMA RAPIDA DE N�O DEIXAR GOAL PASSAR COM A QUANTIDADE DO ULTIMO CLIQUE
        currentLevel.currentCumulative = currentLevel.cumulativeGoal;


        
        if (currentLevelIndex + 1 == allLevels.Count) return;

        var nextLevel = allLevels[currentLevelIndex + 1];

        //var fromCamera = allLevels[currentLevelIndex].virtualCamera;
        //var toCamera = nextLevel.virtualCamera;

        //SwitchCameraByPriority(fromCamera, toCamera);
        SwitchLevel(nextLevel);
    }


    public void SwitchLevel(LevelData targetLevel)
    {
        SwitchCameraByPriority(currentLevel.virtualCamera, targetLevel.virtualCamera);

        currentLevel = targetLevel;
        var levelAnimation = currentLevel.GetComponent<LevelAnimation>();

        clickAnimation.currentInDisplay = levelAnimation.levelOnClickAnimations;
    }

    public void UISwitchLevelHandler(bool next)
    {
        var currentLevelIndex = allLevels.IndexOf(currentLevel);
        int targetLevelIndex = 0;

        if (next)
        {
            if (currentLevelIndex >= allLevels.Count) return;

            targetLevelIndex = currentLevelIndex + 1;
        }

        else
        {
            if (currentLevelIndex <= 0) return;

            targetLevelIndex = currentLevelIndex -1;
        }

        SwitchLevel(allLevels[targetLevelIndex]);
    }



}
