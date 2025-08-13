CREATE TABLE [ApplicationUser] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [FullName] varchar(100) NOT NULL,
  [Email] varchar(150) NOT NULL,
  [PasswordHash] text NOT NULL,
  [Role] varchar(50) NOT NULL
)
GO

CREATE TABLE [CoachingPackage] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] varchar(100) NOT NULL,
  [DurationInMonths] int NOT NULL,
  [Price] decimal NOT NULL,
  [Description] text
)
GO

CREATE TABLE [OnlineCoachingSubscription] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [ClientId] int NOT NULL,
  [PackageId] int NOT NULL,
  [Status] varchar(50) NOT NULL,
  [StartDate] datetime,
  [EndDate] datetime,
  [QuestionnaireId] int,
  [AssignedNutritionProgramId] int,
  [AssignedExerciseProgramId] int
)
GO

CREATE TABLE [Questionnaire] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [SubscriptionId] int NOT NULL
)
GO

CREATE TABLE [Question] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Text] text NOT NULL,
  [QuestionnaireId] int NOT NULL
)
GO

CREATE TABLE [ClientAnswer] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [QuestionnaireId] int NOT NULL,
  [QuestionId] int NOT NULL,
  [Answer] text NOT NULL
)
GO

CREATE TABLE [NutritionProgram] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] varchar(200) NOT NULL
)
GO

CREATE TABLE [NutritionMeal] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] varchar(200) NOT NULL
)
GO

CREATE TABLE [FoodItem] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] varchar(200) NOT NULL,
  [Calories] decimal,
  [Protein] decimal,
  [Carbs] decimal,
  [Fat] decimal
)
GO

CREATE TABLE [ExerciseProgram] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] varchar(200)
)
GO

CREATE TABLE [Exercise] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] varchar(200) NOT NULL,
  [MuscleGroup] varchar(200),
  [Instructions] text
)
GO

CREATE TABLE [Book] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Title] varchar(200) NOT NULL,
  [Description] text,
  [DemoFileUrl] text,
  [FullBookFileUrl] text
)
GO

CREATE TABLE [BookPurchase] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [ClientId] int NOT NULL,
  [BookId] int NOT NULL,
  [Status] varchar(50) NOT NULL
)
GO

CREATE TABLE [Course] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Title] varchar(200) NOT NULL,
  [Description] text,
  [DemoVideoUrl] text
)
GO

CREATE TABLE [CourseVideo] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Title] varchar(200) NOT NULL,
  [YoutubeUrl] text
)
GO

CREATE TABLE [CoursePurchase] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [ClientId] int NOT NULL,
  [CourseId] int NOT NULL,
  [Status] varchar(50) NOT NULL
)
GO

CREATE TABLE [CourseProgress] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [PurchaseId] int NOT NULL,
  [VideoId] int NOT NULL,
  [Watched] bool
)
GO

ALTER TABLE [OnlineCoachingSubscription] ADD FOREIGN KEY ([ClientId]) REFERENCES [ApplicationUser] ([Id])
GO

ALTER TABLE [BookPurchase] ADD FOREIGN KEY ([ClientId]) REFERENCES [ApplicationUser] ([Id])
GO

ALTER TABLE [CoursePurchase] ADD FOREIGN KEY ([ClientId]) REFERENCES [ApplicationUser] ([Id])
GO

ALTER TABLE [OnlineCoachingSubscription] ADD FOREIGN KEY ([PackageId]) REFERENCES [CoachingPackage] ([Id])
GO

ALTER TABLE [Questionnaire] ADD FOREIGN KEY ([SubscriptionId]) REFERENCES [OnlineCoachingSubscription] ([Id])
GO

ALTER TABLE [Question] ADD FOREIGN KEY ([QuestionnaireId]) REFERENCES [Questionnaire] ([Id])
GO

ALTER TABLE [ClientAnswer] ADD FOREIGN KEY ([QuestionnaireId]) REFERENCES [Questionnaire] ([Id])
GO

ALTER TABLE [ClientAnswer] ADD FOREIGN KEY ([QuestionId]) REFERENCES [Question] ([Id])
GO

ALTER TABLE [OnlineCoachingSubscription] ADD FOREIGN KEY ([AssignedNutritionProgramId]) REFERENCES [NutritionProgram] ([Id])
GO

ALTER TABLE [NutritionMeal] ADD FOREIGN KEY ([Id]) REFERENCES [NutritionProgram] ([Id])
GO

ALTER TABLE [FoodItem] ADD FOREIGN KEY ([Id]) REFERENCES [NutritionMeal] ([Id])
GO

ALTER TABLE [OnlineCoachingSubscription] ADD FOREIGN KEY ([AssignedExerciseProgramId]) REFERENCES [ExerciseProgram] ([Id])
GO

ALTER TABLE [Exercise] ADD FOREIGN KEY ([Id]) REFERENCES [ExerciseProgram] ([Id])
GO

ALTER TABLE [BookPurchase] ADD FOREIGN KEY ([BookId]) REFERENCES [Book] ([Id])
GO

ALTER TABLE [CourseVideo] ADD FOREIGN KEY ([Id]) REFERENCES [Course] ([Id])
GO

ALTER TABLE [CoursePurchase] ADD FOREIGN KEY ([CourseId]) REFERENCES [Course] ([Id])
GO

ALTER TABLE [CourseProgress] ADD FOREIGN KEY ([PurchaseId]) REFERENCES [CoursePurchase] ([Id])
GO

ALTER TABLE [CourseProgress] ADD FOREIGN KEY ([VideoId]) REFERENCES [CourseVideo] ([Id])
GO
