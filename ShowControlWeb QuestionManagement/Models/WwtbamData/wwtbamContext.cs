using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using ShowControlWeb_QuestionManagement.Models.WwtbamData.ViewModels;

namespace ShowControlWeb_QuestionManagement
{
    public sealed partial class wwtbamContext : DbContext
    {
        //Scaffold-DbContext 'Data Source=IDEA-PC\SQLEXPRESS02;Initial Catalog=wwtbam;Integrated Security=True' Microsoft.EntityFrameworkCore.SqlServer   
        public wwtbamContext()
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public wwtbamContext(DbContextOptions<wwtbamContext> options)
            : base(options)
        {

        }


        public DbSet<Audiencevotes> Audiencevotes { get; set; }
        public DbSet<Contestantbank> Contestantbank { get; set; }
        public DbSet<Contestantonshow> Contestantonshow { get; set; }
        public DbSet<Gamequestions> Gamequestions { get; set; }
        public DbSet<Mapingtypes> Mapingtypes { get; set; }
        public DbSet<Moneytreetypes> Moneytreetypes { get; set; }
        public DbSet<Moneytreevalues> Moneytreevalues { get; set; }
        public DbSet<Qleveldifficultymaping> Qleveldifficultymaping { get; set; }
        public DbSet<Questioncategories> Questioncategories { get; set; }
        public DbSet<Livestacks> Livestacks { get; set; }
        public DbSet<Questionstackitems> Questionstackitems { get; set; }
        public DbSet<Questionstacks> Questionstacks { get; set; }
        public DbSet<Questionsubcategories> Questionsubcategories { get; set; }
        public DbSet<Showsetup> Showsetup { get; set; }
        public DbSet<Stackconfigurations> Stackconfigurations { get; set; }
        public DbSet<Stateofgameplay> Stateofgameplay { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Userslastactions> Userslastactions { get; set; }
        public Gamequestions GetRandomlySelectedQuestion(int StackId, int Type, int Difficulty, int TimesAnswered)
        {
            var query = $"EXEC [dbo].[proc_GetRandomlySelectedQuestion] " +
                $"@StackID = {StackId}, @Type = {Type}, @Difficulty = {Difficulty}, @TimesAnswered = {TimesAnswered}";
            Gamequestions qid = this.Gamequestions.FromSqlRaw(query).ToList().FirstOrDefault();
            return qid;
        }
        public List<Gamequestions> GetReplacementQuestions(int Type, int Difficulty, int TimesAnswered, int NumberOfQuestions = 8)
        {
            var query = $"EXEC [dbo].[proc_GetReplacementQuestions] " +
                $"@Type = {Type}, @Difficulty = {Difficulty}, @TimesAnswered = {TimesAnswered}, @NumberOfQuestions = {NumberOfQuestions}";
            List<Gamequestions> qid = this.Gamequestions.FromSqlRaw(query).ToList();
            return qid;
        }
        public void SwapStackQuestionLevel(int StackId, int CurrentStackLevel, int NextStackLevel)
        {
            var query = $"EXEC [dbo].[proc_SwapStackQuestionLevel] " +
                $"@StackID = {StackId}, @CurrentStackLevel = {CurrentStackLevel}, @NextStackLevel = {NextStackLevel}";
            this.Database.ExecuteSqlRaw(query);
        }
        public void SwapGamequestionTables()
        {
            var query = $"" +
            //"USE [wwtbam];" +
            "EXEC sp_rename 'dbo.gamequestions', 'gamequestions_temp';" +
            "EXEC sp_rename 'dbo.gamequestions-2ndLang', 'gamequestions';" +
            "EXEC sp_rename 'dbo.gamequestions_temp', 'gamequestions-2ndLang'; ";
            this.Database.ExecuteSqlRaw(query);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DbConnectionString_wwtbam"].ConnectionString);
                //"Data Source=IDEA-PC\\SQLEXPRESS03;Initial Catalog=wwtbam;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Audiencevotes>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("audiencevotes");

                entity.HasIndex(e => e.Username)
                    .HasName("Username")
                    .IsUnique();

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.TimeOfAnswering)
                    .HasColumnType("datetime2(6)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contestantbank>(entity =>
            {
                entity.HasKey(e => e.ContestantId)
                    .HasName("PK__contesta__2FE55A65D93FA056");

                entity.ToTable("contestantbank");

                entity.Property(e => e.ContestantId).HasColumnName("ContestantID");

                entity.Property(e => e.Biography)
                    .IsRequired()
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Education)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PersonalInfo)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Preferences)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contestantonshow>(entity =>
            {
                entity.HasKey(e => new { e.ContestantId, e.ShowId })
                    .HasName("PK__contesta__193B64689435AD13");

                entity.ToTable("contestantonshow");

                entity.HasIndex(e => new { e.ContestantId, e.ShowId })
                    .HasName("ContestantID")
                    .IsUnique();

                entity.HasIndex(e => new { e.ShowId, e.SeatPosition })
                    .HasName("ShowID")
                    .IsUnique();

                entity.Property(e => e.ContestantId).HasColumnName("ContestantID");

                entity.Property(e => e.ShowId).HasColumnName("ShowID");

                entity.Property(e => e.Finished).HasDefaultValueSql("('0')");

                entity.Property(e => e.MoneyWon).HasDefaultValueSql("('0')");

                entity.Property(e => e.SeatPosition).HasDefaultValueSql("('0')");
            });

            modelBuilder.Entity<Gamequestions>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("PK__gameques__0DC06F8C3E26130E");

                entity.ToTable("gamequestions");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("QuestionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AdditionalCategoryId)
                    .HasColumnName("AdditionalCategoryID")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.AdditionalSubcategoryId)
                    .HasColumnName("AdditionalSubcategoryID")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Answer1)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Answer2)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Answer3)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Answer4)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Comments).IsUnicode(false);

                entity.Property(e => e.DateOfCreation)
                    .HasColumnType("datetime2(0)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Difficulty).HasDefaultValueSql("('1')");

                entity.Property(e => e.LastDateAnswered).HasColumnType("datetime2(0)");

                entity.Property(e => e.MoreInformation).IsUnicode(false);

                entity.Property(e => e.Pronunciation).IsUnicode(false);

                entity.Property(e => e.Question)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.QuestionCreator)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('John Doe')");

                entity.Property(e => e.SubcategoryId)
                    .HasColumnName("SubcategoryID")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.TimesAnswered).HasDefaultValueSql("('0')");

                entity.Property(e => e.Type).HasDefaultValueSql("('1')");
            });

            modelBuilder.Entity<Livestacks>(entity =>
            {
                entity.HasKey(e => new { e.StackType, e.IsReplacement });

                entity.ToTable("livestacks");

                entity.HasIndex(e => e.StackId)
                    .HasName("UK_livestacks")
                    .IsUnique();

                entity.Property(e => e.StackId).HasColumnName("StackID");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<Mapingtypes>(entity =>
            {
                entity.HasKey(e => e.MapingId)
                    .HasName("PK__mapingty__78AC50120F087242");

                entity.ToTable("mapingtypes");

                entity.HasIndex(e => e.Maping)
                    .HasName("Maping")
                    .IsUnique();

                entity.Property(e => e.MapingId)
                    .HasColumnName("MapingID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Maping)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Moneytreetypes>(entity =>
            {
                entity.HasKey(e => e.MoneyTreeId)
                    .HasName("PK__moneytre__1F9A6DFF1B4C1F60");

                entity.ToTable("moneytreetypes");

                entity.HasIndex(e => e.MoneyTreeName)
                    .HasName("MoneyTreeName")
                    .IsUnique();

                entity.Property(e => e.MoneyTreeId)
                    .HasColumnName("MoneyTreeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsSet).HasDefaultValueSql("('0')");

                entity.Property(e => e.MoneyTreeName)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Moneytreevalues>(entity =>
            {
                entity.HasKey(e => new { e.MoneyTreeId, e.Level })
                    .HasName("PK__moneytre__3535E469DECF3B0B");

                entity.ToTable("moneytreevalues");

                entity.Property(e => e.MoneyTreeId).HasColumnName("MoneyTreeID");

                entity.Property(e => e.Level).HasDefaultValueSql("('0')");

                entity.Property(e => e.Currency)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Qaway).HasColumnName("QAway");

                entity.Property(e => e.SafeHeaven).HasDefaultValueSql("('0')");
            });

            modelBuilder.Entity<Qleveldifficultymaping>(entity =>
            {
                entity.HasKey(e => new { e.Maping, e.Level })
                    .HasName("PK__qleveldi__886ED14E96CE10A1");

                entity.ToTable("qleveldifficultymaping");

                entity.Property(e => e.Maping)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Questioncategories>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__question__19093A2B0ECA38AA");

                entity.ToTable("questioncategories");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Questionstackitems>(entity =>
            {
                entity.HasKey(e => new { e.StackId, e.StackLevel }) //
                    .HasName("PK__question__33D98D41F58782AA");

                entity.ToTable("questionstackitems");

                entity.Property(e => e.StackId).HasColumnName("StackID");

                entity.Property(e => e.AdditionalCategoryId)
                    .HasColumnName("AdditionalCategoryID")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.AdditionalSubcategoryId)
                    .HasColumnName("AdditionalSubcategoryID")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.SubcategoryId)
                    .HasColumnName("SubcategoryID")
                    .HasDefaultValueSql("('0')");
            });

            modelBuilder.Entity<Questionstacks>(entity =>
            {
                entity.HasKey(e => e.StackId)
                    .HasName("PK__question__E117F127FCE90586");

                entity.ToTable("questionstacks");

                entity.Property(e => e.StackId).HasColumnName("StackID");

                entity.Property(e => e.Stack)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime2(0)")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Questionsubcategories>(entity =>
            {
                entity.HasKey(e => new { e.CategoryId, e.SubcategoryId })
                    .HasName("PK__question__D0CDDD2CA2AD6D31");

                entity.ToTable("questionsubcategories");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.SubcategoryId).HasColumnName("SubcategoryID");

                entity.Property(e => e.Subcategory)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Showsetup>(entity =>
            {
                entity.HasKey(e => e.ShowId)
                    .HasName("PK__showsetu__6DE3E0D21E7D2A09");

                entity.ToTable("showsetup");

                entity.Property(e => e.ShowId).HasColumnName("ShowID");

                entity.Property(e => e.Broadcasted).HasDefaultValueSql("('0')");

                entity.Property(e => e.DateOfBroadcasting).HasColumnType("date");

                entity.Property(e => e.DateOfShooting).HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.Shooted).HasDefaultValueSql("('0')");

                entity.Property(e => e.ShowName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Stackconfigurations>(entity =>
            {
                entity.HasKey(e => e.StackConfigurationId);

                entity.ToTable("stackconfigurations");

                entity.Property(e => e.StackConfigurationId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Stateofgameplay>(entity =>
            {
                entity.HasKey(e => new { e.MessageType, e.TimeStamp })
                    .HasName("PK__stateofg__8E1937E7988AE9ED");

                entity.ToTable("stateofgameplay");

                entity.Property(e => e.MessageType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime2(2)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Answer1)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Answer2)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Answer3)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Answer4)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Audience1).HasDefaultValueSql("('0')");

                entity.Property(e => e.Audience2).HasDefaultValueSql("('0')");

                entity.Property(e => e.Audience3).HasDefaultValueSql("('0')");

                entity.Property(e => e.Audience4).HasDefaultValueSql("('0')");

                entity.Property(e => e.CorrectAnswer)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Explanation)
                    .IsRequired()
                    .HasMaxLength(700)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FinalAnswer)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Lifeline1).HasDefaultValueSql("('0')");

                entity.Property(e => e.Lifeline2).HasDefaultValueSql("('0')");

                entity.Property(e => e.Lifeline3).HasDefaultValueSql("('0')");

                entity.Property(e => e.Lifeline4).HasDefaultValueSql("('0')");

                entity.Property(e => e.PhoneFriend).HasDefaultValueSql("('30')");

                entity.Property(e => e.PlayerNameLocation)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProducerChat)
                    .IsRequired()
                    .HasMaxLength(700)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Pronunciation)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReadBy).HasDefaultValueSql("('0')");

                entity.Property(e => e.ShowControlMessage)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__users__536C85E55A7DA81A");

                entity.ToTable("users");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Typeofuser)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Userslastactions>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__userslas__536C85E56D4A8C17");

                entity.ToTable("userslastactions");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastAction)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime2(0)")
                    .HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
