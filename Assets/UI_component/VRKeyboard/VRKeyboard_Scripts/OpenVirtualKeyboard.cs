using UnityEngine;
using TMPro;

public class OpenVirtualKeyboard : MonoBehaviour
{
    public TMP_InputField inputField; // Asigna desde el Inspector
    private GameObject virtualKeyboard;
    private RectTransform keyboardBackground;

    [HideInInspector]
    public bool onExitKeyboardArea;

    private void Awake()
    {
        virtualKeyboard = GameObject.Find("Virtual Keyboard");
        if (virtualKeyboard == null)
            Debug.LogError("Pls drag the {Virtual Keyboard} prefab in your scene");
        else
            keyboardBackground = virtualKeyboard.transform.Find("Background").GetComponent<RectTransform>();

        if (inputField == null)
            Debug.LogError("Asigna el TMP_InputField al script OpenVirtualKeyboard");
    }

    private void Update()
    {
        if (onExitKeyboardArea)
            OnCloseVirtualKeyboard();
        else
            OnOpenVirtualKeyboard();
    }

    public void OnOpenVirtualKeyboard()
    {
        if (virtualKeyboard.activeSelf)
            return;

        SetupKeyboardSize();
        virtualKeyboard.SetActive(true);
        inputField.ActivateInputField(); // <-- Esto enfoca el campo
    }

    public void OnCloseVirtualKeyboard()
    {
        if (!virtualKeyboard.activeSelf)
            return;

        virtualKeyboard.SetActive(false);
    }

    private void SetupKeyboardSize()
    {
        float keyboardWidth = 485;
        float keyboardHeight = 485;
        keyboardBackground.sizeDelta = new Vector2(keyboardWidth, keyboardHeight);
    }

    // --- Métodos que llamarán las teclas ---
    public void AddCharacter(string character)
    {
        inputField.text += character;
        inputField.caretPosition = inputField.text.Length;
    }

    public void Backspace()
    {
        if (inputField.text.Length > 0)
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
    }

    public void ConfirmName()
    {
        PlayerPrefs.SetString("PlayerName", inputField.text);
        PlayerPrefs.Save();
        Debug.Log("Nombre guardado: " + inputField.text);
        OnCloseVirtualKeyboard();
        // Aquí podrías cargar la siguiente escena
    }

    public void SetInputField(TMP_InputField field)
    {
        inputField = field;
        inputField.ActivateInputField();
    }
}
