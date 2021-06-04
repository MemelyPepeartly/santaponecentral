using SharkTank.Data.Entities;
using SharkTank.Logic.Constants;
using SharkTank.Logic.Objects;
using SharkTank.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTank.Data.Repository
{
    public class Mapper
    {
        #region Event
        public static Event MapEvent(EventType contextEventType)
        {
            Event logicEvent = new Event()
            {
                eventTypeID = contextEventType.EventTypeId,
                eventDescription = contextEventType.EventDescription,
                active = contextEventType.IsActive,
                removable = contextEventType.ClientRelationXrefs.Count == 0 && contextEventType.Surveys.Count == 0,
                immutable = contextEventType.EventDescription == Constants.CARD_EXCHANGE_EVENT || contextEventType.EventDescription == Constants.GIFT_EXCHANGE_EVENT
            };
            return logicEvent;
        }
        public static EventType MapEvent(Event logicEvent)
        {
            EventType contextEvent = new EventType()
            {
                EventTypeId = logicEvent.eventTypeID,
                EventDescription = logicEvent.eventDescription,
                IsActive = logicEvent.active
            };
            return contextEvent;
        }
        #endregion

        #region Assignment Status
        public static Entities.AssignmentStatus MapAssignmentStatus(Logic.Objects.AssignmentStatus logicAssignmentStatus)
        {
            Entities.AssignmentStatus contextAssignmentStatus = new Entities.AssignmentStatus()
            {
                AssignmentStatusId = logicAssignmentStatus.assignmentStatusID,
                AssignmentStatusName = logicAssignmentStatus.assignmentStatusName,
                AssignmentStatusDescription = logicAssignmentStatus.assignmentStatusDescription
            };

            return contextAssignmentStatus;
        }

        public static Logic.Objects.AssignmentStatus MapAssignmentStatus(Entities.AssignmentStatus contextAssignmentStatus)
        {
            Logic.Objects.AssignmentStatus logicAssignmentStatus = new Logic.Objects.AssignmentStatus()
            {
                assignmentStatusID = contextAssignmentStatus.AssignmentStatusId,
                assignmentStatusName = contextAssignmentStatus.AssignmentStatusName,
                assignmentStatusDescription = contextAssignmentStatus.AssignmentStatusDescription
            };

            return logicAssignmentStatus;
        }
        #endregion

        #region Category
        public static Logic.Objects.Base_Objects.Logging.Category MapCategory(Entities.Category contextCategory)
        {
            Logic.Objects.Base_Objects.Logging.Category logicCategory = new Logic.Objects.Base_Objects.Logging.Category()
            {
                categoryID = contextCategory.CategoryId,
                categoryName = contextCategory.CategoryName,
                categoryDescription = contextCategory.CategoryDescription
            };
            return logicCategory;
        }
        public static Entities.Category MapCategory(Logic.Objects.Base_Objects.Logging.Category logicCategory)
        {
            Entities.Category contextCategory = new Entities.Category()
            {
                CategoryId = logicCategory.categoryID,
                CategoryName = logicCategory.categoryName,
                CategoryDescription = logicCategory.categoryDescription
            };
            return contextCategory;
        }
        #endregion

        #region Yule log
        public static Logic.Objects.Base_Objects.Logging.YuleLog MapLog(Entities.YuleLog contextLog)
        {
            Logic.Objects.Base_Objects.Logging.YuleLog logicLog = new Logic.Objects.Base_Objects.Logging.YuleLog()
            {
                logID = contextLog.LogId,
                category = MapCategory(contextLog.Category),
                logDate = contextLog.LogDate,
                logText = contextLog.LogText
            };
            return logicLog;
        }
        public static Entities.YuleLog MapLog(Logic.Objects.Base_Objects.Logging.YuleLog logicLog)
        {
            Entities.YuleLog contextLog = new Entities.YuleLog()
            {
                LogId = logicLog.logID,
                CategoryId = logicLog.category.categoryID,
                LogDate = logicLog.logDate,
                LogText = logicLog.logText
            };
            return contextLog;
        }
        #endregion

        #region Client Meta
        public static ClientMeta MapClientMeta(Entities.Client contextClient)
        {
            ClientMeta logicMeta = new ClientMeta()
            {
                clientId = contextClient.ClientId,
                clientName = contextClient.ClientName,
                clientNickname = contextClient.Nickname,
                hasAccount = contextClient.HasAccount,
                isAdmin = contextClient.IsAdmin
            };

            return logicMeta;
        }
        public static ClientMeta MapClientMeta(Logic.Objects.Client logicClient)
        {
            ClientMeta logicMeta = new ClientMeta()
            {
                clientId = logicClient.clientID,
                clientName = logicClient.clientName,
                clientNickname = logicClient.nickname,
                hasAccount = logicClient.hasAccount,
                isAdmin = logicClient.isAdmin
            };

            return logicMeta;
        }
        public static ClientMeta MapClientMeta(HQClient logicHQClient)
        {
            ClientMeta logicMeta = new ClientMeta()
            {
                clientId = logicHQClient.clientID,
                clientName = logicHQClient.clientName,
                clientNickname = logicHQClient.nickname,
                hasAccount = logicHQClient.hasAccount,
                isAdmin = logicHQClient.isAdmin
            };

            return logicMeta;
        }
        #endregion
    }
}
