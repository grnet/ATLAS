UPDATE [dbo].[InternshipPosition]
SET UpdatedAt = CreatedAt, UpdatedBy = CreatedBy
WHERE UpdatedAt IS null AND GroupID IN 
	(SELECT ID 
	 FROM [dbo].[InternshipPositionGroup]
	 WHERE PositionCreationType = 1)