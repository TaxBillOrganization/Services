using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.IO;
using TaxBill.Models;
using System.Text;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace TaxBill.Controllers
{
 

    public class ValuesController : ApiController
    {   //	database-2.cluster-clriaz8or7de.eu-central-1.rds.amazonaws.com

        MySqlConnection mysqlConnection = new MySqlConnection ("SERVER=database-2.cluster-clriaz8or7de.eu-central-1.rds.amazonaws.com;" +
            "DATABASE=TaxBill;UID=admin;PASSWORD=fatih3458;");

        private Boolean checkUser(long tckn, string name, string surname, int birthDate)  
        {

            bool status = true;
            try
            {
                using (Kimlik.KPSPublicSoapClient servis = new Kimlik.KPSPublicSoapClient())
                {
                    status = servis.TCKimlikNoDogrula(tckn, name, surname, birthDate);
                }
            }
            catch
            {
                status = false;
            }
            return status;
        }


        [HttpGet]
        public HttpResponseMessage UserList()
    {
            string XML = "<Users>";
            string query = "select * from TaxBill.Users";

            //Create a list to store the result
            mysqlConnection.Open();
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, mysqlConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                XML += "<User><Id>"+ (dataReader["Id"])+"</Id>";
                XML += "<Name>"+ (dataReader["Name"]) + "</Name>";
                XML += "<Surname>"+ (dataReader["Surname"]) + "</Surname></User>";              
                }
                XML += "</Users>";
            //close Data Reader
            dataReader.Close();

            //close Connection
            mysqlConnection.Close();

            //return list to be displayed
            System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
            return new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = new StringContent(XML, Encoding.UTF8, "application/xml")
            };

        }




        [HttpGet]
        public HttpResponseMessage InsertUser(UserModel userInfo) {


            bool status = checkUser(userInfo.tckn, userInfo.name, userInfo.surname, int.Parse(userInfo.birthDay));
            if (status)
            {


                string query = "INSERT INTO TaxBill.Users (TCKN,Name,Surname,Email,Gender,BirthDay,Password) " +
                    "VALUES (" + userInfo.tckn + ",'" + userInfo.name + "','" + userInfo.surname + "','" + userInfo.email + "','" +
                    userInfo.gender + "','" + userInfo.birthDay + "','" + userInfo.password + "');";


                //This is command class which will handle the query and connection object.  
                mysqlConnection.Open();
                MySqlCommand MyCommand2 = new MySqlCommand(query, mysqlConnection);
                MySqlDataReader MyReader2;

                MyReader2 = MyCommand2.ExecuteReader();     // Here our query will be executed and data saved into the database.  
                                                            //MessageBox.Show("Save Data");

                mysqlConnection.Close();

            }

            string XML = "<Response>" + status + "</Response>";
            System.Net.HttpStatusCode statusCode = System.Net.HttpStatusCode.OK;
            return new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = new StringContent(XML, Encoding.UTF8, "application/xml")
            };
        }


    }
}
