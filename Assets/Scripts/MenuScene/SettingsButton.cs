using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsButton : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.name.Contains("Tongue"))
        {
            StartCoroutine(LoadGame());
        }
    }

    private IEnumerator LoadGame ()
    {
        buttonAudio.SetActive(true);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,0);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("SettingsScene");
        yield break;
    }
}
