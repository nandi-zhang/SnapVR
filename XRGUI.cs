using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XRGUI : MonoBehaviour
{
    [SerializeField]
    private Button lilbutton;

    public Button exit;
    public Button robodemo;

    public GameObject cube;
    public GameManager instance;

    // Start is called before the first frame update
    void Start()
    {
        lilbutton.onClick.AddListener(() =>
        {
            // Vector3 loc = new Vector3(0f, 0.7f, -0.2f);
            // GameObject lilcube = Instantiate(cube, loc, Quaternion.identity);
            instance.UpdateGameState(GameState.Snapshot);
        });

        exit.onClick.AddListener(() =>
        {
            // Vector3 loc = new Vector3(0f, 0.7f, -0.2f);
            // GameObject lilcube = Instantiate(cube, loc, Quaternion.identity);
            instance.UpdateGameState(GameState.EndDemo);
        });

        robodemo.onClick.AddListener(() =>
        {
            // Vector3 loc = new Vector3(0f, 0.7f, -0.2f);
            // GameObject lilcube = Instantiate(cube, loc, Quaternion.identity);
            instance.UpdateGameState(GameState.Collaboration);
        });
    }

    // Update is called once per frames
    void Update()
    {
        
    }
}
