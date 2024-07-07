using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [Header("Global stuff")]
    [SerializeField]
    private GameObject screen_1;
    [SerializeField]
    private GameObject screen_2;
    [SerializeField]
    private GameObject screen_3;
    private GameObject buttonDone;


    [Header("Character creation")]
    [SerializeField]
    private Image hairImage;
    [SerializeField]
    private Image beardImage;
    [SerializeField]
    private Image bodyImage;
    [SerializeField]
    private TextMeshProUGUI hairText;
    [SerializeField]
    private TextMeshProUGUI beardText;
    [SerializeField]
    private TextMeshProUGUI bodyText;
    public enum PicType
    {
        Hair,
        Beard,
        Body,
    }





    public void Start()
    {
        screen_1.SetActive(true);
        screen_2.SetActive(false);
        screen_3.SetActive(false);
        buttonDone = screen_1.transform.Find("Button_done").gameObject;
        // Character creation
        hairImage.sprite = hairImage.GetComponent<SpriteLibrary>().sprites[0];
        beardImage.sprite = beardImage.GetComponent<SpriteLibrary>().sprites[0];
        bodyImage.sprite = bodyImage.GetComponent<SpriteLibrary>().sprites[0];

        // Difficulty settings


        // Class settings

    }

    public void ButtonDoneClicked(int screenIndex)
    {
        switch(screenIndex)
        {
            case 0:
                screen_1.SetActive(false);
                screen_2.SetActive(true);
                break;
            case 1:
                screen_2.SetActive(false);
                screen_3.SetActive(true);
                break;
            case 2:
                screen_3.SetActive(false);

                // TODO - spawn player into tutorial dungeon

                gameObject.SetActive(false);
                break;
        }
    }

    public void ChangePic(PicType type, int index, out int newIndex)
    {
        List<Sprite> sprites = new();
        string text = "";
        switch (type)
        {
            case PicType.Hair:
                sprites = hairImage.GetComponent<SpriteLibrary>().sprites;
                text = "Hair types (";
                break;
            case PicType.Beard:
                sprites = beardImage.GetComponent<SpriteLibrary>().sprites;
                text = "Beard types (";
                break;
            case PicType.Body:
                sprites = bodyImage.GetComponent<SpriteLibrary>().sprites;
                text = "Body types (";
                break;
        }

        if (index < 0)
            newIndex = sprites.Count - 1;
        else if (index >= sprites.Count)
            newIndex = 0;
        else
            newIndex = index;

        text += (newIndex + 1).ToString() + " / " + sprites.Count + ")";

        switch (type)
        {
            case PicType.Hair:
                hairImage.sprite = sprites[newIndex];
                hairText.text = text;
                break;
            case PicType.Beard:
                beardImage.sprite = sprites[newIndex];
                beardText.text = text;
                break;
            case PicType.Body:
                bodyImage.sprite = sprites[newIndex];
                bodyText.text = text;
                break;
        }
    }
}
