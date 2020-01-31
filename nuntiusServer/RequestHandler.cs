using System;
using System.Collections.Generic;
using nuntiusModel;

namespace NuntiusServer
{
	public static class RequstHandler
	{
		public static Response NewRequest(Request request)
		{
			Response response = new Response();

			// Check the Request Type
			switch (request.Type)
			{
				case "login":
					response = LoginResponse(request);
					break;

				case "nagg":

					List<Message> m = new List<Message>();
					m.Add(new Message { From = "adelGames", To = "fynn002", Sent = DateTime.Now, Text = "Hallo, wie geht es Ihnen? Mit freindlichen Grüßen AdelGames!"});
					m.Add(new Message { From = "fynn002", To = "adelGames", Sent = DateTime.Now, Text = "Sehr geehret Herr Fynn002 mit geht es gut. Ebenfalls feindliche Grüße!"});
					response.ParentResponse(m);
					//response.UnknownErrorRespone();
					break;

				case "register":
					response = RegisterResponse(request);
					break;

				case "send":
					response.SendSuccessResponse();
					break;

				default:
					response.UnknownErrorRespone();
					break;
			}

			return response;
		}

		/// <summary>
		/// Handel the login request
		/// </summary>
		private static Response LoginResponse(Request request)
		{
			Response response = new Response();
			string alias = request.Parameters[0].ToString();
			string password = request.Parameters[1].ToString();

			//Was the login successfull?
			string token = UserController.Login(alias, password);
			if (token != null)
				response.LoginSuccessResponse(token);
			else
				response.LoginErrorResponse("Invalid alias or password");

			return response;
		}

		/// <summary>
		/// Handel the register request
		/// </summary>
		private static Response RegisterResponse(Request request)
		{
			Response response = new Response();

			string alias = request.Parameters[0].ToString();
			string password = request.Parameters[1].ToString();

			//Was the registration successfull?
			string token = UserController.RegisterNewUser(alias, password);
			if (token != null)
				response.RegistrationSuccessResponse(token);
			else
				response.RegistrationErrorResponse();

			return response;
		}
	}
}