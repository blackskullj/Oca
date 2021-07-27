using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class firebaseManagerScript : MonoBehaviour
{

    [SerializeField]
    private UIManager _uiManager;

    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

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

    public IEnumerator PrepareQuestion(string question)
    {
        string value = "-1";
        int questionCount;
        var DBTask = DBreference.Child("questionCount").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.Log(DBTask.Exception.ToString());
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            if (snapshot.Exists)
                value = snapshot.Child("Count").Value.ToString();
        }

        int.TryParse(value, out questionCount);

        if (questionCount == -1)
            Debug.Log("Homie, algo pasó. No habemos preguntas");
        else {
            if (questionCount > 1)
            {
                System.Random random = new System.Random();
                int questionRange = random.Next(questionCount);
                questionCount = questionRange;
            }
            else
                questionCount--;

            StartCoroutine(GetQuestion(question, questionCount));
        }
    }

    private IEnumerator GetQuestion(string question, int questionID)
    {
        var DBTask = DBreference.Child("questions").Child(questionID.ToString()).GetValueAsync();
        string answer = "";
        string option1 = "";
        string option2 = "";

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
            Debug.Log(DBTask.Exception.ToString());
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            question = snapshot.Child("question").Value.ToString();
            answer = snapshot.Child("answer").Value.ToString();
            option1 = snapshot.Child("option 1").Value.ToString();
            option2 = snapshot.Child("option 2").Value.ToString();
            StartCoroutine(_uiManager.GetComponent<UIManager>().PrepareForQuestion(question, answer, option1, option2));
        }

    }

}