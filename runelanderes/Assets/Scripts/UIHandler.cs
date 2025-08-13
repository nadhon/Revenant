using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance { get; private set; }
    private VisualElement m_HealthBar;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        UIDocument uiDocument = GetComponent<UIDocument>();
        m_HealthBar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);
        Debug.Log("Achei o erro");
        
    }

    public void SetHealthValue(float percentage)
    {
        if (m_HealthBar != null)
        {
            m_HealthBar.style.width = Length.Percent(100 * percentage);
            
        }
    }

}
