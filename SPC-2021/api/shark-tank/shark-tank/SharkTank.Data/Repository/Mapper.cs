using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTank.Data.Repository
{
    public class Mapper
    {
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
    }
}
