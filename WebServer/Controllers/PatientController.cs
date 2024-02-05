using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using WebServer.Models;
using WpfApp;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : Controller
    {
        ObservableCollection<Patient> patientsCollection;

        public PatientController()
        {
            patientsCollection = new ObservableCollection<Patient>();

            GetData();
        }

        [HttpGet]
        public IEnumerable<Patient> Get()
        {
            return patientsCollection;
        }

        [HttpGet("{login}")]
        public IActionResult GetLogin(string login)
        {
            var patient = patientsCollection.FirstOrDefault(x => x.Login == login);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public IActionResult Post(Patient patient)
        {
            using (SqliteConnection connection = dbConnector.OpenConnection())
            {
                SqliteCommand cmd = new SqliteCommand("INSERT INTO Patients (Login, Password, FullName, Phone, Email, InsurancePolicyNumber) VALUES (@login, @password, @fullName, @phone, @email, @insurancePolicyNumber)", connection);
                cmd.Parameters.AddWithValue("login", patient.Login);
                cmd.Parameters.AddWithValue("password", patient.Password);
                cmd.Parameters.AddWithValue("fullName", patient.FullName);
                cmd.Parameters.AddWithValue("phone", patient.Phone);
                cmd.Parameters.AddWithValue("email", patient.Email);
                cmd.Parameters.AddWithValue("insurancePolicyNumber", patient.InsurancePolicyNumber);

                cmd.ExecuteNonQuery();
            }

            return Ok("Новый пользователь зарегистрирован.");
        }

        [HttpPut]
        [Route("edit")]
        public IActionResult Edit(Patient patient)
        {
            using (SqliteConnection connection = dbConnector.OpenConnection())
            {
                SqliteCommand cmd = new SqliteCommand("UPDATE Patients SET Phone=@phone, Email=@email, Password=@password WHERE PatientID=@id", connection);
                cmd.Parameters.AddWithValue("id", patient.PatientId);
                cmd.Parameters.AddWithValue("phone", patient.Phone);
                cmd.Parameters.AddWithValue("email", patient.Email);
                cmd.Parameters.AddWithValue("password", patient.Password);

                cmd.ExecuteNonQuery();
                return Ok();
            }
        }

        private void GetData()
        {
            using (SqliteConnection connection = dbConnector.OpenConnection())
            {
                SqliteCommand command = new SqliteCommand("SELECT * FROM Patients", connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Patient patient = new Patient();
                        patient.PatientId = reader.GetInt32(0);
                        patient.Login = reader.GetString(1) ?? "";
                        patient.Password = reader.GetString(2) ?? "";
                        patient.FullName = reader.GetString(3) ?? "";
                        patient.Phone = reader.GetString(4) ?? "";
                        patient.Email = reader.GetString(5) ?? "";
                        patient.InsurancePolicyNumber = reader.GetString(6) ?? "";
                        patientsCollection.Add(patient);
                    }
                }
            }
        }
    }
}
