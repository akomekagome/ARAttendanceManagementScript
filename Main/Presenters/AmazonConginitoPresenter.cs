using ARAM.Main.AWS;
using ARAM.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;

namespace ARAM.Main.Presenters
{
    
    public class AmazonConginitoPresenter : MonoBehaviour
    {
        [Inject] private AmazonConginitoProvider amazonConginitoProvider;
        [SerializeField] private Button signInButton;
        [SerializeField] private Button signUpButton;
        [SerializeField] private Button submitAuthCodeButton;
        [SerializeField] private InputField emailInputField;
        [SerializeField] private InputField passwordInputField;
        [SerializeField] private InputField authCodeInputField;
        
        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            emailInputField.text = JsonUtilityUtils.LoadJsonData<string>(SaveDataFilePaths.UserNamePath);

            signInButton
                .OnClickAsObservable()
                .Subscribe(_ => amazonConginitoProvider.SignInAsync(emailInputField.text, passwordInputField.text, token).Forget())
                .AddTo(this);
            
            signUpButton
                .OnClickAsObservable()
                .Subscribe(_ => amazonConginitoProvider.SignUpAsync(emailInputField.text, passwordInputField.text, token).Forget())
                .AddTo(this);
            
            submitAuthCodeButton
                .OnClickAsObservable()
                .Subscribe(_ => amazonConginitoProvider.ConfirmationAsync(emailInputField.text, authCodeInputField.text, token).Forget())
                .AddTo(this);
        }
    }
}
