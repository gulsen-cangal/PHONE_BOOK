using Microsoft.EntityFrameworkCore;
using PersonProcesses.API.Data;
using PersonProcesses.API.Services;
using PersonProcesses.Entities;
using PersonProcesses.Entities.Context;
using PersonProcesses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhoneBook.Tests.PersonProcessesTests.ServicesTests
{
    public class ContactInformationServiceTests
    {
        public static DbContextOptions<PersonProcessesContext> GetPhoneBookContextForInMemoryDb()
        {
            return new DbContextOptionsBuilder<PersonProcessesContext>()
                .UseInMemoryDatabase(databaseName: "Phone_Book" + Guid.NewGuid())
                .Options;
        }

        [Fact]
        public async Task AddContactInformation_With_Valid_Params_Add_New_Contact_Information()
        {
            var context = new PersonProcessesContext(GetPhoneBookContextForInMemoryDb());

            var personId = Guid.NewGuid();
            context.Persons.Add(new Person()
            {
                UUID = personId,
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

            var service = new ContactInformationService(context);

            var result = await service.AddContactInformation(personId, new ContactInformationData()
            {
                InformationType = InformationTypesEnums.Location,
                InformationContent = "Çorlu"
            });

            Assert.True(result.Response);
        }

        [Fact]
        public async Task AddContactInformation_With_Invalid_Params_Not_Add_New_Contact_Information()
        {
            var context = new PersonProcessesContext(GetPhoneBookContextForInMemoryDb());

            var personId = Guid.NewGuid();
            context.Persons.Add(new Person()
            {
                UUID = personId,
                Name = "Gülşen",
                Surname = "Çangal",
                Company = "Test"
            });

            context.Persons.Add(new Person()
            {
                Name = "Çağrı",
                Surname = "Yıldız",
                Company = "Test"
            });
            await context.SaveChangesAsync();

            var service = new ContactInformationService(context);

            var result = await service.AddContactInformation(Guid.Empty, new ContactInformationData()
            {
                InformationType = InformationTypesEnums.Location,
                InformationContent = "Çorlu"
            });

            Assert.False(result.Response);
        }

        [Fact]
        public async Task DeleteContactInformation_With_Valid_Params_Delete_Contact_Information()
        {
            var context = new PersonProcessesContext(GetPhoneBookContextForInMemoryDb());

            var contactInformationId = Guid.NewGuid();
            context.ContactInformations.Add(new ContactInformation()
            {
                UUID = contactInformationId,
                InformationType = InformationTypesEnums.Location,
                InformationContent = "Çorlu"
            });

            await context.SaveChangesAsync();

            var service = new ContactInformationService(context);

            var result = await service.DeleteContactInformation(contactInformationId);

            Assert.True(result.Response);
        }

        [Fact]
        public async Task DeleteContactInformation_With_Invalid_Params_Not_Delete_Contact_Information()
        {
            var context = new PersonProcessesContext(GetPhoneBookContextForInMemoryDb());

            var contactInformationId = Guid.NewGuid();
            context.ContactInformations.Add(new ContactInformation()
            {
                UUID = contactInformationId,
                InformationType = InformationTypesEnums.Location,
                InformationContent = "Çorlu"
            });

            await context.SaveChangesAsync();

            var service = new ContactInformationService(context);

            var result = await service.DeleteContactInformation(Guid.Empty);

            Assert.False(result.Response);
        }

        [Fact]
        public async Task GetAllContactInformations_Get_All_Contact_Information()
        {
            var context = new PersonProcessesContext(GetPhoneBookContextForInMemoryDb());

            var contactInformationId = Guid.NewGuid();
            context.ContactInformations.Add(new ContactInformation()
            {
                UUID = contactInformationId,
                InformationType = InformationTypesEnums.Location,
                InformationContent = "Çorlu"
            });

            await context.SaveChangesAsync();

            var service = new ContactInformationService(context);

            var result = await service.DeleteContactInformation(contactInformationId);

            Assert.NotNull(result);
        }
    }
}
