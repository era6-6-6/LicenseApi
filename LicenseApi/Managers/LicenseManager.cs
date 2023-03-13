namespace LicenseApi.Managers
{
    public static class LicenseManager
    {
        private static readonly Dictionary<int, bool> Licenses = new Dictionary<int, bool>()
        {
            
        };

        private static readonly object Locker = new();

        public static void Add(int id, bool valid)
        {
            lock (Locker)
            {
                
                Licenses.Add(id , valid);
            }
        }

        public static void Remove(int id)
        {
            lock (Locker)
            {
                Licenses.Remove(id);
            }
        }

        public static bool ValidateLicense(int id)
        {
            lock (Locker)
            {
                if (Licenses.ContainsKey(id))
                {
                    return Licenses[id];
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
