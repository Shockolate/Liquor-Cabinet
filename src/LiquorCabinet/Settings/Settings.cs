namespace LiquorCabinet.Settings
{
    public class Settings
    {
        public DatabaseSettings Database { get; set; }

        public static Settings Instance { get; } = new Settings();
    }
}