CREATE TABLE [ApplicationUsers] (
  [id] nvarchar(255) PRIMARY KEY,
  [full_name] varchar(100),
  [email] nvarchar(255) UNIQUE,
  [user_name] nvarchar(255) UNIQUE,
  [age] int,
  [is_deleted] bool,
  [created_by_id] nvarchar(255),
  [created_on] timestamp,
  [last_updated_by_id] nvarchar(255),
  [last_updated_on] timestamp,
  [role] nvarchar(255)
)
GO

CREATE TABLE [Clients] (
  [id] int PRIMARY KEY,
  [full_name] varchar(100),
  [birth_date] date,
  [phone_number] varchar(20),
  [address] varchar(200),
  [user_id] nvarchar(255)
)
GO

CREATE TABLE [Muscles] (
  [id] int PRIMARY KEY,
  [name] varchar(100),
  [description] text
)
GO

CREATE TABLE [Exercises] (
  [id] int PRIMARY KEY,
  [name] varchar(150),
  [description] text,
  [video_url] nvarchar(255),
  [muscle_id] int
)
GO

CREATE TABLE [AssignExercises] (
  [id] int PRIMARY KEY,
  [sets] int,
  [reps] int,
  [notes] text,
  [client_id] int,
  [exercise_id] int,
  [assigned_on] timestamp
)
GO

CREATE TABLE [Foods] (
  [id] int PRIMARY KEY,
  [name] varchar(100),
  [calories] int,
  [protein] decimal,
  [carbs] decimal,
  [fats] decimal,
  [description] text
)
GO

CREATE TABLE [AssignFoods] (
  [id] int PRIMARY KEY,
  [assigned_on] timestamp,
  [notes] text,
  [quantity] int,
  [client_id] int,
  [food_id] int
)
GO

CREATE TABLE [Questions] (
  [id] int PRIMARY KEY,
  [text] text,
  [type] nvarchar(255)
)
GO

CREATE TABLE [Options] (
  [id] int PRIMARY KEY,
  [text] text,
  [question_id] int
)
GO

CREATE TABLE [ClientAnswers] (
  [id] int PRIMARY KEY,
  [answer_text] text,
  [client_id] int,
  [question_id] int,
  [option_id] int
)
GO

CREATE TABLE [Books] (
  [id] int PRIMARY KEY,
  [title] nvarchar(255),
  [description] text,
  [file_url] nvarchar(255),
  [has_demo] bool,
  [demo_url] nvarchar(255)
)
GO

CREATE TABLE [BookRequests] (
  [id] int PRIMARY KEY,
  [client_id] int,
  [book_id] int,
  [status] nvarchar(255),
  [is_approved] bool
)
GO

CREATE TABLE [Courses] (
  [id] int PRIMARY KEY,
  [title] nvarchar(255),
  [description] text,
  [has_demo] bool
)
GO

CREATE TABLE [CourseVideos] (
  [id] int PRIMARY KEY,
  [title] nvarchar(255),
  [video_url] nvarchar(255),
  [course_id] int
)
GO

CREATE TABLE [CourseRequests] (
  [id] int PRIMARY KEY,
  [client_id] int,
  [course_id] int,
  [status] nvarchar(255),
  [is_approved] bool
)
GO

CREATE TABLE [CoachingPackages] (
  [id] int PRIMARY KEY,
  [title] varchar(150),
  [description] text,
  [duration_in_months] int,
  [price] decimal
)
GO

CREATE TABLE [CoachingPackageRequests] (
  [id] int PRIMARY KEY,
  [status] nvarchar(255),
  [start_date] date,
  [end_date] date,
  [package_id] int,
  [client_id] int
)
GO

CREATE TABLE [Transformations] (
  [id] int PRIMARY KEY,
  [titles] nvarchar(255),
  [notes] text,
  [before_image_url] nvarchar(255),
  [after_image_url] nvarchar(255),
  [date] timestamp,
  [client_id] int
)
GO

CREATE TABLE [CoachFeedbacks] (
  [id] int PRIMARY KEY,
  [feedback_text] text,
  [rating] int,
  [client_id] int
)
GO

ALTER TABLE [Clients] ADD FOREIGN KEY ([user_id]) REFERENCES [ApplicationUsers] ([id])
GO

ALTER TABLE [Exercises] ADD FOREIGN KEY ([muscle_id]) REFERENCES [Muscles] ([id])
GO

ALTER TABLE [AssignExercises] ADD FOREIGN KEY ([client_id]) REFERENCES [Clients] ([id])
GO

ALTER TABLE [AssignExercises] ADD FOREIGN KEY ([exercise_id]) REFERENCES [Exercises] ([id])
GO

ALTER TABLE [AssignFoods] ADD FOREIGN KEY ([client_id]) REFERENCES [Clients] ([id])
GO

ALTER TABLE [AssignFoods] ADD FOREIGN KEY ([food_id]) REFERENCES [Foods] ([id])
GO

ALTER TABLE [Options] ADD FOREIGN KEY ([question_id]) REFERENCES [Questions] ([id])
GO

ALTER TABLE [ClientAnswers] ADD FOREIGN KEY ([client_id]) REFERENCES [Clients] ([id])
GO

ALTER TABLE [ClientAnswers] ADD FOREIGN KEY ([question_id]) REFERENCES [Questions] ([id])
GO

ALTER TABLE [ClientAnswers] ADD FOREIGN KEY ([option_id]) REFERENCES [Options] ([id])
GO

ALTER TABLE [BookRequests] ADD FOREIGN KEY ([client_id]) REFERENCES [Clients] ([id])
GO

ALTER TABLE [BookRequests] ADD FOREIGN KEY ([book_id]) REFERENCES [Books] ([id])
GO

ALTER TABLE [CourseVideos] ADD FOREIGN KEY ([course_id]) REFERENCES [Courses] ([id])
GO

ALTER TABLE [CourseRequests] ADD FOREIGN KEY ([client_id]) REFERENCES [Clients] ([id])
GO

ALTER TABLE [CourseRequests] ADD FOREIGN KEY ([course_id]) REFERENCES [Courses] ([id])
GO

ALTER TABLE [CoachingPackageRequests] ADD FOREIGN KEY ([package_id]) REFERENCES [CoachingPackages] ([id])
GO

ALTER TABLE [CoachingPackageRequests] ADD FOREIGN KEY ([client_id]) REFERENCES [Clients] ([id])
GO

ALTER TABLE [Transformations] ADD FOREIGN KEY ([client_id]) REFERENCES [Clients] ([id])
GO

ALTER TABLE [CoachFeedbacks] ADD FOREIGN KEY ([client_id]) REFERENCES [Clients] ([id])
GO
