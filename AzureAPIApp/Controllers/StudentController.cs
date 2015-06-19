using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoPoco;
using AutoPoco.Engine;
using AzureAPIApp.Models;

namespace AzureAPIApp.Controllers
{
    public class StudentController : ApiController
    {
        public IHttpActionResult Get()
        {
            var students = Students();

            return Ok(students);
        }

        private List<Student> Students()
        {
            var students = new List<Student>();
            IGenerationSessionFactory factory = AutoPocoContainer.Configure(
                x =>
                {
                   x.Conventions(c => { c.UseDefaultConventions();});
                    x.AddFromAssemblyContainingType<Student>();
                });

            //Create a session
            IGenerationSession session = factory.CreateSession();
            students = session.List<Student>(50)
                .First(25)
                .Impose(x => x.FirstName, "Bob")
                .Next(25)
                .Impose(x => x.FirstName, "Alice")
                .All()
                .Impose(x => x.LastName, "Pius")
                .Random(10).Impose(x => x.LastName, "Peter")
                .All().Get().ToList();

            return students;
        }
    }
}
