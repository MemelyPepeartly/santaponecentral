using System;
using System.Collections.Generic;
using System.Text;
using Santa.Logic.Objects;

namespace Santa.Data.Repository
{
    public static class Mapper
    {
        #region Client
        public static Logic.Objects.Client MapClient (Entities.Client contextCharacter)
        {
            Logic.Objects.Client LogicClient = new Client()
            {
                clientID = contextCharacter.ClientId,
                clientStatusID = contextCharacter.ClientStatusId,
                email = contextCharacter.Email,
                nickname = contextCharacter.Nickname,
                clientName = contextCharacter.ClientName,
                address = new Address
                {
                    addressLineOne = contextCharacter.AddressLine1,
                    addressLineTwo = contextCharacter.AddressLine2,
                    city = contextCharacter.City,
                    country = contextCharacter.Country,
                    state = contextCharacter.State,
                    postalCode = contextCharacter.State
                }

            };

            return LogicClient;
        }
        #endregion
    }
}