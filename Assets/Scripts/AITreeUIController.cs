using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Required for TextMeshProUGUI

public class AITreeUIController : MonoBehaviour
{
    [Header("Sliders")]
    public Slider orbitSpeedSlider;
    public Slider orbitZoomSlider;
    public Slider growthSpeedSlider;

    [Header("TextMeshPro Displays")]
    public TextMeshProUGUI currentEnergyText;
    public TextMeshProUGUI energyInText;
    public TextMeshProUGUI energyOutText;

    [Header("Buttons")]
    public Button resetButton;

    [Header("Controllers")]
    public OrbitCamera orbitCamera;
    public AITreeRunner treeRunner;

    private bool isAdjustingGrowthRate = false;

    void Start()
    {
        orbitSpeedSlider.onValueChanged.AddListener(OnOrbitSpeedChanged);
        orbitZoomSlider.onValueChanged.AddListener(OnOrbitZoomChanged);
        growthSpeedSlider.onValueChanged.AddListener(OnGrowthSpeedChanged);
        resetButton.onClick.AddListener(OnResetClicked);

        UpdateAllUI();
    }

    void Update()
    {
        UpdateEnergyDisplay();
    }

    void UpdateAllUI()
    {
        orbitCamera.Speed = orbitSpeedSlider.value;
        orbitCamera.PaddingMultiplier = orbitZoomSlider.value;
        treeRunner.SetGrowthInterval(growthSpeedSlider.value);
        UpdateEnergyDisplay();
    }

    void UpdateEnergyDisplay()
    {
        if (treeRunner == null) return;

        currentEnergyText.text = $"Current Energy: {treeRunner.GetCurrentEnergy()}";
        energyInText.text = $"Energy In: {treeRunner.GetEnergyIn()}";
        energyOutText.text = $"Energy Out: {treeRunner.GetEnergyOut()}";
    }

    void OnOrbitSpeedChanged(float value)
    {
        orbitCamera.Speed = value;
    }

    void OnOrbitZoomChanged(float value)
    {
        orbitCamera.PaddingMultiplier = value;
    }

    void OnGrowthSpeedChanged(float value)
    {
        if (isAdjustingGrowthRate) return;

        isAdjustingGrowthRate = true;
        treeRunner.SetGrowthInterval(value);
        isAdjustingGrowthRate = false;
    }

    void OnEnergyChanged(float value)
    {
        treeRunner.SetCurrentEnergy((int)value);
        UpdateEnergyDisplay();
    }

    void OnResetClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
