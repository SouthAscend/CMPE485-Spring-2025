using UnityEngine;
using TMPro;

public class EnterAgentCount : MonoBehaviour
{
    private TMP_InputField inputField;
    [SerializeField] private InitializeMap mapScript;
    [SerializeField] private AIManager aiManager;
    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onEndEdit.AddListener(OnInputEndEdit);
    }

    void OnInputEndEdit(string input)
    {
        string[] values = input.Split(' ');
        if (values.Length != 2)
        {
            Debug.Log("Please enter two values separated by a space: <int> <float>");
            return;
        }

        if (int.TryParse(values[0], out int agentCount) && float.TryParse(values[1], out float ratio))
        {
            if (ratio < 0.05f || ratio > 1.0f)
            {
                Debug.Log("The ratio must be between 0.05 and 1.0");
                return;
            }

            Debug.Log($"Entered agent count: {agentCount}, ratio: {ratio}");
            aiManager.agentRatio = ratio;
            Debug.Log($"Set GlobalMaps.agentRatio to: {aiManager.agentRatio}");

            if (mapScript != null)
            {
                mapScript.Initialize(agentCount);
            }
            else
            {
                Debug.LogWarning("Map script reference not set!");
            }
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Debug.Log("Please enter valid numbers. First value should be an integer, second should be a float between 0.05 and 1.0");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
