// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using System.Security.Authentication;
using GildedRose.ServiceLibrary.DataAccess;
using GildedRose.ServiceLibrary.Exceptions;
using GildedRose.ServiceLibrary.Model;

namespace GildedRose.ServiceLibrary.Authentication
{
   /// <summary>
   /// This is a placeholder Authenticator that should be reviewed/implemented by a security specialist
   /// </summary>
   /// <seealso cref="GildedRose.ServiceLibrary.Authentication.IAuthenticator" />
   public class Authenticator : IAuthenticator
   {
      private readonly IUserRepository _userRepository;

      /// <summary>
      /// Initializes a new instance of the <see cref="Authenticator"/> class.
      /// </summary>
      /// <param name="userRepository">The user repository.</param>
      public Authenticator(IUserRepository userRepository)
      {
         _userRepository = userRepository;
      }

      /// <summary>
      /// Requests the token based on the username and password.  If the user isn't found or the password is incorrect an AuthenticationException is thrown.
      /// </summary>
      /// <param name="username">The username.</param>
      /// <param name="password">The password.</param>
      /// <returns></returns>
      /// <exception cref="System.Security.Authentication.AuthenticationException">
      /// Invalid username
      /// or
      /// Invalid password
      /// </exception>
      public string RequestToken(string username, string password)
      {
         User user = _userRepository.GetUser(username);
         if (user == null)
         {
            throw new AuthenticationException("Invalid username");
         }
         if (!IsCorrectPassword(user, password))
         {
            throw new AuthenticationException("Invalid password");
         }
         return CreateToken(user);
      }

      /// <summary>
      /// Creates a token for the client to use for future requests.
      /// </summary>
      /// <param name="user">The user.</param>
      /// <returns></returns>
      private string CreateToken(User user)
      {
         //NEED A REAL TOKEN GENERATOR!!  This is placeholder code! 
         return user.Id.ToString();
      }

      /// <summary>
      /// Determines whether the password is correct for the specified user.
      /// </summary>
      /// <param name="user">The user.</param>
      /// <param name="password">The password.</param>
      /// <returns></returns>
      private bool IsCorrectPassword(User user, string password)
      {
         return HashProvider.IsMatch(user.PasswordHash, user.Id.ToString(), password);
      }

      /// <summary>
      /// Determines whether is token valid.
      /// </summary>
      /// <param name="token">The token.</param>
      /// <returns></returns>
      public bool IsTokenValid(string token)
      {
         bool valid = false;
         try
         {
            valid = _userRepository.GetUser(new Guid(token)) != null;
         }
         catch (UnknownUserException)
         {
            //log error
         }
         catch (Exception)
         {
            //log error
         }
         return valid;
      }
   }
}