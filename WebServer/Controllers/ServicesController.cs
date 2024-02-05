using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using WebServer.Models;
using WpfApp;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : Controller
    {

        ObservableCollection<Service> servicesCollection;
        public ServicesController()
        {
            servicesCollection = new ObservableCollection<Service>();
            GetData();
        }

        [HttpGet]
        public IEnumerable<Service> Get()
        {
            return servicesCollection;
        }

        private void GetData()
        {
            using (SqliteConnection connection = dbConnector.OpenConnection())
            {
                SqliteCommand command = new SqliteCommand("SELECT * FROM Services", connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Service service = new Service();
                        service.ServiceCode = reader.GetString(0) ?? "";
                        service.ServiceName = reader.GetString(1) ?? "";
                        service.Price = reader.GetString(2) ?? "";
                        service.ResultType = reader.GetString(3) ?? "";
                        service.AvailableAnalyzers = reader.GetString(4) ?? "";
                        servicesCollection.Add(service);
                    }
                }
            }
        }
    }
}
