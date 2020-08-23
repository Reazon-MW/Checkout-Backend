using Microsoft.EntityFrameworkCore;
using CheckoutProj.Models;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CheckoutProj
{
    public class Utils
    {
        private readonly ILogger logger;

        public Utils()
        {
            logger = Log.Create<Utils>();
        }

        public void FillDB()
        {
            /* string con = "Server=(localdb)\\mssqllocaldb;Database=HelpingHandDB;Trusted_Connection=True;";
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(con);
            using var DBContext = new DatabaseContext(optionsBuilder.Options);

            DBContext.Users.RemoveRange(DBContext.Users.ToList());
            DBContext.Types.RemoveRange(DBContext.Types.ToList());
            DBContext.Disabilities.RemoveRange(DBContext.Disabilities.ToList());
            DBContext.UsersDisabilities.RemoveRange(DBContext.UsersDisabilities.ToList());
            DBContext.Places.RemoveRange(DBContext.Places.ToList());
            DBContext.SaveChanges();
            logger.LogInformation("Cleared Database");

            User U_Ivan = new User
            {
                EMail = "example@gmail.com",
                Name = "Ivan",
                Surname = "Ivanov",
                PasswordHash = "12345678",
                Role = "admin"
            }, U_Alexandr = new User
            {
                EMail = "second@gmail.com",
                Name = "Alexandr",
                Surname = "Petrov",
                PasswordHash = "87654321",
                Role = "user"
            };
            DBContext.Users.AddRange(U_Ivan, U_Alexandr);
            logger.LogInformation("UsersAdded");

            Type_ T_Blind = new Type_
            {
                Name = "Full Blindness",
                Description = "Inability to see completely"
            }, T_NoWalk = new Type_
            {
                Name = "Inability to walk",
                Description = "Inability to walk, that supposes wheelchair"
            };
            DBContext.Types.AddRange(T_Blind, T_NoWalk);
            logger.LogInformation("TypesAdded");


            Disability D_Osteogenesis = new Disability
            {
                Name = "Osteogenesis",
                Type = T_NoWalk
            }, D_Glaucoma = new Disability
            {
                Name = "Glaucoma",
                Type = T_Blind
            };
            DBContext.Disabilities.AddRange(D_Osteogenesis, D_Glaucoma);
            logger.LogInformation("DisabilitiesAdded");


            UserDisability UD_IvansGLaucoma = new UserDisability
            {
                User = U_Ivan,
                Disability = D_Glaucoma
            }, UD_AlexandrOsteogen = new UserDisability
            {
                User = U_Alexandr,
                Disability = D_Osteogenesis
            };

            DBContext.SaveChanges();
            var user = DBContext.Users.Include(p => p.UsersDisabilities).First();
            user.UsersDisabilities.Add(UD_IvansGLaucoma);



            DBContext.SaveChanges();


            DBContext.Dispose();
            */
        }
    }
}