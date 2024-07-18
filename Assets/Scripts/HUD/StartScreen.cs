using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour, IDataPersistance
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

    [Header("Class")]
    [SerializeField]
    private List<string> classDescriptions;
    [SerializeField]
    private TextMeshProUGUI descriptionTextField;
    public enum ClassType
    {
        OneHandDexterity,
        OneHandStrenght,
        TwoHandDexterity,
        TwoHandStrenght,
        RangeDexterity,
        RangeStrenght,
        Magic
    }

    [Header("Difficulty")]
    [HideInInspector]
    public int difficulty;
    [SerializeField]
    private List<string> difficultyDescriptions;
    [SerializeField]
    private TextMeshProUGUI diffDescriptionTextField;


    // Saving
    private int[] savedCharacterSprites = new int[3] { 0, 0, 0};



    public void CreateNewCharacter()
    {
        screen_1.SetActive(true);
        screen_2.SetActive(false);
        screen_3.SetActive(false);
        buttonDone = screen_1.transform.Find("Button_done").gameObject;
        buttonDone.GetComponent<Button_Done>().isActive = true;
        // Character creation
        ChangePic(PicType.Hair, 0, out int nothing);
        ChangePic(PicType.Beard, 0, out nothing);
        ChangePic(PicType.Body, 0, out nothing);

        // Difficulty settings
        descriptionTextField.text = "Click on any class to see some description here.";

        // Class settings
        diffDescriptionTextField.text = "Click on any difficulty to see some description here.";
    }

    public void ButtonDoneClicked(int screenIndex)
    {
        switch(screenIndex)
        {
            case 0:
                screen_1.SetActive(false);
                screen_2.SetActive(true);
                buttonDone = screen_2.transform.Find("Button_done").gameObject;
                buttonDone.GetComponent<Button_Done>().isActive = false;

                // Apply gfx to player obj
                StartCoroutine(LoadingDelay());
                break;
            case 1:
                screen_2.SetActive(false);
                screen_3.SetActive(true);
                buttonDone = screen_3.transform.Find("Button_done").gameObject;
                buttonDone.GetComponent<Button_Done>().isActive = false;
                break;
            case 2:
                screen_3.SetActive(false);

                // TODO - spawn player into tutorial dungeon

                gameObject.SetActive(false);
                FindAnyObjectByType<DataPersistanceManager>().SaveGame();
                break;
        }
    }
    public void ClassButtonClicked(ClassType type)
    {
        switch(type)
        {
            case ClassType.OneHandDexterity:
                if (classDescriptions[0] != null)
                    descriptionTextField.text = classDescriptions[0];
                else
                    descriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
            case ClassType.OneHandStrenght:
                if (classDescriptions[1] != null)
                    descriptionTextField.text = classDescriptions[1];
                else
                    descriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
            case ClassType.TwoHandDexterity:
                if (classDescriptions[2] != null)
                    descriptionTextField.text = classDescriptions[2];
                else
                    descriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
            case ClassType.TwoHandStrenght:
                if (classDescriptions[3] != null)
                    descriptionTextField.text = classDescriptions[3];
                else
                    descriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
            case ClassType.RangeDexterity:
                if (classDescriptions[4] != null)
                    descriptionTextField.text = classDescriptions[4];
                else
                    descriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
            case ClassType.RangeStrenght:
                if (classDescriptions[5] != null)
                    descriptionTextField.text = classDescriptions[5];
                else
                    descriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
            case ClassType.Magic:
                if (classDescriptions[6] != null)
                    descriptionTextField.text = classDescriptions[6];
                else
                    descriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
        }
        buttonDone.GetComponent<Button_Done>().isActive = true;
    }
    public void DifficultyButtonClicked(int dif)
    {
        switch (dif)
        {
            case 0:
                if(difficultyDescriptions[0] != null)
                    diffDescriptionTextField.text = difficultyDescriptions[0];
                else
                    diffDescriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
            case 1:
                if (difficultyDescriptions[1] != null)
                    diffDescriptionTextField.text = difficultyDescriptions[1];
                else
                    diffDescriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
            case 2:
                if (difficultyDescriptions[2] != null)
                    diffDescriptionTextField.text = difficultyDescriptions[2];
                else
                    diffDescriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
            case 3:
                if (difficultyDescriptions[3] != null)
                    diffDescriptionTextField.text = difficultyDescriptions[3];
                else
                    diffDescriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
            case 4:
                if (difficultyDescriptions[4] != null)
                    diffDescriptionTextField.text = difficultyDescriptions[4];
                else
                    diffDescriptionTextField.text = "No description yet... (Devs are noobs)";
                break;
        }
        difficulty = dif;
        buttonDone.GetComponent<Button_Done>().isActive = true;
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
        else if (index > sprites.Count - 1)
            newIndex = 0;
        else
            newIndex = index;

        text += (newIndex + 1).ToString() + " / " + sprites.Count + ")";

        switch (type)
        {
            case PicType.Hair:
                hairImage.sprite = sprites[newIndex];
                hairText.text = text;
                savedCharacterSprites[0] = newIndex;
                break;
            case PicType.Beard:
                beardImage.sprite = sprites[newIndex];
                beardText.text = text;
                savedCharacterSprites[1] = newIndex;
                break;
            case PicType.Body:
                bodyImage.sprite = sprites[newIndex];
                bodyText.text = text;
                savedCharacterSprites[2] = newIndex;
                break;
        }
    }


    public void LoadData(GameData data)
    {
        savedCharacterSprites = data.characterSprites;
        difficulty = data.difficulty;
        StartCoroutine(LoadingDelay());
    }
    private IEnumerator LoadingDelay()
    {
        yield return new WaitForEndOfFrame();
        PlayerGFXManager playerGFXManager = transform.parent.parent.GetComponent<PlayerGFXManager>();
        playerGFXManager.hairObj.GetComponent<SpriteRenderer>().sprite = hairImage.GetComponent<SpriteLibrary>().sprites[savedCharacterSprites[0]];
        playerGFXManager.beardObj.GetComponent<SpriteRenderer>().sprite = beardImage.GetComponent<SpriteLibrary>().sprites[savedCharacterSprites[1]];
        playerGFXManager.torsoObj.GetComponent<SpriteRenderer>().sprite = bodyImage.GetComponent<SpriteLibrary>().sprites[savedCharacterSprites[2]];
        playerGFXManager.defaultHair = hairImage.GetComponent<SpriteLibrary>().sprites[savedCharacterSprites[0]];
        playerGFXManager.defaultBeard = beardImage.GetComponent<SpriteLibrary>().sprites[savedCharacterSprites[1]];
        playerGFXManager.defaultBody = bodyImage.GetComponent<SpriteLibrary>().sprites[savedCharacterSprites[2]];
    }
    public void SaveData(ref GameData data)
    {
        data.characterSprites = savedCharacterSprites;
        data.difficulty = difficulty;
    }
}
