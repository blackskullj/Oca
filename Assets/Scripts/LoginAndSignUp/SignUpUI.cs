using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class SignUpUI : MonoBehaviour
{
    #region Firebase Variables

    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    #endregion

    #region Serialized Fields

    [SerializeField]
    private TMP_InputField UserInput;
    [SerializeField]
    private TMP_InputField MailInput;
    [SerializeField]
    private TMP_InputField PasswordInput;
    [SerializeField]
    private TMP_Dropdown RoleInput;

    #endregion

    private const int NO_ROLE = 0;
    private const int STUDENT_ROLE = 1;
    private const int TEACHER_ROLE = 2;

    void Awake()
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

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene(Scenes.LOGIN_SCENE);
    }

    public void OnRegisterButtonClicked()
    {
        Register();
    }

    private void Register()
    {
        if (UserInput.text == "")
        {
            Debug.Log("Username is missing!");
        }
        else if (MailInput.text == "")
        {
            Debug.Log("Mail is missing!");
        }
        else if (PasswordInput.text == "")
        {
            Debug.Log("Password is missing!");
        }
        else if (RoleInput.value == NO_ROLE)
        {
            Debug.Log("Role is missing!");
        }
        else
        {
            StartCoroutine(RegisterOnDatabase(UserInput.text, MailInput.text, PasswordInput.text, RoleInput.options[RoleInput.value].text));
        }

    }

    private IEnumerator RegisterOnDatabase(string userName, string email, string password, string role)
    {
        //Call the Firebase auth signin function passing the email and password
        var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Register Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WeakPassword:
                    message = "Weak Password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Email Already In Use";
                    break;
            }
            Debug.Log(message);
        }
        else
        {
            //User has now been created
            //Now get the result
            User = RegisterTask.Result;

            if (User != null)
            {
                //Create a user profile and set the username
                UserProfile profile = new UserProfile { DisplayName = userName };

                //Call the Firebase auth update user profile function passing the profile with the username
                var ProfileTask = User.UpdateUserProfileAsync(profile);
                //Wait until the task completes
                yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                if (ProfileTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                    Debug.Log("Username Set Failed!");
                }
                else
                {
                    //Username is now set
                    StartCoroutine(SaveUserRole(role));
                }
            }
        }
    }

    private IEnumerator SaveUserRole(string role)
    {
        //Set the currently logged in user
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("role").SetValueAsync(role);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Role is now updated
            Debug.Log("Everything worked! Congratulations, you now have a new user!");
        }
    }

    /*private IEnumerator RegisterUsername()
    {
        //Set value on database
        var DBTask = DBreference.Child("users").Child("01").Child("username").SetValueAsync(UserInput.text);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //User is now set
        }
    }

    private IEnumerator RegisterPassword()
    {
        //Set value on database
        var DBTask = DBreference.Child("users").Child("01").Child("password").SetValueAsync(PasswordInput.text);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Password is now set
        }
    }

    private IEnumerator RegisterRole()
    {
        string role = "";

        if (RoleInput.value == 1)
        {
            role = "ALUMNO";
        }
        else if (RoleInput.value == 2)
        {
            role = "DOCENTE";
        }
        //Set value on database
        var DBTask = DBreference.Child("users").Child("01").Child("role").SetValueAsync(role);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Role is now set
        }
    }*/

}
