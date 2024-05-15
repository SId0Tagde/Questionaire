using Form.DTO;
using Form.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Form.Data
{
    public class CosmosDBContext
    {
        #region Private Varibles.
        #region Dependencies.
        private readonly CosmosClient _cosmosClient;
        private readonly string _questionaireDatabaseName = "QuestionaireDatabase";
        private readonly string _questionaireContainerName = "QuestionaireTable";
        private readonly string _responsePartitionKey = "candidateId";
        private readonly string _responseContainerName = "ResponseTable";
        #endregion
        private Container _container { get; set; } = null!;
        #endregion

        #region Constructor
        public CosmosDBContext(CosmosClient cc)
        {
            _cosmosClient = cc;
        }
        #endregion

        #region Public Methods.

        /// <summary>
        ///  Creates employer form.
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public async Task<bool> CreateEmployerFormAsync(QuestionaireModel question)
        {
            if (question.Id == null)
            {
                question.Id = Guid.NewGuid().ToString();
            }
            foreach (var questionModel in question.questions)
            {
                questionModel.Id = Guid.NewGuid().ToString();
            }
            _container = _cosmosClient.GetContainer(_questionaireDatabaseName, _questionaireContainerName);
            var response = await _container.CreateItemAsync<QuestionaireModel>(question, new PartitionKey(question.Id));
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Edit employer form.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="questions"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> EditEmployerFormAsync(string id, QuestionaireModel questions)
        {
            _container = _cosmosClient.GetContainer(_questionaireDatabaseName, _questionaireContainerName);
            var response = await _container.ReadItemAsync<QuestionaireModel>(id, new PartitionKey(id));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                questions.Id = id;
                foreach (var questionModel in questions.questions)
                {
                    questionModel.Id = Guid.NewGuid().ToString();
                }
                var replaceResponse = await _container.ReplaceItemAsync<QuestionaireModel>(questions, id, new PartitionKey(id));
                return replaceResponse.StatusCode;
            }
            else
            {
                return System.Net.HttpStatusCode.NotFound;
            }
        }

        /// <summary>
        /// Deletes employer form.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> DeleteQuestionaireForm(string id)
        {
            _container = _cosmosClient.GetContainer(_questionaireDatabaseName, _questionaireContainerName);
            var response = await _container.DeleteItemAsync<QuestionaireModel>(id, new PartitionKey(id));
            return response.StatusCode;
        }

        /// <summary>
        /// Get questions list based on question type inside an emplyer form.
        /// </summary>
        /// <param name="formId">employer form id</param>
        /// <param name="questionType">question type</param>
        /// <returns></returns>
        public async Task<List<QuestionModel>> GetQuestionsBasedOnQuestionTypeForSpecificForm(string formId, string questionType)
        {
            var questionsList = new List<QuestionModel>();
            string sqlQueryText = $"SELECT q " +
                              $"FROM c " +
                              $"JOIN q IN c.questions " +
                              $"WHERE c.id = @specificQuestionnaireId AND q.type = @specificQuestionType";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText)
                .WithParameter("@specificQuestionnaireId", formId)
                .WithParameter("@specificQuestionType", questionType);
            _container = _cosmosClient.GetContainer(_questionaireDatabaseName, _questionaireContainerName);
            var queryResultSetIterator = _container.GetItemQueryIterator<dynamic>(
                queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (var document in currentResultSet)
                {
                    dynamic q = document["q"];
                    QuestionModel questionModel = new QuestionModel
                    {
                        Id = q["Id"],
                        type = q["type"],
                        question = q["question"],
                        choices = ((JArray)q["choices"]).Select(c => c.ToString()).ToList(),
                        answers = ((JArray)q["answers"]).Select(a => a.ToString()).ToList(),
                        answer = q["answer"]
                    };
                    var choices = q["answers"];
                    questionsList.Add(questionModel);
                }
            }
            return questionsList;
        }

        /// <summary>
        /// Save Candidate Form
        /// </summary>
        /// <param name="responseForm">Candidate form</param>
        /// <returns></returns>
        public async Task<Response> SaveCandidateForm(Response responseForm)
        {
            var container = _cosmosClient.GetContainer(_questionaireDatabaseName, _responseContainerName);
            if (responseForm.Id == null)
            {
                responseForm.Id = Guid.NewGuid().ToString();
            }
            var response = await container.CreateItemAsync<Response>(responseForm, new PartitionKey(responseForm.Id));
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return response.Resource;
            }
            else
            {
                return new Response();
            }
        }

        /// <summary>
        /// Deletes candidate form. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> DeleteCandidateForm(string id)
        {
            _container = _cosmosClient.GetContainer(_questionaireDatabaseName, _responseContainerName);
            var response = await _container.DeleteItemAsync<Response>(id, new PartitionKey(id));
            return response.StatusCode;
        }
        #endregion
    }
}
