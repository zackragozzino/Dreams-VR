using UnityEngine;
using System.Collections;

namespace Hybriona.Facebook
{
	public class HybFacebookConstants 
	{


		private static string m_apiURL= "http://www.hybriona.com/services/fbapi/FBProcess.php";
		public static string apiURL
		{
			get
			{
				return m_apiURL;
			}
		}

		private static string m_serverKey;
		public static string GetServerKey()
		{
			return m_serverKey;
		}

		public static void SetApiURL(string url,string serverKey)
		{
			m_apiURL = url;
			m_serverKey = serverKey;
		}

		//public const string APIVERISON = "2.5";
	}
}
