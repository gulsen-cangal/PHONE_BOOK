using PersonProcesses.API.Data;
using PersonProcesses.API.Services.Base;
using PersonProcesses.API.Services.Interfaces;
using PersonProcesses.Entities;
using PersonProcesses.Entities.Context;
using Microsoft.EntityFrameworkCore;

namespace PersonProcesses.API.Services
{
    public class PersonService:BaseService,IPersonService
    {
        public PersonService(PersonProcessesContext context) : base(context)
        {
        }

        public async Task<ReturnData> AddPerson(PersonData personData)
        {
            var person = new Person()
            {
                Name = personData.Name,
                Surname = personData.Surname,
                Company = personData.Company
            };

            await context.Persons.AddAsync(person);
            await context.SaveChangesAsync();

            if (person.UUID != Guid.Empty)
            {
                return new ReturnData
                {
                    Response = true,
                    Message = "Kişi eklendi.",
                    Data = person
                };
            }
            else
            {
                return new ReturnData
                {
                    Response = false,
                    Message = "Kişi eklenemedi.",
                    Data = null
                };
            }
        }

        public async Task<ReturnData> DeletePerson(Guid personId)
        {
            var deletePerson = await context.Persons.Where(p => p.UUID == personId).FirstOrDefaultAsync();

            if (deletePerson == null)
            {
                return new ReturnData
                {
                    Response = false,
                    Message = "Silinecek böyle bir kayıt bulunmamaktadır.",
                    Data = null
                };
            }
            else
            {
                context.Persons.Remove(deletePerson);
                await context.SaveChangesAsync();

                return new ReturnData
                {
                    Response = true,
                    Message = "Kişi silindi.",
                    Data = deletePerson
                };
            }
        }

        public async Task<ReturnData> GetAllPersons()
        {
            int personCount = context.Persons.Count();

            if (personCount == 0)
            {
                return new ReturnData
                {
                    Response = false,
                    Message = "Listelenecek kayıt bulunmamaktadır.",
                    Data = null
                };
            }

            var allPersons = await context.Persons.Select(p => new PersonData
            {
                UUID = p.UUID,
                Name = p.Name,
                Surname = p.Surname,
                Company = p.Company,
            }).ToListAsync();

          
            return new ReturnData
            {
                Response = true,
                Message = "Kişiler listelenmektedir.",
                Data = allPersons
            };
            
        }

        public async Task<ReturnData> GetPersonDetail(Guid personId)
        {
            var result = await context.Persons.Where(p => p.UUID == personId).Select(p => new PersonDetailData
            {
                Person = new PersonData()
                {
                    UUID = p.UUID,
                    Name = p.Name,
                    Surname = p.Surname,
                    Company = p.Company
                },
                ContactInformations = p.ContactInformations.Select(c => new ContactInformationData
                {
                    UUID = c.UUID,
                    InformationType = c.InformationType,
                    InformationContent = c.InformationContent
                }).ToList()
            }).FirstOrDefaultAsync();

            return new ReturnData
            {
                Response = true,
                Message = "Kişi listelenmektedir.",
                Data = result
            };
            
        }

        public async Task<ReturnData> GetPerson(Guid personId)
        {
            int personCount = context.Persons.Count(a => a.UUID == personId);
            if (personCount == 0)
            {
                return new ReturnData
                {
                    Response = false,
                    Message = "Sorgulanan kişi rehberde bulunmamaktadır.",
                    Data = null
                };
            }

            var getPerson = await context.Persons.Where(p => p.UUID == personId).Select(p => new PersonData
            {
                UUID = p.UUID,
                Name = p.Name,
                Surname = p.Surname,
                Company = p.Company
            }).FirstOrDefaultAsync();

            return new ReturnData
            {
                Response = true,
                Message = "Kişi listelenmektedir.",
                Data = getPerson
            };
            
        }
    }
}
