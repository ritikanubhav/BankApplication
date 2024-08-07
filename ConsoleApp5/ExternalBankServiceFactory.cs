namespace ConsoleApp5
{
    public class ExternalBankServiceFactory
    {
        private static ExternalBankServiceFactory _instance;
        private static readonly object _lock = new object();
        private Dictionary<string, IExternalBankService> _serviceBankPool;

        private ExternalBankServiceFactory()
        {
            _serviceBankPool = new Dictionary<string, IExternalBankService>();
            LoadBankServices();
        }

        public static ExternalBankServiceFactory Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ExternalBankServiceFactory();
                    }
                    return _instance;
                }
            }
        }

        private void LoadBankServices()
        {
            var properties = new Dictionary<string, string>();

            // Load configuration file (serviceBanks.properties)
            var lines = File.ReadAllLines("serviceBanks.properties");
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    properties[parts[0].Trim()] = parts[1].Trim();
                }
            }

            foreach (var key in properties.Keys)
            {
                var className = properties[key];
                var type = Type.GetType(className);
                if (type != null)
                {
                    var service = (IExternalBankService)Activator.CreateInstance(type);
                    _serviceBankPool[key] = service;
                }
            }
        }

        public IExternalBankService GetService(string bankCode)
        {
            if (_serviceBankPool.TryGetValue(bankCode, out var service))
            {
                return service;
            }
            throw new ArgumentException("No service found for bank code: " + bankCode);
        }
    }
}
