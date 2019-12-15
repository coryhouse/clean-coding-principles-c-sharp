using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace CodeLuau.Tests
{
	[TestClass]
	public class SpeakerTests
	{
		private FakeRepository repository = new FakeRepository();

		[TestMethod]
		public void Register_EmptyFirstName_ReturnsFirstNameRequired()
		{
			//arrange
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.FirstName = "";

			//act
			var result = speaker.Register(repository);

			//assert
			Assert.AreEqual(RegisterError.FirstNameRequired, result.Error);
		}

		[TestMethod]
		public void Register_EmptyLastName_ReturnsLastNameRequired()
		{
			//arrange
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.LastName = "";

			//act
			var result = speaker.Register(repository);

			//assert
			Assert.AreEqual(RegisterError.LastNameRequired, result.Error);
		}

		[TestMethod]
		public void Register_EmptyEmail_ReturnsEmailRequired()
		{
			//arrange
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Email = "";

			//act
			var result = speaker.Register(repository);

			//assert
			Assert.AreEqual(RegisterError.EmailRequired, result.Error);
		}

		[TestMethod]
		public void Register_WorksForPrestigiousEmployerButHasRedFlags_ReturnsSpeakerId()
		{
			//arrange
			var speaker = GetSpeakerWithRedFlags();
			speaker.Employer = "Microsoft";

			//act
			var result = speaker.Register(new FakeRepository());

			//assert
			Assert.IsNotNull(result.SpeakerId);
		}

		[TestMethod]
		public void Register_HasBlogButHasRedFlags_ReturnsSpeakerId()
		{
			//arrange
			var speaker = GetSpeakerWithRedFlags();

			//act
			var result = speaker.Register(new FakeRepository());

			//assert
			Assert.IsNotNull(result.SpeakerId);
		}

		[TestMethod]
		public void Register_HasCertificationsButHasRedFlags_ReturnsSpeakerId()
		{
			//arrange
			var speaker = GetSpeakerWithRedFlags();
			speaker.Certifications = new List<string>()
		{
			"cert1",
			"cert2",
			"cert3",
			"cert4"
		};

			//act
			var result = speaker.Register(new FakeRepository());

			//assert
			Assert.IsNotNull(result.SpeakerId);
		}

		[TestMethod]
		public void Register_SingleSessionThatsOnOldTech_ReturnsNoSessionsApproved()
		{
			//arrange
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Sessions = new List<Session>() {
			new Session("Cobol for dummies", "Intro to Cobol")
		};

			//act
			var result = speaker.Register(repository);

			//assert
			Assert.AreEqual(RegisterError.NoSessionsApproved, result.Error);
		}

		[TestMethod]
		public void Register_NoSessionsPassed_ReturnsNoSessionsProvidedError()
		{
			//arrange
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Sessions = new List<Session>();

			//act
			var result = speaker.Register(repository);

			//assert
			Assert.AreEqual(RegisterError.NoSessionsProvided, result.Error);
		}

		[TestMethod]
		public void Register_DoesntAppearExceptionalAndUsingOldBrowser_ReturnsSpeakerDoesNotMeetStandards()
		{
			//arrange
			var speakerThatDoesntAppearExceptional = GetSpeakerThatWouldBeApproved();
			speakerThatDoesntAppearExceptional.HasBlog = false;
			speakerThatDoesntAppearExceptional.Browser = new WebBrowser("IE", 6);

			//act
			var result = speakerThatDoesntAppearExceptional.Register(repository);

			//assert
			Assert.AreEqual(RegisterError.SpeakerDoesNotMeetStandards, result.Error);
		}

		[TestMethod]
		public void Register_DoesntAppearExceptionalAndHasAncientEmail_ReturnsSpeakerDoesNotMeetStandards()
		{
			//arrange
			var speakerThatDoesntAppearExceptional = GetSpeakerThatWouldBeApproved();
			speakerThatDoesntAppearExceptional.HasBlog = false;
			speakerThatDoesntAppearExceptional.Email = "name@aol.com";

			//act
			var result = speakerThatDoesntAppearExceptional.Register(repository);

			//assert
			Assert.AreEqual(RegisterError.SpeakerDoesNotMeetStandards, result.Error);
		}

		#region Helpers
		private Speaker GetSpeakerThatWouldBeApproved()
		{
			return new Speaker()
			{
				FirstName = "First",
				LastName = "Last",
				Email = "example@domain.com",
				Employer = "Example Employer",
				HasBlog = true,
				Browser = new WebBrowser("test", 1),
				Exp = 1,
				Certifications = new System.Collections.Generic.List<string>(),
				BlogURL = "",
				Sessions = new System.Collections.Generic.List<Session>() {
				new Session("test title", "test description")
			}
			};
		}

		private Speaker GetSpeakerWithRedFlags()
		{
			var speaker = GetSpeakerThatWouldBeApproved();
			speaker.Email = "tom@aol.com";
			speaker.Browser = new WebBrowser("IE", 6);
			return speaker;
		}
		#endregion
	}

}
