using FluentAssertions;

namespace Interview.Tests;

[TestFixture(Category = "Integration")]
public class ProgramTests
{

	#region Setup/Teardown

	[SetUp]
	public void Setup(){
		_stringWriter = new StringWriter();
		Console.SetOut(_stringWriter);
	}

	[TearDown]
	public void TearDown(){
		_stringWriter.Dispose();
	}

	#endregion

	#region Fields: Private

	private StringWriter _stringWriter;

	#endregion

	[Test]
	public async Task GetProfile_WritesError_WhenEmailIsNotProvided(){
		// Arrange
		const string expectedMessage = "Error - Email must be provided\r\n";
		string[] args = ["GetProfile", "email", ""];

		// Act
		int result = await Program.Main(args);

		// Assert
		result.Should().Be(0);
		_stringWriter.ToString().Should().Be(expectedMessage);
	}

	//TODO: How would I improve this test?
	[Test]
	public async Task GetProfile_WritesError_WhenUserNotFoundByEmail(){
		// Arrange
		const string expectedMessage = "Warning - User not found\r\n";
		string[] args = ["GetProfile", "email", "aaa@gmail.com"];

		// Act
		int result = await Program.Main(args);

		// Assert
		result.Should().Be(0);
		_stringWriter.ToString().Should().Be(expectedMessage);
	}

}