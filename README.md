I had used .net web API template, with C# language with dot net 8.0 version.

I had used QuestionaireDTO which contains title of employer form and it contains list of QuestionDTO which represents question type,questions and answer related to it.

QuestionAnswerDTO is used by candidate form, and it contains List of QuestionDTO which contains employer form questions and answers submitted by the candidates.

I have created three API related to employer form: to create employer, edit employer form and delete employer form.

I have created three API related to candidate form: to create candidate form,delete candidate form, and get list of questions in employer form based on question type.

I have used dependency injection here in the project, where I had register CosmosDBContext class which is used to store data in database.

I have used unit test also which depicts tests to succesfull creation for container and database, and to read data from container.

To start the unit tests which is in Form.Tests project, there is only one class UnitTest1.cs, in this class replace EndpointUrl and PrimaryKey with cosmos db emulator, so that you can successfully run tests.

In the unit tests whenever database and container is created at the end of test it is also deleted so it should not have effect on other tests.

To run Form project, remember to replace the value of key with cosmos db configuration keys where "CosmosDb:EndpointUri" represents endpoint and "CosmosDb:Primarykey" represents primary key.
You can use swagger to enter data in the web API project, for some scenarios like editing candidate form you might need to go to database to get formID inside questionaireTable container to edit form,so other than getting primary key for the rows while editing employer form and saving candidate form, you don't need to go to the database.
