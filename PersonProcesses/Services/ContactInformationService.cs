using PersonProcesses.API.Data;
using PersonProcesses.API.Services.Base;
using PersonProcesses.API.Services.Interfaces;
using PersonProcesses.Entities;
using PersonProcesses.Entities.Base;
using PersonProcesses.Entities.Context;
using Microsoft.EntityFrameworkCore;

namespace PersonProcesses.API.Services
{
    public class ContactInformationService:BaseService, IContactInformationService
    {
        public ContactInformationService(PersonProcessesContext context) : base(context)
        {
        }

        public async Task<ReturnData> AddContactInformation(Guid personId, ContactInformationData contactInformationData)
        { 
            int personCount = context.Persons.Count(a => a.UUID == personId);
            if (personCount == 0)
            {
                return new ReturnData
                {
                    Response = false,
                    Message = "İletişim bilgisi eklenecek kişi rehberde bulunmamaktadır.",
                    Data = null
                };
            }
            var checkPerson = await context.Persons.Where(p => p.UUID == personId).FirstOrDefaultAsync();

            var contactInfoData = new ContactInformation()
            {
                PersonUUId = personId,
                InformationType = contactInformationData.InformationType,
                InformationContent = contactInformationData.InformationContent,
            };

            await context.AddAsync(contactInfoData);
            await context.SaveChangesAsync();

            return new ReturnData()
            {
                Response = true,
                Message = "İletişim bilgileri " + checkPerson.Name + " " + checkPerson.Surname + " isimli kişi için eklendi.",
                Data = contactInfoData
            };
        }

        public async Task<ReturnData> DeleteContactInformation(Guid contactInformationId)
        {
            var deleteContactInfo = await context.ContactInformations.Where(p => p.UUID == contactInformationId).FirstOrDefaultAsync();

            if (deleteContactInfo == null)
            {
                return new ReturnData
                {
                    Response = false,
                    Message = "Silinecek böyle bir iletişim bilgisi rehberde bulunmamaktadır.",
                    Data = null
                };
            }
            else
            {
                context.ContactInformations.Remove(deleteContactInfo);
                await context.SaveChangesAsync();

                return new ReturnData
                {
                    Response = true,
                    Message = "İletişim bilgisi silindi.",
                    Data = deleteContactInfo
                };
            }
        }

        public async Task<ReturnData> GetAllContactInformations()
        {
            int contactInfoCount = context.Persons.Count();

            if (contactInfoCount == 0)
            {
                return new ReturnData
                {
                    Response = false,
                    Message = "Listelenecek iletişim bilgisi rehberde bulunmamaktadır.",
                    Data = null
                };
            }

            var contactInfos = await context.ContactInformations.Select(p => new ContactInformationData
            {
                UUID = p.UUID,
                InformationContent = p.InformationContent,
                InformationType = p.InformationType

            }).ToListAsync();

           
            return new ReturnData
            {
                Response = true,
                Message = "İletişim bilgileri listelenmektedir.",
                Data = contactInfos
            };
            
        }
    }
}
