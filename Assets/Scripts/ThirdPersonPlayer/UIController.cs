using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerHealth))]
public class UIController : MonoBehaviour
{
    [SerializeField] Canvas UIParent = null;

    [Header("HealthComponents")]
    [SerializeField] Generator _generator = null;
    [SerializeField] Slider _healthSlider = null;
    [SerializeField] Image _healthSliderFill = null;
    [SerializeField] Text _healthText = null;
    [SerializeField] Text _changeText = null;
    [SerializeField] Text _timerText = null;
    [SerializeField] Color _damageColor;
    [SerializeField] float _damageFlashTime = 0.25f;

    PlayerController _movementScript = null; 
    PlayerHealth _playerHealth = null;
    Coroutine _damageCoroutine = null;

    private void Awake()
    {
        _movementScript = GetComponent<PlayerController>();
        _playerHealth = GetComponent<PlayerHealth>();
    }

    #region subscriptions
    private void OnEnable()
    {
        _generator.TimerUpdated += UpdateTimerDisplay;
        _playerHealth.HealthSet += UpdateHealthSlider;
        _playerHealth.TookDamage += DamageFeedback;

    }

    private void OnDisable()
    {
        _generator.TimerUpdated -= UpdateTimerDisplay;
        _playerHealth.HealthSet -= UpdateHealthSlider;
        _playerHealth.TookDamage -= DamageFeedback;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _healthSlider.minValue = 0;
        _healthSlider.maxValue = _playerHealth.MaxHealth;
        UpdateHealthSlider(_playerHealth.HP);
    }


    void CreateUI()
    {
        Debug.Log("Create");
        UIParent.gameObject.SetActive(true);
        _playerHealth.HealthSet += UpdateHealthSlider;
        _playerHealth.TookDamage += DamageFeedback;
        _playerHealth.HealthRestored += HealFeedback;
        UpdateHealthSlider(_playerHealth.HP);
    }

    void HideUI()
    {
        Debug.Log("Hide");
        UIParent.gameObject.SetActive(false);
        _playerHealth.HealthSet -= UpdateHealthSlider;
        _playerHealth.TookDamage -= DamageFeedback;
        _playerHealth.HealthRestored -= HealFeedback;
    }

    private void UpdateHealthSlider(int healthToSet)
    {
        _healthSlider.value = healthToSet;
        _healthText.text = healthToSet.ToString() + " / " + _playerHealth.MaxHealth;
    }

    private void DamageFeedback(int damageAmount)
    {
        _damageCoroutine = null;
        _damageCoroutine = StartCoroutine(HealthBarFlash());
    }

    private void UpdateTimerDisplay(float timer)
    {
        if (timer <= 0)
            _timerText.gameObject.SetActive(false);
        else
        {
            if(!_timerText.gameObject.activeSelf)
                _timerText.gameObject.SetActive(true);
            int timerVal = Mathf.CeilToInt(timer);
            _timerText.text = "00 : " + (timerVal < 10 ? "0" : "") + timerVal;
        }
    }

    private void HealFeedback(int healAmount)
    {
        // something with the numbers here
    }

    IEnumerator HealthBarFlash()
    {
        Color tempColor = _healthSliderFill.color;
        _healthSliderFill.color = _damageColor;
        yield return new WaitForSeconds(_damageFlashTime);
        _healthSliderFill.color = tempColor;
        _damageCoroutine = null;
    }
}
