using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class TeacherFirebaseManager : MonoBehaviour
{

    #region Firebase Variables

    private DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    private FirebaseUser User;
    private DatabaseReference DBreference;

    #endregion

    #region UI Objects

    [SerializeField]
    private TMP_InputField QuestionInput;
    [SerializeField]
    private TMP_InputField AnswerInput;
    [SerializeField]
    private TMP_InputField[] OptionsInput;

    #endregion

    private void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        Debug.Log("It seems everything's alright!");
    }

    public void OnCreateButtonClicked()
    {
        CreateQuestion(QuestionInput.text, AnswerInput.text, OptionsInput[0].text, OptionsInput[1].text);
    }

    private void CreateQuestion(string question, string answer, string option1, string option2)
    {
        if (question == "")
            Debug.Log("Question is missing!");
        else if (answer == "")
            Debug.Log("Answer is missing!");
        else if (option1 == "")
            Debug.Log("Option a) is missing!");
        else if (option2 == "")
            Debug.Log("Option b) is missing!");
        else
            RegisterOnDatabase(question, answer, option1, option2);

    }

    private void RegisterOnDatabase(string question, string answer, string option1, string option2)
    {
        /*StartCoroutine(RegisterQuestion(question));
        StartCoroutine(RegisterAnswer(answer));
        StartCoroutine(RegisterOption1(option1));
        StartCoroutine(RegisterOption2(option2));*/
        //StartCoroutine(GetQuestion());
        //Debug.Log(GetQuestion());
        StartCoroutine(CheckDatabase(question, answer, option1, option2));
    }

    private IEnumerator CheckDatabase(string question, string answer, string option1, string option2)
    {
        string value = "-2";
        //Retrieve the data and convert it to string...
        //FirebaseDatabase.DefaultInstance.GetReference("test").Child("questionCount").GetValueAsync().ContinueWith(task =>
        /*FirebaseDatabase.DefaultInstance.GetReference("questionCount").GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot snapshot = task.Result;
            if (!snapshot.Exists)
            {
                Debug.Log("Hola esta");
                value = "0";
                esta = 0;
            }
            else
            {
                Debug.Log("Hola");
                value = snapshot.Child("Count").Value.ToString();
                Debug.Log("Value when exists: " + value);
                esta = 1;
            }
        });*/

        var dbTask = DBreference.Child("questionCount").GetValueAsync();

        yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

        if (dbTask.Exception != null)
        {
            Debug.Log("F, algo pasó");
        }
        else
        {
            DataSnapshot snapshot = dbTask.Result;

            if (!snapshot.Exists)
                value = "0";
            else
            {
                value = snapshot.Child("Count").Value.ToString();
                Debug.Log(value);
            }
        }

        StartCoroutine(RegisterQuestion(question, value));
        StartCoroutine(RegisterAnswer(answer, value));
        StartCoroutine(RegisterOption1(option1, value));
        StartCoroutine(RegisterOption2(option2, value));
        StartCoroutine(UpdateDatabase(value));

    }

    private IEnumerator RegisterQuestion(string question, string questionID)
    {
        var DBTask = DBreference.Child("questions").Child(questionID).Child("question").SetValueAsync(question);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Question is now updated
            Debug.Log("Question has now been updated!");
        }
    }

    private IEnumerator RegisterAnswer(string answer, string questionID)
    {
        var DBTask = DBreference.Child("questions").Child(questionID).Child("answer").SetValueAsync(answer);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Option1 is now updated
            Debug.Log("Answer has now been updated!");
        }
    }

    private IEnumerator RegisterOption1(string option, string questionID)
    {
        var DBTask = DBreference.Child("questions").Child(questionID).Child("option 1").SetValueAsync(option);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Option 1 is now updated
            Debug.Log("Option 1 has now been updated!");
        }
    }

    private IEnumerator RegisterOption2(string option, string questionID)
    {
        var DBTask = DBreference.Child("questions").Child(questionID).Child("option 2").SetValueAsync(option);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Option 2 is now updated
            Debug.Log("Option 2 has now been updated!");
        }
    }

    private IEnumerator UpdateDatabase(string value)
    {
        int countValue = 0;
        int.TryParse(value, out countValue);
        countValue++;

        var DBTask = DBreference.Child("questionCount").Child("Count").SetValueAsync(countValue.ToString());

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //questionCount is now updated
        }

    }

}
