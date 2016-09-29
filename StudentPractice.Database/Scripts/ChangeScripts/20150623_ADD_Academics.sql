INSERT INTO Academic (ID, InstitutionID, Institution, School, Department, Address, ZipCode, CityID, PrefectureID, Semesters, MaxAllowedPreAssignedPositions, PreAssignedPositions, IsActive, InstitutionInLatin, SchoolInLatin, DepartmentInLatin)
VALUES (779, 20, N'ΠΑΝΕΠΙΣΤΗΜΙΟ ΙΩΑΝΝΙΝΩΝ', N'ΑΡΧΙΤΕΚΤΟΝΩΝ ΜΗΧΑΝΙΚΩΝ', N'ΑΡΧΙΤΕΚΤΟΝΩΝ ΜΗΧΑΝΙΚΩΝ', NULL, NULL, NULL, NULL, 10, 300, 0, 1, N'UNIVERSITY OF IOANNINA', N'ARCHITECTURE', N'ARCHITECTURE')

INSERT INTO Academic (ID, InstitutionID, Institution, School, Department, Address, ZipCode, CityID, PrefectureID, Semesters, MaxAllowedPreAssignedPositions, PreAssignedPositions, IsActive, InstitutionInLatin, SchoolInLatin, DepartmentInLatin)
VALUES (778, 18, N'ΠΑΝΕΠΙΣΤΗΜΙΟ ΔΥΤΙΚΗΣ ΜΑΚΕΔΟΝΙΑΣ', N'ΠΟΛΥΤΕΧΝΙΚΗ', N'ΜΗΧΑΝΙΚΩΝ ΠΕΡΙΒΑΛΛΟΝΤΟΣ', NULL, NULL, NULL, NULL, 10, 300, 0, 1, N'UNIVERSITY OF WESTERN MACEDONIA', N'ENGINEERING', N'ENVIRONMENTAL ENGINEERING')

INSERT ReporterAcademicXRef (ReporterID, AcademicID)
VALUES (0, 779)

INSERT ReporterAcademicXRef (ReporterID, AcademicID)
VALUES (0, 778)