SET IDENTITY_INSERT [dbo].[Questionnaire] ON 

GO
INSERT [dbo].[Questionnaire] ([ID], [QuestionnaireType], [Title]) VALUES (1, 0, N'Φοιτητές - Αξιολόγηση ΓΠΑ')
GO
INSERT [dbo].[Questionnaire] ([ID], [QuestionnaireType], [Title]) VALUES (2, 1, N'Φοιτητές - Αξιολόγηση ΦΥΠΑ ')
GO
INSERT [dbo].[Questionnaire] ([ID], [QuestionnaireType], [Title]) VALUES (3, 2, N'Φοιτητές - Αξιολόγηση ΑΤΛΑΣ')
GO
INSERT [dbo].[Questionnaire] ([ID], [QuestionnaireType], [Title]) VALUES (4, 3, N'ΓΠΑ - Αξιολόγηση φοιτητών ')
GO
INSERT [dbo].[Questionnaire] ([ID], [QuestionnaireType], [Title]) VALUES (5, 4, N'ΓΠΑ - Αξιολόγηση ΦΥΠΑ ')
GO
INSERT [dbo].[Questionnaire] ([ID], [QuestionnaireType], [Title]) VALUES (6, 5, N'ΓΠΑ - Αξιολόγηση ΑΤΛΑΣ ')
GO
INSERT [dbo].[Questionnaire] ([ID], [QuestionnaireType], [Title]) VALUES (7, 6, N'ΦΥΠΑ - Αξιολόγηση φοιτητών')
GO
INSERT [dbo].[Questionnaire] ([ID], [QuestionnaireType], [Title]) VALUES (8, 7, N'ΦΥΠΑ - Αξιολόγηση ΓΠΑ ')
GO
INSERT [dbo].[Questionnaire] ([ID], [QuestionnaireType], [Title]) VALUES (9, 8, N'ΦΥΠΑ - Αξιολόγηση ΑΤΛΑΣ ')
GO
SET IDENTITY_INSERT [dbo].[Questionnaire] OFF
GO
SET IDENTITY_INSERT [dbo].[QuestionnaireQuestion] ON 

GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (14, 1, 0, 1, N'Πόσο ικανοποιημένος/η είστε συνολικά από τη συνεργασία σας με το Γραφείο Πρακτικής Άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (15, 1, 0, 2, N'Πόσο γρήγορα ανταποκρίθηκε το Γραφείο Πρακτικής Άσκησης κατά την διαχείριση θεμάτων που άπτονταν της πρακτικής σας άσκησης (διαδικαστικά ζητήματα, ενδεχόμενα προβλήματα – δυσλειτουργίες);', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (16, 1, 1, 3, N'Αντιμετωπίσατε κάποιο πρόβλημα ή έχετε κάποια παρατήρηση – πρόταση για βελτίωση που σχετίζεται με τη συνεργασία σας με το ΓΠΑ;', NULL)
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (17, 2, 0, 1, N'Πόσο ικανοποιημένος είστε συνολικά από τη συνεργασία σας με τον επόπτη πρακτικής άσκησης του φορέα υποδοχής της πρακτικής σας άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (18, 2, 0, 2, N'Θεωρείτε το χώρο και τις συνθήκες απασχόλησης στο φορέα υποδοχής πρακτικής άσκησης κατάλληλες;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (19, 2, 0, 3, N'Θεωρείτε ότι η πρακτική άσκηση που πραγματοποιήσατε ανταποκρινόταν στο αντικείμενο των σπουδών σας;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (20, 2, 0, 4, N'Θεωρείτε ότι οι γνώσεις που αποκτήσατε στο Τμήμα σας (θεωρητικές και πρακτικές) ήταν επαρκείς για να ανταπεξέλθετε στις ανάγκες που απαιτήθηκαν κατά την διενέργεια της Πρακτικής σας άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (21, 2, 0, 5, N'Πόσο κατάλληλο θεωρείτε ότι ήταν το χρονικό σημείο (εξάμηνο σπουδών) που πραγματοποιήσατε την πρακτική σας άσκηση;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (22, 2, 0, 6, N'Θεωρείτε επαρκές το χρονικό διάστημα (διάρκεια) της πρακτικής σας άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (23, 2, 0, 7, N'Σας δόθηκαν δυνατότητες ανάπτυξης πρωτοβουλιών κατά τη διενέργεια της πρακτικής σας άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (24, 2, 0, 8, N'Θα σας ενδιέφερε να απασχοληθείτε στο συγκεκριμένο φορέα υποδοχής πρακτικής άσκησης μετά το πέρας των σπουδών σας;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (25, 2, 0, 9, N'Θα συστήνατε σε συναδέλφους σας να πραγματοποιήσουν πρακτική άσκηση στο συγκεκριμένο Φορέα;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (26, 2, 0, 10, N'Πόσο θεωρείτε ότι θα σας βοηθήσει η πρακτική άσκηση που διενεργήσατε στη μετέπειτα εύρεση εργασίας στον ίδιο ή σε διαφορετικό εργοδότη;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (27, 2, 1, 11, N'Τι πιστεύετε ότι θα μπορούσε να βελτιώσει τη διαδικασία της πρακτικής άσκησης; Καταγράψτε σύντομα τις προτάσεις σας στο παρακάτω πλαίσιο (μέχρι 500 χαρακτήρες):', NULL)
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (28, 3, 0, 1, N'Πόσο εύχρηστο θεωρείτε το σύστημα ΑΤΛΑΣ;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (29, 3, 0, 2, N'Πόσο θεωρείτε ότι βοήθησε το σύστημα ΑΤΛΑΣ στην εύρεση της θέσης πρακτικής άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (30, 3, 0, 3, N'Πόσο ευχαριστημένος/η είστε από την εξυπηρέτηση που λάβατε από το Helpdesk της υπηρεσίας ΑΤΛΑΣ;', N'<PossibleAnswers><Answer Text="Δεν χρειάστηκε επικοινωνία με το Helpdesk" Value="0" /><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (31, 3, 1, 4, N'Πώς πιστεύετε ότι θα μπορούσε να βελτιωθεί το σύστημα ΑΤΛΑΣ; 
Καταγράψτε σύντομα τις προτάσεις σας στο παρακάτω πλαίσιο (μέχρι 500 χαρακτήρες):
', NULL)
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (32, 4, 0, 1, N'Πόσο ικανοποιημένος/η είστε από τη συνεργασία, το ενδιαφέρον και τη συνέπεια του φοιτητή/τριας πριν και κατά τη διάρκεια της διενέργειας της θέσης πρακτικής άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (33, 4, 0, 2, N'Το αντικείμενο της Πρακτικής Άσκησης ήταν ανάλογο των σπουδών και της ειδίκευσης του φοιτητή/τριας;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (34, 4, 0, 3, N'Πόσο επαρκείς θεωρείτε ότι ήταν οι γνώσεις (θεωρητικές, πρακτικές) που είχε αποκομίσει ο φοιτητής/τρια κατά τη διάρκεια των σπουδών του ώστε να ανταπεξέλθει στις απαιτήσεις της πρακτικής άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (35, 4, 1, 4, N'Αντιμετωπίσατε κάποιο πρόβλημα  ή έχετε κάποια παρατήρηση που σχετίζεται με τη συνεργασία σας με το φοιτητή/τρια; 
Καταγράψτε σύντομα τις προτάσεις σας στο παρακάτω πλαίσιο (μέχρι 500 χαρακτήρες):
', NULL)
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (36, 5, 0, 1, N'Πόσο ικανοποιημένος/η είστε από τη συνεργασία και την επικοινωνία με τον φορέα υποδοχής πρακτικής άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (37, 5, 0, 2, N'Η εκπαίδευση και η καθοδήγηση των φοιτητών από τον φορέα υποδοχής ήταν ικανοποιητική;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (38, 5, 0, 3, N'Θεωρείτε το χώρο, τις υποδομές και τις συνθήκες στο φορέα υποδοχής πρακτικής άσκησης κατάλληλες για την εκτέλεση των πρακτικών ασκήσεων των φοιτητών;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (40, 5, 0, 4, N'Θα συστήνατε σε άλλους φοιτητές να πραγματοποιήσουν πρακτική άσκηση στο συγκεκριμένο φορέα;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (41, 5, 1, 5, N'Τι πιστεύετε ότι θα μπορούσε να βελτιωθεί στη συνεργασία σας με το φορέα; 
Καταγράψτε σύντομα τις προτάσεις σας στο παρακάτω πλαίσιο (μέχρι 500 χαρακτήρες):
', NULL)
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (42, 6, 0, 1, N'Πόσο εύχρηστο θεωρείτε το σύστημα ΑΤΛΑΣ;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (43, 6, 0, 2, N'Πόσο θεωρείτε ότι βοήθησε το σύστημα ΑΤΛΑΣ στην επικοινωνία με τους φορείς υποδοχής και στην εύρεση νέων θέσεων πρακτικής άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (44, 6, 0, 3, N'Πόσο ευχαριστημένος/η είστε από την εξυπηρέτηση που λάβατε από το Helpdesk της υπηρεσίας ΑΤΛΑΣ;', N'<PossibleAnswers><Answer Text="Δεν χρειάστηκε επικοινωνία με το Helpdesk" Value="0" /><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (45, 6, 1, 4, N'Πώς πιστεύετε ότι θα μπορούσε να βελτιωθεί το σύστημα ΑΤΛΑΣ; 
Καταγράψτε σύντομα τις προτάσεις σας στο παρακάτω πλαίσιο (μέχρι 500 χαρακτήρες):
', NULL)
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (46, 7, 0, 1, N'Πόσο ικανοποιημένος/η είστε από το βαθμό συνεργασίας και το ενδιαφέρον του φοιτητή για τη διενέργεια της θέσης πρακτικής άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (47, 7, 0, 2, N'Το αντικείμενο της Πρακτικής Άσκησης ήταν ανάλογο των σπουδών και της ειδίκευσης του φοιτητή;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (48, 7, 0, 3, N'Ήταν επαρκείς οι θεωρητικές γνώσεις  που είχε αποκομίσει ο/η φοιτητής κατά τη διάρκεια των σπουδών του ώστε να ανταπεξέλθει στις απαιτήσεις της πρακτικής άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (49, 7, 0, 4, N'Ήταν επαρκείς οι πρακτικές γνώσεις  που είχε αποκομίσει ο/η φοιτητής/τρια κατά τη διάρκεια των σπουδών του ώστε να ανταπεξέλθει στις απαιτήσεις της πρακτικής άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (51, 7, 0, 5, N'Πόσο επαγγελματική θεωρείτε ότι ήταν η συμπεριφορά του ασκούμενου φοιτητή/τριας (π.χ. τήρηση ωραρίου, συνεργασία με συναδέλφους);', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (52, 7, 0, 6, N'Θα προσλαμβάνατε ή θα συστήνατε σε άλλο φορέα τον ασκούμενο φοιτητή/τρια;', N'<PossibleAnswers><Answer Text="Όχι" Value="1" /><Answer Text="Ίσως" Value="2" /><Answer Text="Ναι, σίγουρα" Value="3" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (53, 7, 1, 7, N'Αντιμετωπίσατε κάποιο πρόβλημα  ή έχετε κάποια παρατήρηση που σχετίζεται με τη συνεργασία σας με το φοιτητή/τρια;', NULL)
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (54, 8, 0, 1, N'Πόσο ικανοποιημένος/η είστε από τη συνεργασία με το Γραφείο Πρακτικής Άσκησης;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (55, 8, 0, 2, N'Προτίθεστε να συνεργαστείτε ξανά με το συγκεκριμένο Γραφείο Πρακτικής Άσκησης;', N'<PossibleAnswers><Answer Text="Όχι, δεν το επιθυμώ" Value="1" /><Answer Text="Ίσως" Value="2" /><Answer Text="Ναι, σίγουρα" Value="3" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (57, 8, 0, 3, N'Πόσο ικανοποιημένος/η είστε συνολικά από την ενημέρωση που σας παρείχε το Γραφείο Πρακτικής Άσκησης τόσο πριν όσο και κατά τη διάρκεια της πρακτικής άσκησης των ασκούμενων φοιτητών;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (59, 8, 1, 4, N'Αντιμετωπίσατε κάποιο πρόβλημα  ή έχετε κάποια παρατήρηση – πρόταση για βελτίωση που σχετίζεται με τη συνεργασία σας με το ΓΠΑ;', NULL)
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (60, 9, 0, 1, N'Πόσο ικανοποιημένος/η είστε από το σύστημα ΑΤΛΑΣ;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (61, 9, 0, 2, N'Πόσο θεωρείτε ότι βοήθησε το σύστημα ΑΤΛΑΣ κατά την αναγγελία μια θέσης πρακτικής άσκησης και την εν γένει επικοινωνία με τα Γραφεία Πρακτικής Άσκησης και τους φοιτητές;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (63, 9, 0, 3, N'Πόσο εύκολη θα χαρακτηρίζετε τη χρήση του συστήματος ΑΤΛΑΣ;', N'<PossibleAnswers><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (64, 9, 0, 4, N'Πόσο ευχαριστημένος/η είστε από την εξυπηρέτηση που λάβατε από το Helpdesk της υπηρεσίας ΑΤΛΑΣ;', N'<PossibleAnswers><Answer Text="Δεν χρειάστηκε επικοινωνία με το Helpdesk" Value="0" /><Answer Text="Καθόλου" Value="1" /><Answer Text="Λίγο" Value="2" /><Answer Text="Πολύ" Value="3" /><Answer Text="Πάρα πολύ" Value="4" /></PossibleAnswers>')
GO
INSERT [dbo].[QuestionnaireQuestion] ([ID], [QuestionnaireID], [QuestionType], [QuestionNumber], [Title], [PossibleAnswers]) VALUES (65, 9, 1, 5, N'Πώς πιστεύετε ότι θα μπορούσε να βελτιωθεί το σύστημα ΑΤΛΑΣ; 
Καταγράψτε σύντομα τις προτάσεις σας στο παρακάτω πλαίσιο (μέχρι 500 χαρακτήρες):
', NULL)
GO
SET IDENTITY_INSERT [dbo].[QuestionnaireQuestion] OFF
GO
