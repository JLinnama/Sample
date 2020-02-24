using UnityEngine;
using TMPro;

public class DataManager : MonoBehaviour
{

    // Handles saves and contains different utility methods

    public static DataManager instance;

    public static string hiscoreSave = "Hiscore";
    public static string aircraftSave = "Aircrafts";

    private void Awake()
    {
        instance = this;
    }

    public void Save(string save, int amount)
    {
        PlayerPrefs.SetInt(save, amount);
    }

    public void UpdateText(TextMeshProUGUI text, string message)
    {
        text.text = message;
    }

    public bool IsItemUnlocked(string type, int index)
    {
        return PlayerPrefs.GetString(type)[index] == 'b';
    }

    // WriteSaveString writes a simple save string as PlayerPrefs for different contents
    // For example if the game has 5 planes, the save will be "aaaaa"
    // If the player owns the first plane, the save will be "baaaa" and so on ('b' = owned)
    // On app launch the method checks if new planes have been added and modifies the save accordingly
    // If the player unlocks a new plane the method will insert 'b' to the planes index on the save string

    public void WriteSaveString(bool onLaunch, string newState, int index, string save, Transform content)
    {
        string previousSave = null;
        string newSaveString = string.Empty;

        if (PlayerPrefs.HasKey(save))
        {
            previousSave = PlayerPrefs.GetString(save);
        }
        else
        {
            foreach (Transform g in content.transform)
            {
                string freshState = "a";

                if (g.GetSiblingIndex() == 0)
                {
                    freshState = "b";
                }

                newSaveString += freshState;
            }
        }

        if (previousSave != null)
        {
            newSaveString = previousSave;
        }

        int newItems = 0;

        if (content.transform.childCount > newSaveString.Length)
        {
            int amountOfZerosToBeAdded = content.transform.childCount - newSaveString.Length;

            for (int i = 0; i < amountOfZerosToBeAdded; i++)
            {
                string addedState = "a";

                newSaveString += addedState;
                newItems += 1;
            }
        }

        if (newItems > 0)
        {

        }

        if (onLaunch == false)
        {
            int currentItemIndex = index;

            string oldState = "a";
            oldState = newSaveString[currentItemIndex].ToString();

            System.Text.StringBuilder stringiBuilderi = new System.Text.StringBuilder(newSaveString);
            stringiBuilderi[currentItemIndex] = char.Parse(newState);
            newSaveString = stringiBuilderi.ToString();
        }

        PlayerPrefs.SetString(save, newSaveString);
    }
}
