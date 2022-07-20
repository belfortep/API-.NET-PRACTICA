namespace Play.Catalog.Service.Settings
{

    public class MongoDbSettings
    {
        public string Host {get; init;}
        //init para no se puedan modificar despues de iniciar el servidor
        public int Port {get; init;}

        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }

}