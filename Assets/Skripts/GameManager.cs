using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("�������")]
    [SerializeField] public ImageTaimer TreeTimer;
    [SerializeField] public ImageTaimer BoostTemperaturaTimer;
    [SerializeField] public Image timeBoostTemperatureImg;
    [SerializeField] public Image timeRecruitmentWorkerImg;

    [Header("������")]
    [SerializeField] public Button workerButton;
    [SerializeField] public Button TemperatureButton;

    [Header("�������")]
    [SerializeField] public Text treeText;
    [SerializeField] public Text TemperatureText;
    [SerializeField] public Text workerText;
    [SerializeField] public int workerCount;
    [SerializeField] public int temperatureCount;
    [SerializeField] public int treeCount;

    [Header("���� � �����������")]
    [SerializeField] public int treePerWorker;
    [SerializeField] public int wheatToTemperature;

    [Header("����")]
    [SerializeField] public int workerCost;
    [SerializeField] public int temperatureCost;

    [Header("����� ��������")]
    [SerializeField] public float workerCreateTime;
    [SerializeField] public float temperatureCreateTime;
    [SerializeField] private float timeRecruitmentWorker = -2;
    [SerializeField] private float timeBoostTemperature = -2;

    [Header("������")]
    [SerializeField] public GameObject winPannel;
    [SerializeField] public GameObject losPannel;
    [SerializeField] public GameObject pausePannel;

    [Header("�����")]
    [SerializeField] public AudioSource createWorkersAudio;
    [SerializeField] public AudioSource createTemperatureAudio;
    [SerializeField] public AudioSource ambientAudio;
    [SerializeField] public AudioSource clickAudio;
    [SerializeField] public AudioSource noWoodAudio;
    [SerializeField] public AudioSource StormAudio;
    [SerializeField] public AudioSource fallingTreeAudio;
    [SerializeField] public AudioSource kindlingFurnaceAudio;
    [SerializeField] public GameObject play;
    [SerializeField] public GameObject stop;

    [Header("������")]
    [SerializeField] public int TreeTasks;
    [SerializeField] public int WorkerTasks;
    [SerializeField] public Text WorkerTasksText;
    [SerializeField] public Text TreeTasksText;

    [Header("����������")]
    [SerializeField] public int TreeStatistics;
    [SerializeField] public int temperatureStatistics;
    [SerializeField] public int WorkerStatistics;
    [SerializeField] public int StormStatistics;
    [SerializeField] public Text StatisticsText;

    [Header("����")]
    [SerializeField] public Image timeStormImg;
    [SerializeField] public float stormMaxTime;
    [SerializeField] public int stormIncrease;
    [SerializeField] public int saveStormWaves;
    [SerializeField] public int stormWaves;
    [SerializeField] public int nextStorm;
    [SerializeField] public int stormCheck;
    [SerializeField] private float timeStorm;
    [SerializeField] public Text nextStormsText;

    [Header("����")]
    [SerializeField] public Text dayText;
    [SerializeField] public int day;



    void Start()
    {
        treeCount = 0; 
        Time.timeScale = 1f;
        winPannel.SetActive(false);
        losPannel.SetActive(false);
        UpdateText();
        timeStorm = stormMaxTime;
    }
    void Update()
    {
        //----------����������
        StatisticsText.text = $"�������� ����: {TreeStatistics}" + "\n" + $"����� � ���������:  {WorkerStatistics}" + "\n" + $"��������� �����������:  {temperatureStatistics}" + "\n" + $"��������� ����:  {stormCheck}" + "\n" +  $"���� ������: {day}";

        //----------������� ����
        dayText.text = $"����: {day}";
        
        nextStormsText.text = $"������ ��������� ����������� ��������� �C -{nextStorm}";

        //----------������
        TreeTasksText.text = $"������� ������:{TreeTasks}/{treeCount}";
        WorkerTasksText.text = $"����� � ������:{WorkerTasks}/{workerCount}";

        //----------����� ������
        if (TreeTasks <= treeCount && WorkerTasks <= workerCount)
        {
            Time.timeScale = 0;
            winPannel.SetActive(true);
            StormAudio.gameObject.SetActive(false);
            fallingTreeAudio.gameObject.SetActive(false);
            kindlingFurnaceAudio.gameObject.SetActive(false);
            noWoodAudio.gameObject.SetActive(false);
            clickAudio.gameObject.SetActive(false);
            createTemperatureAudio.gameObject.SetActive(false);
            createWorkersAudio.gameObject.SetActive(false);
        }

        //----------����� ���������
        if (temperatureCount < 0)
        {
            stormCheck = StormStatistics - 2;
            Time.timeScale = 0;
            losPannel.SetActive(true);
            StormAudio.gameObject.SetActive(false);
            fallingTreeAudio.gameObject.SetActive(false);
            kindlingFurnaceAudio.gameObject.SetActive(false);
            noWoodAudio.gameObject.SetActive(false);
            clickAudio.gameObject.SetActive(false);
            createTemperatureAudio.gameObject.SetActive(false);
            createWorkersAudio.gameObject.SetActive(false);

        }

        //----------������ ����
        timeStorm -= Time.deltaTime;
        timeStormImg.fillAmount = timeStorm / stormMaxTime;
       
        if (timeStorm <= 0)
        {
            if (stormWaves >= saveStormWaves)
            {
                StormStatistics++;
                temperatureCount -= nextStorm;
                nextStorm += stormIncrease;
                StormAudio.Play();
            }
            timeStorm = stormMaxTime;
            stormWaves++;
            day++;
        }

        //----------������ ����� ����
        if (TreeTimer.Tick)
        {
            treeCount += workerCount * treePerWorker;
            TreeStatistics += workerCount * treePerWorker;
            fallingTreeAudio.Play();
        }

        //----------������ ����������� ���� ������������
        if (BoostTemperaturaTimer.Tick && temperatureCount >= 1)
        {
            treeCount -= temperatureCount * wheatToTemperature;
            kindlingFurnaceAudio.Play();
        }

        //----------������ �������� ��������
        if (timeRecruitmentWorker > 0 )
        {
            timeRecruitmentWorker -= Time.deltaTime;
            timeRecruitmentWorkerImg.fillAmount = timeRecruitmentWorker / workerCreateTime;
        }
        else if (timeRecruitmentWorker > -1)
        {
            timeRecruitmentWorkerImg.fillAmount = 1;
            workerButton.interactable = true;
            workerCount += 1;
            WorkerStatistics = workerCount;
            timeRecruitmentWorker = -2;
            createWorkersAudio.Play();
        }

        //----------������ �������� �����������
        if (timeBoostTemperature > 0)
        {
            timeBoostTemperature -= Time.deltaTime;
            timeBoostTemperatureImg.fillAmount = timeBoostTemperature / temperatureCreateTime;
        }
        else if (timeBoostTemperature > -1)
        {
            timeBoostTemperatureImg.fillAmount = 1;
            TemperatureButton.interactable = true;
            temperatureCount += 1;
            temperatureStatistics += 1;
            timeBoostTemperature = -2;
            createTemperatureAudio.Play();
        }

        UpdateText();
    }
    public void CreatePeasant()
    {
        if (treeCount >= workerCost)
        {
            treeCount -= workerCost;
            timeRecruitmentWorker = workerCreateTime;
            workerButton.interactable = false;
            UpdateText();
        }
        else
        {
            noWoodAudio.Play();
        }
    }

    public void CreateTemperature()
    {
        if (treeCount >= temperatureCost)
        {
            treeCount -= temperatureCost;
            timeBoostTemperature = temperatureCreateTime;
            TemperatureButton.interactable = false;
            UpdateText();
        }
        else
        {
            noWoodAudio.Play();
        }
    }
    public void onPlaySaund()
    {
        if (ambientAudio.isPlaying)
        {
            StormAudio.gameObject.SetActive(false);
            fallingTreeAudio.gameObject.SetActive(false);
            kindlingFurnaceAudio.gameObject.SetActive(false);
            noWoodAudio.gameObject.SetActive(false);
            clickAudio.gameObject.SetActive(false);
            createTemperatureAudio.gameObject.SetActive(false);
            createWorkersAudio.gameObject.SetActive(false);
            ambientAudio.Pause();
            play.SetActive(false);
            stop.SetActive(true);
        }
        else
        {
            fallingTreeAudio.gameObject.SetActive(true);
            kindlingFurnaceAudio.gameObject.SetActive(true);
            clickAudio.gameObject.SetActive(true);
            createTemperatureAudio.gameObject.SetActive(true);
            createWorkersAudio.gameObject.SetActive(true);
            ambientAudio.Play();
            StormAudio.gameObject.SetActive(true);
            noWoodAudio.gameObject.SetActive(true);
            play.SetActive(true);
            stop.SetActive(false);
        }
    }
    public void OnPauseClick()
    {
        Time.timeScale = 0f;
        pausePannel.SetActive(true);
    }
    public void OffPauseClick()
    {
        Time.timeScale = 1f;
        pausePannel.SetActive(false);
    }
    public void onResetClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    
    private void UpdateText()
    {
        treeText.text = treeCount.ToString();
        TemperatureText.text = "�C" + " " + temperatureCount;
        workerText.text = workerCount.ToString();
    }
}
