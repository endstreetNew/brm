using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using razor.Components.Models;
using Sassa.Surveys.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sassa.eServices.Admin.Services
{
    public class SurveyService
    {
        //SurveyContext  _context;
        string connectionstring;
        public SurveyService(IConfiguration _config)
        {
            connectionstring = _config.GetConnectionString("SurveysOracle");
        }

        //public SurveyService(SurveyContext context)
        //{
        //    _context = context;
        //}

        private static List<Survey> surveys = new List<Survey> {
          new Survey {
              Id = 1,
              Title = "Sassa online grants customer satisfaction survey.",
              Options = new List<SurveyOption>{
                  new SurveyOption{ Id = 1,OptionName = "How easy was it to complete your social grant application online?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 2,OptionName = "Could you complete the application in reasonable time?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 3,OptionName = "How likely is it that you would recommend SASSA’s online application to someone ?", optionType = OptionType.Rating, OptionValue = 3},
                  new SurveyOption{ Id = 4,OptionName = "To what extent were you satisfied with the online application process?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 5,OptionName = "Have you benefitted form the online application?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 6,OptionName = "Do you feel reassured that your application would be processed efficiently?", optionType = OptionType.Rating, OptionValue = 3},
                  new SurveyOption{ Id = 7,OptionName = "Do you feel that the documentation you submitted would be handled with confidentiality?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 8,OptionName = "Were you kept informed regarding progress and next step during your application?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 9,OptionName = "Was the online application form and annexures clear and easy to understand?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 10,OptionName = "How easy was it to upload supporting documents?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 11,OptionName = "Did we meet your expectations during the application process?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 12,OptionName = "Please tell us how can we improve the applications experience?", optionType = OptionType.Comment }
              }
          },
          new Survey {
              Id = 2,
              Title = "Sassa online grants internal survey.",
              Options = new List<SurveyOption>{
                  new SurveyOption{ Id = 1,OptionName = "Did the online application system reduce a long queue at local office?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 2,OptionName = "Did the online application lead to reduction of functions performed by officials at a local office?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 3,OptionName = "Time taken to complete attesting by Grant Administrator (screening and capturing of application)? ", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 4,OptionName = "Time taken to complete verification of application by a Senior Grant Administrator?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 5,OptionName = "Time taken to complete an application (attesting to approval?)", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 6,OptionName = "Are all officials able to complete the allocated cases for the day?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 7,OptionName = "How easy to communicate outcomes of application to the applicants?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 8,OptionName = "System accessible to clients?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 9,OptionName = "System user friendly to clients?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 10,OptionName = "Did clients receive feedback about outcomes in good time?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 11,OptionName = "Were outcomes easy to understand by clients?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 12,OptionName = "Were the payment options clear to clients?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 13,OptionName = "Does the office still use service points to bring social grants application services closer to communities?", optionType = OptionType.Rating, OptionValue = 3 },
                  new SurveyOption{ Id = 14,OptionName = "Suggestions to improve:", optionType = OptionType.Comment }
                }
          },
        };
        public Survey GetSurvey(int id)
        {
            return surveys.SingleOrDefault(t => t.Id == id);
        }

        public async Task SaveSurvey(SurveyResult answer)
        {
            var sql = $"INSERT INTO SURVEYRESULT (SURVEYID,OPTIONID,OPTIONTYPE,OPTIONVALUE,OCOMMENT) VALUES({answer.SurveyId},{answer.OptionId},{answer.optionType},{answer.OptionValue},'{answer.OComment}')";
            using (OracleConnection connection = new OracleConnection(connectionstring))
            {

                connection.Open();
                using (OracleCommand command = new OracleCommand(sql, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }

            //_context.SurveyResult.Add(answer);
            //await _context.SaveChangesAsync();
        }

        public List<SurveyResult> GetSurveyResults(int surveyId)
        {
            DataTable dataTable = new DataTable();
            using (OracleConnection connection = new OracleConnection(connectionstring))
            {
                //String sql =;
                using (OracleCommand command = new OracleCommand($@"SELECT * FROM SURVEYRESULT WHERE SURVEYID = {surveyId} AND OptionType=0", connection))
                {
                    connection.Open();
                    OracleDataAdapter da = new OracleDataAdapter(command);
                    da.Fill(dataTable);
                    connection.Close();
                    da.Dispose();
                }
            }
            return dataTable.ToList<SurveyResult>();
        }
        //public void oldPieData()
        //{
        //    Survey survey = GetSurvey(1);
        //    surveys = new List<Survey>();
        //    surveys.Add(survey);
        //    var results = GetSurveyResults(1);
        //    survey.PieData = new PieData(survey, results);
        //}
        /// <summary>
        /// Gets PieData on File requests
        /// </summary>
        /// <returns></returns>
        public PieData GetPieData()
        {
            try
            {


                PieData pd = new PieData();
                pd.ChartName = "User satisfaction survey results.";
                var results = GetSurveyResults(1);
                double total = results.Count();
                //double fraction = total / 100;
                pd.TotalItems = (int)total;
                pd.Segments.Add(new PieSegment { Name = "Very Dissatisfied", Percent = (results.Where(c => c.OptionValue == 0).Count()) * 100 / total, Color = SmileyColors["Very Dissatisfied"] });
                pd.Segments.Add(new PieSegment { Name = "Dissatisfied", Percent = (results.Where(c => c.OptionValue == 1).Count()) * 100 / total, Color = SmileyColors["Dissatisfied"] });
                pd.Segments.Add(new PieSegment { Name = "OK", Percent = (results.Where(c => c.OptionValue == 2).Count()) * 100 / total, Color = SmileyColors["OK"] });
                pd.Segments.Add(new PieSegment { Name = "Not Perfect", Percent = (results.Where(c => c.OptionValue == 3).Count()) * 100 / total, Color = SmileyColors["Not Perfect"] });
                pd.Segments.Add(new PieSegment { Name = "Satisfied", Percent = (results.Where(c => c.OptionValue == 4).Count()) * 100 / total, Color = SmileyColors["Satisfied"] });
                pd.Segments.Add(new PieSegment { Name = "Very Satisfied", Percent = (results.Where(c => c.OptionValue == 5).Count()) * 100 / total, Color = SmileyColors["Very Satisfied"] });

                return pd;
            }
            catch //(Exception ex)
            {
                throw;
            }
        }

        private static Dictionary<string, string> _smileyColors;
        private static Dictionary<string,string> SmileyColors
        {
            get
            {
                if (_smileyColors != null) return _smileyColors;
                _smileyColors = new Dictionary<string, string>();
                _smileyColors.Add("Very Dissatisfied", "Red");
                _smileyColors.Add("Dissatisfied", "Orange");
                _smileyColors.Add("Not Perfect", "Yellow");
                _smileyColors.Add("OK", "LightGreen");
                _smileyColors.Add("Satisfied", "Green");
                _smileyColors.Add("Very Satisfied", "DarkGreen");
                return _smileyColors;
            }
        }
    }
}
