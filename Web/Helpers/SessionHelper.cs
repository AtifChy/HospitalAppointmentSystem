using BLL.DTOs;

namespace Web.Helpers;

public static class SessionHelper
{
    public static void SetUserSession(ISession session, UserDto user)
    {
        session.SetInt32("UserId", user.Id);
        session.SetString("UserName", user.Name);
        session.SetString("UserEmail", user.Email);
        session.SetString("UserRole", user.Role);
    }

    public static void ClearSession(ISession session)
    {
        session.Clear();
    }

    public static int? GetUserId(ISession session)
    {
        return session.GetInt32("UserId");
    }

    public static string? GetUserName(ISession session)
    {
        return session.GetString("UserName");
    }

    public static string? GetUserEmail(ISession session)
    {
        return session.GetString("UserEmail");
    }

    public static string? GetUserRole(ISession session)
    {
        return session.GetString("UserRole");
    }

    public static bool IsLoggedIn(ISession session)
    {
        return session.GetInt32("UserId") != null;
    }

    public static bool IsAdmin(ISession session)
    {
        return session.GetString("UserRole") == "Admin";
    }

    public static bool IsDoctor(ISession session)
    {
        return session.GetString("UserRole") == "Doctor";
    }

    public static bool IsPatient(ISession session)
    {
        return session.GetString("UserRole") == "Patient";
    }
}