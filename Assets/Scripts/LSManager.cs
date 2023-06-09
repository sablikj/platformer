using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LSManager : MonoBehaviour
{
    public LSPlayer player;
    private MapPoint[] allPoints;

    // Start is called before the first frame update
    void Start()
    {
        // Setting correct position of player in the overworld after level finish
        allPoints = FindObjectsOfType<MapPoint>();

        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            foreach (MapPoint point in allPoints)
            {
                if(point.levelToLoad == PlayerPrefs.GetString("CurrentLevel"))
                {
                    player.transform.position = point.transform.position;
                    player.currentPoint = point;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelCo());
    }

    public IEnumerator LoadLevelCo()
    {
        AudioManager.instance.PlaySFX(4);
        LSUIController.instance.FadeToBlack();
        yield return new WaitForSeconds((1f / LSUIController.instance.fadeSpeed) + .25f);
        SceneManager.LoadScene(player.currentPoint.levelToLoad);
    }
}
