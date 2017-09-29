using UnityEngine;

public class AndroidInfo {
	public static string OsVersion() {
		var retval = "unknown";
		using (var buildVersion = new AndroidJavaClass("android.os.Build$VERSION")) 
		{
			retval = buildVersion.GetStatic<string>("RELEASE");
		}
		return retval;
	}
}
