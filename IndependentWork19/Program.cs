using System;

namespace IndependentWork19
{
    public interface IDataAccess
    {
        void GetData(string query);
    }

    public class SqlDataAccess : IDataAccess
    {
        public void GetData(string query)
        {
            Console.WriteLine($"[SQL] Виконання запиту: {query}");
        }
    }

    public class NoSqlDataAccess : IDataAccess
    {
        public void GetData(string query)
        {
            Console.WriteLine($"[NoSQL] Виконання запиту: {query}");
        }
    }

    public class XmlDataAccess : IDataAccess
    {
        public void GetData(string query)
        {
            Console.WriteLine($"[XML] Виконання запиту: {query}");
        }
    }

    public abstract class DataAccessFactory
    {
        protected abstract IDataAccess CreateDataAccess();

        public void ExecuteQuery(string query)
        {
            var dataAccess = CreateDataAccess();
            dataAccess.GetData(query);
        }
    }

    public class SqlDataAccessFactory : DataAccessFactory
    {
        protected override IDataAccess CreateDataAccess()
        {
            return new SqlDataAccess();
        }
    }

    public class NoSqlDataAccessFactory : DataAccessFactory
    {
        protected override IDataAccess CreateDataAccess()
        {
            return new NoSqlDataAccess();
        }
    }

    public class XmlDataAccessFactory : DataAccessFactory
    {
        protected override IDataAccess CreateDataAccess()
        {
            return new XmlDataAccess();
        }
    }

    public class DataManager
    {
        private static DataManager _instance;
        private static readonly object _lock = new object();

        private DataAccessFactory _factory;

        private DataManager() { }

        public static DataManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new DataManager();
                }
                return _instance;
            }
        }

        public void SetFactory(DataAccessFactory factory)
        {
            _factory = factory;
        }

        public void GetData(string query)
        {
            if (_factory == null)
            {
                Console.WriteLine("Фабрика не встановлена!");
                return;
            }

            _factory.ExecuteQuery(query);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var manager = DataManager.Instance;

            manager.SetFactory(new SqlDataAccessFactory());
            manager.GetData("SELECT * FROM Users");
            manager.GetData("SELECT * FROM Orders");

            Console.WriteLine();

            manager.SetFactory(new NoSqlDataAccessFactory());
            manager.GetData("{ users: true }");
            manager.GetData("{ orders: true }");

            Console.WriteLine();

            manager.SetFactory(new XmlDataAccessFactory());
            manager.GetData("<users></users>");
            manager.GetData("<orders></orders>");
        }
    }
}
