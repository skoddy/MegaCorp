namespace MegaCorp
{
    interface Database
    {
        void Connect();
        void Disconnect();
        void Create<T>(string table, T obj);
        void Read();
        void Update<T>(string table, T obj);
        void Delete();
    }
}
