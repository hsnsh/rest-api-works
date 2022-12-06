﻿namespace HsNsH.ApiWorks.DataApi.Models
{
    public class UserWithToken : User
    {
        public UserWithToken(User user)
        {
            this.UserId = user.UserId;
            this.EmailAddress = user.EmailAddress;
            this.FirstName = user.FirstName;
            this.MiddleName = user.MiddleName;
            this.LastName = user.LastName;
            this.PubId = user.PubId;
            this.HireDate = user.HireDate;

            this.Role = user.Role;
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}