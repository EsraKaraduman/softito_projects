using System;
using Microsoft.Data.SqlClient;

namespace Project_7.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(string connectionString)
        {
            var masterBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master"
            };

            using (var connection = new SqlConnection(masterBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ArtCourseDb') CREATE DATABASE ArtCourseDb", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                ExecuteNonQuery(connection, @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                    CREATE TABLE Users (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        FullName NVARCHAR(100) NOT NULL,
                        Email NVARCHAR(100) UNIQUE NOT NULL,
                        PasswordHash NVARCHAR(256) NOT NULL,
                        Role NVARCHAR(20) NOT NULL
                    )
                ");

                ExecuteNonQuery(connection, @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Instructors')
                    CREATE TABLE Instructors (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        FullName NVARCHAR(100) NOT NULL,
                        Specialty NVARCHAR(100) NOT NULL,
                        Bio NVARCHAR(MAX) NULL,
                        ImageUrl NVARCHAR(500) NULL
                    )
                ");

                ExecuteNonQuery(connection, @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses')
                    CREATE TABLE Courses (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Title NVARCHAR(100) NOT NULL,
                        Description NVARCHAR(MAX) NULL,
                        Category NVARCHAR(50) NOT NULL,
                        Price DECIMAL(18,2) NOT NULL,
                        Duration NVARCHAR(50) NOT NULL,
                        InstructorId INT NULL FOREIGN KEY REFERENCES Instructors(Id) ON DELETE SET NULL,
                        ImageUrl NVARCHAR(500) NULL
                    )
                ");

                ExecuteNonQuery(connection, @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Enrollments')
                    CREATE TABLE Enrollments (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
                        CourseId INT NOT NULL FOREIGN KEY REFERENCES Courses(Id) ON DELETE CASCADE,
                        EnrollmentDate DATETIME DEFAULT GETDATE(),
                        Status NVARCHAR(20) DEFAULT 'Active'
                    )
                ");

                CreateOrRecreateProcedures(connection);
                SeedData(connection);
            }
        }

        private static void ExecuteNonQuery(SqlConnection connection, string sql)
        {
            using (var command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static void CreateOrRecreateProcedures(SqlConnection connection)
        {
            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_RegisterUser', 'P') IS NOT NULL DROP PROCEDURE sp_RegisterUser");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_RegisterUser
                    @FullName NVARCHAR(100),
                    @Email NVARCHAR(100),
                    @PasswordHash NVARCHAR(256),
                    @Role NVARCHAR(20)
                AS
                BEGIN
                    INSERT INTO Users (FullName, Email, PasswordHash, Role)
                    VALUES (@FullName, @Email, @PasswordHash, @Role);
                    SELECT SCOPE_IDENTITY();
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_GetUserByEmail', 'P') IS NOT NULL DROP PROCEDURE sp_GetUserByEmail");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_GetUserByEmail
                    @Email NVARCHAR(100)
                AS
                BEGIN
                    SELECT * FROM Users WHERE Email = @Email;
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_GetAllCourses', 'P') IS NOT NULL DROP PROCEDURE sp_GetAllCourses");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_GetAllCourses
                AS
                BEGIN
                    SELECT 
                        c.Id, c.Title, c.Description, c.Category, c.Price, c.Duration, c.InstructorId, c.ImageUrl,
                        i.Id, i.FullName, i.Specialty, i.Bio, i.ImageUrl
                    FROM Courses c 
                    LEFT JOIN Instructors i ON c.InstructorId = i.Id;
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_GetCourseById', 'P') IS NOT NULL DROP PROCEDURE sp_GetCourseById");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_GetCourseById
                    @Id INT
                AS
                BEGIN
                    SELECT 
                        c.Id, c.Title, c.Description, c.Category, c.Price, c.Duration, c.InstructorId, c.ImageUrl,
                        i.Id, i.FullName, i.Specialty, i.Bio, i.ImageUrl
                    FROM Courses c 
                    LEFT JOIN Instructors i ON c.InstructorId = i.Id 
                    WHERE c.Id = @Id;
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_InsertCourse', 'P') IS NOT NULL DROP PROCEDURE sp_InsertCourse");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_InsertCourse
                    @Title NVARCHAR(100),
                    @Description NVARCHAR(MAX),
                    @Category NVARCHAR(50),
                    @Price DECIMAL(18,2),
                    @Duration NVARCHAR(50),
                    @InstructorId INT,
                    @ImageUrl NVARCHAR(500)
                AS
                BEGIN
                    DECLARE @ActualInstructorId INT = NULL;
                    IF @InstructorId > 0 SET @ActualInstructorId = @InstructorId;

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES (@Title, @Description, @Category, @Price, @Duration, @ActualInstructorId, @ImageUrl);
                    SELECT SCOPE_IDENTITY();
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_UpdateCourse', 'P') IS NOT NULL DROP PROCEDURE sp_UpdateCourse");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_UpdateCourse
                    @Id INT,
                    @Title NVARCHAR(100),
                    @Description NVARCHAR(MAX),
                    @Category NVARCHAR(50),
                    @Price DECIMAL(18,2),
                    @Duration NVARCHAR(50),
                    @InstructorId INT,
                    @ImageUrl NVARCHAR(500)
                AS
                BEGIN
                    DECLARE @ActualInstructorId INT = NULL;
                    IF @InstructorId > 0 SET @ActualInstructorId = @InstructorId;

                    UPDATE Courses
                    SET Title = @Title, Description = @Description, Category = @Category, Price = @Price, 
                        Duration = @Duration, InstructorId = @ActualInstructorId, ImageUrl = @ImageUrl
                    WHERE Id = @Id;
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_DeleteCourse', 'P') IS NOT NULL DROP PROCEDURE sp_DeleteCourse");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_DeleteCourse
                    @Id INT
                AS
                BEGIN
                    DELETE FROM Courses WHERE Id = @Id;
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_SearchCourses', 'P') IS NOT NULL DROP PROCEDURE sp_SearchCourses");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_SearchCourses
                    @SearchQuery NVARCHAR(100)
                AS
                BEGIN
                    SELECT 
                        c.Id, c.Title, c.Description, c.Category, c.Price, c.Duration, c.InstructorId, c.ImageUrl,
                        i.Id, i.FullName, i.Specialty, i.Bio, i.ImageUrl
                    FROM Courses c 
                    LEFT JOIN Instructors i ON c.InstructorId = i.Id
                    WHERE c.Title LIKE '%' + @SearchQuery + '%' 
                       OR c.Description LIKE '%' + @SearchQuery + '%' 
                       OR c.Category LIKE '%' + @SearchQuery + '%';
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_GetAllInstructors', 'P') IS NOT NULL DROP PROCEDURE sp_GetAllInstructors");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_GetAllInstructors
                AS
                BEGIN
                    SELECT * FROM Instructors;
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_GetInstructorById', 'P') IS NOT NULL DROP PROCEDURE sp_GetInstructorById");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_GetInstructorById
                    @Id INT
                AS
                BEGIN
                    SELECT * FROM Instructors WHERE Id = @Id;
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_InsertInstructor', 'P') IS NOT NULL DROP PROCEDURE sp_InsertInstructor");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_InsertInstructor
                    @FullName NVARCHAR(100),
                    @Specialty NVARCHAR(100),
                    @Bio NVARCHAR(MAX),
                    @ImageUrl NVARCHAR(500)
                AS
                BEGIN
                    INSERT INTO Instructors (FullName, Specialty, Bio, ImageUrl)
                    VALUES (@FullName, @Specialty, @Bio, @ImageUrl);
                    SELECT SCOPE_IDENTITY();
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_UpdateInstructor', 'P') IS NOT NULL DROP PROCEDURE sp_UpdateInstructor");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_UpdateInstructor
                    @Id INT,
                    @FullName NVARCHAR(100),
                    @Specialty NVARCHAR(100),
                    @Bio NVARCHAR(MAX),
                    @ImageUrl NVARCHAR(500)
                AS
                BEGIN
                    UPDATE Instructors
                    SET FullName = @FullName, Specialty = @Specialty, Bio = @Bio, ImageUrl = @ImageUrl
                    WHERE Id = @Id;
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_DeleteInstructor', 'P') IS NOT NULL DROP PROCEDURE sp_DeleteInstructor");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_DeleteInstructor
                    @Id INT
                AS
                BEGIN
                    DELETE FROM Instructors WHERE Id = @Id;
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_EnrollUser', 'P') IS NOT NULL DROP PROCEDURE sp_EnrollUser");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_EnrollUser
                    @UserId INT,
                    @CourseId INT
                AS
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM Enrollments WHERE UserId = @UserId AND CourseId = @CourseId AND Status = 'Active')
                    BEGIN
                        INSERT INTO Enrollments (UserId, CourseId, EnrollmentDate, Status)
                        VALUES (@UserId, @CourseId, GETDATE(), 'Active');
                        SELECT SCOPE_IDENTITY();
                    END
                    ELSE
                    BEGIN
                        SELECT 0;
                    END
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_GetEnrollments', 'P') IS NOT NULL DROP PROCEDURE sp_GetEnrollments");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_GetEnrollments
                    @UserId INT = NULL
                AS
                BEGIN
                    SELECT 
                        e.Id AS EnrollmentId,
                        u.Id AS UserId,
                        u.FullName AS UserFullName,
                        u.Email AS UserEmail,
                        c.Id AS CourseId,
                        c.Title AS CourseTitle,
                        c.Price AS CoursePrice,
                        c.Category AS Category,
                        e.EnrollmentDate AS EnrollmentDate,
                        e.Status AS Status
                    FROM Enrollments e
                    INNER JOIN Users u ON e.UserId = u.Id
                    INNER JOIN Courses c ON e.CourseId = c.Id
                    WHERE (@UserId IS NULL OR @UserId = 0 OR e.UserId = @UserId);
                END
            ");

            ExecuteNonQuery(connection, "IF OBJECT_ID('sp_CancelEnrollment', 'P') IS NOT NULL DROP PROCEDURE sp_CancelEnrollment");
            ExecuteNonQuery(connection, @"
                CREATE PROCEDURE sp_CancelEnrollment
                    @EnrollmentId INT
                AS
                BEGIN
                    UPDATE Enrollments SET Status = 'Cancelled' WHERE Id = @EnrollmentId;
                END
            ");
        }

        private static void SeedData(SqlConnection connection)
        {
            int userCount = 0;
            using (var command = new SqlCommand("SELECT COUNT(*) FROM Users", connection))
            {
                userCount = (int)command.ExecuteScalar();
            }

            if (userCount == 0)
            {
                ExecuteNonQuery(connection, @"
                    INSERT INTO Users (FullName, Email, PasswordHash, Role)
                    VALUES ('Admin User', 'admin@artcourse.com', 'AQAAAAIAAYagAAAAEG3cM/Lh8xMeez91bI1o5HhL9L8P8sKz+U8fJdG1ZfG8=', 'Admin');
                ");

                ExecuteNonQuery(connection, @"
                    INSERT INTO Users (FullName, Email, PasswordHash, Role)
                    VALUES ('Ahmet Yilmaz', 'ahmet@artcourse.com', 'AQAAAAIAAYagAAAAEG3cM/Lh8xMeez91bI1o5HhL9L8P8sKz+U8fJdG1ZfG8=', 'Student');
                ");
            }

            int courseCount = 0;
            using (var command = new SqlCommand("SELECT COUNT(*) FROM Courses", connection))
            {
                courseCount = (int)command.ExecuteScalar();
            }

            if (courseCount <= 4)
            {
                ExecuteNonQuery(connection, "DELETE FROM Enrollments");
                ExecuteNonQuery(connection, "DELETE FROM Courses");
                ExecuteNonQuery(connection, "DELETE FROM Instructors");

                ExecuteNonQuery(connection, @"
                    INSERT INTO Instructors (FullName, Specialty, Bio, ImageUrl)
                    VALUES ('Selin Aksoy', 'Painting', 'Güzel Sanatlar Fakültesi mezunu, 10 yıllık yağlı boya ve sulu boya eğitmeni.', 'https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=300');
                    INSERT INTO Instructors (FullName, Specialty, Bio, ImageUrl)
                    VALUES ('Can Demir', 'Guitar', 'Klasik ve elektro gitar ustası, konservatuvar mezunu bestekar.', 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=300');
                    INSERT INTO Instructors (FullName, Specialty, Bio, ImageUrl)
                    VALUES ('Derya Yildiz', 'Wood Painting', 'Geleneksel ahşap boyama ve oymacılık sanatçısı.', 'https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=300');
                    INSERT INTO Instructors (FullName, Specialty, Bio, ImageUrl)
                    VALUES ('Mert Aslan', 'Music', 'Piyano ve kompozisyon eğitmeni, Mimar Sinan Konservatuvarı mezunu.', 'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=300');
                    INSERT INTO Instructors (FullName, Specialty, Bio, ImageUrl)
                    VALUES ('Elif Karaca', 'Painting', 'Suluboya ve karakalem sanatçısı. 6 yıllık atölye deneyimi.', 'https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=300');
                    INSERT INTO Instructors (FullName, Specialty, Bio, ImageUrl)
                    VALUES ('Kaan Yilmaz', 'Guitar', 'Klasik gitar ve bas gitar eğitmeni. Birçok yerel grupta performans sergilemiştir.', 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=300');
                ");

                ExecuteNonQuery(connection, @"
                    DECLARE @SelinId INT = (SELECT TOP 1 Id FROM Instructors WHERE FullName = 'Selin Aksoy');
                    DECLARE @CanId INT = (SELECT TOP 1 Id FROM Instructors WHERE FullName = 'Can Demir');
                    DECLARE @DeryaId INT = (SELECT TOP 1 Id FROM Instructors WHERE FullName = 'Derya Yildiz');
                    DECLARE @MertId INT = (SELECT TOP 1 Id FROM Instructors WHERE FullName = 'Mert Aslan');
                    DECLARE @ElifId INT = (SELECT TOP 1 Id FROM Instructors WHERE FullName = 'Elif Karaca');
                    DECLARE @KaanId INT = (SELECT TOP 1 Id FROM Instructors WHERE FullName = 'Kaan Yilmaz');

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES ('Yağlı Boya Başlangıç', 'Tuval hazırlamadan fırça tekniklerine, renk karıştırmadan kompozisyon oluşturmaya kadar yağlı boya temel eğitimi.', 'Painting', 3500.00, '8 Hafta', @SelinId, 'https://images.unsplash.com/photo-1579783902614-a3fb3927b6a5?w=600');

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES ('Temel Akustik Gitar', 'Temel akorlar, ritim kalıpları, arpej teknikleri ve popüler şarkıların çalınması üzerine gitar eğitimi.', 'Guitar', 3000.00, '10 Hafta', @CanId, 'https://images.unsplash.com/photo-1510915361894-db8b60106cb1?w=600');

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES ('Ahşap Boyama ve Süsleme', 'Eski ahşap eşyaları yenileme, eskitme teknikleri, dekupaj ve rölyef pasta uygulamaları.', 'Wood Painting', 2800.00, '6 Hafta', @DeryaId, 'https://images.unsplash.com/photo-1534080391025-09795d197a5b?w=600');

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES ('İleri Seviye Yağlı Boya', 'Detaylı portre çalışmaları, ışık gölge oyunları ve peyzaj kompozisyonları.', 'Painting', 4500.00, '12 Hafta', @SelinId, 'https://images.unsplash.com/photo-1579783928591-7e4091158fba?w=600');

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES ('Temel Piyano Eğitimi', 'Piyano tuşlarının tanınması, çift el koordinasyonu, solfej ve klasik eserlerin çalınması.', 'Music', 4200.00, '12 Hafta', @MertId, 'https://images.unsplash.com/photo-1520523839897-bd0b52f945a0?w=600');

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES ('Karakalem Portre Teknikleri', 'Yüz anatomisi, göz, burun, ağız çizimi ve gölgelendirme teknikleri ile gerçekçi portre çizimi.', 'Painting', 2500.00, '8 Hafta', @ElifId, 'https://images.unsplash.com/photo-1579783900882-c0d3dad7b119?w=600');

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES ('Klasik Gitar Orta Seviye', 'İleri bareli akorlar, klasik eser yorumlama ve hız çalışmaları.', 'Guitar', 3200.00, '10 Hafta', @KaanId, 'https://images.unsplash.com/photo-1461749280684-dccba630e2f6?w=600');

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES ('Eskitme ve Ahşap Oymacılığı', 'Geleneksel ahşap oyma teknikleri ve antik görünümlü eskitme boyama yöntemleri.', 'Wood Painting', 3100.00, '8 Hafta', @DeryaId, 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=600');

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES ('Şan ve Ses Eğitimi', 'Nefes egzersizleri, ses açma teknikleri, diyafram kullanımı ve repertuar çalışması.', 'Music', 3800.00, '8 Hafta', @MertId, 'https://images.unsplash.com/photo-1511671782779-c97d3d27a1d4?w=600');

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES ('Sulu Boya Manzara Çalışması', 'Sulu boyada transparanlık, ıslak üstüne ıslak tekniği ile doğa ve deniz manzaraları.', 'Painting', 2700.00, '6 Hafta', @ElifId, 'https://images.unsplash.com/photo-1579783928591-7e4091158fba?w=600');

                    INSERT INTO Courses (Title, Description, Category, Price, Duration, InstructorId, ImageUrl)
                    VALUES ('Elektro Gitar & Sololar', 'Pentatonik gamlar, solo teknikleri (bending, tapping) ve amfi/pedal kullanımı.', 'Guitar', 3600.00, '12 Hafta', @CanId, 'https://images.unsplash.com/photo-1510915361894-db8b60106cb1?w=600');
                ");
            }
        }
    }
}
