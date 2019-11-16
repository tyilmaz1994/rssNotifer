using notifier.dal.entities;
using System.Collections.Generic;

namespace notifer.api.models
{
    public class BaseResponseModel<TEntity> : BaseTransferModel<TEntity> where TEntity: BaseEntity
    {
        public bool IsSuccess { get; set; } = true;

        public IList<string> Messages { get; set; }

        /// <summary>
        /// Add messages to response such as error, information, warning ..
        /// also IsSuccess will be set false, if any error occurs
        /// </summary>
        /// <param name="message">message(error, information, warnings)</param>
        public void AddMessage(string message, bool isSuccessMessage = true)
        {
            if (Messages == null)
                Messages = new List<string>();
            IsSuccess = isSuccessMessage;
            Messages.Add(message);
        }
    }
}