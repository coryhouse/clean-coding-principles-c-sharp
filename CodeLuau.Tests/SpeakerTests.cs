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
			public void Register_EmptyFirstName_ThrowsArgumentNullException()
			{
				//arrange
				var speaker = GetSpeakerThatWouldBeApproved();
				speaker.FirstName = "";

				//act
				var exception = ExceptionAssert.Throws<ArgumentNullException>(() => speaker.Register(repository));

				//assert
				Assert.AreEqual(exception.GetType(), typeof(ArgumentNullException));
			}

			[TestMethod]
			public void Register_EmptyLastName_ThrowsArgumentNullException()
			{
				//arrange
				var speaker = GetSpeakerThatWouldBeApproved();
				speaker.LastName = "";

				//act
				var exception = ExceptionAssert.Throws<ArgumentNullException>(() => speaker.Register(repository));

				//assert
				Assert.AreEqual(exception.GetType(), typeof(ArgumentNullException));
			}

			[TestMethod]
			public void Register_EmptyEmail_ThrowsArgumentNullException()
			{
				//arrange
				var speaker = GetSpeakerThatWouldBeApproved();
				speaker.Email = "";

				//act
				var exception = ExceptionAssert.Throws<ArgumentNullException>(() => speaker.Register(repository));

				//assert
				Assert.AreEqual(exception.GetType(), typeof(ArgumentNullException));
			}

			[TestMethod]
			public void Register_WorksForPrestigiousEmployerButHasRedFlags_ReturnsSpeakerId()
			{
				//arrange
				var speaker = GetSpeakerWithRedFlags();
				speaker.Employer = "Microsoft";

				//act
				int? speakerId = speaker.Register(new FakeRepository());

				//assert
				Assert.IsFalse(speakerId == null);
			}

			[TestMethod]
			public void Register_HasBlogButHasRedFlags_ReturnsSpeakerId()
			{
				//arrange
				var speaker = GetSpeakerWithRedFlags();

				//act
				int? speakerId = speaker.Register(new FakeRepository());

				//assert
				Assert.IsFalse(speakerId == null);
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
				int? speakerId = speaker.Register(new FakeRepository());

				//assert
				Assert.IsFalse(speakerId == null);
			}

			[TestMethod]
			public void Register_SingleSessionThatsOnOldTech_ThrowsNoSessionsApprovedException()
			{
				//arrange
				var speaker = GetSpeakerThatWouldBeApproved();
				speaker.Sessions = new List<Session>() {
				new Session("Cobol for dummies", "Intro to Cobol")
			};

				//act
				var exception = ExceptionAssert.Throws<Speaker.NoSessionsApprovedException>(() => speaker.Register(repository));

				//assert
				Assert.AreEqual(exception.GetType(), typeof(Speaker.NoSessionsApprovedException));
			}

			[TestMethod]
			public void Register_NoSessionsPassed_ThrowsArgumentException()
			{
				//arrange
				var speaker = GetSpeakerThatWouldBeApproved();
				speaker.Sessions = new List<Session>();

				//act
				var exception = ExceptionAssert.Throws<ArgumentException>(() => speaker.Register(repository));

				//assert
				Assert.AreEqual(exception.GetType(), typeof(ArgumentException));
			}

			[TestMethod]
			public void Register_DoesntAppearExceptionalAndUsingOldBrowser_ThrowsNoSessionsApprovedException()
			{
				//arrange
				var speakerThatDoesntAppearExceptional = GetSpeakerThatWouldBeApproved();
				speakerThatDoesntAppearExceptional.HasBlog = false;
				speakerThatDoesntAppearExceptional.Browser = new WebBrowser("IE", 6);

				//act
				var exception = ExceptionAssert.Throws<Speaker.SpeakerDoesntMeetRequirementsException>(() => speakerThatDoesntAppearExceptional.Register(repository));

				//assert
				Assert.AreEqual(exception.GetType(), typeof(Speaker.SpeakerDoesntMeetRequirementsException));
			}

			[TestMethod]
			public void Register_DoesntAppearExceptionalAndHasAncientEmail_ThrowsNoSessionsApprovedException()
			{
				//arrange
				var speakerThatDoesntAppearExceptional = GetSpeakerThatWouldBeApproved();
				speakerThatDoesntAppearExceptional.HasBlog = false;
				speakerThatDoesntAppearExceptional.Email = "name@aol.com";

				//act
				var exception = ExceptionAssert.Throws<Speaker.SpeakerDoesntMeetRequirementsException>(() => speakerThatDoesntAppearExceptional.Register(repository));

				//assert
				Assert.AreEqual(exception.GetType(), typeof(Speaker.SpeakerDoesntMeetRequirementsException));
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
					YearsExperience = 1,
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
