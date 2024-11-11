using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialCanvasScript : MonoBehaviour
{
    public List<GameObject> tutorialPages;
    public GameObject tutorialPanelGameobject;

    int actualPage = 0;

    private void Start()
    {
        LeanTween.scale(this.gameObject, Vector3.one, 1.5f);
        tutorialPages[actualPage].SetActive(true);
    }

    public void nextPage()
    {
        tutorialPages[actualPage].SetActive(false);
        actualPage++;
        tutorialPages[actualPage].SetActive(true);
    }

    public void previousPage()
    {
        tutorialPages[actualPage].SetActive(false);
        actualPage--;
        tutorialPages[actualPage].SetActive(true);
    }

    public void startGame()
    {
        LeanTween.scale(this.gameObject, Vector3.zero, 1.5f).setOnComplete(() => { this.gameObject.SetActive(false); });
    }
}
