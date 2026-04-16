using UnityEngine;

public class enableTest : MonoBehaviour
{
    public ProfitBoard profitBoard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {

        profitBoard.StartNewDay();
    }
}
