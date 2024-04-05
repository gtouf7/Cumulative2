﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cumulative1.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
// using System.Web.Http.Cors;  

namespace Cumulative1.Controllers
{
    ///<summary>
    /// The following controller will be returning the list of the teachers' information from the MySql Database
    /// </summary>
    /// <example>
    /// GET: api/TeacherData/TeacherInfo
    /// </example>
    /// <returns>
    /// complete list of teachers: ("#id teacher first name" "teacher last name" - "employee number" was hired on:"date" - "current salary:"salary")
    /// </returns>
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();

        [HttpGet]

        public IEnumerable<Teacher> ListTeachers(string searchKey = null)
        {
            
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();  //database connection: ON

            MySqlCommand cmd = Conn.CreateCommand();
            //MySQL command declaration
            cmd.CommandText = "Select * from teachers";

            MySqlDataReader resultSet = cmd.ExecuteReader();

            //creation of a list to store the teachers' info
            List<Teacher> TeacherInfo = new List<Teacher> { };

            while (resultSet.Read())
            {

                int TeacherId = (int)resultSet["teacherid"];
                string TeacherFname = resultSet["teacherfname"].ToString();
                string TeacherLname = resultSet["teacherlname"].ToString();
                string EmployeeNumber = resultSet["employeenumber"].ToString();
                string HireDate = resultSet["hiredate"].ToString();
                string Salary = resultSet["salary"].ToString();

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                TeacherInfo.Add(NewTeacher);
            }
            //database connection: OFF
            Conn.Close();

            return TeacherInfo;
            //the list of the teachers' names is returned

         
        }

        /// <summary>
        /// Finds a teacher using the teacher's ID as an input
        /// </summary>
        [HttpGet]
        public Teacher FindTeacher(int IDinput)
        {
            Teacher NewTeacher = new Teacher();
            //creates the connection to the school database
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open(); //connection: ON

            MySqlCommand cmd = Conn.CreateCommand();
            //MySQL command to get the specified teacher from the ID given
            cmd.CommandText = "select * from Teachers where teacherid = " + IDinput;

            MySqlDataReader resultSet = cmd.ExecuteReader();

            while (resultSet.Read())
            {
                int TeacherId = (int)resultSet["teacherid"];
                string TeacherFname = resultSet["teacherfname"].ToString();
                string TeacherLname = resultSet["teacherlname"].ToString();
                string EmployeeNumber = resultSet["employeenumber"].ToString();
                string HireDate = resultSet["hiredate"].ToString();
                string Salary = resultSet["salary"].ToString();

                
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
            }

            Conn.Close();
            return NewTeacher;

        }
        //deletes a teacher
        /// <summary>
        /// teacher/Delete/{id}
        /// </summary>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Delete from teachers where teacherid =@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            Conn.Close();
        }


        [HttpPost]
        //[EnableCors(origins: "*", Methods: "*", HttpCacheVaryByHeaders: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Debug.WriteLine(NewTeacher.TeacherFname);

            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "insert into teachers (teacherid, teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherId, @TeacherFname, @TeacherLname, @EmployeeNumber, CURRENT_DATE(), @Salary)";
            cmd.Parameters.AddWithValue("@TeacherId", NewTeacher.TeacherId);
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            Conn.Close();

        }





    }
    
}
