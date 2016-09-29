CREATE TRIGGER [dbo].[ProviderCertificationNumberAssignment] 
   ON  dbo.Reporter
   AFTER INSERT
AS 
BEGIN	
	DECLARE @ID	as int
	
	SELECT 
		@ID = ID			
	FROM 
		inserted
	
	BEGIN
		UPDATE Reporter
		SET CertificationDate = GetDate(),
			CertificationNumber = 
			(
				SELECT MAX(CertificationNumber) FROM Reporter
				WHERE ReporterType = 3
				AND DeclarationType = 1
				AND IsMasterAccount = 1
			) + 1
		WHERE ID = @ID
		AND ReporterType = 3
		AND DeclarationType = 1
		AND IsMasterAccount = 1
	END
END
