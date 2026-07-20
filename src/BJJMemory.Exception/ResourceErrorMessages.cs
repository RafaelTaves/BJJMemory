using System.Globalization;
using System.Resources;

namespace BJJMemory.Exception;

public static class ResourceErrorMessages
{
    private static readonly ResourceManager ResourceManager = new(
        "BJJMemory.Exception.ExceptionsBase.ResourceErrorMessages",
        typeof(ResourceErrorMessages).Assembly);

    private static readonly CultureInfo Culture = CultureInfo.GetCultureInfo("pt-BR");

    public static string EMAIL_ALREADY_EXISTS => GetString(nameof(EMAIL_ALREADY_EXISTS));
    public static string EMAIL_INVALID => GetString(nameof(EMAIL_INVALID));
    public static string EMAIL_REQUIRED => GetString(nameof(EMAIL_REQUIRED));
    public static string INVALID_LOGIN => GetString(nameof(INVALID_LOGIN));
    public static string PASSWORD_LOWERCASE_LETTER => GetString(nameof(PASSWORD_LOWERCASE_LETTER));
    public static string PASSWORD_MIN_LENGTH => GetString(nameof(PASSWORD_MIN_LENGTH));
    public static string PASSWORD_NUMBER => GetString(nameof(PASSWORD_NUMBER));
    public static string PASSWORD_REQUIRED => GetString(nameof(PASSWORD_REQUIRED));
    public static string PASSWORD_SPECIAL_CHARACTER => GetString(nameof(PASSWORD_SPECIAL_CHARACTER));
    public static string PASSWORD_UPPERCASE_LETTER => GetString(nameof(PASSWORD_UPPERCASE_LETTER));
    public static string TITLE_REQUIRED => GetString(nameof(TITLE_REQUIRED));
    public static string UNKNOWN_ERROR => GetString(nameof(UNKNOWN_ERROR));
    public static string USER_NOT_FOUND => GetString(nameof(USER_NOT_FOUND));
    public static string USER_NAME_LENGTH => GetString(nameof(USER_NAME_LENGTH));
    public static string USER_NAME_REQUIRED => GetString(nameof(USER_NAME_REQUIRED));

    private static string GetString(string name)
    {
        return ResourceManager.GetString(name, Culture) ?? name;
    }
}
