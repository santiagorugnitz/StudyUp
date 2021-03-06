// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebAPI.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20210515145016_Comments")]
    partial class Comments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Deck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Decks");
                });

            modelBuilder.Entity("Domain.DeckGroup", b =>
                {
                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("DeckId")
                        .HasColumnType("int");

                    b.HasKey("GroupId", "DeckId");

                    b.HasIndex("DeckId");

                    b.ToTable("DeckGroups");
                });

            modelBuilder.Entity("Domain.Exam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("GroupId");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("Domain.ExamCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Answer")
                        .HasColumnType("bit");

                    b.Property<int?>("ExamId")
                        .HasColumnType("int");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExamId");

                    b.ToTable("ExamCard");
                });

            modelBuilder.Entity("Domain.Flashcard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DeckId")
                        .HasColumnType("int");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DeckId");

                    b.ToTable("Flashcards");
                });

            modelBuilder.Entity("Domain.FlashcardComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FlashcardId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FlashcardId");

                    b.ToTable("FlashcardComment");
                });

            modelBuilder.Entity("Domain.FlashcardScore", b =>
                {
                    b.Property<int>("FlashcardId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("FlashcardId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("FlashcardScore");
                });

            modelBuilder.Entity("Domain.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirebaseToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsStudent")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.UserExam", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ExamId")
                        .HasColumnType("int");

                    b.Property<double?>("Score")
                        .HasColumnType("float");

                    b.HasKey("UserId", "ExamId");

                    b.HasIndex("ExamId");

                    b.ToTable("UserExam");
                });

            modelBuilder.Entity("Domain.UserFollowing", b =>
                {
                    b.Property<int>("FollowingUserId")
                        .HasColumnType("int");

                    b.Property<int>("FollowerUserId")
                        .HasColumnType("int");

                    b.HasKey("FollowingUserId", "FollowerUserId");

                    b.HasIndex("FollowerUserId");

                    b.ToTable("UserFollowing");
                });

            modelBuilder.Entity("Domain.UserGroup", b =>
                {
                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("Domain.Deck", b =>
                {
                    b.HasOne("Domain.User", "Author")
                        .WithMany("Decks")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Domain.DeckGroup", b =>
                {
                    b.HasOne("Domain.Deck", "Deck")
                        .WithMany("DeckGroups")
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Group", "Group")
                        .WithMany("DeckGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Deck");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Domain.Exam", b =>
                {
                    b.HasOne("Domain.User", "Author")
                        .WithMany("Exams")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Group", "Group")
                        .WithMany("AssignedExams")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Author");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Domain.ExamCard", b =>
                {
                    b.HasOne("Domain.Exam", "Exam")
                        .WithMany("ExamCards")
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Exam");
                });

            modelBuilder.Entity("Domain.Flashcard", b =>
                {
                    b.HasOne("Domain.Deck", "Deck")
                        .WithMany("Flashcards")
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Deck");
                });

            modelBuilder.Entity("Domain.FlashcardComment", b =>
                {
                    b.HasOne("Domain.Flashcard", "Flashcard")
                        .WithMany("Comments")
                        .HasForeignKey("FlashcardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Flashcard");
                });

            modelBuilder.Entity("Domain.FlashcardScore", b =>
                {
                    b.HasOne("Domain.Flashcard", "Flashcard")
                        .WithMany("UserScores")
                        .HasForeignKey("FlashcardId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.User", "User")
                        .WithMany("FlashcardScores")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Flashcard");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Group", b =>
                {
                    b.HasOne("Domain.User", "Creator")
                        .WithMany("Groups")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Domain.UserExam", b =>
                {
                    b.HasOne("Domain.Exam", "Exam")
                        .WithMany("AlreadyPerformed")
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.User", "User")
                        .WithMany("SolvedExams")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Exam");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.UserFollowing", b =>
                {
                    b.HasOne("Domain.User", "FollowerUser")
                        .WithMany("FollowedUsers")
                        .HasForeignKey("FollowerUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.User", "FollowingUser")
                        .WithMany("FollowingUsers")
                        .HasForeignKey("FollowingUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FollowerUser");

                    b.Navigation("FollowingUser");
                });

            modelBuilder.Entity("Domain.UserGroup", b =>
                {
                    b.HasOne("Domain.Group", "Group")
                        .WithMany("UserGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.User", "User")
                        .WithMany("UserGroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Deck", b =>
                {
                    b.Navigation("DeckGroups");

                    b.Navigation("Flashcards");
                });

            modelBuilder.Entity("Domain.Exam", b =>
                {
                    b.Navigation("AlreadyPerformed");

                    b.Navigation("ExamCards");
                });

            modelBuilder.Entity("Domain.Flashcard", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("UserScores");
                });

            modelBuilder.Entity("Domain.Group", b =>
                {
                    b.Navigation("AssignedExams");

                    b.Navigation("DeckGroups");

                    b.Navigation("UserGroups");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Navigation("Decks");

                    b.Navigation("Exams");

                    b.Navigation("FlashcardScores");

                    b.Navigation("FollowedUsers");

                    b.Navigation("FollowingUsers");

                    b.Navigation("Groups");

                    b.Navigation("SolvedExams");

                    b.Navigation("UserGroups");
                });
#pragma warning restore 612, 618
        }
    }
}
