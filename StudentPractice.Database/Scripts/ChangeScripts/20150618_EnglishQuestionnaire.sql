UPDATE Questionnaire SET TitleEN = N'Student Evaluation' WHERE ID = 7
UPDATE Questionnaire SET TitleEN = N'Evaluation of the Internship Office' WHERE ID = 8
UPDATE Questionnaire SET TitleEN = N'Evaluation of the Atlas Service' WHERE ID = 9

UPDATE QuestionnaireQuestion 
SET TitleEN = N'How satisfied are you with the Atlas Service?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Unsatisfied" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="Satisfied" Value="3" /><Answer Text="Very satisfied" Value="4" /></PossibleAnswers>'
WHERE ID = 60
UPDATE QuestionnaireQuestion 
SET TitleEN = N'How helpful was the Atlas Service in the advertisement of an internship position and the overall communication with Internship Offices and students?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Not helpful" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="Helpful" Value="3" /><Answer Text="Very helpful" Value="4" /></PossibleAnswers>'
WHERE ID = 61
UPDATE QuestionnaireQuestion 
SET TitleEN = N'How user friendly do you consider of the Atlas Service?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Not user friendly" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="User friendly" Value="3" /><Answer Text="Very user friendly" Value="4" /></PossibleAnswers>'
WHERE ID = 63
UPDATE QuestionnaireQuestion 
SET TitleEN = N'How satisfied are you with the service you received from the Atlas Service Helpdesk?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Communication with the Helpdesk was not necessary" Value="0" /><Answer Text="Unsatisfied" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="Satisfied" Value="3" /><Answer Text="Very Satisfied" Value="4" /></PossibleAnswers>'
WHERE ID = 64
UPDATE QuestionnaireQuestion 
SET TitleEN = N'In your opinion, how could the Atlas Service improve? Please provide suggestions you may have in the following box (up to 500 characters)'
WHERE ID = 65

UPDATE QuestionnaireQuestion 
SET TitleEN = N'How satisfied are you with your cooperation with the Internship Office (professionalism, efficiency)?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Unsatisfied" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="Satisfied" Value="3" /><Answer Text="Very Satisfied" Value="4" /></PossibleAnswers>'
WHERE ID = 54
UPDATE QuestionnaireQuestion 
SET TitleEN = N'Would you be willing to cooperate again with the particular Internship Office?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="No, I am not willing" Value="1" /><Answer Text="Maybe" Value="2" /><Answer Text="Yes, definitely" Value="3" /></PossibleAnswers>'
WHERE ID = 55
UPDATE QuestionnaireQuestion 
SET TitleEN = N'How satisfied are you with the information that the Internship Office provided you with before and during the students’ internship?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Unsatisfied" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="Satisfied" Value="3" /><Answer Text="Very Satisfied" Value="4" /></PossibleAnswers>'
WHERE ID = 57
UPDATE QuestionnaireQuestion 
SET TitleEN = N'Did you encounter any problems or have any remarks / suggestions for improvements in regard to your cooperation with the Internship Office? (Please limit your answer up to 500 characters)'
WHERE ID = 59
UPDATE QuestionnaireQuestion 
SET TitleEN = N'How well do you consider the response time of Internship Office upon the information you request?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Not well" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="Well" Value="3" /><Answer Text="Very well" Value="4" /></PossibleAnswers>'
WHERE ID = 66

UPDATE QuestionnaireQuestion 
SET TitleEN = N'How satisfied are you with the student’s interest and perfomance in the job during the internship?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Unsatisfied" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="Satisfied" Value="3" /><Answer Text="Very Satisfied" Value="4" /></PossibleAnswers>'
WHERE ID = 46
UPDATE QuestionnaireQuestion 
SET TitleEN = N'Was the subject of the internship relevant to the studies of the student?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Irrelevant" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="Relevant" Value="3" /><Answer Text="Very relevant" Value="4" /></PossibleAnswers>'
WHERE ID = 47
UPDATE QuestionnaireQuestion 
SET TitleEN = N'Was the theoretical knowledge that the student had acquired during his/her studies sufficient for him/her to respond to the internship’s requirements?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Insufficient" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="Sufficient" Value="3" /><Answer Text="Very sufficient" Value="4" /></PossibleAnswers>'
WHERE ID = 48
UPDATE QuestionnaireQuestion 
SET TitleEN = N'Were the skills that the student had acquired during his/her studies sufficient for him/her to respond to the internship’s requirements?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Insufficient" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="Sufficient" Value="3" /><Answer Text="Very sufficient" Value="4" /></PossibleAnswers>'
WHERE ID = 49
UPDATE QuestionnaireQuestion 
SET TitleEN = N'How professional would you consider the intern’s behavior (e.g. punctuality, cooperation with his/her colleagues)?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="Unprofessional" Value="1" /><Answer Text="Neutral" Value="2" /><Answer Text="Professional" Value="3" /><Answer Text="Very professional" Value="4" /></PossibleAnswers>'
WHERE ID = 51
UPDATE QuestionnaireQuestion 
SET TitleEN = N'Would you hire the intern as an employee or recommend him/her to another provider?', PossibleAnswersEN = N'<PossibleAnswers><Answer Text="No" Value="1" /><Answer Text="Maybe" Value="2" /><Answer Text="Yes, definitely" Value="3" /></PossibleAnswers>'
WHERE ID = 52
UPDATE QuestionnaireQuestion 
SET TitleEN = N'Did you encounter any problems or have any remarks considering your cooperation with the student? (Please limit your answer up to 500 characters)'
WHERE ID = 53