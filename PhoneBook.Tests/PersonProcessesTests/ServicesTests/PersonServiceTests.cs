using Microsoft.EntityFrameworkCore;
using PersonProcesses.API.Data;
using PersonProcesses.API.Services;
using PersonProcesses.Entities;
using PersonProcesses.Entities.Context;
using Xunit;

namespace PhoneBook.Tests.PersonProcessesTests.ServicesTests
{
    public class PersonServiceTests
    {
        public static DbContextOptions<PersonProcessesContext> GetPhoneBookContextForInMemoryDb()
        {
            return new DbContextOptionsBuilder<PersonProcessesContext>()
                .UseInMemoryDatabase(databaseName: "Phone_Book" + Guid.NewGuid())
                .Options;
        }
        [Fact]
        public async Task AddPerson_With_Valid_Params_Add_New_Person()
        {
            var context = new PersonProcessesContext(GetPhoneBookContextForInMemoryDb());

            context.Persons.Add(new Person()
            {
                Name = "Gülşen",
                Surname = "Çangal",
                Company = "Test1"
            });

            context.Persons.Add(new Person()
            {
                Name = "Çağrı",
                Surname = "Yldız",
                Company = "Test2"
            });

            await context.SaveChangesAsync();

            var service = new PersonService(context);

            var result = await service.AddPerson(new PersonData()
            {
                Name = "Cenk",
                Surname = "Çangal",
                Company = "Test3"
            });

            Assert.Equal(3, context.Persons.Local.Count);
        }

        [Fact]
        public async Task DeletePerson_With_Valid_Params_Delete_Person()
        {
            var context = new PersonProcessesContext(GetPhoneBookContextForInMemoryDb());

            var deletedPersonId = Guid.NewGuid();

            context.Persons.Add(new Person()
            {
                UUID = deletedPersonId,
                Name = "Gülşen",
                Surname = "Çangal",
                Company = "Test1"
            });

            context.Persons.Add(new Person()
            {
                Name = "Çağrı",
                Surname = "Yıldız",
                Company = "Test2"
            });

            await context.SaveChangesAsync();

            var service = new PersonService(context);

            var result = await service.DeletePerson(deletedPersonId);

            Assert.Equal(1, context.Persons.Local.Count);
        }

        [Fact]
        public async Task GetAllPersons_Get_All_Persons()
        {
            var context = new PersonProcessesContext(GetPhoneBookContextForInMemoryDb());

            context.Persons.Add(new Person()
            {
                Name = "Gülşen",
                Surname = "Çangal",
                Company = "Test1"
            });

            context.Persons.Add(new Person()
            {
                Name = "Çağrı",
                Surname = "Yıldız",
                Company = "Test2"
            });

            await context.SaveChangesAsync();

            var service = new PersonService(context);

            var result = await service.GetAllPersons();

            Assert.Equal(2, context.Persons.Local.Count);
        }

        [Fact]
        public async Task GetPerson_With_Valid_Params_Get_Person()
        {
            var context = new PersonProcessesContext(GetPhoneBookContextForInMemoryDb());

            var searchPersonId = Guid.NewGuid();

            context.Persons.Add(new Person()
            {
                UUID = searchPersonId,
                Name = "Gülşen",
                Surname = "Çangal",
                Company = "Test1"
            });

            context.Persons.Add(new Person()
            {
                Name = "Çağrı",
                Surname = "Yıldız",
                Company = "Test2"
            });

            await context.SaveChangesAsync();

            var service = new PersonService(context);

            var result = await service.GetPerson(searchPersonId);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetPersonDetail_With_Valid_Params_Get_Person_And_Details()
        {
            var context = new PersonProcessesContext(GetPhoneBookContextForInMemoryDb());

            var searchPersonId = Guid.NewGuid();

            context.Persons.Add(new Person()
            {
                UUID = searchPersonId,
                Name = "Gülşen",
                Surname = "Çangal",
                Company = "Test1"
            });

            context.Persons.Add(new Person()
            {
                Name = "Çağrı",
                Surname = "Yıldız",
                Company = "Test2"
            });

            await context.SaveChangesAsync();

            var service = new PersonService(context);

            var result = await service.GetPersonDetail(searchPersonId);

            Assert.NotNull(result);
        }
    }
}
