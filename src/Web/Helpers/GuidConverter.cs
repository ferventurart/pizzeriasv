namespace Web.Helpers;

using System;

public static class GuidConverter
{
    public static string ConvertGuidToShortString(Guid guid)
    {
        // Convert GUID to byte array
        byte[] bytes = guid.ToByteArray();

        // Convert bytes to a Base64 string
        string base64String = Convert.ToBase64String(bytes);

        // Replace unsafe characters for URL and remove padding
        base64String = base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=').ToUpper();
        
        // Convert to uppercase and take first 6 characters
        string shortString = base64String.ToUpper().Substring(0, 6);
        
        return shortString;
    }
}
