// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using GildedRose.ServiceLibrary.Model;

namespace GildedRose.ServiceLibrary.DataAccess
{
   public interface IUserRepository
   {
      /// <summary>
      ///    Gets the user based on the username.  If the user is not found an exception is thrown.
      /// </summary>
      /// <param name="username">The username.</param>
      /// <returns></returns>
      User GetUser(string username);

      /// <summary>
      ///    Gets the user based on the user id.  If the user is not found an exception is thrown.
      /// </summary>
      /// <param name="id">The identifier.</param>
      /// <returns></returns>
      User GetUser(Guid id);
   }
}