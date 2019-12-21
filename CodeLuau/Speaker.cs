using System.Collections.Generic;
using System.Linq;

namespace CodeLuau
{
	/// <summary>
	/// Represents a single speaker
	/// </summary>
	public class Speaker
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int? YearsExperience { get; set; }
		public bool HasBlog { get; set; }
		public string BlogURL { get; set; }
		public WebBrowser Browser { get; set; }
		public List<string> Certifications { get; set; }
		public string Employer { get; set; }
		public int RegistrationFee { get; set; }
		public List<Session> Sessions { get; set; }

		/// <summary>
		/// Register a speaker
		/// </summary>
		/// <returns>speakerID</returns>
		public RegisterResponse Register(IRepository repository)
		{
			var error = ValidateRegistration();
			if (error != null) return new RegisterResponse(error);
			int speakerId = repository.SaveSpeaker(this);
			return new RegisterResponse(speakerId);
		}

		private RegisterError? ValidateRegistration()
		{
			var error = ValidateData();
			if (error != null) return error;
			bool speakerAppearsQualified = AppearsExceptional() || !HasObviousRedFlags();
			if (!speakerAppearsQualified) return RegisterError.SpeakerDoesNotMeetStandards;
			var approvalError = ApproveSessions();
			if (approvalError == null) return null;
			return approvalError;
		}

		private RegisterError? ApproveSessions()
		{
			foreach (var session in Sessions)
			{
				session.Approved = !SessionIsAboutOldTechnology(session);
			}
			if (Sessions.Any(s => s.Approved)) return null;
			return RegisterError.NoSessionsApproved;
		}

		private bool SessionIsAboutOldTechnology(Session session)
		{
			var oldTechnologies = new List<string> { "Cobol", "Punch Cards", "Commodore", "VBScript" };
			foreach (var oldTech in oldTechnologies)
			{
				if (session.Title.Contains(oldTech) || session.Description.Contains(oldTech)) return true;
			}
			return false;
		}

		private bool HasObviousRedFlags()
		{
			//need to get just the domain from the email
			string emailDomain = Email.Split('@').Last();
			var ancientEmailDomains = new List<string>() { "aol.com", "prodigy.com", "compuserve.com" };
			return (ancientEmailDomains.Contains(emailDomain) || ((Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < 9)));
		}

		private bool AppearsExceptional()
		{
			if (YearsExperience > 10) return true;
			if (HasBlog) return true;
			if (Certifications.Count() > 3) return true;

			var preferredEmployers = new List<string>() { "Pluralsight", "Microsoft", "Google" };
			if (preferredEmployers.Contains(Employer)) return true;
			return false;
		}

		private RegisterError? ValidateData()
		{
			if (string.IsNullOrEmpty(FirstName)) return RegisterError.FirstNameRequired;
			if (string.IsNullOrEmpty(LastName)) return RegisterError.LastNameRequired;
			if (string.IsNullOrEmpty(Email)) return RegisterError.EmailRequired;
			if (!Sessions.Any()) return RegisterError.NoSessionsProvided;
			return null;
		}
	}
}