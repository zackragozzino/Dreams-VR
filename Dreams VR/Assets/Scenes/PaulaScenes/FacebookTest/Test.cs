using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Hybriona.Facebook;
using System.Collections.Generic;

public class Friends
{
	public string Name;
	public void LoadImage(string link)
	{
		HybFacebook.Instance.LoadImage(link,delegate(Texture2D texture) {
			img = texture;

		});
	}
	public Texture2D img;
}
public class Test : MonoBehaviour {


	public Text text;


	public void Login()
	{
		if(!HybFacebook.Instance.IsLoggedIn())
		{
			HybFacebook.Instance.Login ();
			text.text = "Logging Process..";
		}
	}

	public Renderer profilePicRenderer;

	void Start () {

		HybFacebookConstants.SetApiURL("http://localhost:8000/fbapi/FBProcess.php","rjproz_secret_temp");
		HybFacebook.Instance.Init ("563027720744270","7e81ce67f67b22c7aebbe04be5f6d66c","email");


		HybFacebook.Instance.onLoginFailedOrCancelled += delegate(string reason) {
			
			text.text = "Login Failed!\n"+reason;
		};

		HybFacebook.Instance.onValidatingUser += delegate() {
			text.text = "On Validating User..";
		};



		HybFacebook.Instance.onLoggedInSuccessfully += delegate() {

			text.text = "Login Successful!";
			GetSelfName();
			GetPicture();
			//ShareVideo();
			//StartCoroutine(ShareURL());
			//StartCoroutine(LoadUploadedPhotos());
			//StartCoroutine( UploadPhoto() );
			//GetTaggableFriends();

//			string extraurl = "attached_media%5B0%5D=%7B%22media_fbid%22%3A%1358855657515083%22%7D";//attached_media={\"media_fbid\":\"1358855657515083\"}";
//			HybFacebook.Instance.ShareDialog("http://hybriona.com","Hello caption",HybFacebook.DialogType.popup,extraurl, delegate(string response) {
//			
//				Debug.Log(response);
//			});


			//PostFBFanPageSchedule();
		};
	}


	#region Friends UI
	string log = "g";

	Vector2 scroll;
	Friends [] friends = null;
	void OnGUI()
	{
		GUILayout.Label (log);
		scroll = GUILayout.BeginScrollView (scroll);
		if(friends != null)
		{
			for(int i=0;i<friends.Length;i++)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Box(friends[i].img);
				GUILayout.Label(friends[i].Name);
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndScrollView ();
	}
	#endregion

	#region Get Taggable Friends
	void GetTaggableFriends()
	{
		
		HybFacebook.Instance.API ("me/taggable_friends?limit=1000", HybFacebook.HTTPMethod.GET, delegate(FacebookResponse response) {


			if(response.error == null)
			{
				Dictionary<string,object> res = (Dictionary<string,object> ) Hybriona.MiniJSON.Json.Deserialize( response.text );
				List<object> array = (List<object>) res["data"];
				friends = new Friends[(array.Count>20 ? 20 : array.Count)];
				for(int i=0;i<friends.Length;i++)
				{
					Dictionary<string,object> element = (Dictionary<string,object> )array[i];
					//Debug.Log(element["name"]);
					Dictionary<string,object> picelement =(Dictionary<string,object>)   ((Dictionary<string,object>) element["picture"])["data"] ;

					friends[i] = new Friends();
					friends[i].Name = element["name"].ToString();;
					friends[i].LoadImage(picelement["url"].ToString());

				}
			}
			else
			{
				Debug.Log(response.error);
			}
		}, null);
	}

	#endregion

	#region Get Player's Info
	void GetSelfName()
	{



		HybFacebook.Instance.API ("me?fields=name,email", HybFacebook.HTTPMethod.GET, delegate(FacebookResponse response) {
			
			if(response.error == null)
			{
				Debug.Log(response.text);
				Dictionary<string,object> res = (Dictionary<string,object> ) Hybriona.MiniJSON.Json.Deserialize( response.text );
				text.text = res["name"].ToString() +"\n"+res["email"].ToString() +"\n";


			}
			else
			{
				Debug.Log(response.error);
				text.text = response.error;
			}
		}, null);

		//GetTaggableFriends();


	}
	#endregion


	#region Screenshot and Upload Image
	public Texture2D tex;
	IEnumerator UploadPhoto()
	{
		yield return new WaitForSeconds(10);
		yield return new WaitForEndOfFrame();
		tex = new Texture2D(Screen.width,Screen.height,TextureFormat.RGB24,false);
		tex.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
		tex.Apply();

		WWWForm form = new WWWForm();
		form.AddBinaryData("photo",tex.EncodeToJPG(50),"screenshot.jpg");
		form.AddField("message","FB SDK Unity Standalone at "+System.DateTime.Now);
		HybFacebook.Instance.API ("me/photos",HybFacebook.HTTPMethod.POST,delegate(FacebookResponse response) {
			Debug.Log(response.text);
		},form);

		//Destroy(tex);

	}

	#endregion


	#region Get Player's uploaded pictures and name
	IEnumerator LoadUploadedPhotos()
	{
		yield return new WaitForSeconds(5);
		HybFacebook.Instance.API("me/photos/uploaded?fields=name,picture",HybFacebook.HTTPMethod.GET,delegate(FacebookResponse response) {
			Debug.Log(response.text);
		});

	}
	#endregion

	#region Share URL as FB status
	IEnumerator ShareURL()
	{
		yield return new WaitForSeconds(5);

		HybFacebook.Instance.ShareDialog("http://hybriona.com","Hello from hybriona",delegate(string response) {
			Debug.Log(response);
		});

	}
	#endregion

	#region Get Player's Profile Image
	void GetPicture()
	{

		HybFacebook.Instance.API ("me/picture?type=large", HybFacebook.HTTPMethod.GET, delegate(FacebookResponse response) {
			
			if(response.error == null)
			{

				Texture2D img = new Texture2D(1,1,TextureFormat.ARGB32,false);
				img.LoadImage(response.bytes);
				profilePicRenderer.material.mainTexture = img;
				Vector3 localScale = profilePicRenderer.transform.localScale;
				localScale.x = localScale.y * (float)img.width / (float) img.height;
				profilePicRenderer.transform.localScale = localScale;

			}
			else
			{
				Debug.Log(response.error);
				text.text = response.error;
			}
		}, null);
	}
	#endregion

	#region Share Video URL
	void ShareVideo()
	{
		HybFacebook.Instance.ShareDialog("https://www.youtube.com/watch?v=xAFfYLR_IRY","Test Video Shared",delegate(string response) {

			Debug.Log(response);
		});
	}
	#endregion

	#region FBFanPage
	void PostFBFanPageSchedule()
	{
		string pageID = "1575434306036383";



		HybFacebook.Instance.API (pageID+"?fields=access_token", HybFacebook.HTTPMethod.GET, delegate(FacebookResponse response) {
			if(response.error != null)
			{
				Debug.Log(response.error);
			}
			else
			{
				Debug.Log(response.text);
				string page_token = ((Dictionary<string,object>) Hybriona.MiniJSON.Json.Deserialize(response.text))[HybFacebook.Key_AccessToken].ToString();

				System.DateTime date = System.DateTime.Now; 
				date = date.AddHours(1); //Adding one hours
				date = date.AddHours(-5.5); // To GMT time. GMT + 5hr 30mins hr in my case
				WWWForm formData = new WWWForm();
				formData.AddField("message","But man is not made for defeat. A man can be destroyed but not defeated.\n\n-Ernest Hemingway");
				formData.AddField("published","false");
				formData.AddField("scheduled_publish_time",date.UnixTimeNow().ToString());
				formData.AddField("access_token",page_token);

				HybFacebook.Instance.API(pageID+"/feed",HybFacebook.HTTPMethod.POST,delegate(FacebookResponse response2) {

					Debug.Log(response2.error);
					Debug.Log(response2.text);
				},formData);
			}

		}, null);




	}
	#endregion

}
