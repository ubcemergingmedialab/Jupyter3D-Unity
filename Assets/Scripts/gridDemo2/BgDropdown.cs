using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgDropdown : MonoBehaviour
{
    [Tooltip("Dropdown to choose the skybox background")]
    public Dropdown dropdown;

    public Material[] backgrounds;  // background materials
    public Sprite[] thumbnails;     // background thumbnails
    public Sprite def;

    Material skybox;

    void Start()
    {
        // Clear initial dropdown options
        dropdown.ClearOptions();

        // Create a list of the skybox options
        List<Dropdown.OptionData> skyboxOptions = new List<Dropdown.OptionData>();

        // Allocate space for default "no skybox" option

        var noSkybox = new Dropdown.OptionData("Default");
        skyboxOptions.Add(noSkybox);

        var listEnumerator = backgrounds.GetEnumerator();
        for (var i = 0; listEnumerator.MoveNext() == true; i++)
        {
            var skyboxOption = new Dropdown.OptionData(backgrounds[i].name, thumbnails[i]);    // format based on name and image
            skyboxOptions.Add(skyboxOption);    // add each option to list
        }

        dropdown.AddOptions(skyboxOptions);     // add list to dropdown

        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    // Update is called once per frame
    void DropdownValueChanged(Dropdown change)
    {
        // string value = backgrounds[dropdown.value].name;
        // Debug.Log("value is " + value);
        if(dropdown.value == 0)
        {
            RenderSettings.skybox = null;
        }
        else
        {
            // Change skybox based on dropdown
            skybox = backgrounds[dropdown.value - 1];
            RenderSettings.skybox = skybox;
        }
    }
}
