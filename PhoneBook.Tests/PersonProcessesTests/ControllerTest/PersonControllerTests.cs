using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonProcesses.API.Controllers;
using PersonProcesses.API.Data;
using PersonProcesses.API.Services.Interfaces;
using PersonProcesses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhoneBook.Tests.PersonProcessesTests.ControllerTest
{
    public class PersonControllerTests
    {
        public static int? GetStatusCodeFromActionResult<T>(ActionResult<T> actionResult)
            => ((Microsoft.AspNetCore.Mvc.ObjectResult)actionResult.Result).StatusCode;

        [Fact]
        public async Task AddPerson_With_Valid_Params_Return_201()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService
                .Setup(x => x.AddPerson(It.IsAny<PersonData>()))
            .ReturnsAsync(() => new ReturnData()
            {
                Response = true
            });

            var personsController = new PersonController(mockPersonService.Object, new Mock<IContactInformationService>().Object);

            var result = await personsController.AddPerson(new PersonData()
            {
                Name = "Gülşen",
                Surname = "Çangal",
                Company = "Test"
            });

            Assert.Equal(201, GetStatusCodeFromActionResult(result));
        }
        [Fact]
        public async Task AddPerson_With_Invalid_Params_Return_400()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService
                .Setup(x => x.AddPerson(It.IsAny<PersonData>()))
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = false
                });

            var personsController = new PersonController(mockPersonService.Object, new Mock<IContactInformationService>().Object);

            var result = await personsController.AddPerson(null);

            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);
            Assert.IsType<BadRequestResult>(badRequestResult);
            Assert.Null(result.Value);
        }
        [Fact]
        public async Task DeletePerson_With_Valid_Params_Return_200()
        {
            var id = Guid.NewGuid();
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService
                .Setup(x => x.DeletePerson(id))
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = true
                });

            var personsController = new PersonController(mockPersonService.Object, new Mock<IContactInformationService>().Object);

            var result = await personsController.DeletePerson(id);

            Assert.Equal(200, GetStatusCodeFromActionResult(result));
        }

        [Fact]
        public async Task DeletePerson_With_Valid_Params_But_No_Person_Return_404()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService
                .Setup(x => x.DeletePerson(It.IsAny<Guid>()))
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = false
                });

            var personsController = new PersonController(mockPersonService.Object, new Mock<IContactInformationService>().Object);

            var result = await personsController.DeletePerson(Guid.NewGuid());

            Assert.Equal(404, GetStatusCodeFromActionResult(result));
        }
        [Fact]
        public async Task GetAllPersons_With_Invalid_Params_Return_200()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService
                .Setup(x => x.GetAllPersons())
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = true
                });

            var personsController = new PersonController(mockPersonService.Object, new Mock<IContactInformationService>().Object);

            var result = await personsController.GetAllPersons();

            Assert.Equal(200, GetStatusCodeFromActionResult(result));
        }

        [Fact]
        public async Task GetAllPersons_With_Invalid_Params_But_No_Person_Return_404()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService
                .Setup(x => x.GetAllPersons())
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = false
                });

            var personsController = new PersonController(mockPersonService.Object, new Mock<IContactInformationService>().Object);

            var result = await personsController.GetAllPersons();

            Assert.Equal(404, GetStatusCodeFromActionResult(result));
        }
        [Fact]
        public async Task GetPerson_With_Valid_Params_Return_200()
        {
            var id = Guid.NewGuid();
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService
                .Setup(x => x.GetPerson(id))
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = true
                });

            var personsController = new PersonController(mockPersonService.Object, new Mock<IContactInformationService>().Object);

            var result = await personsController.GetPerson(id);

            Assert.Equal(200, GetStatusCodeFromActionResult(result));
        }

        [Fact]
        public async Task GetPerson_With_Valid_Params_But_No_Person_Return_404()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService
                .Setup(x => x.GetPerson(It.IsAny<Guid>()))
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = false
                });

            var personsController = new PersonController(mockPersonService.Object, new Mock<IContactInformationService>().Object);

            var result = await personsController.GetPerson(Guid.NewGuid());

            Assert.Equal(404, GetStatusCodeFromActionResult(result));
        }
        [Fact]
        public async Task AddContactInformation_With_Valid_Params_Should_Return_201()
        {
            var mockContactInformationService = new Mock<IContactInformationService>();

            var id = Guid.NewGuid();

            mockContactInformationService
                .Setup(x => x.AddContactInformation(id, It.IsAny<ContactInformationData>()))
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = true
                });

            var personsController = new PersonController(new Mock<IPersonService>().Object, mockContactInformationService.Object);


            var result = await personsController.AddContactInformation(id, new ContactInformationData()
            {
                InformationType = InformationTypesEnums.PhoneNumber,
                InformationContent = "05330001257",
                PersonId = Guid.NewGuid(),
            });

            Assert.Equal(201, GetStatusCodeFromActionResult(result));
        }

        [Fact]
        public async Task AddContactInformation_With_Invalid_Params_Should_Return_400()
        {
            var mockContactInformationService = new Mock<IContactInformationService>();

            var id = Guid.NewGuid();

            mockContactInformationService
                .Setup(x => x.AddContactInformation(id, It.IsAny<ContactInformationData>()))
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = true
                });

            var personsController = new PersonController(new Mock<IPersonService>().Object, mockContactInformationService.Object);


            var result = await personsController.AddContactInformation(id, null);

            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);
            Assert.IsType<BadRequestResult>(badRequestResult);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task AddContactInformation_With_Valid_Params_But_No_Person_Should_Return_404()
        {
            var mockContactInformationService = new Mock<IContactInformationService>();

            mockContactInformationService
                .Setup(x => x.AddContactInformation(Guid.Empty, It.IsAny<ContactInformationData>()))
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = false
                });

            var personsController = new PersonController(new Mock<IPersonService>().Object, mockContactInformationService.Object);


            var result = await personsController.AddContactInformation(Guid.Empty, new ContactInformationData()
            {
                InformationType = InformationTypesEnums.PhoneNumber,
                InformationContent = "05330001257",
                PersonId = Guid.NewGuid(),
            });

            Assert.Equal(404, GetStatusCodeFromActionResult(result));
        }
        [Fact]
        public async Task DeleteContactInformation_With_Valid_Params_Return_200()
        {
            var id = Guid.NewGuid();
            var mockContactInformationService = new Mock<IContactInformationService>();
            mockContactInformationService
                .Setup(x => x.DeleteContactInformation(id))
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = true
                });

            var personsController = new PersonController(new Mock<IPersonService>().Object, mockContactInformationService.Object);

            var result = await personsController.DeleteContactInformation(id);

            Assert.Equal(200, GetStatusCodeFromActionResult(result));
        }

        [Fact]
        public async Task DeleteContactInformation_With_Valid_Params_But_No_Person_Return_404()
        {
            var mockContactInformationService = new Mock<IContactInformationService>();
            mockContactInformationService
                .Setup(x => x.DeleteContactInformation(It.IsAny<Guid>()))
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = false
                });

            var personsController = new PersonController(new Mock<IPersonService>().Object, mockContactInformationService.Object);

            var result = await personsController.DeleteContactInformation(Guid.NewGuid());

            Assert.Equal(404, GetStatusCodeFromActionResult(result));
        }
        [Fact]
        public async Task GetAllContactInformations_With_Invalid_Params_Return_200()
        {
            var mockContactInformationService = new Mock<IContactInformationService>();
            mockContactInformationService
                .Setup(x => x.GetAllContactInformations())
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = true
                });

            var personsController = new PersonController(new Mock<IPersonService>().Object, mockContactInformationService.Object);

            var result = await personsController.GetAllContactInformations();

            Assert.Equal(200, GetStatusCodeFromActionResult(result));
        }

        [Fact]
        public async Task GetAllContactInformations_With_Invalid_Params_But_No_Person_Return_404()
        {
            var mockContactInformationService = new Mock<IContactInformationService>();
            mockContactInformationService
                .Setup(x => x.GetAllContactInformations())
                .ReturnsAsync(() => new ReturnData()
                {
                    Response = false
                });

            var personsController = new PersonController(new Mock<IPersonService>().Object, mockContactInformationService.Object);
            var result = await personsController.GetAllContactInformations();

            Assert.Equal(404, GetStatusCodeFromActionResult(result));
        }
    }
}
