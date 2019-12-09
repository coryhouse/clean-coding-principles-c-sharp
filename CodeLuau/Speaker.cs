using System;
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
		public int? Register(IRepository repository)
		{
			ValidateRegistration();
			int speakerId = repository.SaveSpeaker(this);
			//if we got this far, the speaker has been accepted and is registered.
			return speakerId;
		}

		private void ValidateRegistration()
		{
			ValidateData();

			bool speakerAppearsQualified = AppearsExceptional() || !ObviousRedFlags();
			if (!speakerAppearsQualified)
			{
				throw new SpeakerDoesntMeetRequirementsException("This speaker doesn't meet our standards.");
			}

			ApproveSessions();
		}

		private void ApproveSessions()
		{
			foreach (var session in Sessions)
			{
				session.Approved = !SessionIsAboutOldTechnology(session);
			}
			bool noSessionsApproved = !Sessions.Any(s => s.Approved);
			if (noSessionsApproved) throw new NoSessionsApprovedException("No sessions approved");
		}

		private bool SessionIsAboutOldTechnology(Session session)
		{
			string[] oldTechnologies = new string[] { "Cobol", "Punch Cards", "Commodore", "VBScript" };
			foreach (var oldTech in oldTechnologies)
			{
				if (session.Title.Contains(oldTech) || session.Description.Contains(oldTech)) return true;
			}
			return false;
		}

		private bool ObviousRedFlags()
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

		private void ValidateData()
		{
			if (string.IsNullOrEmpty(FirstName)) throw new ArgumentNullException("First Name is required.");
			if (string.IsNullOrEmpty(LastName)) throw new ArgumentNullException("Last Name is required.");
			if (string.IsNullOrEmpty(Email)) throw new ArgumentNullException("Email is required.");
			if (Sessions.Count() == 0) throw new ArgumentException("Can't register a speaker without sessions.");
		}

		#region Custom Exceptions
		[Serializable]
		public class SpeakerDoesntMeetRequirementsException : Exception
		{
			public SpeakerDoesntMeetRequirementsException(string message)
				: base(message)
			{
			}

			public SpeakerDoesntMeetRequirementsException(string format, params object[] args)
				: base(string.Format(format, args)) { }
		}

		[Serializable]
		public class NoSessionsApprovedException : Exception
		{
			public NoSessionsApprovedException(string message)
				: base(message)
			{
			}
		}
		#endregion
	}
}