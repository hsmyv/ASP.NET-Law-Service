﻿using System.ComponentModel.DataAnnotations;


namespace Aspİntro.ViewModels.Account
{
    public class ForgotPasswordVM
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
