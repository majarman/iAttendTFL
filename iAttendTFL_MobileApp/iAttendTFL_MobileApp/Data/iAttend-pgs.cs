using iAttendTFL_MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iAttendTFL_MobileApp.Data
{
    public class iAttend_pgs
    {
        //TODO: postgresql database connection goes here
    }
}


/*
//////Connection string is for PostgreSQL.//////
string ConnectionString = "Server=localhost; Port=5432; User Id=admin; Password=123456;
Database=demo";


//////To connect database//////
try{ 
    NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
    connection.Open();
}catch(Exception ex){ 
  Console.WriteLine(ex.ToString()); 
}


//////To read table from database//////
NpgsqlCommand command = connection.CreateCommand();
command.CommandText = "SELECT * FROM people";
try{
    NpgsqlDataReader reader = command.ExecuteReader();
 
while (reader.Read()) {
    Id.Text = reader[0].ToString();
    Name.Text = reader[1].ToString();
 }
    connection.Close();
 
 }catch(Exception ex){
    Console.WriteLine(ex.ToString());
 }

https://www.selmanalpdundar.com/how-to-connect-and-read-data-from-postgresql-in-xamarin.html

*/
