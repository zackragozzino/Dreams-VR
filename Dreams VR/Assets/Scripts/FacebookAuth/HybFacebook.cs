using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.IO;



using System.Collections.Generic;
using System.Text;
using System;
using System.Threading;

namespace Hybriona.Facebook
{

	public static class HybFacebookExtensions
	{
		public static int UnixTimeNow(this System.DateTime date)
		{
			var timeSpan = (date - new DateTime(1970, 1, 1, 0, 0, 0));
			return (int)timeSpan.TotalSeconds;
		}
	}



	public class HybFacebook : MonoBehaviour 
	{
		
		public const string Key_AccessToken = "access_token";
		private Queue<Action> ExecuteOnMainThread = new Queue<Action>();
		IEnumerator ExecuteFromQueue()
		{
			while(true)
			{
				yield return new WaitForSeconds(1);
				while(ExecuteOnMainThread.Count > 0)
				{
					Action a = ExecuteOnMainThread.Dequeue();
		
					a();
				}
			}
		}

		private string appID;
		private string appSecret;
		private string scope;

		public float LoginProcessTimeOut = 20;
		public string GetAppID()
		{
			return appID;
		}

		private static HybFacebook instance;
		public static HybFacebook Instance
		{
			get
			{
				if(instance == null)
				{
					GameObject o = new GameObject("Hybriona.Facebook.HybFacebook");
					instance = o.AddComponent<HybFacebook>();
					DontDestroyOnLoad(o);
				}
				return instance;
			}
		}


		IEnumerator GetToken(string code)
		{
			string url = "https://graph.facebook.com/oauth/access_token?"
				+ "client_id=" + appID + "&redirect_uri="+requestWebURL+"&client_secret="+appSecret+"&code=" + code;
		
			WWW www = new WWW (url);
			yield return www;

			if(www.error != null)
			{
				if(onLoginFailedOrCancelled != null)
				{
					onLoginFailedOrCancelled(www.error);
				}
			}
			else
			{
				
				OnReceivedToken(www.text);

			}
			www.Dispose ();

		}

		private string mLoggedinToken;
		public void OnReceivedToken(string token)//,bool isfullytrue)
		{
			
			try
			{
				
				string parsed = token.Split ('\n') [0];
				//Debug.Log(parsed);

				#if OLD
				parsed = parsed.Replace ("access_token=", "");
				parsed = parsed.Split('&')[0];
				logonToken = parsed;
				#else
				mLoggedinToken = ((Dictionary<string,object>) MiniJSON.Json.Deserialize(parsed))["access_token"].ToString();
				#endif
				isLoggedIn = true;

				Action a = () => 
				{
					StopCoroutine (CheckForTimeOut ());
					if(onLoggedInSuccessfully != null)
					{
						onLoggedInSuccessfully();
					}
					
				};
				ExecuteOnMainThread.Enqueue (a);


			}
			catch(System.Exception exception)
			{

				try
				{
					Dictionary<string,object> res = (Dictionary<string,object> ) Hybriona.MiniJSON.Json.Deserialize( token );
					res = (Dictionary<string,object> )res["error"];
					if(res["message"].ToString().Contains("expired") || res["message"].ToString().Contains("invalid"))
					{
						GetToken();
					}
					System.GC.Collect();
					return;
				}
				catch(System.Exception e)
				{
					Debug.Log(e.Message+","+e.StackTrace);
				}

				Action a = () => 
				{
					StopCoroutine (CheckForTimeOut ());


					if(onLoginFailedOrCancelled != null)
					{
						onLoginFailedOrCancelled(exception.Message+","+exception.StackTrace);
					}

				};
				ExecuteOnMainThread.Enqueue (a);
			}
		}

		private string codeFetched;
		public void GetToken()
		{
			Action a = () => {
				StartCoroutine (GetToken(codeFetched));
			};
			ExecuteOnMainThread.Enqueue (a);
		}
		public void OnReceivedCode(string token)
		{
			codeFetched = token;
			GetToken();
		}


		public void Init(string _appID,string _appSecret,string _scope)
		{

			appID = _appID;
			appSecret = _appSecret;
			scope = _scope;
			StartCoroutine(ExecuteFromQueue());
			//SimpleServer server = new SimpleServer ();

		}



		IEnumerator FetchCode()
		{

			yield return new WaitForSeconds (.1f);
			if (onValidatingUser != null) {
				onValidatingUser();
			}


			string url = HybFacebookConstants.apiURL + "?process=login&gettoken=" + SystemInfo.deviceUniqueIdentifier+"&serverKey="+HybFacebookConstants.GetServerKey();
			WWW www = new WWW (url);
			yield return www;
			if(www.error != null)
			{
				StopCoroutine (CheckForTimeOut ());

				if(onLoginFailedOrCancelled != null)
				{
					onLoginFailedOrCancelled(www.error);
				}

			}
			else
			{
				
				try
				{
					Dictionary<string,object> res = (Dictionary<string,object> ) Hybriona.MiniJSON.Json.Deserialize(  www.text );

					if(res["iserror"].ToString().ToLower() == "false")
					{
						OnReceivedCode(res["code"].ToString());
					}
					else
					{
						if(onLoginFailedOrCancelled != null)
						{
							onLoginFailedOrCancelled(res["error_description"].ToString());
						}
					}
					System.GC.Collect();

				}
				catch(System.Exception e)
				{
					
					StopCoroutine (CheckForTimeOut ());
					if(onLoginFailedOrCancelled != null)
					{
						onLoginFailedOrCancelled(e.Message +","+e.StackTrace);
					}


				}
			}
		}

		private bool isLoggedIn;
		public bool IsLoggedIn()
		{
			return isLoggedIn;
		}



		private bool isLoginProcessing;
		private string randomStringSession;
		private string requestWebURL = "";
		public void Login()
		{
			if(isLoggedIn)
			{
				return;
			}

			randomStringSession = UnityEngine.Random.Range(100000,999999)+"-"+UnityEngine.Random.Range(100000,999999);
			requestWebURL =  HybFacebookConstants.apiURL + "?process=login";
			isLoginProcessing = true;
			string _redirectEndpoint = requestWebURL;
			string token = SystemInfo.deviceUniqueIdentifier;
		
			StartCoroutine (CheckForTimeOut ());

			string url = System.String.Format ("https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&state={2}&scope={3}", appID, _redirectEndpoint, token, scope);
			OpenURL(url);
			StartCoroutine (FetchCode ());
		}

		IEnumerator CheckForTimeOut()
		{
			yield return new WaitForSeconds (LoginProcessTimeOut);
			isLoginProcessing = false;
			if(onLoginFailedTimeOut != null)
			{
				onLoginFailedTimeOut ();
			}
		}

		#region External URL Handling
		public void OpenURL(string url)
		{
			#if (UNITY_WEBPLAYER || UNITY_WEBGL) && !UNITY_EDITOR
			Application.ExternalEval("window.open('"+url+"','_blank')");
			#else
			Application.OpenURL(url);	
			#endif
		}
		#endregion

		#region Callbacks
		public delegate void OnHybURLCallback(string response);

		IEnumerator GetURLCallback(string url,OnHybURLCallback onHybURLCallback)
		{
			WWW www = new WWW(url);
			yield return www;

			if(www.error != null)
			{
				string data = "{\"iserror\":true,\"error_code\":0,\"error_message\":\""+www.error+"\"}";
				onHybURLCallback(data);
			}
			else
			{
				onHybURLCallback(www.text);
			}
		}
		#endregion

		#region Events & Delegates
		public delegate void OnLoggedInSuccessfully();
		public delegate void OnLoginFailedOrCancelled(string reason = null);
		public delegate void OnLoginFailedTimeOut();
		public delegate void OnValidatingUser ();
		public delegate void OnValidatingUserFailed ();

		public OnLoggedInSuccessfully onLoggedInSuccessfully;
		public OnLoginFailedOrCancelled onLoginFailedOrCancelled;
		public OnLoginFailedTimeOut onLoginFailedTimeOut;
		public OnValidatingUser onValidatingUser;
		public OnValidatingUserFailed onValidatingUserFailed;

		public delegate void OnFacebookResponseReceived(FacebookResponse response);

		public delegate void OnHybFBProcessCallback(string response);
		#endregion

		#region API
		public enum HTTPMethod
		{
			POST,
			GET
		};

		private string apiUrlOffset = "https://graph.facebook.com/";//v"+HybFacebookConstants.APIVERISON+"/";
		public void API(string query,HTTPMethod method,OnFacebookResponseReceived onFacebookResponseReceived,WWWForm data = null)
		{
			if(query[0] =='/')
			{
				query = query.Substring(1);
			}
			string url = apiUrlOffset + query;
			//if(method == HTTPMethod.GET)
			{
				if(url.Contains("?"))
				{
					url += "&access_token="+mLoggedinToken;
				}
				else
				{
					url += "?access_token="+mLoggedinToken;
				}

			}


			StartCoroutine (API_I (url, method, onFacebookResponseReceived, data));
		}

		IEnumerator API_I(string query,HTTPMethod method,OnFacebookResponseReceived onFacebookResponseReceived ,WWWForm data = null)
		{
			WWW www = null;
			if(method == HTTPMethod.GET)
			{
				www = new WWW(query);
			}
			else
			{
				www = new WWW(query,data);
			}

			yield return www;

			string errorDetail = null;
			Dictionary<string,string> resheaders = www.responseHeaders;
			foreach(KeyValuePair<string,string> e in resheaders)
			{
				if(e.Key == "WWW-AUTHENTICATE")
				{
					errorDetail = e.Value;
				}
			}


			if(onFacebookResponseReceived != null)
			{
				
				FacebookResponse response = new FacebookResponse();
				if(errorDetail == null)
				{
					response.error = www.error;
				}
				else
				{
					response.error = errorDetail;
				}
			
				if(response.error == null)
				{
					
					response.text = www.text;
					response.bytes = www.bytes;
					response.texture = www.texture;
					response.textureNotReadable = www.textureNonReadable;
				}
				onFacebookResponseReceived(response);
			}

		}
		#endregion
		#region Dialogues
		public enum DialogType {page,popup}
		public void ShareDialog(string url,string quoteText = null,OnHybFBProcessCallback callback = null)
		{
			ShareDialog(url,quoteText,DialogType.page,null,callback);
		}
		public void ShareDialog(string url,string quoteText = null,DialogType type = DialogType.page,string extraURL = null,OnHybFBProcessCallback callback = null)
		{
			if(extraURL != null)
			{
				extraURL = "&"+WWW.EscapeURL(extraURL);
			}
			string completeURL = "https://www.facebook.com/dialog/share?app_id="+appID+"&display="+type.ToString()+extraURL+"&href="+url+"&quote="+WWW.EscapeURL(quoteText)+"&redirect_uri="+HybFacebookConstants.apiURL + "?process=share$"+SystemInfo.deviceUniqueIdentifier;
			Debug.Log(completeURL);
			OpenURL(completeURL);
			if(callback != null)
			{
				StartCoroutine(GetURLCallback(HybFacebookConstants.apiURL+"?getstatus&process=share$"+SystemInfo.deviceUniqueIdentifier,delegate(string response) {
					callback(response);
				}));
			}
		}
		#endregion
		#region Misc


		public delegate void OnImageLoaded(Texture2D texture);
		public void LoadImage(string url,OnImageLoaded onImageLoaded)
		{
			StartCoroutine (LoadImage_I (url,onImageLoaded));
		}
		IEnumerator LoadImage_I(string url,OnImageLoaded onImageLoaded)
		{
			WWW www = new WWW (url);
			yield return www;

			if(www.error == null)
			{
				Texture2D texture = new Texture2D(1,1,TextureFormat.ARGB32,false);
				www.LoadImageIntoTexture(texture);
				onImageLoaded(texture);
			}
			else
			{
				onImageLoaded(null);
			}
			www.Dispose ();
		}


		#endregion
	}


	public struct FacebookResponse
	{
		public string error;
		public string text;
		public byte [] bytes;
		public Texture2D texture;
		public Texture2D textureNotReadable;
	
	}


}
