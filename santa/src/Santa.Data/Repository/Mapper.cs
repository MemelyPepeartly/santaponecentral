using System;
using System.Collections.Generic;
using System.Text;
using Santa.Logic.Objects;

namespace Santa.Data.Repository
{
    public static class Mapper
    {
        #region Client
        public static Logic.Objects.Client MapClient (Entities.Client ContextCharacter)
        {
            Logic.Objects.Client LogicClient = new Client()
            {
                clientID = ContextCharacter.ClientId,
                email = ContextCharacter.Email,
                nickname = ContextCharacter.Nickname
            };

            return LogicClient;
        }
        #endregion
    }
}