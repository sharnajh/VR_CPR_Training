//-----------------------------------------------------------------------
// <copyright file="ObjectController.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls target objects behaviour.
/// </summary>
public class ObjectController : MonoBehaviour
{
    bool Identified = false;
    bool ConditionQuiz = false;
    bool CheckPulse = false;
    bool Compressions = false;
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;

    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;

    // References to UI elements
    public Text textObject;
    public Text countdownObject;
    public Text compressionTextObject;
    public Button trueButton;
    public Button falseButton;
    public GameObject pulseUI;
    public GameObject compressionsUI;
    public GameObject thankYouSound;

    public GameObject Player;
    private UserMovement CrouchedBool;

    // The objects are about 1 meter in radius, so the min/max target distance are
    // set so that the objects are always within the room (which is about 5 meters
    // across).
    private const float _minObjectDistance = 2.5f;
    private const float _maxObjectDistance = 3.5f;
    private const float _minObjectHeight = 0.5f;
    private const float _maxObjectHeight = 3.5f;

    private Renderer _myRenderer;
    private Vector3 _startingPosition;

    float pulseTimer = 10.0f;
    // For the sake of presentation, set to 25. Functionally, it should be 60.
    float compressionTimer = 25.0f;
    int compressionCount = 0;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        CrouchedBool = Player.GetComponent<UserMovement>();
        textObject.text = "Looks like someone needs your help! Please identify the subject who needs CPR.";
        _startingPosition = transform.parent.localPosition;
        _myRenderer = GetComponent<Renderer>();
        SetMaterial(false);
    }

    public void Update()
    {
        if (CheckPulse)
        {
            pulseUI.gameObject.SetActive(true);
            if (pulseTimer > 0)
            {
                countdownObject.text = Mathf.Round(pulseTimer).ToString();
                pulseTimer -= Time.deltaTime;
            }
            else
            {
                pulseUI.gameObject.SetActive(false);
                compressionsUI.gameObject.SetActive(true);
                textObject.text = "The patient's pulse is very faint! Start chest compressions now! Compression Rate is 100 - 120 compressions per minute!";
                CheckPulse = false;
                Compressions = true;
            }
        }

        if (Compressions && compressionCount > 0)
        {
            if (compressionTimer > 0)
            {
                compressionTextObject.text = "Compressions: " + compressionCount.ToString() + "\nTimer: 00:" + Mathf.Round(compressionTimer).ToString();
                compressionTimer -= Time.deltaTime;
            }
            else
            {
                Compressions = false;
                compressionsUI.gameObject.SetActive(false);
                thankYouSound.gameObject.SetActive(true);
                textObject.text = "\"Thank you! You saved my life!\"";
            }
        }
    }


    /// <summary>
    /// Teleports this instance randomly when triggered by a pointer click.
    /// </summary>
    public void TeleportRandomly()
    {
        // Picks a random sibling, activates it and deactivates itself.
      /*  int sibIdx = transform.GetSiblingIndex();
        int numSibs = transform.parent.childCount;
        sibIdx = (sibIdx + Random.Range(1, numSibs)) % numSibs;
        GameObject randomSib = transform.parent.GetChild(sibIdx).gameObject;*/

        // Computes new object's location.
  /*      float angle = Random.Range(-Mathf.PI, Mathf.PI);
        float distance = Random.Range(_minObjectDistance, _maxObjectDistance);
        float height = Random.Range(_minObjectHeight, _maxObjectHeight);
        Vector3 newPos = new Vector3(Mathf.Cos(angle) * distance, height,
                                     Mathf.Sin(angle) * distance);

        // Moves the parent to the new position (siblings relative distance from their parent is 0).
        transform.parent.localPosition = newPos;*/
/*
        gameObject.SetActive(false);
        SetMaterial(false);*/
    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        SetMaterial(true);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        SetMaterial(false);
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        /*TeleportRandomly();*/
        Debug.Log(gameObject.name);

        if (!Identified)
        {
            Identified = true;
            textObject.text = "Assess the situation. What has happened to the patient?";
            trueButton.gameObject.SetActive(true);
            falseButton.gameObject.SetActive(true);
        }
        if (ConditionQuiz && CrouchedBool.crouched)
        {
            textObject.text = "Check the pulse and breathing of the patient for 10 seconds...";
            CheckPulse = true;
            ConditionQuiz = false;
        }
        if (Compressions)
        {
            compressionCount++;
        }
    }

    public void FalseQuiz()
    {
        textObject.text = "The patient is clearly not drowning. Assess and try again.";
        falseButton.gameObject.SetActive(false);
    }

    public void CorrectQuiz()
    {
        ConditionQuiz = true;
        trueButton.gameObject.SetActive(false);
        falseButton.gameObject.SetActive(false);
        textObject.text = "The patient is choking. Crouch down to the patient's level to check their pulse.";
    }

    /// <summary>
    /// Sets this instance's material according to gazedAt status.
    /// </summary>
    ///
    /// <param name="gazedAt">
    /// Value `true` if this object is being gazed at, `false` otherwise.
    /// </param>
    private void SetMaterial(bool gazedAt)
    {
        if (InactiveMaterial != null && GazedAtMaterial != null)
        {
            _myRenderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
        }
    }
}
