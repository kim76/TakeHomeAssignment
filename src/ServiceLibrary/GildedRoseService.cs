// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using System.Linq;
using System.Security.Authentication;
using System.ServiceModel;
using GildedRose.ServiceLibrary.Authentication;
using GildedRose.ServiceLibrary.DataAccess;
using GildedRose.ServiceLibrary.Exceptions;
using GildedRose.ServiceLibrary.Model;

namespace GildedRose.ServiceLibrary
{
   /// <summary>
   /// A Service to allow customers access to the Gilded Rose
   /// </summary>
   /// <seealso cref="IGildedRoseService" />
   [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
   public class GildedRoseService : IGildedRoseService
   {

      private readonly IDataRepository _dataRepository;
      private readonly IAuthenticator _authenticator;


      /// <summary>
      /// Initializes a new instance of the <see cref="GildedRoseService"/> class using default data structures.
      /// </summary>
      public GildedRoseService()
      {
         _dataRepository = new InMemoryDataRepository();
         _authenticator = new Authenticator(new InMemoryUserRepository());

      }
      /// <summary>
      /// Initializes a new instance of the <see cref="GildedRoseService"/> class and allows dependency injection.
      /// </summary>
      /// <param name="dataRepository">The repository.</param>
      /// <param name="authenticator">The authenticator.</param>
      public GildedRoseService(IDataRepository dataRepository, IAuthenticator authenticator)
      {
         _dataRepository = dataRepository;
         _authenticator = authenticator;
      }

      /// <summary>
      /// Retrieves the entire inventory.
      /// </summary>
      /// <returns></returns>
      /// <remarks>
      /// As inventory increases new methods to provide searching and paging can be introduced.
      /// </remarks>
      public ItemsRequestResponse GetAllItems()
      {
         ItemsRequestResponse respone = new ItemsRequestResponse();
         try
         {
            respone.Items = _dataRepository.GetAllItems().ToList();
            respone.Status = ResponseStatus.Success;
         }
         catch(Exception e)
         { 
            //log message: e
            respone.Status = ResponseStatus.Failed;
            respone.Message = "Cannot retrieve inventory at this time.  Please contact support.";
         }
         return respone;
      }

      /// <summary>
      /// Buys required number of the item and reduces the stock accordingly.  If the entire amount is not in stock, no items are purchased.
      /// </summary>
      /// <param name="token">The token.</param>
      /// <param name="itemId">The Guid of the item in string format.</param>
      /// <param name="quantity">The quantity to purchase.</param>
      /// <returns></returns>
      public RequestResponse BuyItem(string token, string itemId, int quantity)
      {
         RequestResponse response = new RequestResponse();
         Guid guid;
         if (!Guid.TryParse(itemId, out guid))
         {
            response.Status = ResponseStatus.Failed;
            response.Message = string.Format("The item Id '{0}' is not a valid Guid", itemId);
            return response;
         }
         try
         {
            Guid tokenGuid;
            if (!Guid.TryParse(token, out tokenGuid) || !_authenticator.IsTokenValid(token))
            {
               response.Status = ResponseStatus.Unauthorized;
               response.Message = "The token is invalid";
               return response;
            }
            var item = _dataRepository.GetItem(guid);
            item.ReduceStock(quantity);
            response.Status = ResponseStatus.Success;
            response.Message = string.Format("Purchase of {0} items complete.", quantity);
         }
         catch (UnknownItemException)
         {
            response.Status = ResponseStatus.Failed;
            response.Message = "This item does not exist.";
         }
         catch (InsufficientStockException)
         {
            response.Status = ResponseStatus.Failed;
            response.Message = string.Format("There is insufficient stock to purchase {0} items.", quantity);
         }
         catch (Exception)
         {
            response.Status = ResponseStatus.Failed;
            response.Message = "An error occurred while processing your request.";
         }
         return response;
      }

      /// <summary>
      /// Authenticates the user and provides a token to use for future requests.
      /// </summary>
      /// <param name="name">The name.</param>
      /// <param name="password">The password.</param>
      /// <returns></returns>
      public TokenRequestResponse RequestToken(string name, string password)
      {
         TokenRequestResponse response = new TokenRequestResponse();
         try
         {
            response.Token = _authenticator.RequestToken(name, password);
            response.Status = ResponseStatus.Success;
         }
         catch (AuthenticationException)
         {
            response.Status = ResponseStatus.Failed;
            response.Message = string.Format("Authentication failed for {0}.", name);
         }
         catch (UnknownUserException)
         {
            response.Status = ResponseStatus.Failed;
            response.Message = string.Format("Authentication failed for {0}.", name);
         }
         catch (Exception)
         {
            response.Status = ResponseStatus.Failed;
            response.Message = "An error occurred while trying to get a token.";
         }
         return response;
      }
   }
}
