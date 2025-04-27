using UnityEngine;
using TMPro;

public class EnterAgentCount : MonoBehaviour
{
    private TMP_InputField inputField;
    [SerializeField] private InitializeMap mapScript;

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener(OnInputEndEdit);
    }

    void OnInputEndEdit(string input)
    {
        int value;
        if (int.TryParse(input, out value))
        {
            Debug.Log("Entered value: " + value);
            if (mapScript != null)
            {
                mapScript.Initialize(value);
            }
            else
            {
                Debug.LogWarning("Map script reference not set!");
            }
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Debug.Log("Please enter a valid integer.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
