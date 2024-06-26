﻿using System.ComponentModel.DataAnnotations;

namespace InternalPortal.Web.ViewModels
{
    public class TestTopicViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Topic text.
        /// </summary>
        [Required(ErrorMessage = "Тема не может быть пустой.")]
        public string TopicName { get; set; }

        /// <summary>
        /// Acual topic.
        /// </summary>
        public bool IsActual { get; set; }

        /// <summary>
        /// Cash test identifier.
        /// </summary>
        public int CashTestId { get; set; }
    }
}
