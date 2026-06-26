using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [SerializeField] private GameObject tooltipPrefab;

    private GameObject panel;
    private TMP_Text title;
    private TMP_Text description;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


    }

    private void Update()
    {
        if (panel == null) return;
        if (panel.activeSelf)
        {
            panel.transform.position = Mouse.current.position.ReadValue() + new Vector2(-100, 100);
        }
    }

    public void Show(string Title, string Description)
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        panel = Instantiate(tooltipPrefab, canvas.transform);

        title = panel.transform.Find("Title").GetComponent<TMP_Text>();
        description = panel.transform.Find("Description").GetComponent<TMP_Text>();
        title.text = Title;
        description.text = Description;
        panel.transform.SetAsLastSibling();
    }

    public void Hide()
    {
        if (panel == null) return;
        Destroy(panel);
        panel = null;
    }
}