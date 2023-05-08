using System.Collections;

namespace TestTaskVK.Helpers
{
    public static class UserHelper
    {
        static List<string> logins = new List<string>();
        public static void Add(string login)
        {
            logins.Add(login);
        }
        public static bool Check(string login)
        {
            return logins.Where(x => x.Equals(login)).Count() > 1;
        }
        public static void Clear()
        {
            logins.Clear();
        }
    }
}
