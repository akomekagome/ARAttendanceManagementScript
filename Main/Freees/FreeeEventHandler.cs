using System;
using System.Collections.Generic;
using System.Linq;
using ARAM.Main.Managers;
using ARAM.Utils;
using Freee;
using UniRx;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Freees
{
    public class FreeeEventHandler : MonoBehaviour
    {
        [Inject] private FreeeAuthData _freeeAuthData;
        [Inject] private PlatformDependentData _platformDependentData;
        private Subject<Unit>　_initSubject = new Subject<Unit>();
        public IObservable<Unit> InitObservable => _initSubject.AsObservable();
        private Client client;
        private int company_id;
        private Subject<FreeeType> _postTimeClockSubject = new Subject<FreeeType>();
        public IObservable<FreeeType> PostTimeClockObservable => _postTimeClockSubject.AsObservable();

        private void Awake()
        {
            GenerateAccessToken();
        }

        // 認証
        private void GenerateAccessToken()
        {
            var refresh_token = PlayerPrefs.GetString("refresh_token", "");
            client = new Freee.Client(_freeeAuthData.clientId, _freeeAuthData.clientSecret, _freeeAuthData.redirectUrl);
            if (string.IsNullOrEmpty(refresh_token))
                client.authorizationCode = _freeeAuthData.authorizationCode;
            else
                client.refreshToken = refresh_token;
            // client.authorizationCode = _freeeAuthData.authorizationCode;
            StartCoroutine(client.GenerateAccessToken(GenerateAccessTokenCallback));
        }

        private void GenerateAccessTokenCallback(bool success, string response)
        {
            if (!success) 
                return;
            GetCompanies(); // 認証が成功したら事業所IDを取りに行く
        }

        // 事業所ID取得
        private void GetCompanies()
        {
            StartCoroutine(client.Get("https://api.freee.co.jp/api/1/companies", callback: GetCompaniesCallback));
        }

        private void GetCompaniesCallback(bool success, string response)
        {
            if (!success) return;

            var companies = JsonUtility.FromJson<Companies>(response).companies;
            var company = companies.FirstOrDefault(c => c.display_name == _freeeAuthData.companyName);
            company_id = company.id;
            
            _initSubject.OnNext(Unit.Default);
            _initSubject.OnCompleted();
            // GetTimeClocksAvailableTypes();
            // GetLoginUser();
        }

        // void GetLoginUser()
        // {
        //     var employee_endpoint = "https://api.freee.co.jp/hr/api/v1/users/me";
        //     StartCoroutine(client.Get(employee_endpoint, callback: OnGetLoginUser));
        // }
        //
        // private void OnGetLoginUser(bool success, string response)
        // {
        //     Debug.Log(success);
        //     if (!success)
        //     {
        //         var msg = JsonUtility.FromJson<Message>(response).message;
        //         Debug.Log(msg);
        //         // _SystemMessageController.ShowMessageWithSad(msg);
        //         return;
        //     }
        //
        //     var companies = JsonUtility.FromJson<LoginUser>(response).companies;
        //     var company = companies.First(e => e.id == company_id);
        //     employee_id = company.employee_id;
        //
        //     GetTimeClocksAvailableTypes();
        // }

        private void GetTimeClocksAvailableTypes(int employeeId)
        {
            GetTimeClocksAvailableTypes(employeeId, OnGetTimeClocksAvailableTypes);
        }
        public void GetTimeClocksAvailableTypes(int employeeId, Client.Callback callback = default) 
        {
            var endpoint = "https://api.freee.co.jp/hr/api/v1/employees/" + employeeId +
                           "/time_clocks/available_types";
            var parameter = new Dictionary<string, string>
            {
                {"company_id", company_id.ToString()}
            };
            StartCoroutine(client.Get(endpoint, parameter, callback));
        }

        void OnGetTimeClocksAvailableTypes(bool success, string response)
        {
            Debug.Log(success);
            
            if (!success)
            {
                var msg = JsonUtility.FromJson<Message>(response).message;
                // _SystemMessageController.ShowMessageWithSad(msg);
                Debug.Log(msg);
                return;
            }

            var available_types = JsonUtility.FromJson<TimeClocksAvailableTypes>(response).available_types;

            // m_TimeClocksController.SetAvailableTypes(available_types);
            DebugExtensions.DebugShowList(available_types);

            // GetTimeClocks();
        }

        // private void GetTimeClocks()
        // {
        //     var endpoint = "https://api.freee.co.jp/hr/api/v1/employees/" + employee_id + "/time_clocks";
        //     var parameter = new Dictionary<string, string>
        //     {
        //         {"company_id", company_id.ToString()},
        //         {"from_date", DateTime.Now.ToString("yyyy-MM-dd")},
        //         {"to_date", DateTime.Now.ToString("yyyy-MM-dd")}
        //     };
        //     StartCoroutine(client.Get(endpoint, parameter, OnGetTimeClocks));
        // }
        //
        // private void OnGetTimeClocks(bool success, string response)
        // {
        //     Debug.Log(success);
        //
        //     if (!success)
        //     {
        //         var msg = JsonUtility.FromJson<Message>(response).message;
        //         // _SystemMessageController.ShowMessageWithSad(msg);
        //         Debug.Log(msg);
        //         return;
        //     }
        //     Debug.Log("hoge");
        //     var timeClocks = JsonUtility.FromJson<TimeClocksResponse>(response).items;
        //     // m_TimeClocksController.SetTimeClocksInfo(timeClocks);
        //     Debug.Log("fuga");
        //     DebugExtensions.DebugShowList(timeClocks.Select(x => x.type));
        //     
        //     _initSubject.OnNext(Unit.Default);
        //     _initSubject.OnCompleted();
        // }
        
        /*public void PostTimeClocks(string type)
        {
            PostTimeClocks(type, employee_id);
        }*/

        public void PostTimeClocks(string type, int employeeId)
        {
            var endpoint = "https://api.freee.co.jp/hr/api/v1/employees/" + employeeId + "/time_clocks";
            var p = TimeClockRequestJson(type, DateTime.Now);
            StartCoroutine(client.Post(endpoint, p, OnPostTimeClocks));
        }

        void OnPostTimeClocks(bool success, string response)
        {
            Debug.Log(success);
            if (!success)
            {
                var msg = JsonUtility.FromJson<Message>(response).message;
                // _SystemMessageController.ShowMessageWithSad(msg);
                return;
            }

            var tc = JsonUtility.FromJson<PostTimeClocksResponse>(response).employee_time_clock;
            Debug.Log(tc.type);
            switch (tc.type)
            {
                case "clock_in":
                    // _SystemMessageController.ShowMessageWithSmile("出勤しました\n今日も一日頑張ろう！");
                    break;
                case "break_begin":
                    // _SystemMessageController.ShowMessageWithSmile("休憩を開始したよ\nゆっくり休んでね");
                    break;
                case "break_end":
                    // _SystemMessageController.ShowMessageWithSmile("休憩は終わりだよ\nもう一息がんばろう！");
                    break;
                case "clock_out":
                    // _SystemMessageController.ShowMessageWithSmile("退勤しました\nおつかれさま！", true);
                    break;
            }
            _postTimeClockSubject.OnNext((FreeeType)Enum.Parse(typeof(FreeeType), tc.type));

            // GetTimeClocksAvailableTypes();
        }

        private string TimeClockRequestJson(string type, DateTime dt)
        {
            var r = new TimeClockRequest();
            r.company_id = company_id;
            r.type = type;
            r.base_date = dt.ToString("yyyy-MM-dd");
            return JsonUtility.ToJson(r);
        }
    }
}