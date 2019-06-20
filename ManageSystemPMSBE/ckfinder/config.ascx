<%@ Control Language="C#" EnableViewState="false" AutoEventWireup="false" Inherits="CKFinder.Settings.ConfigFile" %>
<%@ Import Namespace="CKFinder.Settings" %>
<%--<%@ Import Namespace="ProjectLibrary.Config" %>
<%@ Import Namespace="ProjectLibrary.Security" %>--%>
<script runat="server">

    /**
	 * This function must check the user session to be sure that he/she is
	 * authorized to upload and access files using CKFinder.
	 */
    public override bool CheckAuthentication()
    {
        // WARNING : DO NOT simply return "true". By doing so, you are allowing
        // "anyone" to upload and list the files in your server. You must implement
        // some kind of session validation here. Even something very simple as...
        //
        //return (Session["IsAuthorized"] != null && (bool)Session["IsAuthorized"] == true);
        //
        // ... where Session[ "IsAuthorized" ] is set to "true" as soon as the
        // user logs on your system.
        return true;
    }

    /**
	 * All configuration settings must be defined here.
	 */

    public override void SetConfig()
    {
        // Paste your license name and key here. If left blank, CKFinder will
        // be fully functional, in Demo Mode.
        LicenseName = "Anonymous";
        LicenseKey = "888888";

        //The Folder name:
        const string folder = "files";

        // The base URL used to reach files in CKFinder through the browser.
        BaseUrl = string.Format("{0}{1}/", "/", folder);

        // The phisical directory in the server where the file will end up. Ifs
        // blank, CKFinder attempts to resolve BaseUrl.
        BaseDir = string.Format("{0}{1}\\", AppDomain.CurrentDomain.BaseDirectory, folder);
        //BaseDir = Controler.ResolveUrl(BaseUrl);

        // Optional: enable extra plugins (remember to copy .dll files first).
        Plugins = new string[] {
			// "CKFinder.Plugins.FileEditor, CKFinder_FileEditor",
			 "CKFinder.Plugins.ImageResize, CKFinder_ImageResize",
			// "CKFinder.Plugins.Watermark, CKFinder_Watermark"
		};
        // Settings for extra plugins.
        CheckSizeAfterScaling = true;
        DisallowUnsafeCharacters = true;
        CheckDoubleExtension = true;
        ForceSingleExtension = true;
        HtmlExtensions = new string[] { "html", "htm", "xml", "js" };
        HideFolders = new string[] { ".*", "CVS" };
        HideFiles = new string[] { ".*" };
        SecureImageUploads = true;
        EnableCsrfProtection = true;
        RoleSessionVar = "CKFinder_UserRole";
        AccessControl acl = AccessControl.Add();
        acl.Role = "*";
        acl.ResourceType = "*";
        acl.Folder = "/";

        acl.FolderView = true;
        acl.FolderCreate = true;
        acl.FolderRename = true;
        acl.FolderDelete = true;

        acl.FileView = true;
        acl.FileUpload = true;
        acl.FileRename = true;
        acl.FileDelete = true;
        DefaultResourceTypes = "";
        ResourceType type;
        int hotelId = 1;
        type = ResourceType.Add("Files");
        type.Url = BaseUrl + "files_" + hotelId + "/";
        type.Dir = BaseDir == "" ? "" : BaseDir + "files_" + hotelId + "/";
        type.MaxSize = 0;
        type.AllowedExtensions = new string[] { "bmp", "gif", "jpeg", "jpg", "png","zip","rar","docx","pdf","xlsx","txt","sql" };
        type.DeniedExtensions = new string[] { };
    }

</script>
