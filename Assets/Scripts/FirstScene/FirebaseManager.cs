using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Google;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;

    public string webClientId = "176728289934-mpg50b416lold1l8ldg3tucpqd86li6r.apps.googleusercontent.com";

    public FirebaseUser user;
    public FirebaseAuth auth;
    private FirebaseDatabase database;
    private GoogleSignInConfiguration configuration;

    public InputField loginEmail;
    public InputField loginPassword;

    public InputField registerEmail;
    public InputField registerPassword;

    public InputField displayName;

    public GameObject autoLoginPanel;

    public Text infoTxt;

    public int playerClass;
    public string nickname;
    private bool isNickname;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
            instance = this;
        }

        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
    }

    private void Start()
    {
        autoLoginPanel.SetActive(true);
        StartCoroutine(CheckAndFixDependencies());
        StartCoroutine(UpdateText());
    }

    IEnumerator UpdateText()
    {
        if (infoTxt != null)
        {
            infoTxt.color = new Color32(255, 255, 255, 255);
            infoTxt.color = new Color32(255, 255, 255, 254);
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(UpdateText());
        }
    }

    IEnumerator CheckAndFixDependencies()
    {
        var checkAndFixDependenciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => checkAndFixDependenciesTask.IsCompleted);

        var dependencyResult = checkAndFixDependenciesTask.Result;

        if (dependencyResult == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
        else
        {
            Debug.LogError($"could not resolve all firebase dependancies : {dependencyResult}");
        }
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        StartCoroutine(CheckAutoLogin());

        auth.StateChanged += AuthStateChange;
        AuthStateChange(this, null);
    }

    private void AuthStateChange(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            bool sighedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!sighedIn && user != null)
                Debug.Log("�α׾ƿ�");

            user = auth.CurrentUser;

            if (sighedIn)
                Debug.Log($"�α��� : {user.Email}");
        }
    }

    IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();
        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();

            yield return new WaitUntil(predicate: () => reloadUserTask.IsCompleted);

            StartCoroutine(AutoLogin());
            yield break;
        }
        autoLoginPanel.SetActive(false);
    }

    IEnumerator AutoLogin()
    {
        if (user != null)
        {
            if (user.DisplayName == "")
            {
                autoLoginPanel.SetActive(false);
                FirstSceneUIManger.instance.NickNameBox();
            }
            else
            {
                var savetask = FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync();

                yield return new WaitUntil(predicate: () => savetask.IsCompleted);

                var result = savetask.Result;
                var data = result.Child(user.UserId).Child("playerClass");

                if (data.Value != null)
                {
                    playerClass = int.Parse(data.Value.ToString());
                    SceneManager.LoadScene(1);
                }
                else
                {
                    autoLoginPanel.SetActive(false);
                    FirstSceneUIManger.instance.ClassSet();
                }
            }
        }

    }

    public void Login()
    {
        AudioManager.instance.PlayButtonClip();
        StartCoroutine(LoginLogic(loginEmail.text, loginPassword.text));
    }

    private IEnumerator LoginLogic(string _email, string _password)
    {
        Credential credential = EmailAuthProvider.GetCredential(_email, _password);

        var loginTask = auth.SignInWithCredentialAsync(credential);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;

            string output = "�� �� ���� ������ �߻��߽��ϴ�.";

            switch (error)
            {
                case AuthError.MissingEmail:
                    output = "�̸����� �Է����ּ���.";
                    break;
                case AuthError.MissingPassword:
                    output = "�н����带 �Է����ּ���.";
                    break;
                case AuthError.InvalidEmail:
                    output = "Invalid Email";
                    break;
                case AuthError.WrongPassword:
                    output = "�н����尡 Ʋ�Ƚ��ϴ�. �ٽ� �õ����ּ���.";
                    break;
                case AuthError.UserNotFound:
                    output = "������ �������� �ʽ��ϴ�.";
                    break;
            }

            infoTxt.text = output;
        }

        else
        {
            user = auth.CurrentUser;
            if (user.DisplayName == "")
            {
                FirstSceneUIManger.instance.NickNameBox();
            }
            else
            {
                var savetask = FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync();

                yield return new WaitUntil(predicate: () => savetask.IsCompleted);

                var result = savetask.Result;
                var data = result.Child(user.UserId).Child("playerClass");

                if (data.Value != null)
                {
                    playerClass = int.Parse(data.Value.ToString());
                    SceneManager.LoadScene(1);
                }
                else
                {
                    FirstSceneUIManger.instance.ClassSet();
                }
            }
        }
    }

    public void Register()
    {
        AudioManager.instance.PlayButtonClip();
        auth.CreateUserWithEmailAndPasswordAsync(registerEmail.text, registerPassword.text).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                infoTxt.text = registerEmail.text + "�� ȸ������ �ϼ̽��ϴ�.";
            }

            else
            {
                infoTxt.text = "ȸ�����Կ� ���� �Ͽ����ϴ�.";
            }
        });
    }

    public void GoogleLogin()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        infoTxt.text = "�α��� �õ� ��...";

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted && task.IsCanceled)
        {
            infoTxt.text = "OnAuthenticationFinished ���� �߻�";
        }
        else
        {
            infoTxt.text = "Google �α��� ����";
            StartCoroutine(SignInWithGoogleOnFireBase(task.Result.IdToken));
        }
    }

    private IEnumerator SignInWithGoogleOnFireBase(string idToken)
    {
        Debug.Log("SignInWithGoogleOnFireBase");
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        var task = auth.SignInWithCredentialAsync(credential);
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            infoTxt.text = "Google Login ���� �߻�" + task.Exception;
        }
        else
        {
            user = auth.CurrentUser;
            if (user.DisplayName == "")
            {
                infoTxt.text = "�г��� ����";
                FirstSceneUIManger.instance.NickNameBox();
            }
            else
            {
                var savetask = FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync();

                yield return new WaitUntil(predicate: () => savetask.IsCompleted);

                var result = savetask.Result;
                var data = result.Child(user.UserId).Child("playerClass");

                if (data.Value != null)
                {
                    playerClass = int.Parse(data.Value.ToString());
                    SceneManager.LoadScene(1);
                }
                else
                {
                    FirstSceneUIManger.instance.ClassSet();
                }
            }
        }
    }

    public void NickNameCheck()
    {
        AudioManager.instance.PlayButtonClip();
        StartCoroutine(SetNickname());
    }

    private IEnumerator SetNickname()
    {
        UserProfile profile = new UserProfile
        {
            DisplayName = displayName.text
        };


        var task = user.UpdateUserProfileAsync(profile);
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            infoTxt.text = "���� �߻�";
        }
        else
        {
            nickname = displayName.text;
            infoTxt.text = "�г��� ���� ����";
            FirstSceneUIManger.instance.ClassSet();
        }
    }

    public void SetClass(int num) // Button
    {
        AudioManager.instance.PlayButtonClip();
        playerClass = num;
        SceneManager.LoadScene(1);
    }
}
