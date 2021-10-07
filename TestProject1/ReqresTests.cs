using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace TestProject1
{

    public class ReqresTests
    {
        private readonly HttpClient client = new HttpClient();
        public HttpResponseMessage WebRequest(string webAddress)
        {
            return client.GetAsync(webAddress).Result;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Responce_With_StatusCodeOk()
        {
            //Arrange
            HttpStatusCode responseMessage = HttpStatusCode.OK;
            string url = "https://reqres.in/api/users?page=2";

            //Act
            HttpStatusCode actual_responce = WebRequest(url).StatusCode;

            //Assert
            Assert.AreEqual(responseMessage, actual_responce);
        }

        [Test]
        public void Responce_With_StatusCodeNotFound()
        {
            //Arrange
            HttpStatusCode responseMessage = HttpStatusCode.NotFound;
            string url = "https://reqres.in/api/users/23";

            //Act
            HttpStatusCode actual_responce = WebRequest(url).StatusCode;

            //Assert
            Assert.AreEqual(responseMessage, actual_responce);
        }

        [Test]
        public void Check_fields_Is_NotEmpty_in_JSON_object()
        {
            //Arrange
            bool fieldsIsNotEmpty = true;
            string url = "https://reqres.in/api/users";

            //Act
            HttpResponseMessage actual_responce = WebRequest(url);
            bool actual_fields = false;

            string responseBody = actual_responce.Content.ReadAsStringAsync().Result;
            JObject jObject = JObject.Parse(responseBody);
            JToken list = jObject["data"];
            List<Datum> trades = list.ToObject<List<Datum>>();

            Datum datum = trades[0];

            if (datum.first_name != "" &&
                datum.last_name != "" &&
                datum.email != "" &&
                datum.avatar != "")
            {
                 actual_fields = true;
            }

            //Assert
            Assert.AreEqual(fieldsIsNotEmpty, actual_fields);
        }

        [Test]
        public void Check_Valid_Email_in_JSON_objects()
        {
            //Arrange
            bool fieldsIsNotEmpty = true;
            string url = "https://reqres.in/api/users";

            //Act
            HttpResponseMessage actual_responce = WebRequest(url);
            bool actual_validEmail = false;

            string responseBody = actual_responce.Content.ReadAsStringAsync().Result;
            JObject jObject = JObject.Parse(responseBody);
            JToken list = jObject["data"];
            List<Datum> trades = list.ToObject<List<Datum>>();

            foreach(var obj in trades)
            {
                if (IsValidEmail(obj.email))
                {
                    actual_validEmail = true;
                }
                else
                {
                    actual_validEmail = false;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(fieldsIsNotEmpty, actual_validEmail);
        }
    }
}